using DevExpress.Xpo;
using ProjectManagementSystem;
using ProjectManagementSystem.DataAccess;
using ProjectManagementSystem.Models;
using ProjectsManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ProjectsManagementSystem
{
    public partial class Login : Form
    {
        private ApplicationDbContext _db;
        private List<User> Userss;

        public Login()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            _db = new ApplicationDbContext();
            txtUserName.Focus();
            Userss = _db.Users.ToList();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            foreach (User admin in Userss)
            {
                if (admin.Email.Equals(txtUserName.Text) && admin.Password.Equals(txtpassword.Text))
                {
                    string UserLevelLogin = admin.UserRole.ToString();

                    string UserLevelNameLogin = admin.FullName;

                    HomeForm frm2 = new HomeForm();
                    frm2.UserLevel = UserLevelLogin;
                    frm2.UserLevelName = UserLevelNameLogin;

                    frm2.Show();
                    this.Hide();
                    return;
                }
            }
            MessageBox.Show("Girilen kullanıcı adı veya parola yanlış.");
            txtpassword.Clear();
            txtUserName.Focus();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void checkBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (checkBox1.Checked)
            {
                txtpassword.UseSystemPasswordChar = true;
            }
            else
            {
                txtpassword.UseSystemPasswordChar = false;
            }
        }
    }
}
