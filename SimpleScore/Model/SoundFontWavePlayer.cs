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
    class SoundFontWavePlayer : Player
    {
        private Synthesizer synthesizer;
        private PlayerWaveProvider playerWaveProvider;
        //private DirectSoundOut direct_out;
        private WasapiOut wasapi_out;

        public SoundFontWavePlayer()
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
            SetVolumn(0.3f);
            LoadBank(@"D:\Download\SF2\TOUHOU INSTRUMENT + DRUM KIT.sf2");
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
                    PlayNote(message);
                }
            }
        }

        public override void PlayNote(Message message)
        {
            synthesizer.ProcessMidiMessage(message.Channel, message.Command*16, message.Data1, message.Data2);
        }
    }
}
