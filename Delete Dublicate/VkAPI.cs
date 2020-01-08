using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using xNet;

namespace Delete_Dublicate
{
    class VkAPI
    {
        public const string __APPID = "6121396";  //ID приложения
        private const string __VKAPIURL = "https://api.vk.com/method/";  //Ссылка для запросов
        private string _Token;  //Токен, использующийся при запросах

        public VkAPI(string AccessToken)
        {
            _Token = AccessToken;
            //_Token = "bbe48b02b118a12552f3c02f7b020557029f03a028bbe48b02b118a12552f3c02f7b020557029f03a028"; 
        }

        public string GetUserInformation(string UserId, string[] Fields)  
        {
            string Username = "";            
            HttpRequest GetInformation = new HttpRequest();
            GetInformation.AddUrlParam("user_id", UserId);
            GetInformation.AddUrlParam("access_token", _Token);
            GetInformation.AddUrlParam("v", "5.103");

            string Result = GetInformation.Get(__VKAPIURL + "users.get").ToString();
            Result = Result.Substring(13, Result.Length - 15);
            Dictionary<string, string> Dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(Result);

            Username = Dict["first_name"] + " " + Dict["last_name"];
            return Username;
        }

        public List<string> GetAudioInformation(string UserId, string[] Fields) 
        {
            List<string> traks = new List<string>(); 
            HttpRequest GetInformation = new HttpRequest();
            GetInformation.AddUrlParam("user_id", UserId);
            GetInformation.AddUrlParam("access_token", _Token);
            GetInformation.AddUrlParam("v", "5.103");

            string Result = GetInformation.Get(__VKAPIURL + "audio.get").ToString();           
            string Count = "{" + Result.Substring(Result.IndexOf("\"count\":"), Result.IndexOf(",\"items\"") - Result.IndexOf("\"count\":")) + "}";
            Dictionary<string, string> audioCount = JsonConvert.DeserializeObject<Dictionary<string, string>>(Count);
            Result = Result.Substring(35, Result.Length - 38);

            string[] stringSeparators = new string[] { "{\"artist\"" };
            string[] subResults = Result.Split(stringSeparators, StringSplitOptions.None);
           
            for (int i = 1; i < subResults.Length; i++)
            {
                subResults[i] = "{\"artist\"" + subResults[i].Substring(0, subResults[i].IndexOf(",\"duration\"")) + "}";
                Dictionary<string, string> Dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(subResults[i]);
                traks.Add(i.ToString() + ". " + Dict["artist"] + " - " + Dict["title"]);
            }
            //traks.Sort();
            traks.Add("Count: " + (short.Parse(audioCount["count"])-1).ToString());   
            return traks;
        }
    }
}
