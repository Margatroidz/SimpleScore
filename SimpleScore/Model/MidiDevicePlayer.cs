using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace SimpleScore.Model
{
    public class MidiDevicePlayer : Player
    {
        //C:\Windows\System32\drivers\gm.dls 內建MIDI音效

        [DllImport("winmm.dll")]
        private static extern UInt32 midiOutOpen(out UInt32 lphMidiOut, int uDeviceID, UInt32 dwCallback, UInt32 dwInstance, UInt32 dwFlags);

        [DllImport("winmm.dll")]
        private static extern UInt32 midiOutShortMsg(UInt32 lphMidiOut, int dwMsg);

        [DllImport("winmm.dll")]
        public static extern int midiOutSetVolume(UInt32 lphMidiOut, int dwVolume);

        [DllImport("winmm.dll")]
        public static extern int midiOutReset(UInt32 lphMidiOut);

        [DllImport("winmm.dll")]
        public static extern int midiOutClose(UInt32 lphMidiOut);

        UInt32 midiOut = 0;

        public MidiDevicePlayer()
        : base()
        {
            midiOutOpen(out midiOut, -1, 0, 0, 0);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (midiOut != 0) midiOutClose(midiOut);
                base.Dispose(disposing);
            }
        }

        public override void Reset()
        {
            midiOutReset(midiOut);
        }

        public override void LoadBank(string path)
        {
            //故意不實做，因為一般midi裝置沒有辦法載入bank，除非使用虛擬midi裝置，但我也不知道怎麼裝虛擬midi裝置
            throw new NotImplementedException();
        }

        public override void ProcessMessage(Message[] messages)
        {
            foreach (Message message in messages)
            {
                //看指定的channel有沒有被靜音，但被靜音的channel仍可以送出NoteOn以外的Message
                if (!muteList.Contains(message.Channel) || !(message.Command == 9 && !((int)message.Data2 == 0)))
                {
                    //只有一行似乎沒有必要再拉出一個函數，畢竟陣列長度等於一時，效果就相當於只傳Message的方法了
                    midiOutShortMsg(midiOut, (int)message.Data2 << 16 | message.Data1 << 8 | message.Status);
                }
            }
        }

        public override float Volumn
        {
            get
            {
                //int value = midiOut.Volume / (0x10001);
                //return (float)value / 0xffff;
                return 0;
            }
            set
            {
                int volumn = (int)(value * 0xffff);
                int result = midiOutSetVolume(midiOut, (int)((0x10001) * volumn));
                switch (result)
                {
                    case 5:
                        Console.WriteLine("Invalid handle");
                        break;
                    case 6:
                        Console.WriteLine("No driver");
                        break;
                    case 8:
                        Console.WriteLine("Not Supported");
                        break;
                }
            }
        }
    }
}