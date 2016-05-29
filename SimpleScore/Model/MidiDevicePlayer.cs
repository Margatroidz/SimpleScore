﻿using System;
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

        public override void Dispose()
        {
            base.Dispose();
            midiOut.Dispose();
            GC.SuppressFinalize(this);
        }

        public override void Reset()
        {
            midiOut.Reset();
        }

        public override void ProcessMessage(Message[] messages)
        {
            foreach (Message message in messages)
            {
                PlayNote(message);
            }
        }

        public override void PlayNote(Message message)
        {
            midiOut.Send((int)message.Data2 << 16 | message.Data1 << 8 | message.Status);
        }
    }
}