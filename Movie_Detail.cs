﻿using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TeamProject
{
    public partial class Movie_Detail : Form
    {
        string strConn = "Server=127.0.0.1; Database=teamproject; uid=project; pwd=1234; Encrypt=false";
        string MovieTitle;
        int UseruId;
        int MovieUid;
       
        
        
        bool bookmarkstatus;


        private Main MainForm;
        public Movie_Detail(Main mainForm)
        {
            InitializeComponent();
            MainForm = mainForm;
            MovieUid = MainForm.movieuid;
            UseruId = MainForm.useruid;
        }
        public void SetMovieDetails(string movieTitle, string moviePosterUrl)
        {
            // 여기에서 영화 제목 및 포스터 URL을 사용하여 Movie_Detail form에 표시할 수 있습니다.
            // 예: movieTitleLabel.Text = movieTitle;
            // 예: moviePosterPictureBox.ImageLocation = moviePosterUrl;
            pictureBox1.ImageLocation = moviePosterUrl;
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void Movie_Detail_Load(object sender, EventArgs e)
        {
            
            Check check = new Check();
            bookmarkstatus = check.bookmarkis(MovieUid, UseruId);
            if (bookmarkstatus== true)
            {
                book.Image = Properties.Resources.bookmarkon;
            }
            else if (bookmarkstatus == false)
            {
                book.Image = Properties.Resources.bookmarkoff;
            }


            MovieTitle = check.FindMvName(MovieUid);
            labeltitle.Text = MovieTitle;
            if (MainForm.logStatus == true)
            {
                NickNameBox.Text = MainForm.userNickname;
            }

            DataViewLoad();
        }

        private void DataViewLoad()
        {
            

            certification cert = new certification(strConn);
            SqlCommand cmd = cert.GetSqlCommand();

            Check check = new Check();
            string movieTitle = labeltitle.Text;
            MovieUid = check.FindMvUid(movieTitle);

            MovieTitle = check.FindMvName(MovieUid);
            cmd.CommandText = $"SELECT u.u_nickname, r.r_rate, r.r_content, r.r_date " +
                              $"FROM review r " +
                              $"INNER JOIN project_user u ON r.u_uid = u.u_uid " +
                              $"WHERE r.MovieUID = {MovieUid}";

            SqlDataReader reader = cmd.ExecuteReader();

            DataTable dataTable = new DataTable();
            dataTable.Load(reader);

            ReviewView.DataSource = dataTable;

            // 리소스 정리
            reader.Close();
            cmd.Dispose();

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            DateTime d1 = DateTime.Now;
            if (ratebox.Text == "별점")
            {
                MessageBox.Show("별점을 입력해주세요!!");
                return;
            }

            int rate = int.Parse(ratebox.Text);

            Check check = new Check();
            

            if (reviewBox.Text.IsNullOrEmpty())
            {
                reviewBox.Text = "재밌어요";
            }
            
           
            if (check.countreview(MovieUid, UseruId) > 0)
            {
                MessageBox.Show("이미 리뷰를 등록한 영화입니다");
            }
            else
            {
                check.Addcontentt(MovieUid, UseruId, reviewBox.Text, rate, d1);
                check.UpdateAvgRate(MovieUid);
                MessageBox.Show("리뷰등록!");
            }

            DataViewLoad();



        }



    

        private void book_Click(object sender, EventArgs e)
        {
            Check check = new Check();
            if (NickNameBox.Text.IsNullOrEmpty())
            {
                MessageBox.Show("로그인하지않으면 즐겨찾기 불가!");
                return;
            }
            if (bookmarkstatus == false)
            {
                MessageBox.Show("즐겨찾기 추가");

                book.Image = Properties.Resources.bookmarkon;
               
                check.bookmarkon(MovieUid, UseruId, bookmarkstatus);
                bookmarkstatus = true;
            }
            else if (bookmarkstatus == true)
            {
                MessageBox.Show("즐겨찾기 해제");
                
                book.Image = Properties.Resources.bookmarkoff;
                check.bookmarkon(MovieUid, UseruId, bookmarkstatus);
                bookmarkstatus = false;
            }
        }
    }
}
