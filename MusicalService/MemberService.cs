using MusicalService.Entity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MusicalService
{
    public class MemberService
    {
        //public static void ShowHeaders(HttpHeaders headers)
        //{
        //    Console.WriteLine("Các Header:");
        //    foreach (var header in headers)
        //    {
        //        string value = string.Join(" ", header.Value);
        //        Console.WriteLine($"{header.Key,20} : {value}");
        //    }
        //    Console.WriteLine();
        //}

        public async Task<string> Login(string email, string password)
        {
            var loginInformation = new
            {
                email = email,
                password = password
            };

            // chuyển đối tượng member sang json string.
            var jsonData = JsonConvert.SerializeObject(loginInformation);
            // tạo ra đối tượng của lớp HttpClient để gửi thông tin.
            var httpClient = new HttpClient();
            // Đóng gói thông tin gửi đi, dán nhãn UTF-8 và application/json
            HttpContent httpContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            // Gửi dữ liệu bất đồng bộ, lấy dữ liệu trả về từ server.
            var responseMessage = await httpClient.PostAsync(APILink.ApiDomain + APILink.MemberLoginPath, httpContent);
            // Kiểm tra trạng thái của request.
            if (responseMessage.StatusCode == System.Net.HttpStatusCode.Created) // tạo thành công 201.
            {
                var jsonResult = responseMessage.Content.ReadAsStringAsync().Result;
                JObject jsonObject = JObject.Parse(jsonResult);

                Debug.WriteLine(jsonObject);

                return (string)jsonObject["token"];

            }
            else
            {
                Debug.WriteLine("ko thanh cong");

                Console.WriteLine(responseMessage.Content.ReadAsStringAsync().Result);
                return null;
            }
        }

        public async Task<Member> GetProfile(String token)
        {

            // tạo ra đối tượng của lớp HttpClient để gửi thông tin.
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Basic {token}");

            // Gửi dữ liệu bất đồng bộ, lấy dữ liệu trả về từ server.
            var responseMessage = await httpClient.GetAsync(APILink.ApiDomain + APILink.MemberInformationPath);
            // Kiểm tra trạng thái của request.
            if (responseMessage.StatusCode == System.Net.HttpStatusCode.Created) // tạo thành công 201.
            {
                var jsonResult = responseMessage.Content.ReadAsStringAsync().Result;                
                Member currentMember = JsonConvert.DeserializeObject<Member>(jsonResult);
                Debug.WriteLine(jsonResult);
                return currentMember;
            }
            else
            {
                Console.WriteLine(responseMessage.Content.ReadAsStringAsync().Result);
                return null;
            }
        }

        public async Task<Member> Register(Member member)
        {
            // chuyển đối tượng member sang json string.
            var jsonData = JsonConvert.SerializeObject(member);
            // tạo ra đối tượng của lớp HttpClient để gửi thông tin.
            var httpClient = new HttpClient();
            // Đóng gói thông tin gửi đi, dán nhãn UTF-8 và application/json
            HttpContent httpContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

            Debug.WriteLine(jsonData);

            // Gửi dữ liệu bất đồng bộ, lấy dữ liệu trả về từ server.
            var responseMessage = await httpClient.PostAsync(APILink.ApiDomain + APILink.MemberRegisterPath, httpContent);
            // Kiểm tra trạng thái của request.
            if (responseMessage.StatusCode == System.Net.HttpStatusCode.Created) // tạo thành công 201.
            {
                var jsonResult = responseMessage.Content.ReadAsStringAsync().Result;
                var createdMember = JsonConvert.DeserializeObject<Member>(jsonResult);
                Debug.WriteLine("Thanh cong");
                return createdMember;

            }
            else
            {
                Debug.WriteLine("That bai");
                Debug.WriteLine(responseMessage.StatusCode.ToString());

                Console.WriteLine(responseMessage.Content.ReadAsStringAsync().Result);
                return null;
            }
        }

        //public async void tokenSave()
        //{
        //    var _Folder = Windows.Storage.ApplicationData.Current.TemporaryFolder;

        //    // acquire file
        //    var _File = await _Folder.CreateFileAsync("token.txt", Windows.Storage.CreationCollisionOption.ReplaceExisting);
        //    //Assert.IsNotNull(_File, "Acquire File");
        //    // write content
        //    var _WriteThis = "Hello World"; // token
        //    await Windows.Storage.FileIO.WriteTextAsync(_File, _WriteThis);
        //    // read content
        //    var _ReadThis = await Windows.Storage.FileIO.ReadTextAsync(_File);
        //    //Assert.AreEqual(_WriteThis, _ReadThis, "Contents correct");
        //    Debug.WriteLine(_ReadThis);
        //}
    }
}
