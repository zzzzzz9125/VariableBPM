// a simple class for serialization and deserialization
public class MarkerInfo
{
    public double Seconds = 0;
    public string Label = null;
    public MarkerInfo(double seconds, string label = null)
    {
        Seconds = seconds;
        Label = label;
    }
}