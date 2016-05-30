using System.Windows.Threading;

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
            player = new WasPlayer();
            loadStyle = LoadStyle.Single;
            dispatcher = Dispatcher.CurrentDispatcher;
            file = new File();
            player.playStatusChanged += NotifyPlayStatusChanged;
            file.loadComplete += NotifyLoadComplete;
            player.endPlay += EndPlay;
            player.playProgressChanged += NotifyPlayProgressChanged;
        }

        private void CreateScore()
        {
            if (score != null) score.Dispose();
            score = new Score();
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
            player.ChangeTime(percent);
        }

        private void EndPlay()
        {
            if (loadStyle == LoadStyle.Single)
            {
                //不做任何事
            }
            else if (loadStyle == LoadStyle.Loop)
            {
                //繼續撥放
                player.PlayOrPause();
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

        public Message[] GetMessageByTrack(int trackNumber)
        {
            return score.GetTrack(trackNumber);
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
                return player.SampleTime;
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
                return player.ProgressPercentage;
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
