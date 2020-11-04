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
    public sealed partial class Profile : Page
    {
        private string myToken;
        private ObservableCollection<Song> ListSong = new ObservableCollection<Song>();

        public Profile()
        {
            this.InitializeComponent();
            //this.getToken();
            this.getProfile();
            this.getMyListSong();
        }


        private async void getToken()
        {
            var _Folder = Windows.Storage.ApplicationData.Current.TemporaryFolder;

            // acquire file
            var _File = await _Folder.GetFileAsync("token.txt");

            // read content
            myToken = await Windows.Storage.FileIO.ReadTextAsync(_File);
            //Assert.AreEqual(_WriteThis, _ReadThis, "Contents correct");
            Debug.WriteLine(myToken);
            
        }

        public async void getProfile()
        {
            var _Folder = Windows.Storage.ApplicationData.Current.TemporaryFolder;

            // acquire file
            var _File = await _Folder.GetFileAsync("token.txt");

            // read content
            myToken = await Windows.Storage.FileIO.ReadTextAsync(_File);
            //Assert.AreEqual(_WriteThis, _ReadThis, "Contents correct");
            Debug.WriteLine("My Token: " + myToken);

            MemberService memberService = new MemberService();
            Member createdmember = await memberService.GetProfile(myToken);


            firstName.Text = createdmember.firstName;
            lastName.Text = createdmember.lastName;
            myImageProfile.Source = new BitmapImage(new Uri(createdmember.avatar));
            email.Text = createdmember.email;
            phone.Text = createdmember.phone;
            address.Text = createdmember.address;
        }

        private async void getMyListSong()
        {
            var _Folder = Windows.Storage.ApplicationData.Current.TemporaryFolder;

            // acquire file
            var _File = await _Folder.GetFileAsync("token.txt");

            // read content
            myToken = await Windows.Storage.FileIO.ReadTextAsync(_File);
            MusicService musicService = new MusicService();

            var listSong = await musicService.GetMySongList(myToken);

            foreach (Song song in listSong)
            {
                ListSong.Add(song);
            }
        }

        private void MyListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var currentSong = e.ClickedItem as Song;
            //MyMedia.Source = new Uri(currentSong.dataUrl);
            //MyMedia.Play();
            MyMedia.Source = MediaSource.CreateFromUri(new Uri(currentSong.link));
        }
    }
}
