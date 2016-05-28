using System;
using System.Collections.Generic;

namespace SimpleScore.Model
{
    public class Score : IDisposable
    {
        public delegate void PlayProgressChangedEventHandler();
        public event PlayProgressChangedEventHandler playProgressChanged;
        int progressChangedCount;

        private List<Track> trackList;
        private List<Message> cacheList;
        private List<TimeData> beatList;
        private int currentBeat;
        private float beatPerMilliSecond;
        private string name;
        float semiquaver;
        int tick;
        int clock;
        int length;

        public Score()
        {
            progressChangedCount = 0;
            trackList = new List<Track>();
            //If not specified, the default tempo is 120 beats/minute, which is equivalent to tttttt=500000
            beatList = new List<TimeData>();
            beatList.Add(new TimeData(0, 0.5f));
            CurrentBeat = 0;
            name = string.Empty;
            Tick = 0;
            clock = 0;
            length = 0;
        }

        public void Dispose()
        {
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

            if (!(message.MessageType == Message.Type.Voice && (message.Command == 8 || message.Command == 9)))
                cacheList.Add(message);
            if (trackList[trackNumber].Length > length)
                length = trackList[trackNumber].Length;
        }

        public Message[] GetTrack(int trackNumber)
        {
            return trackList[trackNumber].GetMessages();
        }

        public Message[] Play()
        {
            List<Message> messageList = new List<Message>();
            foreach (Track track in trackList)
            {
                messageList.AddRange(track.Play(clock));
            }
            return messageList.ToArray();
        }

        public void AddBeatTime(int clock, float beatPerMilliSecond)
        {
            beatList.Add(new TimeData(clock, beatPerMilliSecond));
        }

        public void IncreaseClock()
        {
            clock += (int)semiquaver;
            UpdateBeat();
            progressChangedCount++;
            if (progressChangedCount >= 4) NotifyPlayProgressChanged();
        }

        public void ChangeClock(float percent)
        {
            clock = (int)((float)length * percent);
            foreach (Track track in trackList)
            {
                track.ChangePosition(Clock);
            }
            CurrentBeat = 0;
            UpdateBeat();
            NotifyPlayProgressChanged();
        }

        private void UpdateBeat()
        {
            //擷取function時遇到bug，要用迴圈，而不能只用if
            while (currentBeat < beatList.Count - 1 && beatList[currentBeat + 1].Clock < clock)
            {
                CurrentBeat++;
            }
        }

        private int CurrentBeat
        {
            get
            {
                return currentBeat;
            }
            set
            {
                currentBeat = value;
                beatPerMilliSecond = beatList[currentBeat].Data;
            }
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

        public int Clock
        {
            get
            {
                return clock;
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

        public float BeatPerMilliSecond
        {
            get
            {
                return beatPerMilliSecond;
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

        public bool IsEnd
        {
            get
            {
                return clock <= length ? false : true;
            }
        }

        public double ProgressPercentage
        {
            get
            {
                return (double)clock / (double)length;
            }
        }

        private void NotifyPlayProgressChanged()
        {
            progressChangedCount = 0;
            if (playProgressChanged != null)
            {
                playProgressChanged();
            }
        }
    }
}
