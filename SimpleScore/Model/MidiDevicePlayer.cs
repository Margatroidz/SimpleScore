using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NAudio.Midi;

namespace SimpleScore.Model
{
    public class MidiDevicePlayer : Player
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
        }

        public override void Reset()
        {
            midiOut.Reset();
        }

        public override void SetVolumn(float volumn)
        {
            //故意不實作，因為部分裝置沒辦法條音量，也可以使用虛擬midi裝置條音量，但我也不知道怎麼裝虛擬midi裝置
            throw new NotImplementedException();
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
                    midiOut.Send((int)message.Data2 << 16 | message.Data1 << 8 | message.Status);
                }
            }
        }
    }
}