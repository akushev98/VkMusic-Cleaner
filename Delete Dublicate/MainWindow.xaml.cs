using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace Delete_Dublicate
{
    public partial class MainWindow : Window
    {
        List<string> MusicList = new List<string>();

        VkAPI _ApiRequest;
        private string _Token;  //Токен, использующийся при запросах
        private string _UserId;  //ID пользователя
        private List<string> _Response;  //Ответ на запросы
        private string _Response2;  //Ответ на запросы

        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                StreamReader ControlInf = new StreamReader("UserInf.txt");
                _Token = ControlInf.ReadLine();
                _UserId = UserId.Text;
                ControlInf.Close();
                if (_Token != null)
                {
                    _ApiRequest = new VkAPI(_Token);
                    string[] Params = { "city", "country", "photo_max" };

                    _Response2 = _ApiRequest.GetUserInformation(_UserId, Params);
                    if (_Response2 != null)
                        UserName.Content = _Response2;

                    _Response = _ApiRequest.GetAudioInformation(_UserId, Params);
                    if (_Response != null)
                    {
                        UserName.Content += "[" + _Response[_Response.Count - 1] + "]";
                        _Response.RemoveAt(_Response.Count - 1);
                        File.WriteAllLines("AudioTraksInf.txt", _Response);
                        //MusicTable.ItemsSource = _Response;
                    }
                    setTrackList("AudioTraksInf.txt");
                }                
            }
            catch { }
        }

        private void Button_GetToken_Click(object sender, EventArgs e)
        {
            AuthorizationForm GetToken = new AuthorizationForm();
            GetToken.ShowDialog();
        }

        private void Button_GetInformation_Click_1(object sender, EventArgs e)
        {
            try
            {
                StreamReader ControlInf = new StreamReader("UserInf.txt");
                _Token = ControlInf.ReadLine();
                ControlInf.Close();
                _ApiRequest = new VkAPI(_Token);
                _UserId = UserId.Text;
                string[] Params = { "city", "country", "photo_max" };
                _Response2 = _ApiRequest.GetUserInformation(_UserId, Params);
                if (_Response2 != null)
                    UserName.Content = _Response2;

                _Response = _ApiRequest.GetAudioInformation(_UserId, Params);
                if (_Response != null)
                {
                    UserName.Content += "[" + _Response[_Response.Count - 1] + "]";
                    _Response.RemoveAt(_Response.Count - 1);
                    File.WriteAllLines("AudioTraksInf.txt", _Response);
                    //MusicTable.ItemsSource = _Response;
                }
                setTrackList("AudioTraksInf.txt");
            }
            catch { }
        }



        private string LineParse(string fileText)
        {
            string str = "";
            while (fileText.First().ToString() != "\r")
            {
                str += fileText.First().ToString();
                fileText = fileText.Remove(0, 1);
            }
            return str;
        }
        private string DelStringFromFile(string fileText)
        {
            try
            {
                while (fileText.First().ToString() != "\r")
                {
                    fileText = fileText.Remove(0, 1);
                }
            }
            catch { return fileText; }
            return fileText.Remove(0, 2);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == false)
            {
                return;
            }
            string filename = openFileDialog.FileName;
            setTrackList(filename);
        }

        private void setTrackList(string AudioTraks)
        {
            Dictionary<string, int> MusicList = new Dictionary<string, int>();
            MusicList.Clear();

            string filename = AudioTraks;
            string fileText = File.ReadAllText(filename);

            while (fileText != "")
            {
                string newTrack = LineParse(fileText);
                fileText = DelStringFromFile(fileText);

                if (!MusicList.ContainsKey(newTrack))
                    MusicList.Add(newTrack, 1);
                else MusicList[newTrack]++;
            }
            MusicTable.ItemsSource = MusicList;
        }
    }
}
