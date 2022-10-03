using System;
using NHibernate.Mapping.Attributes;

namespace CastleSample.Models
{
    [Class(Table = "weather_forecast")]
    public class WeatherForecast
    {
        [Id(Name = "Id", Column = "id", Generator = "trigger-identity")]
        public virtual long Id { get; set; }
        
        [Property(Column = "weather_date", Type = "datetime")]
        public virtual DateTime Date { get; set; }

        [Property(Column = "temperature_c")]
        public virtual int TemperatureC { get; set; }

        public virtual int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        [Property(Column = "summary", Type = "string")]
        public virtual string Summary { get; set; }
    }
}
