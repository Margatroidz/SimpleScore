﻿using SimpleScore.Model;
using SimpleScore.View;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace SimpleScore
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        //const int UI_TRACK_COUNT = 24;
        SSSystem model;
        ViewModel viewModel;
        Rectangle indicator;
        Grid[] trackGrid;
        ScrollViewer trackScrollViewer;

        public MainWindow()
        {
            InitializeComponent();
            model = new SSSystem();
            viewModel = new ViewModel(model);

            trackGrid = new Grid[ViewModel.UI_TRACK_COUNT + 1];
            for (int i = 0; i < trackGrid.Length; i++)
            {
                trackGrid[i] = new Grid();
                trackGrid[i].HorizontalAlignment = HorizontalAlignment.Left;
                trackGrid[i].Height = 1040;
                trackGrid[i].Width = 5000;
            }
            //從後面的gird開始放，這樣第1個grid的指針可以擺在最上層
            for (int i = trackGrid.Length - 1; i > 0; i--)
            {
                trackGrid[0].Children.Add(trackGrid[i]);
            }
            trackScrollViewer = uiTrackScrollViewer;
            trackScrollViewer.Content = trackGrid[0];

            /*rollNotation = new List<Rectangle>[UI_TRACK_COUNT];
            for (int i = 0; i < UI_TRACK_COUNT; i++)
            {
                rollNotation[i] = new List<Rectangle>();
            }*/

            indicator = new Rectangle();
            indicator = viewModel.CreateIndicator();

            model.playStatusChanged += PlayStatusChange;
            model.playProgressChanged += PlayProgressChange;
            model.playProgressChanged += IndicatorChange;
            model.loadComplete += LoadComplete;
            viewModel.viewScaleChanged += ResizeUI;

            string s = string.Empty;
            for (int i = 0; i < 2000; i++)
            {
                s = s + i + "\t";
            }
            uiClockScrollViewer.Content = s;
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            model.PlayOrPause();
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            model.Stop();
        }

        private void RecordButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void PreviousButton_Click(object sender, RoutedEventArgs e)
        {
            model.LoadSequential(-1);
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            model.LoadSequential(1);
        }

        private void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            viewModel.SelectFile();
        }

        //壓住ctrl時並滾動滑鼠時，改為水平移動
        private void TrackScrollViewer_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftShift))
            {
                ScrollViewer s = (ScrollViewer)sender;
                s.ScrollToHorizontalOffset(s.HorizontalOffset - (e.Delta * 2));//之後想改成固定數字，且可以在option改
                e.Handled = true;
            }
            else if (Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                viewModel.ViewScale += 0.1 * Math.Sign(e.Delta);
                e.Handled = true;
            }
        }

        private void TrackScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            uiPianoScrollViewer.ScrollToVerticalOffset(e.VerticalOffset);
            uiClockScrollViewer.ScrollToHorizontalOffset(e.HorizontalOffset);
        }

        private void AutoPlayCheckBox_Click(object sender, RoutedEventArgs e)
        {
            model.AutoPlay = (bool)((CheckBox)sender).IsChecked;
        }

        private void TimeBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Point point = e.GetPosition(Mouse.DirectlyOver);
            model.ChangeClock((float)point.X / (float)maxTimeBar.ActualWidth);
        }

        private void PlayStyleRadioButton_Click(object sender, RoutedEventArgs e)
        {
            int style = Convert.ToInt32(AutomationProperties.GetName((RadioButton)sender));
            model.ChangeLoadStyle(style);
        }

        //自動更新UI事件
        private void LoadComplete()
        {
            scoreNameLabel.Content = model.ScoreName;
            viewModel.LoadScoreToUI(trackGrid, indicator);
            AdjustGridWidth();
        }
        //自動更新UI事件
        private void PlayStatusChange()
        {
            if (model.IsPlay)
            {
                playButton.Content = "Pause";
            }
            else
            {
                playButton.Content = "Play";
            }
        }
        //自動更新UI事件
        private void PlayProgressChange()
        {
            currentTimeBar.Width = (int)((double)475 * model.ProgressPercentage);
        }
        //自動更新UI事件
        private void IndicatorChange()
        {
            //UI只顯示2個track，所以限制指針上限 //  5/11 改剩一個
            indicator.Margin = new Thickness((model.Clock / model.Semiquaver) * ViewModel.SemiquaverWidth, 0, 0, 0);
            if (indicator.Margin.Left * viewModel.ViewScale > trackScrollViewer.HorizontalOffset + mainWindow.trackScrollViewer.ActualWidth ||
                indicator.Margin.Left * viewModel.ViewScale < (trackScrollViewer.HorizontalOffset -50))
            {
                trackScrollViewer.ScrollToHorizontalOffset((indicator.Margin.Left - 50) * viewModel.ViewScale);
            }
            else if (indicator.Margin.Left == 0)
            {
                trackScrollViewer.ScrollToHorizontalOffset(0);
            }
        }
        //自動更新UI事件
        private void ResizeUI()
        {
            double scale = viewModel.ViewScale;
            ScaleTransform scaleTransform = new ScaleTransform(scale, scale);
            Console.WriteLine(scale);
            //layoutTransform除了轉換外，還會自動排版，用renderTransform的話，畫面上的排版會以"原大小"排版
            trackGrid[0].LayoutTransform = scaleTransform;
            uiPianoTagGrid.RenderTransform = scaleTransform;
            scaleTransform = new ScaleTransform(1, scale);
            uiPianoImage.RenderTransform = scaleTransform;
            scaleTransform = new ScaleTransform(scale, 1);
            uiClockLabel.RenderTransform = scaleTransform;
        }

        /*private void LoadScoreToUI()
        {
            foreach (List<Rectangle> rectangle in rollNotation) rectangle.Clear();
            for (int i = 0; i < model.TrackCount; i++) trackGrid[i + 1].Children.Clear();

            Voice[] trackData;
            //目前只做了16(已改為常數)個音軌，所以限制最高16個
            for (int i = 0; i < model.TrackCount && i < UI_TRACK_COUNT; i++)
            {
                List<int[]> tmp = new List<int[]>();
                trackData = model.GetVoiceByTrack(i);
                for (int j = 0; j < trackData.Length; j++)
                {
                    //0 channel、1 control、2 scale、3 time 
                    int[] tmpNote = new int[4];
                    tmpNote[0] = trackData[j].Status % 16;
                    tmpNote[1] = trackData[j].Status / 16;
                    tmpNote[2] = trackData[j].Data1;
                    tmpNote[3] = trackData[j].Time;
                    if (!Match(tmp, tmpNote, i)) tmp.Add(tmpNote);
                }
                foreach (Rectangle r in rollNotation[i]) trackGrid[i + 1].Children.Add(r);
                //頭兩個grid需要指針指向目前播放位置
                if (i == 0) trackGrid[i + 1].Children.Add(indicator);
            }
        }*/

        /*private bool Match(List<int[]> matchTarget, int[] matchItem, int trackNumber)
        {
            foreach (int[] target in matchTarget)
            {
                if (target[0] == matchItem[0] && target[2] == matchItem[2])
                {
                    matchTarget.Remove(target);
                    //在rollNotation範圍內才畫
                    if (target[2] < 108 && target[2] > 21)
                        rollNotation[trackNumber].Add(viewModel.CreateRollNotation(target[2], target[3], matchItem[3], trackNumber, model.Semiquaver));
                    return true;
                }
            }
            //if (matchItem[1] == 8) throw new Exception("Error when build Roll Notation");
            if (matchItem[1] == 9) return false;
            else return true;
        }*/

        private void AdjustGridWidth()
        {
            foreach (Grid grid in trackGrid)
            {
                grid.Width = ((model.ScoreLength / model.Semiquaver) * ViewModel.SemiquaverWidth) + 100;
            }
        }
    }
}
