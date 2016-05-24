using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleScore.Model
{
    public class TimeData
    {
        int clock;
        float data;

        public TimeData(int clock, float data)
        {
            this.clock = clock;
            this.data = data;
        }

        public int Clock
        {
            get
            {
                return clock;
            }
            set
            {
                clock = value;
            }
        }

        public float Data
        {
            get
            {
                return data;
            }
            set
            {
                data = value;
            }
        }
    }
}