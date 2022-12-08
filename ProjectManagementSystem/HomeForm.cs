using DevExpress.Mvvm.Native;
using DevExpress.Xpo;
using DevExpress.XtraRichEdit.Import.Html;
using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.DataAccess;
using ProjectsManagementSystem;
using ProjectsManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Windows.Forms;



namespace ProjectManagementSystem
{
    public partial class HomeForm : DevExpress.XtraEditors.XtraForm
    {
        private ApplicationDbContext _db;
        List<User> Users;
        List<User> UserForTeamMembers;
        List<Project> ProjectsMilestones;
        List<User> UsersMilestones;
        List<Milestone> MilestoneForTask;
        IList<Project> Projects;
        public BindingList<Project> DataSource { get; private set; }
        public HomeForm()
        {
            InitializeComponent();

               }
        protected  override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            _db = new ApplicationDbContext();


            SqlConnection con = new SqlConnection("Server=.;Database=ProjectManagement;Trusted_Connection=True;");
            SqlCommand com = new SqlCommand("Select * from dbo.Users", con);
            SqlDataAdapter d = new SqlDataAdapter(com);
            System.Data.DataTable dt = new System.Data.DataTable();
            d.Fill(dt);
            dataGridView_Users.DataSource = dt;


            for (int i = tabControl1.TabPages.Count - 1; i >= 0; i--)
            {
                tabControl1.TabPages[i].PageVisible = false;
            }

            txt_NowUser.Text = UserLevel;
            txt_NowUserName.Text = UserLevelName;



            refreshCombobox();
        }

        public void reflesh_TreeVieww()
        {

            //treeView1.ExpandAll();
            treeView1.Nodes.Clear();

            System.Data.DataTable dtt = new System.Data.DataTable();
            SqlDataAdapter pro = new SqlDataAdapter("Select * from Projects", cn);
            pro.Fill(dtt);

            System.Data.DataTable dttt = new System.Data.DataTable();
            SqlDataAdapter mil = new SqlDataAdapter("Select * from Milestones", cn);
            mil.Fill(dttt);

            System.Data.DataTable dtttt = new System.Data.DataTable();
            SqlDataAdapter tas = new SqlDataAdapter("Select * from Tasks", cn);
            tas.Fill(dtttt);

            treeView1.Nodes.Add("Projects");

            foreach (DataRow dr in dtt.Rows)
            {
                //treeView1.Nodes.Add(dr["Name"].ToString());

                TreeNode nodd = new TreeNode(dr["Name"].ToString());//Porje gelecek


                foreach (DataRow drr in dttt.Rows)
                {
                    if (drr["ProjectId"].ToString().Equals(dr["Id"].ToString()))
                    {
                        nodd.Nodes.Add(drr["Name"].ToString());//Kilometre Taşı
                    }
                }
                treeView1.Nodes.Add(nodd);
            }

            string nameMil = "1";
            int MilestoneCurrentId = 0;
            foreach (TreeNode node in treeView1.Nodes)
            {
                foreach (TreeNode nodee in node.Nodes)
                {
                    foreach (DataRow drr in dttt.Rows)
                    {
                        if (nodee.Text.Equals(drr["Name"]))
                        {
                            MilestoneCurrentId = Int32.Parse(drr["Id"].ToString());
                            foreach (DataRow TaskId in dtttt.Rows)
                            {
                                if (MilestoneCurrentId.Equals(TaskId["MilestoneId"]))
                                {
                                    nodee.Nodes.Add(TaskId["Subject"].ToString());//tasklar ekleniyor
                                }
                            }
                        }
                    }
                }
            }

            refreshTreeViewColor();

        }


        string CurrentNode;
        string CurrentNodeeId = "0";  
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            System.Data.DataTable dtt = new System.Data.DataTable();
            SqlDataAdapter pro = new SqlDataAdapter("Select * from Projects", cn);
            pro.Fill(dtt);

            System.Data.DataTable dttt = new System.Data.DataTable();
            SqlDataAdapter mil = new SqlDataAdapter("Select * from Milestones", cn);
            mil.Fill(dttt);

