using Serilog.Events;

namespace Minesharp.Server.Plugins;

public class PluginConfiguration
{
    public string Name { get; init; }
    public string Assembly { get; init; }
    public LogEventLevel MinimumLevel { get; init; }
}