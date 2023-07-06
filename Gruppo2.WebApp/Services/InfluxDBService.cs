using Azure;
using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Writes;
using InfluxDB.Client.Core;
using InfluxDB.Client.Core.Flux.Domain;
using System.Net.Sockets;
using Microsoft.AspNetCore.Http.HttpResults;
using Gruppo2.WebApp.Models.Dtos;
using AutoMapper;
using Gruppo2.WebApp.ClassUtils;
using Gruppo2.WebApp.Entities;

namespace Gruppo2.WebApp
{
    public class InfluxDBService
    {
        private readonly string _token;
        private readonly string _bucket;
        private readonly string _organization;
        private readonly string _url;
        private readonly IMapper _mapper;
        public InfluxDBService(IConfiguration configuration, IMapper mapper)
        {
            _token = configuration.GetValue<string>("InfluxDB:Token");
            _bucket = configuration.GetValue<string>("InfluxDB:Bucket");
            _organization = configuration.GetValue<string>("InfluxDB:Organization");
            _url = configuration.GetValue<string>("InfluxDB:Url");
            _mapper = mapper;
        }


        public async Task<IEnumerable<ActivityContentDto>> GetActivityContentsByIDActivity(Guid idActivity)
        {
            using var client = InfluxDBClientFactory.Create(_url, _token);//apri connessione a influxdb


            string queryPulseRate = "from(bucket: \"" + _bucket + "\") |> range(start: 0) " +
                               "|> filter(fn: (r) => r[\"idActivity\"] == \"" + idActivity + "\")" +
                               "|> filter(fn: (r) => r._field == \"pulseRate\")"; // Esempio di query con filtro sul campo idActivity

            string queryPosition = "from(bucket: \"" + _bucket + "\") |> range(start: 0) " +
                   "|> filter(fn: (r) => r[\"idActivity\"] == \"" + idActivity + "\")" +
                   "|> filter(fn: (r) => r._field == \"position\")"; // Esempio di query con filtro sul campo idActivity


            QueryApi queryApi = client.GetQueryApi();            
            List<FluxTable> tablePulseRate = await queryApi.QueryAsync(queryPulseRate, _organization);

            QueryApi queryApiPosition = client.GetQueryApi();
            List<FluxTable> tablePosition = await queryApi.QueryAsync(queryPosition, _organization);


            client.Dispose();//per chiudere connessione

            if (!tablePulseRate.Any())
                throw new Exception("questa attività non ha dati");

            if (!tablePosition.Any())
                throw new Exception("questa attività non ha dati");


            List<ActivityContent> activityContents = new List<ActivityContent>();
            List<ActivityContent> activityPositions = new List<ActivityContent>();


            FluxTable firstTablePulseRate = new FluxTable();
            firstTablePulseRate = tablePulseRate[0];
            //// Leggi i dati restituiti
            FluxTable firstTablePosition = new FluxTable();
            firstTablePosition = tablePosition[0];

            // Leggi le righe dei risultati
            List<FluxRecord> recordsPulseRate = firstTablePulseRate.Records;
            List<FluxRecord> recordsPosition = firstTablePosition.Records;

            int i = 0;
            foreach (FluxRecord record in recordsPulseRate)
            {
                // Leggi i campi e i valori del record
                Dictionary<string, object> values = record.Values;
                ActivityContent activityContent = new ActivityContent();

                //per trovare l'idActivity 
                string idActivityStr = values.First(x => x.Key == "idActivity").Value.ToString();
                Guid idActivitytoInsert = Guid.Parse(idActivityStr);
                activityContent.IdActivity = idActivitytoInsert;

                //per trovare il pulseRate
                int pulseRate = Convert.ToInt32(tablePulseRate[0].Records[i].GetValue());
                activityContent.PulseRate = pulseRate;

                //per trovare il position
                string position = tablePosition[0].Records[i].GetValue().ToString();
                activityContent.Position = position;


                string time = values.First(x => x.Key == "_time").Value.ToString();
                DateTime dateTime = Convert.ToDateTime(time);
                string strTime = dateTime.ToString("dd/MM/yyyy HH:mm:ss");
                DateTime timeToInsert = Convert.ToDateTime(strTime);
                activityContent.Time = timeToInsert;



                //aggiungo alla lista 
                activityContents.Add(activityContent);
                ++i;
            }
           
            List<ActivityContentDto> listDtos = new List<ActivityContentDto>();

            foreach(ActivityContent content in activityContents)
            {
                ActivityContentDto activityContentDto = new ActivityContentDto();
                _mapper.Map(content, activityContentDto);
                listDtos.Add(activityContentDto);
            }

            return listDtos;

        }

        public async Task<bool> Write(DataFromSimulator dataFromSimulator)
        {
            using var client = InfluxDBClientFactory.Create(_url, _token);//apri connessione a influxdb
                      
             
            var mem = new Mem { IdActivity = dataFromSimulator.IdActivity, Position = dataFromSimulator.GeoCoordinates, PulseRate = dataFromSimulator.PulseRate, Time = DateTime.UtcNow };
            using (var writeApi = client.GetWriteApi())
            {
                writeApi.WriteMeasurement(mem, WritePrecision.Ns, _bucket, _organization);
            }

            client.Dispose();//per chiudere connessione
            return true;
        }
         

    public List<ActivityContent> Read(decimal idActivity)
        {
            using var client = InfluxDBClientFactory.Create(_url, _token);//apri connessione a influxdb

            List<ActivityContent> list = new List<ActivityContent>();

            client.Dispose();//per chiudere connessione

            return list;
        }

        [Measurement("ActivityContent")]
        private class Mem
        {
            [Column("idActivity", IsTag = true)] public string IdActivity { get; set; }
            [Column("position")] public string? Position { get; set; }
            [Column("pulseRate")] public int? PulseRate { get; set; }            
            [Column(IsTimestamp = true)] public DateTime Time { get; set; }
        }

    }

   
}
