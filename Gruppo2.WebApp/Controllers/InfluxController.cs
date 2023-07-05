﻿using AutoMapper;
using Gruppo2.WebApp.ClassUtils;
using Gruppo2.WebApp.Entities;
using Gruppo2.WebApp.Models.Dtos;
using Gruppo2.WebApp.Services;
using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Core.Flux.Domain;
using InfluxDB.Client.Writes;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Runtime.CompilerServices;

namespace Gruppo2.WebApp.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class InfluxController : ControllerBase
    {
        private readonly InfluxDBService _InfluxDbService;
        private readonly DBAdminContext _adminContext;
        private readonly IMapper _mapper;
        private readonly IServiceProvider _serviceProvider;

        public InfluxController(InfluxDBService influxDbService, IMapper mapper, DBAdminContext adminContext, IServiceProvider serviceProvider)
        {
            _InfluxDbService = influxDbService;
            _mapper = mapper;
            _adminContext = adminContext;
            _serviceProvider = serviceProvider;
        }




        [HttpGet("GetActivitiesContentbyIDActivity/{idActivityStr}")]//per leggere le tabelle di influx filtrate per idActivity
        public async Task<IEnumerable<ActivityContentDto>> GetActivitiesContentbyIDActivity(string idActivityStr)
        {
            Guid idActivity = Guid.Parse(idActivityStr);
            return await _InfluxDbService.GetActivityContentsByIDActivity(idActivity);
        }





        [HttpGet("{idDevice}/{idUser}")]
        public Task Invoke(string idDevice, string idUser, int duration = 200)
        {
            Task.Factory.StartNew(async () =>
            {
                await Start(_serviceProvider, idDevice, idUser, duration);//per adesso simulatore qua per prova ma sul suo servizio

            });
            return Task.CompletedTask;
        }




        [HttpPost("AddActivityContentInflux")]
        public async Task<bool> SendFromSimulator(DataFromSimulator dataFromSimulator)
        {
            await _InfluxDbService.Write(dataFromSimulator);
            return true;
        }


        [HttpPost("AddNotificationError")]
        public async Task<bool> AddNotificationError(NotificationError error)
        {
            //_errorService.PostNotificationError(error);
            _adminContext.NotificationError.Add(error);
            await _adminContext.SaveChangesAsync();
            return true;
        }




        public async Task Start(IServiceProvider serviceProvider, string idDevice, string idUser, int duration)
        {
            var startTime = DateTime.Now;
            using var scope = serviceProvider.CreateScope();
            var adminContext = scope.ServiceProvider.GetRequiredService<DBAdminContext>();
            var influxDBService = scope.ServiceProvider.GetRequiredService<InfluxDBService>();

            bool moving = true;     // true quando sta avanzando, false quando torna indietro (andamento nella piscina)
            int nPools = 0;
            string geoCoordinates = "";
            geoCoordinates = InitGeoCoordinates();
            Console.WriteLine($"Init coordinate: {geoCoordinates}");

            Guid idActivityGuid = Guid.NewGuid();

            Random nRandom = new Random();
            int randLatLong = nRandom.Next(0, 2);   // 0 viene modificata la latitudine (↔) (spostamento vert.)
                                                    // 1 viene modificate la longitudine (↕) (spostamento orizz.)

            Console.WriteLine($"Se 0 modifica latit. / se 1 modifica longit.: {randLatLong}");

            int minValMovement = 0, maxValMovement = 0;
            LimitsForMovement(geoCoordinates, randLatLong, ref minValMovement, ref maxValMovement);

            int pulseRate = 0;
            pulseRate = InitPulseRate();
            Console.WriteLine($"Init battito cardiaco: {pulseRate}\n\n");

            while (startTime.AddSeconds(duration) > DateTime.Now)
            {

                ModifyGeoCoordinates(ref geoCoordinates, randLatLong, minValMovement, maxValMovement, ref moving, ref nPools);

                if (pulseRate > 30 && pulseRate < 200)
                {
                    Console.WriteLine($"Battito cardiaco: {pulseRate}");
                    ModifyPulseRate(ref pulseRate);
                }
                else
                {
                    Console.WriteLine($"Battito cardiaco: {pulseRate} - giro precedente fuori soglia!");
                    ReturnToNormalPulseRate(ref pulseRate);


                    Guid idDeviceGuid = Guid.Parse(idDevice);
                    Guid idUserGuid = Guid.Parse(idUser);

                    //inserimento riga su NotificationError
                    NotificationError notificationError = new NotificationError();
                    notificationError.IdActivity = idActivityGuid;
                    notificationError.IdDevice = idDeviceGuid;
                    notificationError.IdUser = idUserGuid;
                    notificationError.Id = Guid.NewGuid();
                    notificationError.PulseRate = pulseRate;
                    notificationError.Date = DateTime.Now;
                    await AddNotificationError(notificationError);
                }

                DataFromSimulator dataFromSimulator = new DataFromSimulator();
                dataFromSimulator.PulseRate = pulseRate;
                dataFromSimulator.GeoCoordinates = geoCoordinates;
                dataFromSimulator.IdActivity = idActivityGuid.ToString();
                dataFromSimulator.NoPools = nPools;
                await SendFromSimulator(dataFromSimulator);

                Thread.Sleep(10000);
            }
        }

        #region GeographicCoordinates
        static string InitGeoCoordinates()
        {
            Random nRandom = new Random();

            int latitudeInt = nRandom.Next(-90, 90);
            int longitudeInt = nRandom.Next(-180, 180);

            int latitudeDecimals = nRandom.Next(501, 999499);       //da 500 a (999999-500) perché dovrei gestire nel caso in cui i movimenti
            int longitudeDecimals = nRandom.Next(501, 999499);      //causino anche una modifica della parte intera delle coord.

            string geoCoordinates = ComponingStringCoordinates(latitudeInt, latitudeDecimals, longitudeInt, longitudeDecimals);

            return geoCoordinates;
        }

        static void ModifyGeoCoordinates(ref string geoCoordinates, int randLatLong, int minValMovement,
                                          int maxValMovement, ref bool moving, ref int nPools)
        {
            int latitudeDecimals = 0, longitudeDecimals = 0, latitudeInt = 0, longitudeInt = 0;
            SplitCoordinates(geoCoordinates, ref latitudeInt, ref latitudeDecimals, ref longitudeInt, ref longitudeDecimals);

            Random nRandom = new Random();
            int movement = nRandom.Next(80, 110);
            //Console.WriteLine(movement.ToString());       //distanza percorsa nell'intervallo delle due rilevazioni

            if (randLatLong == 0)
            {
                // SPOSTAMENTO LATITUDINE: mi sposto verticalmente, longitudine rimane uguale, modifico latitudine
                if (moving)
                {
                    // controllo se lo spostamento andrebbe oltre il limite della piscina: in questo caso torno indietro della differenza
                    if ((latitudeDecimals + movement) > maxValMovement)
                    {
                        moving = false;     // l'atleta cambia direzione
                        nPools += 1;

                        int diff = (latitudeDecimals + movement) - maxValMovement;
                        latitudeDecimals = maxValMovement - diff;
                    }
                    else
                    {
                        latitudeDecimals += movement;
                    }
                }
                else
                {
                    // controllo se lo spostamento andrebbe oltre il limite della piscina: in questo caso torno indietro della differenza
                    if ((latitudeDecimals - movement) < minValMovement)
                    {
                        moving = true;      // l'atleta cambia direzione
                        nPools += 1;

                        int diff = minValMovement - (latitudeDecimals - movement);
                        latitudeDecimals = minValMovement + diff;
                    }
                    else
                    {
                        latitudeDecimals -= movement;
                    }
                }
            }
            else
            {
                // SPOSTAMENTO LONGITUDINE: mi sposto orizzontalmente, latitudine rimane uguale, modifico longitudine
                if (moving)
                {
                    // controllo se lo spostamento andrebbe oltre il limite della piscina: in questo caso torno indietro della differenza
                    if ((longitudeDecimals + movement) > maxValMovement)
                    {
                        moving = false;      // l'atleta cambia direzione
                        nPools += 1;

                        int diff = (longitudeDecimals + movement) - maxValMovement;
                        longitudeDecimals = maxValMovement - diff;
                    }
                    else
                    {
                        longitudeDecimals += movement;
                    }
                }
                else
                {
                    // controllo se lo spostamento andrebbe oltre il limite della piscina: in questo caso torno indietro della differenza
                    if ((longitudeDecimals - movement) < minValMovement)
                    {
                        moving = true;      // l'atleta cambia direzione
                        nPools += 1;

                        int diff = (longitudeDecimals - movement) - minValMovement;
                        longitudeDecimals = minValMovement - diff;
                    }
                    else
                    {
                        longitudeDecimals -= movement;
                    }
                }

            }

            geoCoordinates = ComponingStringCoordinates(latitudeInt, latitudeDecimals, longitudeInt, longitudeDecimals);
            Console.WriteLine($"Coordinate modificate: {geoCoordinates}");
            Console.WriteLine($"Vasche fatte dall'inizio dell'allenamento: {nPools}");
        }

        static string ComponingStringCoordinates(int latitudeInt, int latitudeDecimals,
                                                 int longitudeInt, int longitudeDecimals)
        {
            string geoCoordinates = latitudeInt.ToString() + "." + latitudeDecimals.ToString().PadLeft(6, '0') + "," +
                                    longitudeInt.ToString() + "." + longitudeDecimals.ToString().PadLeft(6, '0');

            return geoCoordinates;
        }

        static void LimitsForMovement(string geoCoordinates, int randLatLong, ref int minValMovement, ref int maxValMovement)
        {
            int latitudeDecimals = 0, longitudeDecimals = 0, latitudeInt = 0, longitudeInt = 0;

            if (randLatLong == 0)
            {
                // modifico solo LATITUDINE
                SplitCoordinates(geoCoordinates, ref latitudeInt, ref latitudeDecimals, ref longitudeInt, ref longitudeDecimals);

                minValMovement = latitudeDecimals;
                maxValMovement = latitudeDecimals + 500;
            }
            else
            {
                // modifico solo LONGITUDINE
                SplitCoordinates(geoCoordinates, ref latitudeInt, ref latitudeDecimals, ref longitudeInt, ref longitudeDecimals);

                minValMovement = longitudeDecimals;
                maxValMovement = longitudeDecimals + 500;
            }
        }

        static void SplitCoordinates(string geoCoordinates, ref int latitudeInt, ref int latitudeDecimals,
                                     ref int longitudeInt, ref int longitudeDecimals)
        {
            string[] splittedCoordinates = geoCoordinates.Split(new char[] { ',' });

            string[] onlyLatitude = splittedCoordinates[0].Split(new char[] { '.' });
            latitudeInt = Int32.Parse(onlyLatitude[0]);
            latitudeDecimals = Int32.Parse(onlyLatitude[1]);

            string[] onlyLongitude = splittedCoordinates[1].Split(new char[] { '.' });
            longitudeInt = Int32.Parse(onlyLongitude[0]);
            longitudeDecimals = Int32.Parse(onlyLongitude[1]);
        }

        #endregion


        #region PulseRate

        static int InitPulseRate()
        {
            Random nRandom = new Random();
            int pulseRate = nRandom.Next(40, 190);        // da 40 a 190

            return pulseRate;
        }

        static void ModifyPulseRate(ref int pulseRate)
        {
            Random nRandom = new Random();
            int incrDecrease = nRandom.Next(-7, 8);

            pulseRate += incrDecrease;
        }

        static void ReturnToNormalPulseRate(ref int pulseRate)
        {
            Random nRandom = new Random();
            int incrDecrease = 0;

            switch (pulseRate)
            {
                case <= 24:
                    incrDecrease = nRandom.Next(3, 5);
                    pulseRate += incrDecrease;
                    break;

                case 25:
                case 26:
                case 27:
                    incrDecrease = nRandom.Next(2, 4);
                    pulseRate += incrDecrease;
                    break;

                case 28:
                case 29:
                case 30:
                    incrDecrease = nRandom.Next(1, 3);
                    pulseRate += incrDecrease;
                    break;

                case 200:
                case 201:
                case 202:
                    incrDecrease = nRandom.Next(-3, -1);
                    pulseRate += incrDecrease;
                    break;

                case 203:
                case 204:
                case 205:
                    incrDecrease = nRandom.Next(-4, -2);
                    pulseRate += incrDecrease;
                    break;

                case >= 206:
                    incrDecrease = nRandom.Next(-5, -3);
                    pulseRate += incrDecrease;
                    break;
            }
        }
        #endregion
    }
}

