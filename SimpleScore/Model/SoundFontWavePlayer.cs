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
        private DirectSoundOut direct_out;

        public SoundFontWavePlayer()
            : base()
        {
            Synthesizer.InterpolationMode = (InterpolationEnum)1;
            synthesizer = new Synthesizer(44100, 2, 441, 3, 100);
            playerWaveProvider = new PlayerWaveProvider(synthesizer);
            //50基本上是最小了，在小會出問題   //聲音都會卡卡的，沒辦法控制
            direct_out = new DirectSoundOut();
            direct_out.Init(playerWaveProvider);
        }

        public new void Reset()
        {
            lock (playerWaveProvider.lockObj)
            {
                synthesizer.NoteOffAll(true);
                synthesizer.ResetPrograms();
                synthesizer.ResetSynthControls();
                playerWaveProvider.Reset();
            }
        }

        public new void Play()
        {
            if (direct_out.PlaybackState != PlaybackState.Playing)
                direct_out.Play();
            base.Play();
        }

        public override void PlayVoices(Voice[] voices)
        {
            lock (playerWaveProvider.lockObj)
            {
                foreach (Voice note in voices)
                {
                    PlayNote(note.Status, note.Data1, note.Data2);
                }
            }
        }

        public override void PlayNote(int status, int data1, int data2)
        {
            synthesizer.ProcessMidiMessage(status % 16, (status / 16) * 16, data1, data2);
        }
    }
}
