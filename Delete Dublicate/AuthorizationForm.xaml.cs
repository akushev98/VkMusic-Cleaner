using System;
using System.IO;
using System.Windows.Forms;
using System.Windows.Navigation;

namespace Delete_Dublicate
{
    public partial class AuthorizationForm 
    {
        public AuthorizationForm()
        {
            InitializeComponent();
        }

        private void AuthorizationForm_Load(object sender, EventArgs e)
        {
            GetToken.LoadCompleted += GetToken_DocumentCompleted;
            GetToken.Navigate("https://oauth.vk.com/authorize?client_id=" + VkAPI.__APPID + "&redirect_uri=https://api.vk.com/blank.html&display=page&scope=offline&response_type=token&revoke=1");
        }

        private void GetToken_DocumentCompleted(object sender, NavigationEventArgs e)
        {
            if (GetToken.Source.ToString().IndexOf("access_token=") != -1)
            {
                GetUserToken();
            }
        }

        private void GetUserToken()
        {
            char[] Symbols = { '=', '&' };
            string[] URL = GetToken.Source.ToString().Split(Symbols);
            File.WriteAllText("UserInf.txt", URL[1] + "\n");
            File.AppendAllText("UserInf.txt", URL[5]);            
        }

        private void GetToken_DocumentCompleted_1(object sender, WebBrowserDocumentCompletedEventArgs e) { }

    }
}
