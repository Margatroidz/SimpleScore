using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleScore.Model
{
    public class MidiParser
    {
        String hex;
        String note1;
        String note2;
        String note3;
        int currentTrack;
        int currentClock;
        public MidiParser()
        {
            note1 = string.Empty;
            note2 = string.Empty;
            note3 = string.Empty;
            currentTrack = 0;
            currentClock = 0;
        }

        public void Parse(byte[] data, Score score)
        {
            hex = string.Empty;
            note1 = string.Empty;
            note2 = string.Empty;
            note3 = string.Empty;
            currentTrack = 0;
            currentClock = 0;
            if (data[0] != 0x4d && data[1] != 0x54 && data[2] != 0x68 && data[3] != 0x64) throw new Exception();
            if (data[4] != 0x0 && data[5] != 0x0 && data[6] != 0x0 && data[7] != 0x6) throw new Exception();
            //int format = int.Parse(Convert.ToString(data[8], 16) + Convert.ToString(data[9], 16));
            int track = Convert.ToInt32(Convert.ToString(data[10], 16) + Convert.ToString(data[11]), 16);
            score.Tick = Convert.ToInt32((Convert.ToString(data[12], 2)).PadLeft(8, '0') + (Convert.ToString(data[13], 2)).PadLeft(8, '0'), 2);
            //第一個track開始
            if (data[14] != 0x4d && data[15] != 0x54 && data[16] != 0x72 && data[17] != 0x6b) throw new Exception();
            //18、19、20、21為資料長度
            int deltaTime;
            string concatenateDeltaTime;
            string metaEvent;
            string temp;
            score.CreateTrack();

            for (int i = 22; i < data.Length; )
            {
                //判斷deltaTime
                concatenateDeltaTime = string.Empty;
                do
                {
                    hex = (Convert.ToString(data[i++], 2)).PadLeft(8, '0');
                    concatenateDeltaTime = concatenateDeltaTime + hex.Substring(1, 7);
                } while (hex.StartsWith("1"));
                deltaTime = Convert.ToInt32(concatenateDeltaTime, 2);
                if (deltaTime != 0) currentClock += Convert.ToInt32(concatenateDeltaTime, 2);

                hex = Convert.ToString(data[i], 16).PadLeft(2, '0');
                if (hex == "ff")
                {
                    metaEvent = Convert.ToString(data[++i], 16).PadLeft(2, '0');
                    if (metaEvent == "2f")
                    {
                        if (currentTrack < track - 1)
                        {
                            //有些檔案再標頭宣告的音軌數超過實際的音軌數，所以用這行來讓灌水的標頭不會出現錯誤
                            if (i + 10 > data.Length) break;
                            currentTrack++;
                            currentClock = 0;
                            if (data[i + 2] != 0x4d && data[i + 3] != 0x54 && data[i + 4] != 0x72 && data[i + 5] != 0x6b) throw new Exception();
                            score.CreateTrack();
                            //接下來4位數為資料長度
                            i += 10;
                        }
                        else break;
                    }
                    else if (metaEvent == "51")
                    {
                        temp = CombineEventData(ref i, data);
                        score.CreateMessage(currentTrack, new Message(Message.Type.Meta, currentClock,
                            0xff, 0x51, Convert.ToInt32(temp, 16) / 1000));
                    }
                    else
                    {
                        CombineEventData(ref i, data);
                    }
                }
                else if (hex == "f7" || hex == "f0")
                {
                    CombineEventData(ref i, data);
                }
                else if (hex.StartsWith("0") || hex.StartsWith("1") || hex.StartsWith("2") || hex.StartsWith("3") || hex.StartsWith("4")
                    || hex.StartsWith("5") || hex.StartsWith("6") || hex.StartsWith("7"))
                {
                    i++;
                    //note1 用上一個的note1
                    if (note1.StartsWith("c") || note1.StartsWith("d"))
                    {
                        CreateMessage(ref i, false, score, data, note1, hex);
                    }
                    else
                    {
                        CreateMessage(ref i, true, score, data, note1, hex);
                    }
                }
                else if (hex.StartsWith("c") || hex.StartsWith("d"))
                {
                    i++;
                    CreateMessage(ref i, false, score, data, hex);
                }
                else
                {
                    i++;
                    CreateMessage(ref i, true, score, data, hex);
                }
            }
        }

        private string CombineEventData(ref int index, byte[] data)
        {
            index++;
            int tmp = Convert.ToInt32(Convert.ToString(data[index++], 16), 16);
            string result = string.Empty;
            for (int i = 0; i < tmp; i++)
            {
                //要padLeft，之前修過一次，不知道為什麼又跑出來了
                result = result + Convert.ToString(data[index++], 16).PadLeft(2, '0');
            }
            return result;
        }

        private void CreateMessage(ref int index, bool isThreeParameter, Score score, byte[] data, string firstValue, string secondValue = "-1")
        {
            note1 = firstValue;
            if (secondValue != "-1")
                note2 = secondValue;
            else
                note2 = Convert.ToString(data[index++], 16);
            if (isThreeParameter)
                note3 = Convert.ToString(data[index++], 16);
            else
                note3 = "0";

            score.CreateMessage(currentTrack, new Message(Message.Type.Voice, currentClock
                , Convert.ToInt32(note1, 16), Convert.ToInt32(note2, 16), Convert.ToInt32(note3, 16)));
        }
    }
}
