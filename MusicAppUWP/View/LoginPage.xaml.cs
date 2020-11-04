using MusicalService;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MusicAppUWP.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        private string myToken;
        StorageFile file;


        public LoginPage()
        {
            this.InitializeComponent();
        }
        public async void tokenSave()
        {
            var _Folder = Windows.Storage.ApplicationData.Current.TemporaryFolder;

            // acquire file
            var _File = await _Folder.CreateFileAsync("token.txt", Windows.Storage.CreationCollisionOption.ReplaceExisting);
            //Assert.IsNotNull(_File, "Acquire File");
            // write content
            //var _WriteThis = "Hello World"; // token
            await Windows.Storage.FileIO.WriteTextAsync(_File, myToken);
            // read content
            var _ReadThis = await Windows.Storage.FileIO.ReadTextAsync(_File);
            //Assert.AreEqual(_WriteThis, _ReadThis, "Contents correct");
            Debug.WriteLine(_ReadThis);
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

        private async void login_Click(object sender, RoutedEventArgs e)
        {
            string email = myEmail.Text;
            string password = myPassword.Password;


            MemberService memberService = new MemberService();


            myToken = await memberService.Login(email, password);

            if (myToken == null || email.Trim() == "" || password.Trim() == "" )
            {
                message.Text = "Thông tin đăng nhập không đúng";                
            } else
            {
                this.tokenSave();
                this.Frame.Navigate(typeof(MyLayout));
            }

        }

        private void toRegister_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(RegisterPage));
        }
    }
}
