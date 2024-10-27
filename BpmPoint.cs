#if !Sony
using ScriptPortal.Vegas;
#else
using Sony.Vegas;
#endif

using System.Collections.Generic;

namespace VariableBpm
{
    public class BpmPoint
    {
        public Timecode Position;
        public double Bpm;
        public string Offset;
        public bool IsZeroPoint;
        public Timecode StartTime;
        public uint Beats;
        public Marker Marker;

        public BpmPoint(Timecode position, double bpm, string offset = null, bool isZeroPoint = false, uint beats = 4, Marker marker = null)
        {
            Position = position;
            Bpm = bpm;
            Offset = offset;
            IsZeroPoint = isZeroPoint;
            Beats = beats;
            Marker = marker;
        }

        public static BpmPoint CopyFrom(BpmPoint p)
        {
            return new BpmPoint(p.Position, p.Bpm, p.Offset, p.IsZeroPoint, p.Beats, p.Marker);
        }

        public static bool operator ==(BpmPoint l, BpmPoint r)
        {
            return l?.Position == r?.Position && l?.Bpm == r?.Bpm && (l?.IsZeroPoint == r?.IsZeroPoint) && l?.Offset == r?.Offset && l?.StartTime == r?.StartTime && l?.Beats == r?.Beats;
        }
        public static bool operator !=(BpmPoint l, BpmPoint r)
        {
            return l?.Position != r?.Position || l?.Bpm != r?.Bpm || (l?.IsZeroPoint != r?.IsZeroPoint) || l?.Offset != r?.Offset || l?.StartTime != r?.StartTime || l?.Beats != r?.Beats;
        }
    }

    public class BpmPointList : List<BpmPoint>
    {
        public MarkerInfoList MarkerInfos
        {
            get
            {
                return MarkerInfoList.GetFrom(this);
            }
        }

        public List<Marker> Markers
        {
            get
            {
                List<Marker> markers = new List<Marker>();
                foreach (BpmPoint p in this)
                {
                    if (p.Marker != null && p.Marker.IsValid())
                    {
                        markers.Add(p.Marker);
                    }
                }
                return markers;
            }
        }

        public void SetStartTime()
        {
            int zeroIndex = 0;
            for (int i = 0; i < Count; i++)
            {
                if (this[i].Bpm == 0)
                {
                    this[i].StartTime = this[i - 1].StartTime;
                    continue;
                }

                if (this[i].IsZeroPoint)
                {
                    zeroIndex = i;
                }

                this[i].StartTime = new Timecode(0) - this[zeroIndex].Position;

                for (int j = zeroIndex; j < i; j++)
                {
                    this[i].StartTime += new Timecode((this[j + 1].Position - this[j].Position).ToMilliseconds() * (this[j].Bpm / this[i].Bpm - 1));
                }
            }
        }

        public bool IsTheSame(BpmPointList p)
        {
            if (this == null || p == null || Count != p.Count)
            {
                return false;
            }

            for (int i = 0; i < Count; i++)
            {
                if (this[i] != p[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}