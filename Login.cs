using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;
using static System.Windows.Forms.DataFormats;

namespace TeamProject
{
    public partial class LoginForm : Form
    {
        string strConn = "Server=127.0.0.1; Database=teamproject; uid=project; pwd=1234; Encrypt=false";
        public LoginForm()
        {
            InitializeComponent();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var db = new certification(strConn);
            SqlCommand cmd = db.GetSqlCommand();
            //cmd.CommandText 
            //if ()
            //{
            
            //    MessageBox.Show("로그인 성공");

            //}
            //    MessageBox.Show("로그인 실패");
        }

        private void btID_Click(object sender, EventArgs e)
        {
            Find_ID FID = new();
            FID.Show();
        }

        private void btPW_Click(object sender, EventArgs e)
        {
            Find_PW FPW = new();
            FPW.Show();
        }

        private void btJoin_Click(object sender, EventArgs e)
        {
            join_membership Jms= new join_membership();
            Jms.Show();
        }
    }
    
}
