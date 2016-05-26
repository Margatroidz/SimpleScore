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
        int command;
        int channel;
        int data1;
        int data2;

        public Voice(int time, int status, int data1, int data2)
        {
            base.Time = time;
            this.status = status;
            this.command = status / 16;
            this.channel = status % 16;
            this.data1 = data1;
            this.data2 = data2;
        }

        public int Command
        {
            get
            {
                return command;
            }
        }

        public int Channel
        {
            get
            {
                return channel;
            }
        }


        public int Status
        {
            get
            {
                return status;
            }
            set
            {
                status = value;
                command = status / 16;
                channel = status % 16;
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
