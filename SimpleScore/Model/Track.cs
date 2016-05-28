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
        int position;
        int length;

        public Track()
        {
            messages = new List<Message>();
            position = 0;
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
            List<Message> messageList = new List<Message>();
            foreach (Message message in messages)
            {
                messageList.Add(message);
            }
            return messageList.ToArray();
        }

        public Message[] Play(int clock)
        {
            List<Message> messageList = new List<Message>();
            while (position < messages.Count && messages[position].Time <= clock)
            {
                messageList.Add(messages[position]);
                position++;
            }
            return messageList.ToArray();
        }

        public void ChangePosition(int clock)
        {
            position = 0;
            while (position < messages.Count && messages[position].Time < clock)
            {
                position++;
            }
        }

        public Message[] Messages
        {
            get
            {
                return messages.ToArray();
            }
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
