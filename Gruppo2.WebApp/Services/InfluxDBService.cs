using Azure;
using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Core;
using System.Net.Sockets;

namespace Gruppo2.WebApp
{
    public class InfluxDBService
    {
        private readonly string _token;
        private readonly string _bucket;
        private readonly string _organization;

        public InfluxDBService(IConfiguration configuration)
        {
            _token = configuration.GetValue<string>("InfluxDB:Token");
            _bucket = configuration.GetValue<string>("InfluxDB:Bucket");
            _organization = configuration.GetValue<string>("InfluxDB:Organization");
        }

        public void Write(string record)
        {
            using var client = InfluxDBClientFactory.Create("http://localhost:8086", _token);


            string[] splits = record.Split(';');

            string idActivity = splits[0];
            string position = splits[1];
            int pulseRate = Convert.ToInt32(splits[2]);   

            var mem = new Mem { IdActivity = idActivity, Position = position, PulseRate = pulseRate, Time = DateTime.UtcNow };
            using (var writeApi = client.GetWriteApi())
            {
                writeApi.WriteMeasurement(mem, WritePrecision.Ns, _bucket, _organization);
            }
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
