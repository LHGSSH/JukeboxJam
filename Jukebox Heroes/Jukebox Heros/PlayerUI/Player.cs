﻿using Jukebox_Heros.Song;
using Jukebox_Heros.SongLibrary;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Threading;

namespace Jukebox_Heros.PlayerUI
{
    class Player
    {
        private MediaElement mediaPlayer;
        private ListBox songList;
        private TextBlock timeText;
        private bool mediaPlayerIsPlaying = false, userIsDraggingSlider = false;
        private Slider slider;

        public Player(ListBox songList, MediaElement mediaPlayer, Slider slider, TextBlock timeText)
        {
            this.songList = songList;
            this.mediaPlayer = mediaPlayer;
            this.slider = slider;
            this.timeText = timeText;

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += tick;
            timer.Start();

        }

        public void tick(object sender, EventArgs e) {
            if ((mediaPlayer.Source != null) && (mediaPlayer.NaturalDuration.HasTimeSpan) && (!userIsDraggingSlider)) {
                slider.Minimum = 0;
                slider.Maximum = mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds;
                slider.Value = mediaPlayer.Position.TotalSeconds;
            }
        }



        public void Play_Click(object sender, RoutedEventArgs e)
        {
            GetSong();
           //Nick I Love you. You are too good to me. Have a sweet spring break
            mediaPlayer.Play();
            mediaPlayerIsPlaying = true;
        }

        public void Pause_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Pause();
        }

        public void Stop_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Stop();
            mediaPlayerIsPlaying = false;
        }

        public void GetSong()
        {
            SongData song = (SongData) songList.SelectedItem;
            if(song != null) mediaPlayer.Source = song.getUri();
        }

        public void slider_DragStarted() {
            userIsDraggingSlider = true;
        }

        public void slider_DragCompleted() {
            userIsDraggingSlider = false;
            mediaPlayer.Position = TimeSpan.FromSeconds(slider.Value);
        }

        public void slider_ValueChanged() {
            timeText.Text = TimeSpan.FromSeconds(slider.Value).ToString(@"hh\:mm\:ss");
        }
    }
}