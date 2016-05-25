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
            foreach (Voice note in voices)
            {
                PlayNote(note.Status, note.Data1, note.Data2);
            }
        }

        public override void PlayNote(int status, int data1, int data2)
        {
            midiOut.Send(data2 << 16 | data1 << 8 | status);
        }
    }
}