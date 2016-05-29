using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleScore.Model
{
    public class Track
    {
        List<Message> messages;
        int length;

        public Track()
        {
            messages = new List<Message>();
            length = 0;
        }

        public void AddMessage(Message message)
        {
            messages.Add(message);
            if (message.Time > length)
                length = message.Time;
        }

        //取出音軌全部message
        public Message[] GetMessages()
        {
            return messages.ToArray();
        }

        public int Length
        {
            get
            {
                return length;
            }
        }
    }
}
