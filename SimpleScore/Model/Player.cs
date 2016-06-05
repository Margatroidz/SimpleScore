using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleScore.Model
{
    public class Player : IDisposable
    {
        public delegate void PlayProgressChangedEventHandler();
        public delegate void PlayStatusChangedEventHandler();
        public event PlayProgressChangedEventHandler playProgressChanged;
        public event PlayStatusChangedEventHandler playStatusChanged;
        public event PlayStatusChangedEventHandler endPlay;

        CancellationTokenSource cancellationTokenSource;
        CancellationToken cancellationToken;
        Task playingTask;
        Message[] messages;
        bool isPlay;
        bool autoPlay;
        int index;
        int sampleTime;
        float semiquaver;
        float beatPerMilliSecond;
        int length;
        int progressChangedCount;
        private EventWaitHandle playingEvent; //用static的話，pricvate object抓不到

        public Player()
        {
            playingEvent = new EventWaitHandle(false, EventResetMode.AutoReset);
            playingTask = null;
            messages = null;
            isPlay = false;
            autoPlay = false;
            index = 0;
            sampleTime = 0;
            semiquaver = 0f;
            beatPerMilliSecond = 500f;
            length = 0;
            progressChangedCount = 0;
        }

        public virtual void Dispose()
        {
            if (playingTask != null)
            {
                if (!playingTask.IsCompleted) cancellationTokenSource.Cancel();
                //delay一下子，給task結束的時間
                Thread.Sleep(100);
                playingTask.Dispose();
            }
            playingEvent.Dispose();
            GC.SuppressFinalize(this);
        }

        public void Stop()
        {
            if (IsPlay) Pause();
            SampleTime = 0;
            index = 0;
            Reset();
        }

        public void PlayOrPause()
        {
            if (IsPlay) Pause();
            else Play();
        }

        public void Pause()
        {
            playingEvent.WaitOne();
            IsPlay = false;
        }

        public virtual void Play()
        {
            playingEvent.Set();
            if (playingTask == null || playingTask.IsCompleted) CreateTask();
            IsPlay = true;
        }

        public virtual void Reset()
        {
        }

        public virtual void ProcessMessage(Message[] message)
        {
        }

        public virtual void SetVolumn(float volumn)
        {
        }

        public virtual void LoadBank(string path)
        {
        }


        public virtual void PlayNote(Message message)
        {
        }

        public void ChangeTime(float percent)
        {
            sampleTime = (int)((float)length * percent);
            NotifyPlayProgressChanged();
            index = 0;
            beatPerMilliSecond = 500f;
            Reset();
            FillMessage();
        }

        public void LoadScore(Score score)
        {
            if (playingTask != null && !playingTask.IsCompleted) cancellationTokenSource.Cancel();
            messages = score.GetMessage();
            semiquaver = score.Semiquaver;
            length = score.Length;
            Stop();
            if (autoPlay) Play();
        }

        private void CreateTask()
        {
            if (playingTask == null || playingTask.IsCompleted)
            {
                cancellationTokenSource = new CancellationTokenSource();
                cancellationToken = cancellationTokenSource.Token;
                playingTask = new Task(PlayTask, cancellationToken);
                playingTask.Start();
            }
        }

        private void PlayTask()
        {
            int sleep = 0;
            int delay = 0;
            Stopwatch sw = new Stopwatch();
            beatPerMilliSecond = 500f;
            while (sampleTime <= length && !cancellationToken.IsCancellationRequested)
            {
                sw.Restart();
                playingEvent.WaitOne();
                playingEvent.Set();
                ProcessMessage(FillMessage());
                SampleTime += (int)semiquaver;
                sleep = Convert.ToInt32(beatPerMilliSecond / 16f);
                delay = (int)sw.ElapsedMilliseconds;
                if (sleep > delay) Thread.Sleep(sleep - delay);
            }
            if (!cancellationToken.IsCancellationRequested) EndPlay();
        }

        private Message[] FillMessage()
        {
            List<Message> result = new List<Message>();
            while (index < messages.Length && messages[index].Time <= sampleTime)
            {
                if (messages[index].MessageType == Message.Type.Voice)
                    result.Add(messages[index]);
                else
                    MetaEvent(messages[index]);
                index++;
            }
            return result.ToArray();
        }

        private void MetaEvent(Message message)
        {
            if (message.MessageType != Message.Type.Meta) throw new Exception();
            switch (message.Data1)
            {
                case 0x51:
                    {
                        beatPerMilliSecond = (int)message.Data2;
                        break;
                    }
            }
        }

        private void EndPlay()
        {
            Stop();
            NotifyEndPlay();
        }

        public bool IsPlay
        {
            get
            {
                return isPlay;
            }
            set
            {
                isPlay = value;
                NotifyPlayStatusChanged();
            }
        }

        public bool AutoPlay
        {
            get
            {
                return autoPlay;
            }
            set
            {
                autoPlay = value;
            }
        }

        public int SampleTime
        {
            get
            {
                return sampleTime;
            }
            private set
            {
                sampleTime = value;
                progressChangedCount++;
                if (progressChangedCount >= 4 || sampleTime == 0) NotifyPlayProgressChanged();
            }
        }

        public double ProgressPercentage
        {
            get
            {
                return (double)sampleTime / (double)length;
            }
        }

        private void NotifyPlayStatusChanged()
        {
            if (playStatusChanged != null)
            {
                playStatusChanged();
            }
        }

        private void NotifyEndPlay()
        {
            if (endPlay != null)
            {
                endPlay();
            }
        }

        private void NotifyPlayProgressChanged()
        {
            progressChangedCount = 0;
            if (playProgressChanged != null && messages != null)
            {
                playProgressChanged();
            }
        }
    }
}