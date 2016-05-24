using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleScore.Model
{
    public class Track
    {
        List<Message> messageList;
        int position;
        int length;

        public Track()
        {
            messageList = new List<Message>();
            position = 0;
            length = 0;
        }

        public void CreateNote(Message message)
        {
            messageList.Add(message);
            if (message.Time > length)
            {
                length = message.Time;
            }
        }

        //取出音軌全部note
        public Voice[] GetNote()
        {
            List<Voice> noteList = new List<Voice>();
            foreach (Message i in messageList)
            {
                if (i.GetType() == typeof(Voice))
                {
                    noteList.Add((Voice)i);
                }
            }
            return noteList.ToArray();
        }

        public Voice[] Play(int clock)
        {
            List<Voice> noteList = new List<Voice>();
            while (position < messageList.Count && messageList[position].Time <= clock)
            {
                if (messageList[position].GetType() == typeof(Voice))
                {
                    noteList.Add((Voice)messageList[position]);
                }
                position++;
            }
            return noteList.ToArray();
        }

        public void ChangePosition(int clock)
        {
            position = 0;
            while (position < messageList.Count && messageList[position].Time < clock)
            {
                position++;
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