            System.Data.DataTable dtttt = new System.Data.DataTable();
            SqlDataAdapter tas = new SqlDataAdapter("Select * from Tasks", cn);
            tas.Fill(dtttt);

            txt_NodeName.Text = treeView1.SelectedNode.Text;

            foreach (DataRow NodePro in dtt.Rows)
            {
                if (treeView1.SelectedNode.Text.Equals(NodePro["Name"]))
                {
                    CurrentNodeeId = NodePro["Id"].ToString();

                    txt_NodeType.Text = "Project";
                    txt_StartDate.Text = NodePro["EstimatedStartTime"].ToString();
                    txt_FinishDate.Text = NodePro["EstimatedFinishTime"].ToString();

                    string a = NodePro["ProjectState"].ToString();
                    if (a.Equals("1")) { cmb_NodeStatus.Text = "NotStarted"; }
                    else if (a.Equals("2")) { cmb_NodeStatus.Text = "InProgress"; }
                    else if (a.Equals("3")) { cmb_NodeStatus.Text = "Completed"; }
                    else if (a.Equals("4")) { cmb_NodeStatus.Text = "Deferred"; }

                    CurrentNode = "Project";
                    break;
                }
                foreach (DataRow NodeMil in dttt.Rows)
                {

                    if (treeView1.SelectedNode.Text.Equals(NodeMil["Name"]))
                    {

                        CurrentNodeeId = NodeMil["Id"].ToString();

                        txt_NodeType.Text = "Milestones";

                        txt_StartDate.Text = NodeMil["StartTime"].ToString();
                        txt_FinishDate.Text = NodeMil["FinishTime"].ToString();

                        string a = NodeMil["MilestoneState"].ToString();
                        if (a.Equals("1")) { cmb_NodeStatus.Text = "NotStarted"; }
                        else if (a.Equals("2")) { cmb_NodeStatus.Text = "InProgress"; }
                        else if (a.Equals("3")) { cmb_NodeStatus.Text = "Completed"; }
                        else if (a.Equals("4")) { cmb_NodeStatus.Text = "Deferred"; }

                        CurrentNode = "Milestones";
                        break;
                    }
                    foreach (DataRow ModTask in dtttt.Rows)
                    {
                        if (treeView1.SelectedNode.Text.Equals(ModTask["Subject"]))
                        {

                            CurrentNodeeId = ModTask["Id"].ToString();

                            txt_NodeType.Text = "Tasks";
                            txt_StartDate.Text = ModTask["StartTime"].ToString();
                            txt_FinishDate.Text = ModTask["FinishTime"].ToString();

                            string a = ModTask["Status"].ToString();
                            if (a.Equals("1")) { cmb_NodeStatus.Text = "NotStarted"; }
                            else if (a.Equals("2")) { cmb_NodeStatus.Text = "InProgress"; }
                            else if (a.Equals("3")) { cmb_NodeStatus.Text = "Completed"; }
                            else if (a.Equals("4")) { cmb_NodeStatus.Text = "Deferred"; }


                            CurrentNode = "Tasks";
                            break;

                        }
                    }
                }
            }



            refreshCombobox();
            //refreshTreeViewColor();


