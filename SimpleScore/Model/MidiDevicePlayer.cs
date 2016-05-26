using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NAudio.Midi;

namespace SimpleScore.Model
{
    class MidiDevicePlayer : Player
    {
        //C:\Windows\System32\drivers\gm.dls 內建MIDI音效
        MidiOut midiOut;

        public MidiDevicePlayer()
            : base()
        {
            midiOut = new MidiOut(0);
        }

        public override void Reset()
        {
            midiOut.Reset();
        }

        public override void PlayVoices(Voice[] voices)
        {
            foreach (Voice voice in voices)
            {
                PlayNote(voice);
            }
        }

        public override void PlayNote(Voice voice)
        {
            midiOut.Send(voice.Data2 << 16 | voice.Data1 << 8 | voice.Status);
        }
    }
}