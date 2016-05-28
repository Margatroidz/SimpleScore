using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Diagnostics;
using NAudio.Midi;

namespace SimpleScore.Model
{
    public class Player
    {
        public delegate void PlayStatusChangedEventHandler();
        public event PlayStatusChangedEventHandler playStatusChanged;
        public event PlayStatusChangedEventHandler endPlay;

        Score score;
        Thread playingThread = null;
        bool isPlay = false;
        bool autoPlay = false;
        private EventWaitHandle playingEvent; //用static的話，pricvate object抓不到

        public Player()
        {
            playingEvent = new EventWaitHandle(false, EventResetMode.AutoReset);
            score = null;
        }

        public void Stop()
        {
            if (IsPlay) Pause();
            if (score != null) score.ChangeClock(0f);
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
            if (playingThread == null && score != null) CreateThread();
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

        public void LoadScore(Score s)
        {
            if (playingThread != null)
            {
                playingThread.Abort();
                playingThread = null;
            }
            //先把score弄進來，再stop，因為load新的score時，我會把舊的score給dispose掉，導致判斷不會改變score位置
            score = s;
            Stop();
            if (autoPlay) Play();
        }

        public void CreateThread()
        {
            if (playingThread == null)
            {
                ThreadStart ts = new ThreadStart(PlayThread);
                playingThread = new Thread(ts);
                playingThread.Priority = ThreadPriority.Highest;
                playingThread.IsBackground = true;
                playingThread.Start();
            }
        }

        public void PlayThread()
        {
            Message[] messages;
            int sleep = 0;
            int delay = 0;
            Stopwatch sw = new Stopwatch();
            while (!score.IsEnd)
            {
                sw.Restart();
                playingEvent.WaitOne();
                playingEvent.Set();
                messages = score.Play();
                if (messages.Count() > 0)
                {
                    ProcessMessage(messages);
                }
                score.IncreaseClock();
                sleep = Convert.ToInt32(score.BeatPerMilliSecond / 16);
                delay = (int)sw.ElapsedMilliseconds;
                if (sleep > delay) Thread.Sleep(sleep - delay);
            }
            EndPlay();
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
    }
}