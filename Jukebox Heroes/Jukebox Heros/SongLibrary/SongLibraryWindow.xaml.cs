﻿using Jukebox_Heroes.Song;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Jukebox_Heroes.SongLibrary
{
    public partial class SongLibraryWindow : Window
    {
        ISongLibraryData songLibrary;
        ISongUpload songUpload;
        IPlaylistData playlist;
        public ObservableCollection<SongData> songList {
            get;
            set;
        }

        public SongLibraryWindow(ISongLibraryData songLibrary, IPlaylistData playlist) {
            InitializeComponent();

            this.songLibrary = songLibrary;
            this.songUpload = new SongUpload(songLibrary);
            this.playlist = playlist;

            syncUI();

        }

        private void syncUI() {
            songList = songLibrary.getSongList();
            Song_Library_List_View.ItemsSource = songList;
        }

        private void Song_Library_Upload_Click(object sender, RoutedEventArgs e) {
            songUpload.UploadSong();
        }

        private void Song_Library_Save_Click(object sender, RoutedEventArgs e) {
            songLibrary.saveLibrary();
        }

        private void Song_Library_Load_Click(object sender, RoutedEventArgs e) {
            songLibrary.loadLibrary();
            syncUI();
        }

        private void Song_Library_Add_Song_To_Playlist_Click(object sender, RoutedEventArgs e) {
            foreach(SongData song in Song_Library_List_View.SelectedItems) {
                playlist.addSong(song);
            }
        }

        private void Song_Library_Remove_Click(object sender, RoutedEventArgs e) {
            List<string> list = new List<string>();
            foreach(SongData song in Song_Library_List_View.SelectedItems) {
                list.Add(song.filePath);
            }

            foreach(string filePath in list) {
                songLibrary.removeSong(filePath);
            }
            
        }
    }
}
