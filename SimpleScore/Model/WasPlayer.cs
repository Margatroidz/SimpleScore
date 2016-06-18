using AudioSynthesis.Bank.Components;
using AudioSynthesis.Synthesis;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleScore.Model
{
    class WasPlayer : Player
    {
        private Synthesizer synthesizer;
        private PlayerWaveProvider playerWaveProvider;
        private WasapiOut wasapi_out;

        public WasPlayer()
            : base()
        {
            Synthesizer.InterpolationMode = (InterpolationEnum)1;
            synthesizer = new Synthesizer(44100, 2, 441, 3, 100);
            playerWaveProvider = new PlayerWaveProvider(synthesizer);
            //50基本上是最小了，在小會出問題   //聲音都會卡卡的，沒辦法控制    //5/25發現directSoundOut並不是最好的聲音輸出，可以的話改用ASIO
            //5/26使用wsapi，目前效果不錯
            //NAudio.CoreAudioApi.AudioClientShareMode.Exclusive 可能會出現可怕的問題，最好不要用
            wasapi_out = new WasapiOut(NAudio.CoreAudioApi.AudioClientShareMode.Shared, 12);
            wasapi_out.Init(playerWaveProvider);
            
            //設定音量，預設是1，最大是3
            synthesizer.MasterVolume = 3;
            LoadBank(@"D:\Download\SF2\SGM-V2.01.sf2");
        }

        public override void Dispose()
        {
            base.Dispose();
            wasapi_out.Dispose();
        }

        public override void Reset()
        {
            lock (playerWaveProvider.lockObj)
            {
                synthesizer.NoteOffAll(true);
                synthesizer.ResetPrograms();
                synthesizer.ResetSynthControls();
                playerWaveProvider.Reset();
            }
        }

        public override void Play()
        {
            if (wasapi_out.PlaybackState != PlaybackState.Playing)
                wasapi_out.Play();
            base.Play();
        }

        public override void SetVolumn(float volumn)
        {
            wasapi_out.Volume = volumn;
        }

        public override void LoadBank(string path)
        {
            synthesizer.LoadBank(new MyFile(path));
        }

        public override void ProcessMessage(Message[] messages)
        {
            lock (playerWaveProvider.lockObj)
            {
                foreach (Message message in messages)
                {
                    //只有一行似乎沒有必要再拉出一個函數，畢竟陣列長度等於一時，效果就相當於只傳Message的方法了
                    synthesizer.ProcessMidiMessage(message.Channel, message.Command * 16, message.Data1, (int)message.Data2);
                }
            }
        }
    }
}
