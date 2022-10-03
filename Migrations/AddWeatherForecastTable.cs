using FluentMigrator;

namespace CastleSample.Migrations;

[Migration(202210031346)]
public class AddWeatherForecastTable : Migration
{
    public override void Up()
    {
        Create.Table("weather_forecast")
            .WithColumn("id").AsInt64().PrimaryKey().Identity()
            .WithColumn("weather_date").AsDateTime()
            .WithColumn("temperature_c").AsInt32()
            .WithColumn("summary").AsString();
    }

    public override void Down()
    {
        Delete.Table("weather_forecast");
    }
}