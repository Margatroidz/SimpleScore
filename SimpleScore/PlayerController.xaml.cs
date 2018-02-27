using SimpleScore.Model;
using System;
using System.Timers;
using System.Windows;
using System.Windows.Controls;

namespace SimpleScore
{
    /// <summary>
    /// WasPlayerController.xaml 的互動邏輯
    /// </summary>
    public partial class PlayerController : Window
    {
        RadioButton[] muteButton;
        RadioButton[] notMuteButton;
        SSSystem model;

        public PlayerController(SSSystem model)
        {
            InitializeComponent();

            this.model = model;
            model.playerChanged += ResetMuteStatus;
            Initialize();
            ResetMuteStatus();
        }

        private void Initialize()
        {
            #region
            model.Volumn = 1;
            VolumnSlider.Value = 10;
            muteButton = new RadioButton[16];
            notMuteButton = new RadioButton[16];

            muteButton[0] = ui_track1MuteButton;
            muteButton[1] = ui_track2MuteButton;
            muteButton[2] = ui_track3MuteButton;
            muteButton[3] = ui_track4MuteButton;
            muteButton[4] = ui_track5MuteButton;
            muteButton[5] = ui_track6MuteButton;
            muteButton[6] = ui_track7MuteButton;
            muteButton[7] = ui_track8MuteButton;
            muteButton[8] = ui_track9MuteButton;
            muteButton[9] = ui_track10MuteButton;
            muteButton[10] = ui_track11MuteButton;
            muteButton[11] = ui_track12MuteButton;
            muteButton[12] = ui_track13MuteButton;
            muteButton[13] = ui_track14MuteButton;
            muteButton[14] = ui_track15MuteButton;
            muteButton[15] = ui_track16MuteButton;

            notMuteButton[0] = ui_track1NotMuteButton;
            notMuteButton[1] = ui_track2NotMuteButton;
            notMuteButton[2] = ui_track3NotMuteButton;
            notMuteButton[3] = ui_track4NotMuteButton;
            notMuteButton[4] = ui_track5NotMuteButton;
            notMuteButton[5] = ui_track6NotMuteButton;
            notMuteButton[6] = ui_track7NotMuteButton;
            notMuteButton[7] = ui_track8NotMuteButton;
            notMuteButton[8] = ui_track9NotMuteButton;
            notMuteButton[9] = ui_track10NotMuteButton;
            notMuteButton[10] = ui_track11NotMuteButton;
            notMuteButton[11] = ui_track12NotMuteButton;
            notMuteButton[12] = ui_track13NotMuteButton;
            notMuteButton[13] = ui_track14NotMuteButton;
            notMuteButton[14] = ui_track15NotMuteButton;
            notMuteButton[15] = ui_track16NotMuteButton;
            #endregion
        }

        private void MuteButtonChecked(object sender, RoutedEventArgs e)
        {
            model.Mute(Convert.ToInt32(((RadioButton)sender).CommandParameter), true);
        }

        private void NotMuteButtonChecked(object sender, RoutedEventArgs e)
        {
            model.Mute(Convert.ToInt32(((RadioButton)sender).CommandParameter), false);
        }

        private void ResetMuteStatus()
        {
            foreach (RadioButton button in notMuteButton)
                button.IsChecked = true;
        }

        private void VolumnChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
                model.Volumn = (((float)e.NewValue) / 10f);
            }
            catch
            {
                Console.WriteLine("Can't not changed the volumn !");
            }
        }
    }
}
