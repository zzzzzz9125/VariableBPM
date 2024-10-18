#if !Sony
using ScriptPortal.Vegas;
#else
using Sony.Vegas;
#endif

using Newtonsoft.Json;
using System.Collections.Generic;
using VariableBpm;

// a simple class for serialization and deserialization
[JsonObject(MemberSerialization.OptOut)]
public class MarkerInfo
{
    public double Seconds
    {
        get
        {
            return Position.ToMilliseconds() * 1000;
        }
        set
        {
            Position = Timecode.FromSeconds(value);
        }
    }
    public string Label = null;


    [JsonIgnore]
    public Timecode Position = new Timecode(0);

    public MarkerInfo(Timecode position, string label = null)
    {
        Position = position;
        Label = label;
    }

    public MarkerInfo(double seconds, string label = null)
    {
        Seconds = seconds;
        Label = label;
    }

    public static MarkerInfo GetFrom(Marker m)
    {
        return new MarkerInfo(m.Position, m.Label);
    }

    public static MarkerInfo GetFrom(BpmPoint p)
    {
        return p.Marker != null ? GetFrom(p.Marker) : null;
    }
}

public class MarkerInfoList : List<MarkerInfo>
{
    public static MarkerInfoList GetFrom(BaseMarkerList<Marker> markers)
    {
        MarkerInfoList markerInfos = new MarkerInfoList();
        foreach (Marker m in markers)
        {
            markerInfos.Add(MarkerInfo.GetFrom(m));
        }
        return markerInfos;
    }

    public static MarkerInfoList GetFrom(BpmPointList list)
    {
        MarkerInfoList markerInfos = new MarkerInfoList();
        foreach (BpmPoint p in list)
        {
            MarkerInfo markerInfo = MarkerInfo.GetFrom(p);
            if (markerInfo != null)
            {
                markerInfos.Add(markerInfo);
            }
        }
        return markerInfos;
    }
}