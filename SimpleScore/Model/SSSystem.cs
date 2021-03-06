﻿using System;
using System.Windows.Threading;

namespace SimpleScore.Model
{
    public class SSSystem : IDisposable
    {
        public delegate void PlayStatusChangedEventHandler();
        public event PlayStatusChangedEventHandler playStatusChanged;
        public delegate void PlayProgressChangedEventHandler();
        public event PlayProgressChangedEventHandler playProgressChanged;
        public delegate void LoadCompleteEventHandler();
        public event LoadCompleteEventHandler loadComplete;
        public delegate void PlayerChangeEventHandler();
        public event PlayerChangeEventHandler playerChanged;

        public enum LoadStyle { Single = 0, Loop = 1, Random = 2, Sequential = 3 };
        public enum PlayerType { MidiDevice = 0, Was = 1 };


        Player player;
        Score score;
        Model.File file;
        MidiParser parser;
        LoadStyle loadStyle;
        private Dispatcher dispatcher;

        public SSSystem()
        {
            loadStyle = LoadStyle.Single;
            dispatcher = Dispatcher.CurrentDispatcher;
            file = new File();
            parser = new MidiParser();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            Stop();
            if (disposing) player.Dispose();
        }

        private void Parse(byte[] data)
        {
            try
            {
                score = parser.Parse(data);
                score.Name = file.CurrentFileName;
                NotifyLoadComplete();
            }
            catch (Exception e)
            {
                Console.WriteLine("Parser error !\n" + e.Message);
            }
        }

        private void LoadToPlayer()
        {
            if (score != null) player.LoadScore(score.GetMessage(), score.Semiquaver, score.Length);
        }

        public void Load(string path)
        {
            byte[] data = file.Load(path);
            Parse(data);
        }

        public void LoadSequential(int offset)
        {
            byte[] data = file.SequentialLoad(offset);
            Parse(data);
        }

        public void RandomLoad()
        {
            byte[] data = file.RandomLoad();
            Parse(data);
        }

        public void ChangeLoadStyle(int style)
        {
            loadStyle = (LoadStyle)style;
        }

        public void ChangePlayerType(int type)
        {
            PlayerType t = (PlayerType)type;

            if (player != null)
            {
                player.playStatusChanged -= NotifyPlayStatusChanged;
                player.endPlay -= EndPlay;
                player.playProgressChanged -= NotifyPlayProgressChanged;
                player.Dispose();
            }

            switch (t)
            {
                case (PlayerType.MidiDevice):
                    {
                        player = new MidiDevicePlayer();
                        break;
                    }
                case (PlayerType.Was):
                    {
                        player = new WasPlayer();
                        break;
                    }
            }
            //必須先新增事件，不然load完後，UI不會即時更新
            player.playStatusChanged += NotifyPlayStatusChanged;
            player.endPlay += EndPlay;
            player.playProgressChanged += NotifyPlayProgressChanged;
            //isplay再變更的時候會有事件回到view，切換player時，可能會從播放變成暫停，但是在建構子建構時，沒有辦發透過事件呼叫
            player.IsPlay = IsPlay;
            LoadToPlayer();
            NotifyPlayerChanged();
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
            player.ChangeTime(percent);
        }

        public void Mute(int channel, bool isMute)
        {
            player.Mute(channel, isMute);
        }

        public Message[] GetMessageByTrack(int trackNumber)
        {
            return score.GetTrack(trackNumber);
        }

        private void EndPlay()
        {
            if (loadStyle == LoadStyle.Single)
            { /*不做任何事*/}
            else if (loadStyle == LoadStyle.Loop)
            {
                player.PlayOrPause();   //繼續撥放
            }
            else if (loadStyle == LoadStyle.Random)
            {
                RandomLoad();   //load完會自動呼叫loadComplete
            }
            else if (loadStyle == LoadStyle.Sequential)
            {
                LoadSequential(1);  //load完會自動呼叫loadComplete
            }
        }

        public float Volumn
        {
            get
            {
                return player.Volumn;
            }
            set
            {
                player.Volumn = value;
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
                dispatcher.Invoke(playStatusChanged);
        }

        private void NotifyPlayProgressChanged()
        {
            if (playProgressChanged != null)
                dispatcher.Invoke(playProgressChanged);
        }

        private void NotifyPlayerChanged()
        {
            if (playerChanged != null)
                dispatcher.Invoke(playerChanged);
        }

        private void NotifyLoadComplete()
        {
            LoadToPlayer();
            if (loadComplete != null)
                dispatcher.Invoke(loadComplete);
        }
    }
}
