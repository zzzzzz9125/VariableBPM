using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VariableBpm
{
    public class MidiBpmPoint
    {
        public long Time;
        public double Bpm;
        public int Beats;

        public MidiBpmPoint(long time, double bpm, int beats = 0)
        {
            Time = time;
            Bpm = bpm;
            Beats = beats;
        }
    }
}
