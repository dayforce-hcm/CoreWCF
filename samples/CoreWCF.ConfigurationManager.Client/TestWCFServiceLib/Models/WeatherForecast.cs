using System.Runtime.Serialization;

namespace TestWCFServiceLib.Models
{
    [DataContract]
    public partial class WeatherForecast
    {
        [DataMember]
        public string Date { get; set; }

        [DataMember]
        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        [DataMember]
        public string Summary { get; set; }
    }
}
