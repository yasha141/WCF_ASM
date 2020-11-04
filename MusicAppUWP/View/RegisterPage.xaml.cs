using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using MusicalService;
using Windows.Media.Capture;
using MusicalService.Entity;
using Windows.UI.Xaml.Media.Imaging;
using System.Diagnostics;
using Windows.Storage;
using System.Text.RegularExpressions;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MusicAppUWP.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RegisterPage : Page
    {

        private int gender;
        private string imageUrl;
        private string birthdayString;
        private ImageService imageService;
        private MemberService memberService;
        private Member member { get; set; }
        private CameraCaptureUI captureUI;

        public RegisterPage()
        {
            this.InitializeComponent();
            this.memberService = new MemberService();
            this.captureUI = new CameraCaptureUI();
            this.imageService = new ImageService();
        }

        private async void takePhoto_Click(object sender, RoutedEventArgs e)
        {

            StorageFile photo = await captureUI.CaptureFileAsync(CameraCaptureUIMode.Photo);

            if (photo == null)
            {
                // User cancelled photo capture
                return;
            }

            //lay link upload
            string uploadUrl = await imageService.GetUploadUrl();

            //Stream imageStream = await photo.OpenStreamForReadAsync();

            imageUrl = await imageService.UploadImageStream(await photo.OpenStreamForReadAsync(), uploadUrl);
            if (imageUrl == null)
            {
                return;
            }
            myAvatar.Source = new BitmapImage(new Uri(imageUrl));
            Debug.WriteLine(imageUrl);
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            var radioButton = sender as RadioButton;
            gender = radioButton.Content as string == "Male" ? 1 : 0;
        }

        private void birthday_DateChanged(object sender, DatePickerValueChangedEventArgs e)
        {
            var datePicker = sender as DatePicker;
            birthdayString = datePicker.SelectedDate.Value.ToString("yyyy-MM-dd");
        }

        private async void register_Click(object sender, RoutedEventArgs e)
        {            

            ContentDialog nowifDialog = new ContentDialog()
            {
                Title = "Confirm register",
                PrimaryButtonText = "Ok Register",
                CloseButtonText = "Cancel"

            };

            ContentDialogResult result = await nowifDialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                if (firstName.Text.Trim() == "")
                {
                    validMessage.Text = "Chưa nhập First Name";
                } else
                if (lastName.Text.Trim() == "")
                {
                    validMessage.Text = "Chưa nhập Last Name";
                } else
                if (string.IsNullOrWhiteSpace(password.Password))
                {
                    validMessage.Text = "Password không hợp lệ";
                } else
                if (string.IsNullOrWhiteSpace(email.Text) || !Regex.IsMatch(email.Text, @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"))
                {
                    address.Text = "Email không hợp lệ hoặc đã tồn tại";
                } else {
                    var member = new Member()
                    {
                        firstName = firstName.Text,
                        lastName = lastName.Text,
                        password = password.Password,
                        address = address.Text,
                        phone = phone.Text,
                        avatar = imageUrl,
                        gender = gender,
                        email = email.Text,
                        birthday = birthdayString
                    };


                    MemberService memberService = new MemberService();

                    Member createdmember = await memberService.Register(member);

                    if (createdmember == null)
                    {
                        email.Text = "Email đã tồn tại";
                    }
                    else
                    {
                        successMessage.Visibility = Visibility.Visible;
                        successMessage.Text = "Register thành công";
                    }
                }
                
            }
            
        }

        private void toLogin_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(LoginPage));
        }

        
    }
}
