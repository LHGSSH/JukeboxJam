﻿using Jukebox_Heroes.Playlist;
using Jukebox_Heroes.Song;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Jukebox_Heroes.PlayerUI
{
    public class Player : IPlayer
    {
        private MediaElement mediaPlayer;
        private TextBlock timeText;
        private bool mediaPlayerIsPlaying = false, userIsDraggingSlider = false;
        private Slider slider;
        private IPlaylistData playList;
        private Image albumArt;
        private TextBlock songInfo;

        public Player(MediaElement mediaPlayer, Slider slider, TextBlock timeText, IPlaylistData playList, Image albumArt, TextBlock songInfo)
        {
            this.mediaPlayer = mediaPlayer;
            this.slider = slider;
            this.timeText = timeText;
            this.playList = playList;
            this.albumArt = albumArt;
            this.songInfo = songInfo;
            
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += tick;
            timer.Start();

            mediaPlayer.MediaEnded += OnMediaEnded;

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
            mediaPlayer.Play();
            mediaPlayerIsPlaying = true;
        }

        public void Pause_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Pause();
        }

        public void Stop_Click(object sender, RoutedEventArgs e)
        {
            Stop();
        }

        public void Stop() {
            mediaPlayer.Stop();
            mediaPlayerIsPlaying = false;
        }

        public void Next_Click(object sender, RoutedEventArgs e)
        {
            playList.nextSong();
            GetSong();
        }

        public void Previous_Click(object sender, RoutedEventArgs e)
        {
            playList.previousSong();
            GetSong();
        }

        public void GetSong()
        {
            SongData song = playList.getCurrentSong();
            
            if (song != null)
            {
                if(mediaPlayer.Source == null || !song.songUri.AbsolutePath.Equals(mediaPlayer.Source.AbsolutePath)) {
                    mediaPlayer.Source = song.songUri;
                    loadSongInfoInWindow(albumArt, songInfo);
                    mediaPlayer.Position = new TimeSpan(0);

                }
            }

        }

        public void OnMediaEnded(object sender, EventArgs e)
        {
            playList.nextSong();
            GetSong();
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

        public void setSource(Uri uri) {
            mediaPlayer.Source = uri;
        }

        public void loadSongInfoInWindow(Image albumArt, TextBlock songInfo)
        {
            SongData currentSong = playList.getCurrentSong();
            String songInfoString;

            if( currentSong == null)
            {
                return;
            }

            albumArt.Source = currentSong.ConvertAlbumArtToWPFImage().Source;

            songInfoString = currentSong.title + "\n" + currentSong.artist + "\n" +
                currentSong.album + "\n";
            for (int i = 0; i < currentSong.genres.Length; i++)
            {
                if (i == 0)
                {
                    songInfoString += currentSong.genres[i];
                }
                else
                {
                    songInfoString += ", " + currentSong.genres[i];
                }
            }
            songInfoString += "\n" + currentSong.year;

            songInfo.Text = songInfoString;
        }

    }
}