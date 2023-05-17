using Gruppo2.WebApp.Models;
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
        
        public InfluxController(InfluxDBService influxDbService)
        {
            _InfluxDbService = influxDbService;
        }

        


        [HttpGet("GetActivitiesContentbyIDActivity")]//per leggere le tabelle di influx filtrate per idActivity
        public async Task<IEnumerable<FluxTable>> GetActivitiesContentbyIDActivity()
        {
            Guid idActivity = new Guid();//arriva diverso in base alla activity selezionata 
            List<FluxTable> fluxTables = (List<FluxTable>)await _InfluxDbService.GetActivityContentsByIDActivity(idActivity);
            return fluxTables;
        }




        [HttpGet]
        public Task Invoke()
        {
            
            Start();//per adesso simulatore qua per prova ma sul suo servizio
            return Task.CompletedTask;
        }
        static void Start()
        {
            bool moving = true;     // true quando sta avanzando, false quando torna indietro (andamento nella piscina)

            string geoCoordinates = "";
            geoCoordinates = InitGeoCoordinates();
            Console.WriteLine($"Init coordinate: {geoCoordinates}");

            Random nRandom = new Random();
            int randLatLong = nRandom.Next(0, 2);   // 0 viene modificata la latitudine (↔) (spostamento vert.)
                                                    // 1 viene modificate la longitudine (↕) (spostamento orizz.)

            Console.WriteLine($"Se 0 modifica latit. / se 1 modifica longit.: {randLatLong}");

            int minValMovement = 0, maxValMovement = 0;
            LimitsForMovement(geoCoordinates, randLatLong, ref minValMovement, ref maxValMovement);

            int pulseRate = 0;
            pulseRate = InitPulseRate();
            Console.WriteLine($"Init battito cardiaco: {pulseRate}\n\n");

            while (true)
            {
                Thread.Sleep(10000);

                ModifyGeoCoordinates(ref geoCoordinates, randLatLong, minValMovement, maxValMovement, ref moving);

                if (pulseRate > 30 && pulseRate < 200)
                {
                    Console.WriteLine($"Battito cardiaco: {pulseRate}");
                    ModifyPulseRate(ref pulseRate);
                }
                else
                {
                    Console.WriteLine($"Battito cardiaco: {pulseRate} - giro precedente fuori soglia!");
                    ReturnToNormalPulseRate(ref pulseRate);
                }
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
                                         int maxValMovement, ref bool moving)
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