            //txt_NodeName.Text = treeView1.SelectedNode.Text;

        }
        private void btn_delete_Click(object sender, EventArgs e){
            if (CurrentNode.Equals("Project"))
            {
                var pro = _db.Projects.FirstOrDefault(x => x.Id == Int32.Parse(CurrentNodeeId));
                _db.Projects.Remove(pro);
                MessageBox.Show("Proje Silindi");


            }
            else if (CurrentNode.Equals("Milestones"))
            {
                var miles = _db.Milestones.FirstOrDefault(x => x.Id == Int32.Parse(CurrentNodeeId));
                _db.Milestones.Remove(miles);
                MessageBox.Show("Kilometre Taşı Silindi");

            }
            else if (CurrentNode.Equals("Tasks"))
            {
                
                var task = _db.Tasks.FirstOrDefault(x => x.Id == Int32.Parse(CurrentNodeeId));
                _db.Tasks.Remove(task);
                MessageBox.Show("Task Silindi");
            }
            reflesh_TreeVieww();
        }
        private void btn_UpdateNode_Click(object sender, EventArgs e)
        {
            

            if (CurrentNode.Equals("Project"))
            {


                var Project = _db.Projects.Where(u => u.Id == Int32.Parse(CurrentNodeeId)).First();
                Project.Name = txt_NodeName.Text;
                Project.ProjectState = cmb_NodeStatus.Text.Equals("NotStarted") ? ProjectAndTaskState.NotStarted : cmb_NodeStatus.Text.Equals("InProgress") ? ProjectAndTaskState.InProgress : cmb_NodeStatus.Text.Equals("Completed") ? ProjectAndTaskState.Completed : ProjectAndTaskState.Deferred;

                if (cmb_NodeStatus.Text == "Completed")
                {
                    Project.FinishTime = DateTime.Now;   
                }else if(cmb_NodeStatus.Text == "InProgress")
                {
                    Project.StartTime = DateTime.Now;
                }

                _db.Update(Project);
                refreshCombobox();

            }
            else if (CurrentNode.Equals("Milestones"))
            {
                var Milestone = _db.Milestones.Where(u => u.Id == Int32.Parse(CurrentNodeeId)).First();
                Milestone.Name = txt_NodeName.Text;
                Milestone.MilestoneState = cmb_NodeStatus.Text.Equals("NotStarted") ? ProjectAndTaskState.NotStarted : cmb_NodeStatus.Text.Equals("InProgress") ? ProjectAndTaskState.InProgress : cmb_NodeStatus.Text.Equals("Completed") ? ProjectAndTaskState.Completed : ProjectAndTaskState.Deferred;
                _db.Update(Milestone);
                refreshCombobox();

            }
            else if (CurrentNode.Equals("Tasks"))
            {
                var Task = _db.Tasks.Where(u => u.Id == Int32.Parse(CurrentNodeeId)).First();
                Task.Subject = txt_NodeName.Text;
                Task.Status = cmb_NodeStatus.Text.Equals("NotStarted") ? ProjectAndTaskState.NotStarted : cmb_NodeStatus.Text.Equals("InProgress") ? ProjectAndTaskState.InProgress : cmb_NodeStatus.Text.Equals("Completed") ? ProjectAndTaskState.Completed : ProjectAndTaskState.Deferred;

                _db.Update(Task);
                refreshCombobox();
            }

        }

        private void refreshTreeViewColor()
        {
            System.Data.DataTable dtt = new System.Data.DataTable();
            SqlDataAdapter pro = new SqlDataAdapter("Select * from Projects", cn);
            pro.Fill(dtt);

            System.Data.DataTable dttt = new System.Data.DataTable();
            SqlDataAdapter mil = new SqlDataAdapter("Select * from Milestones", cn);
            mil.Fill(dttt);

            System.Data.DataTable dtttt = new System.Data.DataTable();
            SqlDataAdapter tas = new SqlDataAdapter("Select * from Tasks", cn);
            tas.Fill(dtttt);

            foreach (TreeNode node in treeView1.Nodes)
            {
                foreach (DataRow NodePro in dtt.Rows)
                {
                    if (node.Text.Equals(NodePro["Name"]) & NodePro["ProjectState"].ToString() == "4")
                    {

                        node.BackColor = Color.Red;
                    }else if (node.Text.Equals(NodePro["Name"]) & NodePro["ProjectState"].ToString() == "2")
                    {
                        node.BackColor = Color.Yellow;
                    }
                    else if (node.Text.Equals(NodePro["Name"]) & NodePro["ProjectState"].ToString() == "3")
                    {
                        node.BackColor = Color.Green;
                    }
                    
                }

                foreach (TreeNode nodee in node.Nodes)
                {

                    foreach (DataRow NodeMil in dttt.Rows)
                    {
                        if (nodee.Text.Equals(NodeMil["Name"]) & NodeMil["MilestoneState"].ToString() == "4")
                        {
                            nodee.BackColor = Color.Red;
                        }else if (nodee.Text.Equals(NodeMil["Name"]) & NodeMil["MilestoneState"].ToString() == "2")
                        {
                            nodee.BackColor = Color.Yellow;
                        }
                        else if (nodee.Text.Equals(NodeMil["Name"]) & NodeMil["MilestoneState"].ToString() == "3")
                        {
                            nodee.BackColor = Color.Green;
                        }
                    }
                    foreach (TreeNode nodeer in nodee.Nodes)
                    {
                        foreach (DataRow ModTask in dtttt.Rows)
                        {
                            if (nodeer.Text.Equals(ModTask["Subject"]) & ModTask["Status"].ToString() == "4")
                            {
                                nodeer.BackColor = Color.Red;
                            }else if (nodeer.Text.Equals(ModTask["Subject"]) & ModTask["Status"].ToString() == "2")
                            {
                                nodeer.BackColor = Color.Yellow;
                            }else if (nodeer.Text.Equals(ModTask["Subject"]) & ModTask["Status"].ToString() == "3")
                            {
                                nodeer.BackColor = Color.Green;
                            }
                        }
                    }
                }
            }

        }


        private void refreshCombobox()
        {
            _db.SaveChanges();
            Users = _db.Users.ToList();
            cboxTeamLeader.DataSource = Users;
            cboxTeamLeader.DisplayMember = "FullName";
            cboxTeamLeader.ValueMember = "Id";

            UserForTeamMembers = _db.Users.ToList();
            chkListTeamMembers.DataSource = UserForTeamMembers;
            chkListTeamMembers.DisplayMember = "FullName";
            chkListTeamMembers.ValueMember = "Id";


            ProjectsMilestones = _db.Projects.ToList();
            cmb_ProjectName.DataSource = ProjectsMilestones;
            cmb_ProjectName.DisplayMember = "Name";
            cmb_ProjectName.ValueMember = "Id";


            UsersMilestones = _db.Users.ToList();
            cmb_MileStonsManager.DataSource = UsersMilestones;
            cmb_MileStonsManager.DisplayMember = "FullName";
            cmb_MileStonsManager.ValueMember = "Id";


            MilestoneForTask = _db.Milestones.ToList();
            cmbMilestonesForTask.DataSource = MilestoneForTask;
            cmbMilestonesForTask.DisplayMember = "Name";
            cmbMilestonesForTask.ValueMember = "Id";

  


            reflesh_DataGrid();
            reflesh_TreeVieww();
        }
        private async void btnSaveUser_Click(object sender, EventArgs e)
        {
            if (txtFirstName.Text.Equals(string.Empty) ||
                txtLastName.Text.Equals(string.Empty) ||
                txtEmail.Text.Equals(string.Empty) ||
                txtPassword.Equals(string.Empty) ||
                txtAddress.Text.Equals(string.Empty) ||
                txtPhoneNumber.Text.Equals(string.Empty))
            {
                MessageBox.Show("Lütfen Tüm alanları doldurunuz");
                return;
            }
            await _db.Users.AddAsync(
                new User
                {
                    FirstName = txtFirstName.Text,
                    LastName = txtLastName.Text,
                    Email = txtEmail.Text,
                    Password = txtPassword.Text,
                    BirthDay = txtBirthDate.DateTime,
                    Address = txtAddress.Text,
                    PhoneNumber = txtPhoneNumber.Text,
                    FullName = txtFirstName.Text + " " + txtLastName.Text
                });
 
            refreshCombobox();
            MessageBox.Show("Başarıyla eklendi.");

        }


        private void btn_selectImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            if (open.ShowDialog() == DialogResult.OK)
            {
                pic_1.Image = new Bitmap(open.FileName);
            }
        }

        private void btn_SelectFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Files(*.jpg; *.jpeg;*.doc;*.xls;*.ppt)|*.jpg; *.jpeg;*.doc;*.xls;*.ppt";

            if (open.ShowDialog() == DialogResult.OK)
            {
                txt_Files.Text += Path.GetFileName(open.FileName) + " ,";
            }
        }
        private void btn_Close_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTabPage.PageVisible = false;
        }
        private void btn_openProjects_Click(object sender, EventArgs e)
        {

            if (tabControl1.TabPages[0].PageVisible == true)
            {
                tabControl1.SelectedTabPage = tabControl1.TabPages[0];
            }
            else
            {
                tabControl1.TabPages[0].PageVisible = true;
            }
        }
        private void simpleButton4_Click(object sender, EventArgs e)
        {
            if (UserLevel.Equals("Admin"))
            {
                if (tabControl1.TabPages[1].PageVisible == true)
                {
                    tabControl1.SelectedTabPage = tabControl1.TabPages[1];
                }
                else
                {
                    tabControl1.TabPages[1].PageVisible = true;
                }
            }
        }
        private void simpleButton5_Click(object sender, EventArgs e)
        {
            if (tabControl1.TabPages[2].PageVisible == true)
            {
                tabControl1.SelectedTabPage = tabControl1.TabPages[2];
            }
            else
            {
                tabControl1.TabPages[2].PageVisible = true;
            }
        }
        private void simpleButton6_Click(object sender, EventArgs e)
        {
            if (UserLevel.Equals("Admin")) { 
                if (tabControl1.TabPages[3].PageVisible == true)
                {
                    tabControl1.SelectedTabPage = tabControl1.TabPages[3];
                }
                else
                {
                    tabControl1.TabPages[3].PageVisible = true;
                }
            }
        }
        private void simpleButton7_Click(object sender, EventArgs e)
        {
            if (tabControl1.TabPages[4].PageVisible == true)
            {
                tabControl1.SelectedTabPage = tabControl1.TabPages[4];
            }
            else
            {
                tabControl1.TabPages[4].PageVisible = true;
            }
        }


        private async void btnSaveProject_Click(object sender, EventArgs e)
        {
            if (txtProjeName.Text.Equals(string.Empty) ||
                txtMoney.Text.Equals(string.Empty) ||
                txtProjectDescription.Text.Equals(string.Empty) ||
                DateTime.Compare(dateEStartTime.DateTime, dateEFinishTime.DateTime) > 0)
            {
                MessageBox.Show("Lütfen tüm alanları doldurunuz veya başlangıç tarihi daha önce olmalıdır.");
                return;
            }
            /*
            List<int> arr = new List<int>();
            foreach (var value in chkListTeamMembers.CheckedItems.GetEnumerator().YieldToArray())
            {
                arr.Add(value.);
            }
            */
            List<User> ProjectUsers = new List<User>();
            var listOfUsers = chkListTeamMembers.CheckedItems.GetEnumerator();
                while (listOfUsers.MoveNext())
                {
                    ProjectUsers.Add((User)listOfUsers.Current);
                }

            await _db.Projects.AddAsync(
                new Project
                {
                    Name = txtProjeName.Text,
                    ManagerId = Convert.ToInt32(cboxTeamLeader.SelectedValue.ToString()),
                    Description = txtProjectDescription.Text,
                    CreateTime = DateTime.Now,
                    StartTime = null,
                    FinishTime = null,
                    EstimatedStartTime = dateEStartTime.DateTime,
                    EstimatedFinishTime = dateEFinishTime.DateTime,
                    ProjectState = ProjectAndTaskState.NotStarted,
                    ProjectDocuments = null,
                    Users = ProjectUsers,
                    Money = new Money
                    {
                        MoneysAmount = txtMoney.Text.ToString(),
                        MoneyType = cmbMoneyType.Text.Equals("Günlük") ? MoneyType.Daily : cmbMoneyType.Text.Equals("Aylık") ? MoneyType.Mounthly : MoneyType.Yearly
                    },
                    ProjectType = cmbProjectType.Text.Equals("Yurtdisi") ? ProjectType.YurtDisi : cmbProjectType.Text.Equals("Tubitak") ? ProjectType.Tubitak : ProjectType.Kobi,

                });
            refreshCombobox();
            MessageBox.Show("Başarıyla eklendi.");
        }

        private async void btnSaveMilestone_Click(object sender, EventArgs e)
        {
            if (txt_MileStonsName.Text.Equals(string.Empty) ||
                dateEStartTimeMileStone == null ||
                dateEFinishTimeMileStone == null || 
                DateTime.Compare(dateEStartTimeMileStone.DateTime, dateEFinishTimeMileStone.DateTime) > 0)
            {
                MessageBox.Show("Lütfen tüm alanları doldurunuz veya başlangıç tarihi daha önce olmalıdır.");
                return;
            }

            await _db.Milestones.AddAsync(
                new Milestone
                {
                    Name = txt_MileStonsName.Text,
                    AssignedUserId = Convert.ToInt32(cmb_MileStonsManager.SelectedValue.ToString()),
                    StartTime = dateEStartTimeMileStone.DateTime,
                    FinishTime = dateEFinishTimeMileStone.DateTime,
                    ProjectId = Convert.ToInt32(cmb_ProjectName.SelectedValue.ToString())
                });

            refreshCombobox();
            MessageBox.Show("Başarıyla eklendi.");
        }

        private async void btnSaveTask_Click(object sender, EventArgs e)
        {
            if (txtTaskSubject.Text.Equals(string.Empty) ||
                dateEStartTimeTask == null ||
                dateEFinishTimeTask == null ||
                DateTime.Compare(dateEStartTimeTask.DateTime, dateEFinishTimeTask.DateTime) > 0)
            {
                MessageBox.Show("Lütfen tüm alanları doldurunuz veya başlangıç tarihi daha önce olmalıdır.");
                return;
            }

            await _db.Tasks.AddAsync(
                new Task
                {
                    Subject = txtTaskSubject.Text,
                    StartTime = dateEStartTimeTask.DateTime,
                    FinishTime = dateEFinishTimeTask.DateTime,
                    MilestoneId = Convert.ToInt32(cmbMilestonesForTask.SelectedValue.ToString())
                });

            refreshCombobox();
            MessageBox.Show("Başarıyla eklendi.");
        }



        SqlConnection cn = new SqlConnection("Server=.;Database=ProjectManagement;Trusted_Connection=True;");

        public string UserLevel { get; set; }//User bazı tablari açamasın diye
        public string UserLevelName { get; set; }//User bazı tablari açamasın diye
        


        private void reflesh_DataGrid()
        {
            SqlConnection con = new SqlConnection("Server=.;Database=ProjectManagement;Trusted_Connection=True;");
            SqlCommand com = new SqlCommand("Select * from dbo.Users", con);
            SqlDataAdapter d = new SqlDataAdapter(com);
            System.Data.DataTable dt = new System.Data.DataTable();
            d.Fill(dt);
            dataGridView_Users.DataSource = dt;
        }

        public int selected_item;

        private void btn_DeleteUser_Click(object sender, EventArgs e)
        {
            var user = _db.Users.FirstOrDefault(x => x.Id == selected_item);
            _db.Users.Remove(user);

            refreshCombobox();

        }

        private void dataGridView_Users_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            selected_item = Convert.ToInt32(dataGridView_Users.Rows[e.RowIndex].Cells["ID"].FormattedValue.ToString());

        }
        private void dataGridView_Users_SelectionChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView_Users.SelectedRows)
            {
                txt_C_Name.Text = row.Cells[2].Value.ToString();
                txt_CLastName.Text = row.Cells[3].Value.ToString();
                txt_CEposta.Text = row.Cells[5].Value.ToString();
                txt_CPassword.Text = row.Cells[6].Value.ToString();
                txt_CAdresse.Text = row.Cells[8].Value.ToString();
                txt_CTelNo.Text = row.Cells[9].Value.ToString();
            }
        }

        private void btn_UpdateUser_Click(object sender, EventArgs e)
        {
            //Kişi Bilgilerini güncelleme ******************************************************************************************

            var user = _db.Users.Where(u => u.Id == selected_item).First();
            user.FirstName = txt_C_Name.Text;
            user.LastName = txt_CLastName.Text;
            user.Email = txt_CEposta.Text;
            user.Password = txt_CPassword.Text;
            user.Address = txt_CAdresse.Text;
            user.PhoneNumber = txt_CTelNo.Text;
            user.FullName = user.FirstName + " " + user.LastName;

            _db.Update(user);

            refreshCombobox();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            new Login().Show();
            this.Hide();
        }

    }
}