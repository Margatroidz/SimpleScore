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
        //C:\Windows\System32\drivers\gm.dls 內建MIDI音效
        public delegate void PlayStatusChangedEventHandler();
        public event PlayStatusChangedEventHandler playStatusChanged;
        public event PlayStatusChangedEventHandler endPlay;
        MidiOut midiOut;

        Score score;
        Thread playingThread = null;
        bool isPlay = false;
        bool autoPlay = false;
        private EventWaitHandle playingEvent; //用static的話，pricvate object抓不到

        public Player()
        {
            //midiOut = new MidiOut(0);
            playingEvent = new EventWaitHandle(false, EventResetMode.AutoReset);
            score = null;
        }

        public virtual void Reset()
        {
            //midiOut.Reset();
        }

        public void Stop()
        {
            if (IsPlay) Pause();
            if (score != null) score.ChangeClock(0f);
            Reset();
        }

        public void Pause()
        {
            playingEvent.WaitOne();
            //AllNoteOff();
            IsPlay = false;
        }

        public void Play()
        {
            playingEvent.Set();
            if (playingThread == null && score != null) CreateThread();
            IsPlay = true;
        }

        public void PlayOrPause()
        {
            if (IsPlay) Pause();
            else Play();
        }

        public virtual void SetVolumn(float volumn)
        {
        }

        public virtual void LoadBank(string path)
        {
        }

        public void LoadScore(Score s)
        {
            Reset();
            if (playingThread != null)
            {
                playingThread.Abort();
                playingThread = null;
            }
            score = s;
            //Stop();
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
            Voice[] noteList;
            //CreateMidiDevice();
            int sleep = 0;
            int delay = 0;
            Stopwatch sw = new Stopwatch();
            while (!score.IsEnd)
            {
                sw.Restart();
                playingEvent.WaitOne();
                playingEvent.Set();
                noteList = score.Play();
                if (noteList.Count() > 0)
                {
                    /*foreach (Voice note in noteList)
                    {
                        //Console.Write("Clock : " + score.Clock + " , ");
                        PlayNote(note.Status, note.Data1, note.Data2);
                        else
                        {
                            if (singlenote[2] == 0x51) beat = singlenote[3];
                            //else if (singleNote[2] == 0x58) beatSpeed = (double)singleNote[3] / (double)100000;
                        }
                    }*/
                    PlayVoices(noteList);
                }
                score.IncreaseClock();
                sleep = Convert.ToInt32(score.BeatPerMilliSecond / 16);
                delay = (int)sw.ElapsedMilliseconds;
                if (sleep > delay) Thread.Sleep(sleep - delay);
            }
            EndPlay();
        }

        public virtual void PlayVoices(Voice[] voices)
        {
        }

        private void EndPlay()
        {
            Stop();
            Reset();
            NotifyEndPlay();
        }

        public virtual void PlayNote(int status, int data1, int data2)
        {
            //midiOut.Send(data2 << 16 | data1 << 8 | status);
            //midiOutmidiOutShortMsg(midiOut, data2 << 16 | data1 << 8 | status);
            //Console.WriteLine(Convert.ToString(code, 16) + "\t" + Convert.ToString(scale, 16) + "\t" + Convert.ToString(volumn, 16));
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

/*[DllImport("winmm.dll")]
public static extern int midiStreamOpen(ref int phms, ref int puDeviceID, int cMidi, int dwCallback, int dwInstance, int fdwOpen);
[DllImport("winmm.dll")]
public static extern int midiStreamClose(int hms);
[DllImport("winmm.dll")]
public static extern int midiStreamOut(int hms, ref LPMIDIHDR pmh, int cbmh);
[DllImport("winmm.dll")]
public static extern int midiStreamPause(int hms);
[DllImport("winmm.dll")]
public static extern int midiStreamStop(int hms);
[DllImport("winmm.dll")]
public static extern int midiStreamRestart(int hms);
[DllImport("winmm.dll")]
public extern static int midiStreamPosition(int hms, ref LPMMTIME pmh, int cbmmt);*/