using Microsoft.Win32;
using SimpleScore.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SimpleScore.View
{
    class ViewModel
    {
        public delegate void ViewScaleChangedEventHandler();
        public event ViewScaleChangedEventHandler viewScaleChanged;

        public const int SemiquaverWidth = 3;
        public const int TopMargin = 0;
        public const int Height = 1;
        public const int UI_TRACK_COUNT = 24;

        SSSystem model;
        List<Rectangle>[] rollNotation;
        OpenFileDialog dialog;
        double viewScale;

        public ViewModel(Model.SSSystem model)
        {
            this.model = model;
            rollNotation = new List<Rectangle>[UI_TRACK_COUNT];
            for (int i = 0; i < UI_TRACK_COUNT; i++)
            {
                rollNotation[i] = new List<Rectangle>();
            }

            viewScale = 1;
            dialog = new OpenFileDialog();
            //dialog.InitialDirectory = @"D:\Download\PianoEasy";
            dialog.Filter = "MIDI (.mid)|*.mid";
        }

        private Dictionary<int, int[]> rollNotationAttribute = new Dictionary<int, int[]> 
        {
            {108, new int[]{0, 20}},{107, new int[]{20, 20}},{106, new int[]{33, 14}},{105, new int[]{40, 20}},{104, new int[]{53, 14}},
            {103, new int[]{60, 20}},{102, new int[]{73, 14}},{101, new int[]{80, 20}},{100, new int[]{100, 20}},{99, new int[]{113, 14}},
            {98, new int[]{120, 20}},{97, new int[]{133, 14}},{96, new int[]{140, 20}},{95, new int[]{160, 20}},{94, new int[]{173, 14}},
            {93, new int[]{180, 20}},{92, new int[]{193, 14}},{91, new int[]{200, 20}},{90, new int[]{213, 14}},{89, new int[]{220, 20}},
            {88, new int[]{240, 20}},{87, new int[]{253, 14}},{86, new int[]{260, 20}},{85, new int[]{273, 14}},{84, new int[]{280, 20}},
            {83, new int[]{300, 20}},{82, new int[]{313, 14}},{81, new int[]{320, 20}},{80, new int[]{333, 14}},{79, new int[]{340, 20}},
            {78, new int[]{353, 14}},{77, new int[]{360, 20}},{76, new int[]{380, 20}},{75, new int[]{393, 14}},{74, new int[]{400, 20}},
            {73, new int[]{413, 14}},{72, new int[]{420, 20}},{71, new int[]{440, 20}},{70, new int[]{453, 14}},{69, new int[]{460, 20}},
            {68, new int[]{473, 14}},{67, new int[]{480, 20}},{66, new int[]{493, 14}},{65, new int[]{500, 20}},{64, new int[]{520, 20}},
            {63, new int[]{533, 14}},{62, new int[]{540, 20}},{61, new int[]{553, 14}},{60, new int[]{560, 20}},{59, new int[]{580, 20}},
            {58, new int[]{593, 14}},{57, new int[]{600, 20}},{56, new int[]{613, 14}},{55, new int[]{620, 20}},{54, new int[]{633, 14}},
            {53, new int[]{640, 20}},{52, new int[]{660, 20}},{51, new int[]{673, 14}},{50, new int[]{680, 20}},{49, new int[]{693, 14}},
            {48, new int[]{700, 20}},{47, new int[]{720, 20}},{46, new int[]{733, 14}},{45, new int[]{740, 20}},{44, new int[]{753, 14}},
            {43, new int[]{760, 20}},{42, new int[]{773, 14}},{41, new int[]{780, 20}},{40, new int[]{800, 20}},{39, new int[]{813, 14}},
            {38, new int[]{820, 20}},{37, new int[]{833, 14}},{36, new int[]{840, 20}},{35, new int[]{860, 20}},{34, new int[]{873, 14}},
            {33, new int[]{880, 20}},{32, new int[]{893, 14}},{31, new int[]{900, 20}},{30, new int[]{913, 14}},{29, new int[]{920, 20}},
            {28, new int[]{940, 20}},{27, new int[]{953, 14}},{26, new int[]{960, 20}},{25, new int[]{973, 14}},{24, new int[]{980, 20}},
            {23, new int[]{1000, 20}},{22, new int[]{1013, 14}},{21, new int[]{1020, 20}}

            // out of piano
            /*{127, new int[]{20, 20}},{126, new int[]{33, 14}},{125, new int[]{40, 20}},{124, new int[]{53, 14}},{123, new int[]{60, 20}},
            {122, new int[]{73, 14}},{121, new int[]{80, 20}},{120, new int[]{100, 20}},{119, new int[]{113, 14}},{118, new int[]{0, 20}},
            {117, new int[]{20, 20}},{116, new int[]{33, 14}},{115, new int[]{40, 20}},{114, new int[]{53, 14}},{113, new int[]{60, 20}},
            {112, new int[]{73, 14}},{111, new int[]{80, 20}},{110, new int[]{100, 20}},{109, new int[]{113, 14}},
            {20, new int[]{913, 14}},{19, new int[]{920, 20}},{18, new int[]{940, 20}},{17, new int[]{953, 14}},{16, new int[]{960, 20}},
            {15, new int[]{973, 14}},{14, new int[]{980, 20}},{13, new int[]{1000, 20}},{12, new int[]{1013, 14}},{11, new int[]{1020, 20}},
            {10, new int[]{913, 14}},{9, new int[]{920, 20}},{8, new int[]{940, 20}},{7, new int[]{953, 14}},{6, new int[]{960, 20}},
            {5, new int[]{973, 14}},{4, new int[]{980, 20}},{3, new int[]{1000, 20}},{2, new int[]{1013, 14}},{1, new int[]{1020, 20}}*/
        };

        public Rectangle CreateRollNotation(int scale, int frontClock, int backClock, int trackNumber, float semiquaver)
        {
            Rectangle rectangle = new Rectangle();
            rectangle.Fill = new SolidColorBrush(GetColor(trackNumber, (rollNotationAttribute[scale])[Height]));
            rectangle.HorizontalAlignment = HorizontalAlignment.Left;
            rectangle.VerticalAlignment = VerticalAlignment.Top;
            rectangle.Height = (rollNotationAttribute[scale])[Height];
            rectangle.Width = ((backClock - frontClock) / semiquaver) * SemiquaverWidth;
            rectangle.Opacity = 0.5;
            rectangle.RadiusX = 4;
            rectangle.RadiusY = 4;
            rectangle.Margin = new Thickness((frontClock / semiquaver) * SemiquaverWidth, (rollNotationAttribute[scale])[TopMargin], 0, 0);
            return rectangle;
        }

        public Rectangle CreateIndicator()
        {
            Rectangle rectangle = new Rectangle();
            rectangle.Fill = new SolidColorBrush(Colors.Red);
            rectangle.HorizontalAlignment = HorizontalAlignment.Left;
            rectangle.VerticalAlignment = VerticalAlignment.Top;
            rectangle.Height = 1040;
            rectangle.Width = 2;
            rectangle.Margin = new Thickness(0, 0, 0, 0);
            return rectangle;
        }

        private Color GetColor(int trackNumber, int height)
        {
            if (trackNumber % 7 == 0)
            {
                if (height == 20) return Colors.Red;
                else return Colors.Crimson;
            }
            else if (trackNumber % 7 == 1)
            {
                if (height == 20) return Colors.Orange;
                else return Colors.DarkOrange;
            }
            else if (trackNumber % 7 == 2)
            {
                if (height == 20) return Colors.Yellow;
                else return Colors.Gold;
            }
            else if (trackNumber % 7 == 3)
            {
                if (height == 20) return Colors.Lime;
                else return Colors.LimeGreen;
            }
            else if (trackNumber % 7 == 4)
            {
                if (height == 20) return Colors.Blue;
                else return Colors.MediumBlue;
            }
            else if (trackNumber % 7 == 5)
            {
                if (height == 20) return Colors.Aqua;
                else return Colors.DarkTurquoise;
            }
            else
            {
                if (height == 20) return Colors.DarkViolet;
                else return Colors.Purple;
            }
        }

        public void SelectFile()
        {
            if (dialog.ShowDialog() == true)
            {
                model.Load(dialog.FileName);
            }
        }

        public void SelectPlayer()
        {

        }

        public void LoadScoreToUI(Grid[] trackGrid, Rectangle indicator)
        {
            foreach (List<Rectangle> rectangle in rollNotation) rectangle.Clear();
            for (int i = 0; i < UI_TRACK_COUNT; i++) trackGrid[i + 1].Children.Clear();

            Message[] trackData;
            //目前只做了16(已改為常數)個音軌，所以限制最高16個
            for (int i = 0; i < model.TrackCount && i < UI_TRACK_COUNT; i++)
            {
                List<int[]> tmp = new List<int[]>();
                trackData = model.GetMessageByTrack(i);
                for (int j = 0; j < trackData.Length; j++)
                {
                    if (trackData[j].MessageType == Message.Type.Voice)
                    {
                        //0 channel、1 control、2 scale、3 time 
                        int[] tmpNote = new int[4];
                        tmpNote[0] = trackData[j].Status % 16;
                        tmpNote[1] = trackData[j].Status / 16;
                        tmpNote[2] = trackData[j].Data1;
                        tmpNote[3] = trackData[j].Time;
                        if (!Match(tmp, tmpNote, i)) tmp.Add(tmpNote);
                    }

                }
                foreach (Rectangle r in rollNotation[i]) trackGrid[i + 1].Children.Add(r);
                //頭兩個grid需要指針指向目前播放位置
                if (i == 0) trackGrid[i + 1].Children.Add(indicator);
            }
        }

        private bool Match(List<int[]> matchTarget, int[] matchItem, int trackNumber)
        {
            foreach (int[] target in matchTarget)
            {
                if (target[0] == matchItem[0] && target[2] == matchItem[2])
                {
                    matchTarget.Remove(target);
                    //在rollNotation範圍內才畫
                    if (target[2] < 108 && target[2] > 21)
                        rollNotation[trackNumber].Add(CreateRollNotation(target[2], target[3], matchItem[3], trackNumber, model.Semiquaver));
                    return true;
                }
            }
            //if (matchItem[1] == 8) throw new Exception("Error when build Roll Notation");
            if (matchItem[1] == 9) return false;
            else return true;
        }

        public string PlayButtonContent(bool isPlay)
        {
            return isPlay ? "Pause" : "Play";
        }

        public double ViewScale
        {
            get
            {
                return viewScale;
            }
            set
            {
                if (value > 0)
                {
                    viewScale = value;
                    NotifyViewScaleChanged();
                }
            }
        }

        private void NotifyViewScaleChanged()
        {
            if (viewScaleChanged != null)
            {
                viewScaleChanged();
            }
        }
    }
}
