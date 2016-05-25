using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Threading;
using System.Threading.Tasks;

namespace SimpleScore.Model
{
    public class SSSystem
    {
        public delegate void PlayStatusChangedEventHandler();
        public event PlayStatusChangedEventHandler playStatusChanged;
        public delegate void PlayProgressChangedEventHandler();
        public event PlayProgressChangedEventHandler playProgressChanged;
        public delegate void LoadCompleteEventHandler();
        public event LoadCompleteEventHandler loadComplete;

        public enum LoadStyle { Single = 0, Loop = 1, Random = 2, Sequential = 3 };
        Player player;
        Score score;
        Model.File file;
        LoadStyle loadStyle;
        private Dispatcher dispatcher;

        public SSSystem()
        {
            player = new MidiDevicePlayer();
            loadStyle = LoadStyle.Single;
            dispatcher = Dispatcher.CurrentDispatcher;
            file = new Model.File();
            player.playStatusChanged += NotifyPlayStatusChanged;
            file.loadComplete += NotifyLoadComplete;
            player.endPlay += EndPlay;
        }

        private void CreateScore()
        {
            Stop();
            if(score != null) score.Dispose();
            if (score != null) score.playProgressChanged -= NotifyPlayProgressChanged;
            score = new Score();
            score.playProgressChanged += NotifyPlayProgressChanged;
        }

        public void Load(string path)
        {
            CreateScore();
            file.Load(path, score);
        }

        public void LoadSequential(int offset)
        {
            CreateScore();
            file.SequentialLoad(score, offset);
        }

        public void ChangeLoadStyle(int style)
        {
            loadStyle = (LoadStyle)style;
        }

        public Voice[] GetVoiceByTrack(int trackNumber)
        {
            return score.GetTrack(trackNumber);
        }

        public void Stop()
        {
            player.Stop();
        }

        public void PlayOrPause()
        {
            player.PlayOrPause();
        }

        public void ChangeClock(float percent)
        {
            //player.AllNoteOff();
            score.ChangeClock(percent);
        }

        private void EndPlay()
        {
            if (loadStyle == LoadStyle.Single)
            {
                //不做任何事
            }
            else if (loadStyle == LoadStyle.Loop)
            {
                //呼叫loadComplete但不載新的譜
                NotifyLoadComplete();
            }
            else if (loadStyle == LoadStyle.Random)
            {
                CreateScore();
                //load完會自動呼叫loadComplete
                file.RandomLoad(score);
            }
            else if (loadStyle == LoadStyle.Sequential)
            {
                //load完會自動呼叫loadComplete
                LoadSequential(1);
            }
        }

        public string ScoreName
        {
            get
            {
                return score.Name;
            }
        }

        public int ScoreLength
        {
            get
            {
                return score.Length;
            }
        }

        public float Semiquaver
        {
            get
            {
                return score.Semiquaver;
            }
        }

        public int Clock
        {
            get
            {
                return score.Clock;
            }
        }

        public int TrackCount
        {
            get
            {
                return score.TrackCount;
            }
        }

        public bool IsPlay
        {
            get
            {
                return player.IsPlay;
            }
        }

        public bool AutoPlay
        {
            get
            {
                return player.AutoPlay;
            }
            set
            {
                player.AutoPlay = value;
            }
        }

        public double ProgressPercentage
        {
            get
            {
                return score.ProgressPercentage;
            }
        }

        private void NotifyPlayStatusChanged()
        {
            if (playStatusChanged != null)
            {
                dispatcher.Invoke(playStatusChanged);
            }
        }

        private void NotifyPlayProgressChanged()
        {
            if (playProgressChanged != null)
            {
                dispatcher.Invoke(playProgressChanged);
            }
        }

        private void NotifyLoadComplete()
        {
            player.LoadScore(score);
            if (loadComplete != null)
            {
                dispatcher.Invoke(loadComplete);
            }
        }
    }
}
