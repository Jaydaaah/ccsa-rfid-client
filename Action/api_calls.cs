using ccsa_rfid_client.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using static System.Net.Mime.MediaTypeNames;

namespace ccsa_rfid_client.Action
{
    class LoginResponse
    {
        [JsonPropertyName("acc_id")]
        public string? AccId { get; set; }

        [JsonPropertyName("attendance_id")]
        public string? AttendanceId { get; set; }
    }

    class RetrieveResponse
    {
        [JsonPropertyName("ccsaID")]
        public string? CcsaID { get; set; }

        [JsonPropertyName("stdName")]
        public string? StdName { get; set; }

        [JsonPropertyName("course")]
        public string? Course { get; set; }
    }

    class JsonConfig
    {
        [JsonPropertyName("BaseAddress")]
        public string BaseAddress { get; set; } = "localhost";

        [JsonPropertyName("Port")]
        public int Port { get; set; } = 8080;
    }

    internal static class api_calls
    {
        private static Uri BaseAddress = new Uri($"http://localhost:8080/");

        private static string temp = "{\\r\\n  \"BaseAddress\": \"localhost\",\\r\\n  \"Port\": 8080\\r\\n}";

        static api_calls()
        {
            try
            {
                string jsonString = File.ReadAllText("./Config/client-config.json");
                var jsonObject = JsonSerializer.Deserialize<JsonConfig>(jsonString);
                if (jsonObject != null)
                {
                    BaseAddress = new Uri($"http://{jsonObject.BaseAddress}:{jsonObject.Port}/");
                }
            }
            catch
            {
                File.WriteAllText("./Config/client-config.json", temp);
                var jsonObject = JsonSerializer.Deserialize<JsonConfig>(temp);
                if (jsonObject != null)
                {
                    BaseAddress = new Uri($"http://{jsonObject.BaseAddress}:{jsonObject.Port}/");
                }
            }
        }


        internal static async void SendRfid(string rfidTag, Action<Account?> callback)
        {
            var login_response = await CallFetch(rfidTag);
            if (login_response != null && login_response.AccId != null && login_response.AttendanceId != null)
            {
                var retrieve = await CallRetrieve(login_response.AccId);

                if (retrieve != null && retrieve.CcsaID != null && retrieve.StdName != null && retrieve.Course != null)
                {
                    var image = await CallRetrieveImage(login_response.AccId);
                    var account = new Account(
                        login_response.AccId,
                        login_response.AttendanceId,
                        rfidTag,
                        retrieve.CcsaID,
                        retrieve.StdName,
                        retrieve.Course,
                        image
                        );
                    callback(account);
                    return;
                }
            }
            callback(null);
            return;
        }

        private static async Task<BitmapImage?> CallRetrieveImage(string _id)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = BaseAddress;

                HttpResponseMessage response = await client.GetAsync($"/retrieve/{_id}/image");
                if (response.IsSuccessStatusCode)
                {
                    var imageBytes = await response.Content.ReadAsByteArrayAsync();
                    BitmapImage bitmapImage = new BitmapImage();
                    using (var stream = new System.IO.MemoryStream(imageBytes))
                    {
                        bitmapImage.BeginInit();
                        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                        bitmapImage.StreamSource = stream;
                        bitmapImage.EndInit();
                        bitmapImage.Freeze(); // Important to freeze the BitmapImage for cross-thread access
                    };
                    return bitmapImage;
                }
                return null;
            }
        }

        private static async Task<RetrieveResponse?> CallRetrieve(string _id)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = BaseAddress;

                HttpResponseMessage response = await client.GetAsync($"/retrieve/{_id}");

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    RetrieveResponse? parsedResponse = JsonSerializer.Deserialize<RetrieveResponse>(responseBody);
                    return parsedResponse;
                }
                else
                {
                    return null;
                }
            }
        }

        private static async Task<LoginResponse?> CallFetch(string rfidTag)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = BaseAddress;
                var json = JsonSerializer.Serialize(new {
                    computer_name = Environment.MachineName
                });
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync($"/login/{rfidTag}", content);

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    LoginResponse? parsedResponse = JsonSerializer.Deserialize<LoginResponse>(responseBody);
                    return parsedResponse;
                }
                else
                {
                    return null;
                }
            }
        }

        internal static async void CallLogout(Account account)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = BaseAddress;
                HttpResponseMessage response = await client.PostAsync($"/logout/{account.AttendanceId}", null);
            }
        }

        internal static async void CallAddFile(Account? account, string created_file)
        {
            if (account != null)
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = BaseAddress;

                    var json = JsonSerializer.Serialize(new {
                        created_file = created_file
                    });
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PutAsync($"/activity/files/{account.AttendanceId}", content);
                }
            }
        }

        internal static async void PingServer(Action<bool> callback)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = BaseAddress;
                    HttpResponseMessage response = await client.GetAsync("/");
                    callback(response.IsSuccessStatusCode);
                }
                catch
                {
                    callback(false);
                }
            }
        }
    }
}
