using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleScore.Model
{
    public class Voice : Message
    {
        int status;
        int data1;
        int data2;

        public Voice(int time, int status, int data1, int data2)
        {
            base.Time = time;
            this.status = status;
            this.data1 = data1;
            this.data2 = data2;
        }

        public int Status
        {
            get
            {
                return status;
            }
        }

        public int Data1
        {
            get
            {
                return data1;
            }
        }

        public int Data2
        {
            get
            {
                return data2;
            }
        }
    }
}
