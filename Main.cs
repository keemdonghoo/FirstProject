using Microsoft.Data.SqlClient;
using System.Drawing;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Windows.Forms;
using System;
using Newtonsoft.Json.Linq;

namespace TeamProject
{
    public partial class Main : Form
    {
        private FlowLayoutPanel flowLayoutPanel;
        public Main()
        {
            InitializeComponent();

            LoginForm lg= new LoginForm();
            lg.Show();
            LoadMovieDataAsync();
        }

        private async void LoadMovieDataAsync()
        {
            const string strConn = "Server=127.0.0.1; Database=projectuserdb; uid=sa; pwd=1234; Encrypt=false";
            string name = "한국";
            using SqlConnection conn = new SqlConnection(strConn);
            conn.Open();
            using SqlCommand cmd = new SqlCommand("SELECT Title FROM MovieList WHERE Country = @name", conn);
            SqlParameter parameter = new SqlParameter("@name", System.Data.SqlDbType.VarChar);
            parameter.Value = name;
            cmd.Parameters.Add(parameter);

            using SqlDataReader reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                string title = reader.GetString(0);
                string imageUrl = await GetPosterUrlAsync(title);
                AddMovieItem(title, imageUrl);
            }
        }

        public async Task<string> GetPosterUrlAsync(string title)
        {
            string clientId = "FXqKsjIoNLVFmlkXvTzt";
            string clientSecret = "RZVA7QWEda";

            string apiUrl = $"https://openapi.naver.com/v1/search/movie.json?query={title}";

            using HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-Naver-Client-Id", clientId);
            client.DefaultRequestHeaders.Add("X-Naver-Client-Secret", clientSecret);

            HttpResponseMessage response = await client.GetAsync(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                JObject json = JObject.Parse(result);
                JArray items = (JArray)json["items"];

                if (items.Count > 0)
                {
                    string imageUrl = (string)items[0]["image"];
                    return imageUrl;
                }
            }

            return null; // 영화 포스터를 찾을 수 없는 경우
        }

        private void AddMovieItem(string title, string posterUrl)
        {
            var panel = new Panel
            {
                Size = new Size(120, 200),
                Margin = new Padding(5)
            };

            var pictureBox = new PictureBox
            {
                Size = new Size(120, 180),
                Location = new Point(0, 0),
                ImageLocation = posterUrl,
                SizeMode = PictureBoxSizeMode.StretchImage
            };
            panel.Controls.Add(pictureBox);

            var titleLabel = new Label
            {
                Text = title,
                Location = new Point(0, 180),
                AutoSize = false,
                Size = new Size(120, 20),
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Arial", 10, FontStyle.Bold)
            };
            panel.Controls.Add(titleLabel);

            fLPMain.Controls.Add(panel);

        }
    }
}