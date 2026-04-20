namespace GovCon.RagPlatform.Core.Monitoring;

public interface IMetrics
{
    void Increment(string counter, params (string Key, string Value)[] tags);
    void Observe(string metric, double value, params (string Key, string Value)[] tags);
}

public sealed class NoOpMetrics : IMetrics
{
    public void Increment(string counter, params (string Key, string Value)[] tags) { }
    public void Observe(string metric, double value, params (string Key, string Value)[] tags) { }
}
