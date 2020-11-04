using MusicalService;
using MusicalService.Entity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Core;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MusicAppUWP.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CreateSong : Page
    {
        private ImageService imageService;
        private string imageUrl;
        private string myToken;
        private ObservableCollection<Song> ListSong = new ObservableCollection<Song>();


        public CreateSong()
        {
            this.InitializeComponent();
            this.imageService = new ImageService();            
            this.getListSong();

        }

        private async void getListSong()
        {
            var _Folder = Windows.Storage.ApplicationData.Current.TemporaryFolder;

            // acquire file
            var _File = await _Folder.GetFileAsync("token.txt");

            // read content
            myToken = await Windows.Storage.FileIO.ReadTextAsync(_File);
            MusicService musicService = new MusicService();

            var listSong = await musicService.GetAllSongList(myToken);
            
            foreach (Song song in listSong)
            {
                ListSong.Add(song);
            }
        }


        private async void takePhoto_Click(object sender, RoutedEventArgs e)
        {

            FileOpenPicker fileOpenPicker = new FileOpenPicker();
            fileOpenPicker.ViewMode = PickerViewMode.List;
            fileOpenPicker.SuggestedStartLocation = PickerLocationId.Desktop;

            fileOpenPicker.FileTypeFilter.Add(".jpg");
            fileOpenPicker.FileTypeFilter.Add(".png");

            StorageFile storageFile = await fileOpenPicker.PickSingleFileAsync();

            string uploadUrl = await imageService.GetUploadUrl();

            Stream imageStream = await storageFile.OpenStreamForReadAsync();



            imageUrl = await imageService.HttpUploadFile(uploadUrl, imageStream, "myFile", "image/jpg");

            thumbnail.Source = new BitmapImage(new Uri(imageUrl));

            Debug.WriteLine(imageUrl);

        }

        public async void getTokenFromFile()
        {
            var _Folder = Windows.Storage.ApplicationData.Current.TemporaryFolder;

            // acquire file
            var _File = await _Folder.GetFileAsync("token.txt");

            // read content
            myToken = await Windows.Storage.FileIO.ReadTextAsync(_File);
            //Assert.AreEqual(_WriteThis, _ReadThis, "Contents correct");
            Debug.WriteLine(myToken);
        }

        private async void msUpload_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog nowifDialog = new ContentDialog()
            {
                Title = "Confirm Upload Music",
                PrimaryButtonText = "Ok Upload",
                CloseButtonText = "Cancel"

            };

            ContentDialogResult result = await nowifDialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {                
                var _Folder = Windows.Storage.ApplicationData.Current.TemporaryFolder;

                // acquire file
                var _File = await _Folder.GetFileAsync("token.txt");

                // read content
                myToken = await Windows.Storage.FileIO.ReadTextAsync(_File);

                var song = new Song()
                {
                    name = songName.Text,
                    singer = singger.Text,
                    author = author.Text,
                    thumbnail = imageUrl,
                    link = link.Text,                    
                };

                //Kiểm tra dữ liệu nhập
                if (songName.Text.Trim() == "")
                {
                    validMessage.Text = "Chưa nhập tên bài hát";
                }
                else if (!link.Text.Trim().EndsWith(".mp3"))
                {
                    validMessage.Text = "link MUST contain '.mp3' string";
                }else
                {
                    MusicService musicService = new MusicService();

                    Song createdSong = await musicService.CreateSong(myToken, song);

                    if (createdSong == null)
                    {
                        validMessage.Text = "Up nhạc thất bại";
                    }
                    else
                    {
                        successMessage.Text = "Up nhạc thành công";
                        successMessage.Visibility = Visibility.Visible;
                    }
                }
               
            }
        }

        private void reset_Click(object sender, RoutedEventArgs e)
        {
            songName.Text = "";
            singger.Text = "";
            author.Text = "";
            link.Text = "";
            
        }
       
    }
}
