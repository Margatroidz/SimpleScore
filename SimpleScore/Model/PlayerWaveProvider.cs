using System;
using System.Collections.Generic;
using NAudio.Wave;
using NAudio.Utils;
using AudioSynthesis.Synthesis;

namespace SimpleScore.Model
{
    public class PlayerWaveProvider : IWaveProvider
    {    
        public volatile object lockObj = new object();
        private CircularBuffer circularBuffer;
        private WaveFormat waveFormat;
        private byte[] sbuff;
        private Synthesizer synth;
        
        public delegate void UpdateTime(int current, int max, int voices);
        public event UpdateTime TimeUpdate;
        public delegate void UpdateTrackBars(int[] ccList);
        public event UpdateTrackBars UpdateMidiControllers;

        public PlayerWaveProvider(Synthesizer synth)
        {
            this.synth = synth;

            waveFormat = new WaveFormat(synth.SampleRate, 16, synth.AudioChannels);
            int bufferSize = (int)Math.Ceiling((2.0 * waveFormat.AverageBytesPerSecond) / synth.RawBufferSize) * synth.RawBufferSize;
            circularBuffer = new CircularBuffer(bufferSize);
            sbuff = new byte[synth.RawBufferSize];
        }
        public WaveFormat WaveFormat
        {
            get { return waveFormat; }
        }
        public int Read(byte[] buffer, int offset, int count)
        {
            int[] ccList = new int[16];
            while (circularBuffer.Count < count)
            {
                lock (lockObj)
                {
                    if (UpdateMidiControllers != null)
                    {
                        IEnumerator<MidiMessage> mmEnum = synth.MidiMessageEnumerator;
                        while (mmEnum.MoveNext())
                        {
                            if (mmEnum.Current.command == 0xC0)
                            {//program change
                                ccList[mmEnum.Current.channel] |= 0x1;
                            }
                            else if (mmEnum.Current.command == 0xE0)
                            {//pitch bend
                                ccList[mmEnum.Current.channel] |= 0x2;
                            }
                            else if (mmEnum.Current.command == 0xB0)
                            {
                                switch (mmEnum.Current.data1)
                                {
                                    case 0x07: //vol
                                        ccList[mmEnum.Current.channel] |= 0x4;
                                        break;
                                    case 0x0A: //pan
                                        ccList[mmEnum.Current.channel] |= 0x8;
                                        break;
                                    case 0x0B: //exp
                                        ccList[mmEnum.Current.channel] |= 0x10;
                                        break;
                                    case 0x40: //hold
                                        ccList[mmEnum.Current.channel] |= 0x20;
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                    }
                    synth.GetNext(sbuff);
                    circularBuffer.Write(sbuff, 0, sbuff.Length);
                }
            }
            if (TimeUpdate != null)
                TimeUpdate(0, 100, synth.ActiveVoices);
            if (UpdateMidiControllers != null)
                UpdateMidiControllers(ccList);
            return circularBuffer.Read(buffer, offset, count);
        }

        public void Reset()
        {
            circularBuffer.Reset();
        }
    }
}
