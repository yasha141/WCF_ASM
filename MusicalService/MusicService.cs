using MusicalService.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MusicalService
{
    public class MusicService
    {
        public async Task<Song> CreateSong(String token, Song song)
        {            

            // chuyển đối tượng song sang json string.
            var jsonData = JsonConvert.SerializeObject(song);
            // tạo ra đối tượng của lớp HttpClient để gửi thông tin.
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Basic {token}");

            // Đóng gói thông tin gửi đi, dán nhãn UTF-8 và application/json
            HttpContent httpContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

            Debug.WriteLine(jsonData);

            // Gửi dữ liệu bất đồng bộ, lấy dữ liệu trả về từ server.
            var responseMessage = await httpClient.PostAsync(APILink.ApiDomain + APILink.SongCreatedPath, httpContent);
            // Kiểm tra trạng thái của request.
            if (responseMessage.StatusCode == System.Net.HttpStatusCode.Created) // tạo thành công 201.
            {
                var jsonResult = responseMessage.Content.ReadAsStringAsync().Result;
                var createdSong = JsonConvert.DeserializeObject<Song>(jsonResult);
                Debug.WriteLine("Tao bai hat thanh cong");
                Debug.WriteLine(createdSong.name + createdSong.singer);
                return createdSong;

            }
            else
            {
                Debug.WriteLine("That bai");
                Debug.WriteLine(responseMessage.StatusCode.ToString());

                Console.WriteLine(responseMessage.Content.ReadAsStringAsync().Result);
                return null;
            }
        }

        public async Task<ObservableCollection<Song>> GetMySongList(String token)
        {

            // tạo ra đối tượng của lớp HttpClient để gửi thông tin.
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Basic {token}");

            // Gửi dữ liệu bất đồng bộ, lấy dữ liệu trả về từ server.
            var responseMessage = await httpClient.GetAsync(APILink.ApiDomain + APILink.SongMyListPath);

            Debug.WriteLine("Gui du lieu thanh cong " + token);

            // Kiểm tra trạng thái của request.
            if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK) // tạo thành công 200.
            {
                Debug.WriteLine("Lay List Song thanh cong");
                var jsonResult = responseMessage.Content.ReadAsStringAsync().Result;
                ObservableCollection<Song> currentListSong = JsonConvert.DeserializeObject<ObservableCollection<Song>>(jsonResult);                
                return currentListSong;
            }
            else
            {
                Debug.WriteLine("that bai");
                Console.WriteLine(responseMessage.Content.ReadAsStringAsync().Result);
                return null;
            }
        }

        public async Task<ObservableCollection<Song>> GetAllSongList(String token)
        {

            // tạo ra đối tượng của lớp HttpClient để gửi thông tin.
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Basic {token}");

            // Gửi dữ liệu bất đồng bộ, lấy dữ liệu trả về từ server.
            var responseMessage = await httpClient.GetAsync(APILink.ApiDomain + APILink.SongListPath);

            Debug.WriteLine("Gui du lieu thanh cong " + token);

            // Kiểm tra trạng thái của request.
            if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK) // tạo thành công 200.
            {
                Debug.WriteLine("Lay List Song thanh cong");
                var jsonResult = responseMessage.Content.ReadAsStringAsync().Result;
                ObservableCollection<Song> currentListSong = JsonConvert.DeserializeObject<ObservableCollection<Song>>(jsonResult);
                return currentListSong;
            }
            else
            {
                Debug.WriteLine("that bai");
                Console.WriteLine(responseMessage.Content.ReadAsStringAsync().Result);
                return null;
            }
        }
    }
}
