using System;
using System.Collections.Generic;

namespace SimpleScore.Model
{
    public class Score
    {
        private List<Track> trackList;
        private string name;
        float semiquaver;
        int tick;
        int length;

        public Score()
        {
            trackList = new List<Track>();
            //If not specified, the default tempo is 120 beats/minute, which is equivalent to tttttt=500000
            name = string.Empty;
            Tick = 0;
            length = 0;
        }

        public void Clear()
        {
            trackList.Clear();
        }

        public void CreateTrack()
        {
            trackList.Add(new Track());
        }

        public void CreateMessage(int trackNumber, Message message)
        {
            if (trackNumber > trackList.Count - 1) throw new Exception("音軌超出範圍");
            trackList[trackNumber].AddMessage(message);

            if (trackList[trackNumber].Length > length)
                length = trackList[trackNumber].Length;
        }

        public Message[] GetTrack(int trackNumber)
        {
            return trackList[trackNumber].GetMessages();
        }

        public Message[] GetMessage()
        {
            Message[] messages;
            int index;
            List<Message> messageList = new List<Message>(trackList[0].GetMessages());
            //除了按照time排序外，還要按照track的順序排，所以用merge的方式來排序，因為track內的messages本來就是SortedList，所以只需要merge，不需要拆
            for (int i = 1; i < trackList.Count; i++)
            {
                messages = trackList[i].GetMessages();
                index = 0;
                foreach (Message message in messages)
                {
                    while (index < messageList.Count && messageList[index].Time <= message.Time) index++;
                    messageList.Insert(index, message);
                }
            }
            return messageList.ToArray();
        }

        public int TrackCount
        {
            get
            {
                return trackList.Count;
            }
        }

        public int Length
        {
            get
            {
                return length;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }

        public int Tick
        {
            get
            {
                return tick;
            }
            set
            {
                tick = value;
                semiquaver = tick / 16;
            }
        }

        public float Semiquaver
        {
            get
            {
                return semiquaver;
            }
        }
    }
}
