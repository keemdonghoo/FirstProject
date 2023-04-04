using Microsoft.Data.SqlClient;
using System.Drawing;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Windows.Forms;
using System;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Text;
using System.Xml;
using MySqlX.XDevAPI;
using TMDbLib.Client;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.Search;
using Microsoft.VisualBasic.Devices;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace TeamProject
{
    public partial class Main : Form
    {

        public bool logStatus { get; set; }
        public string userNickname { get; set; }
        public string userid { get; set; }
        public int useruid { get; set; }
        public int movieuid { get; set; }


        const string strConn = "Server=127.0.0.1; Database=teamproject; uid=project; pwd=1234; Encrypt=false";
        SqlConnection conn;
        SqlDataReader reader;
        private FlowLayoutPanel flowLayoutPanel;
        int rank;

        public Main()
        {
            InitializeComponent();
            LoadMovieDataAsync();
            CB_Category.SelectedIndex = 0;
        }

        private async void LoadMovieDataAsync()
        {
            //Admin_Page adminPage = new Admin_Page();
            //adminPage.Show();


            const string strConn = "Server=127.0.0.1; Database=teamproject; uid=project; pwd=1234; Encrypt=false";
            string Country = "한국";

            using SqlConnection conn = new SqlConnection(strConn);
            conn.Open();
            using SqlCommand cmd = new SqlCommand("SELECT TOP 50 Title FROM MovieList WHERE Country = @Country", conn);
            SqlParameter parameter = new SqlParameter("@Country", System.Data.SqlDbType.VarChar);
            parameter.Value = Country;

            cmd.Parameters.Add(parameter);
            using SqlDataReader reader = await cmd.ExecuteReaderAsync();
            rank = 0;
            while (await reader.ReadAsync())
            {
                string title = reader.GetString(0);
                string imageUrl = await GetPosterUrlAsync(title);
                AddMovieItem(title, imageUrl);
            }

        }

        public async Task<string> GetPosterUrlAsync(string title)
        {
            //https://api.themoviedb.org/3/discover/movie?api_key=9587124340afc34dae9ecf63d2710f6f&language=ko-KR
            //TMDbClient client = new TMDbClient("9587124340afc34dae9ecf63d2710f6f");
            //Movie movie = client.GetMovieAsync(299536).Result;
            ////Console.WriteLine($"Movie name: {movie.Title}\n\n");

            //SearchContainer<SearchMovie> results = client.SearchMovieAsync("어벤져스").Result;

            //Console.WriteLine($"Got {results.Results.Count:N0} of {results.TotalResults:N0} results");
            //foreach (SearchMovie result in results.Results)
            //    Console.WriteLine($"| TItle: {result.Title,-45} | Poster Path: {result.PosterPath,-35} |{result.Id}");
            return null;
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
            rank++;
            var titleLabel = new Label
            {
                Text = $"[{rank}] {title}",
                Location = new Point(0, 180),
                AutoSize = true,
                AutoEllipsis = false,
                Size = new Size(120, 40),
                MaximumSize = new Size(120, 40),
                TextAlign = ContentAlignment.TopCenter,
                Font = new Font("Arial", 10, FontStyle.Bold)
            };
            titleLabel.DoubleClick += TitleLabel_DoubleClick;
            panel.Controls.Add(titleLabel);

            fLPMain.Controls.Add(panel);

        }
        private void TitleLabel_DoubleClick(object sender, EventArgs e)
        {
            Check check1 = new();
            if (sender is Label titleLabel)
            {
                string movieTitle = titleLabel.Text;
                movieuid = check1.FindMvUid(movieTitle);   
                
            }
        }
   


        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (btnLogin.Text == "로그아웃")
            {
                MessageBox.Show("로그아웃되었습니다.");
                this.Close();
                Main main = new Main();
                main.Main_Load(sender, e);
                main.Show();
                return;
            }
            LoginForm lg = new(this);
            lg.Show();

        }

        private void Main_Load(object sender, EventArgs e)
        {
            logStatus = false;
        }

        public void Main_Load_1(object sender, EventArgs e)
        {
            logStatus = true;
            btnLogin.Text = "로그아웃";
            label_id.Text = userid;
            label_nn.Text = userNickname;
            mypage.Visible = true;


        }



        private async void CB_Category_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            //오름차순
            //내림차순
            //매출순위
            //개봉일자
            bool startDTPChanged = false;
            bool endDTPChanged = false;
            dTPStart.ValueChanged += (sender, e) => startDTPChanged = true;
            dTPEnd.ValueChanged += (sender, e) => endDTPChanged = true;
            string orderByColumn;
            switch (CB_Category.SelectedIndex)
            {
                case 0: 
                    orderByColumn = "ReleaseDate ASC";
                    break;
                case 1: 
                    orderByColumn = "ReleaseDate DESC";
                    break;
                case 2: 
                    orderByColumn = "Sales";
                    break;
                case 3: 
                    orderByColumn = "ReleaseDate";
                    break;
                default:
                    orderByColumn = "ReleaseDate ASC";
                    break;

            }
            
                DateTime startDate = dTPStart.Value;
                DateTime endDate = dTPEnd.Value;
                GetDataAndDisplay(startDate, endDate, orderByColumn);
                return;
        }

        private async void GetDataAndDisplay(DateTime startDate, DateTime endDate, string orderByColumn)
        {
            const string strConn = "Server=127.0.0.1; Database=teamproject; uid=project; pwd=1234; Encrypt=false";

            using SqlConnection conn = new SqlConnection(strConn);
            await conn.OpenAsync();

            string query = $"SELECT TOP 50 Title FROM MovieList WHERE ReleaseDate BETWEEN @StartDate AND @EndDate ORDER BY {orderByColumn}";
            using SqlCommand cmd = new SqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@StartDate", startDate);
            cmd.Parameters.AddWithValue("@EndDate", endDate);

            using SqlDataReader reader = await cmd.ExecuteReaderAsync();
            fLPMain.Controls.Clear();
            rank = 0;
            while (await reader.ReadAsync())
            {
                string title = reader.GetString(0);
                string imageUrl = await GetPosterUrlAsync(title);
                AddMovieItem(title, imageUrl);
            }
        }

        private async void btnSearch_Click(object sender, EventArgs e)
        {
            //paramBeginDate.Value = dtBegin.Value.ToString("yyyy-MM-dd");
            if (txtName.Text.Length == 0)
            {
                MessageBox.Show("1글자 이상 검색하세요");
                return;
            }

            const string strConn = "Server=127.0.0.1; Database=teamproject; uid=project; pwd=1234; Encrypt=false";
            string name = txtName.Text;
            using SqlConnection conn = new SqlConnection(strConn);
            conn.Open();

            bool startDatePickerChanged = false;
            bool endDatePickerChanged = false;
            dTPStart.ValueChanged += (sender, e) => startDatePickerChanged = true;
            dTPEnd.ValueChanged += (sender, e) => endDatePickerChanged = true;
            btnSearch.Click += async (sender, e) =>
            {
                if (startDatePickerChanged && endDatePickerChanged)
                {
                    DateTime startDate = dTPStart.Value;
                    DateTime endDate = dTPEnd.Value;

                    using SqlCommand cmd = new SqlCommand("SELECT Title FROM MovieList WHERE ReleaseDate BETWEEN @startDate AND @endDate", conn);
                    SqlParameter startParam = new SqlParameter("@startDate", System.Data.SqlDbType.Date);
                    startParam.Value = startDate;
                    cmd.Parameters.Add(startParam);

                    SqlParameter endParam = new SqlParameter("@endDate", System.Data.SqlDbType.Date);
                    endParam.Value = endDate;
                    cmd.Parameters.Add(endParam);

                    using SqlDataReader reader = await cmd.ExecuteReaderAsync();
                    fLPMain.Controls.Clear();
                    rank = 0;
                    while (await reader.ReadAsync())
                    {
                        string title = reader.GetString(0);
                        string imageUrl = await GetPosterUrlAsync(title);
                        AddMovieItem(title, imageUrl);
                    }
                    return;
                }
            };

            using SqlCommand cmd = new SqlCommand("SELECT TOP 50 Title FROM MovieList WHERE Title LIKE @name", conn);
            SqlParameter parameter = new SqlParameter("@name", System.Data.SqlDbType.VarChar);
            parameter.Value = "%" + name + "%";
            cmd.Parameters.Add(parameter);
            using SqlDataReader reader = await cmd.ExecuteReaderAsync();
            fLPMain.Controls.Clear();
            rank = 0;
            while (await reader.ReadAsync())
            {
                string title = reader.GetString(0);
                string imageUrl = await GetPosterUrlAsync(title);
                AddMovieItem(title, imageUrl);
            }
        }

        private void txtName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSearch_Click(sender, e);
            }
        }

        private void Main_Load_2(object sender, EventArgs e)
        {

        }

        private void mypage_Click(object sender, EventArgs e)
        {
            Check chk = new();
            useruid = chk.FindUid(label_id.Text);
            Admin_Page page = new Admin_Page(this);
            page.Show();
            page.Close();
            MyPage mypage = new MyPage(page,this);
            mypage.Show();

        }


    }
}