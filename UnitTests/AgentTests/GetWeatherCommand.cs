using AgentLibrary;

namespace UnitTests;

public class GetWeatherCommand : ICommand
{
    private WeatherStation _weatherStation;

    public GetWeatherCommand(WeatherStation weatherStation)
    {
        _weatherStation = weatherStation;
    }
    
    [ToolInfo("GetWeather", "A tool to get the weather in a city.")]
    [ToolParameter("city", "string", "The city to get the weather for.")]
    public Task<string> Execute(ToolCallContext toolCallContext)
    {
        return Task.FromResult(_weatherStation.GetWeather(toolCallContext.GetArgument("city")));
    }
}