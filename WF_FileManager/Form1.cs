using Microsoft.VisualBasic;
using System.IO;
using System.IO.Enumeration;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using static System.Net.WebRequestMethods;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WF_FileManager
{
    public partial class Form1 : Form
    {
        private System.Collections.Specialized.StringCollection folderCol;
        Anime an = new Anime();
        int i = 1;
        int flag = 0;
        string gPath = "";
        public Form1()
        {
            InitializeComponent();
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;            
            label2.Width = 0;
            label2.BackColor = Color.FromArgb(176, 212, 226);
            if (flag == 0)
                PictureG();

        }

        async private void button1_Click(object sender, EventArgs e)
        {
            button1.Text = "Зачекайте";
            if (pictureBox1.Visible == true)
            {
                string str = "  Tree Nodes ";
                char[] ch = str.ToCharArray();
                label2.Text = "";
                for (int i = 0; i < 892; await Task.Delay(50), i += 10)
                {
                    label2.Width = i;
                }
                foreach (char ch2 in ch)
                {
                    label2.Text += ch2.ToString();
                    await Task.Delay(150);
                }
                await Task.Delay(250);
                for (int i = 159; i > 0; await Task.Delay(50), i -= 2)
                {
                    label2.Height = i;
                }
                flag = 1;
            }
            label2.Height = 0;
            label2.Visible = false;
            treeView1.Visible = true;
            listView1.Visible = true;
            listView2.Visible = true;
            toolStrip1.Visible = true;
            pictureBox1.Visible = false;
        }
        async public void PictureG()
        {
            if (flag == 0)
            {
                pictureBox1.Image = System.Drawing.Image.FromFile(an.Animation(i)); await Task.Delay(50);
                i++;
                if (i > 97) i = 1;
                PictureG();
            }
        }
        /// //////////////////////////////////////////////////////////////////////////////        
        private void toolStripButton10_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (toolStripButton10.Text == "показати панель") toolStrip2.Visible = true;
            else toolStrip2.Visible = false;
        }
       
        private void Form1_Load(object sender, EventArgs e)
        {
            treeView1.Nodes.Clear();
            listView1.Items.Clear();
            listView2.Items.Clear();
            EnterWay();

        }

        private void EnterWay()  // лише знайти всі диски
        {
            try
            {
                DriveInfo[] din = DriveInfo.GetDrives();//показати всі диски в системі
                foreach (var temp in din) //показати диски
                {
                    if (temp.Name != "C:\\" && temp.IsReady)
                    {
                        TreeNode trNode = new TreeNode(temp.Name);
                        FillTreeNode(trNode, temp.Name);
                        treeView1.Nodes.Add(trNode);
                    }
                }
            }
            catch (Exception ex) { }
        }

        //заповнити наступны вузли  // знайти всі папки
        private void FillTreeNode(TreeNode driveNode, string path)
        {
            try
            {
                string[] dirs = Directory.GetDirectories(path);                   /////////////
                foreach (var dir in dirs)
                {
                    TreeNode dirNode = new TreeNode(dir);

                    dirNode.Text = dir.Remove(0, dir.LastIndexOf("\\") + 1);
                    driveNode.Nodes.Add(dirNode);

                    SubTreeNodes(dirNode, dir);
                }
            }
            catch (Exception ex) { }
        }

        private void SubTreeNodes(TreeNode driveNode, string path) // знайти папки в папках та файли в папках
        {
            //string[] dirse = Directory.GetDirectories(path);
            try
            {
                string[] dirse = Directory.GetDirectories(path);                   /////////////
                string[] files = Directory.GetFiles(path);
                if (dirse.Length > 0)
                    foreach (var dir in dirse)
                    {
                        TreeNode dirNode = new TreeNode(dir, 0, 0);
                        dirNode.Text = dir.Remove(0, dir.LastIndexOf("\\") + 1);
                        driveNode.Nodes.Add(dirNode);
                        SubTreeNodes(dirNode, dir);
                    }
                if (files.Length > 0)
                    foreach (var dir in files)
                    {
                        TreeNode dirNode = new TreeNode(dir, 0, 0);
                        dirNode.Text = dir.Remove(0, dir.LastIndexOf("\\") + 1);
                        driveNode.Nodes.Add(dirNode);
                        SubTreeNodes(dirNode, dir);
                    }
            }
            catch (Exception ex) { }
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            try
            {
                listView1.Items.Clear();
                listView2.Items.Clear();
                string path = e.Node.FullPath + @"\";

                SomethingImportant(path);
            
            } catch (Exception ex) { }
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            listView2.View = View.LargeIcon;
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            listView2.View = View.SmallIcon;
        }

        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            listView2.View = View.List;
        }

        private void toolStripButton9_Click(object sender, EventArgs e)
        {
            listView2.View = View.Tile;
        }

        private void listView2_Click(object sender, EventArgs e)
        {
            try
            {                
                string filename = listView2.FocusedItem.Text;
                FileInfo fInfo = new FileInfo(treeView1.SelectedNode.FullPath + @"\" + filename);
                double fSize = fInfo.Length / 1024;
                DateTime creatdt = fInfo.CreationTime;
                DateTime modifdt = fInfo.LastWriteTime;
                listView1.Items.Clear();
                ListViewItem file = listView1.Items.Add(filename);
                file.SubItems.Add(fSize.ToString());
                file.SubItems.Add(creatdt.ToString());
                file.SubItems.Add(modifdt.ToString());

            }
            catch { }
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Ви дійсно хочете закрити програму?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                Close();
        }


        private void toolStripButton2_Click(object sender, EventArgs e)  // створення нового файла з розширенням .txt
        {
            string path = treeView1.SelectedNode.FullPath + @"\";
            string newFileName = Interaction.InputBox("Введіть ім'я нового файлу", "Створення файлу", "new file");
            FileStream fs = System.IO.File.Create(path + newFileName + ".txt");
            fs.Close();
            listView2.Items.Add(newFileName + ".txt", 2);
            listView2.Refresh();
            
        }

        private void toolStripButton3_Click(object sender, EventArgs e)   // перейменування файлу
        {
            try
            {
                string path = treeView1.SelectedNode.FullPath + @"\" + listView2.FocusedItem.Text;
                string rename = Interaction.InputBox("Введіть нове ім'я файлу", "Переназвати файл", listView2.FocusedItem.Text);
                if (string.IsNullOrEmpty(rename)) return;
                FileSystem.Rename(path, treeView1.SelectedNode.FullPath + @"\" + rename + ".txt");
                listView2.FocusedItem.Remove();
                listView2.Items.Add(rename + ".txt", 2);
                listView1.Items.Clear();
            }catch(Exception ex) { }
            Validate();
        }
        //////////////////////////////////////////////////////////////////
        private void toolStripButton4_Click(object sender, EventArgs e)  // видалення файлу
        {
            if (MessageBox.Show("Ви дійсно хочете видалити цей файл?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {               
                try
                {                   
                    treeView1.Nodes.Remove(treeView1.SelectedNode);
                }
                catch { }
            }
        }

        private void перейменуватиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string path = treeView1.SelectedNode.FullPath;
                string rename = Interaction.InputBox("Введіть нову назву", "нове ім'я файлу", "new name");
                if (string.IsNullOrEmpty(rename)) return;
                FileSystem.Rename(path, treeView1.SelectedNode.FullPath + @"\" + rename + ".txt");
                listView2.FocusedItem.Remove();
                listView1.Items.Add(rename + ".txt");
            }
            catch { }
            Validate();            
        }

        private void видалитиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Ви дійсно хочете видалити цей файл?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                try
                {
                    treeView1.Nodes.Remove(treeView1.SelectedNode);
                }
                catch { }
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            string path = treeView1.SelectedNode.FullPath + @"\";
            string subpath = Interaction.InputBox("Введіть ім'я нової теки", "Створення теки", "new file");
            DirectoryInfo dirinf = new DirectoryInfo(path);
            if (!dirinf.Exists) 
            { 
               dirinf.Create();
            }            
            dirinf.CreateSubdirectory(subpath);            
            treeView1.Nodes.Clear();          
            EnterWay();
        }

        private void SomethingImportant(string path)
        {
            try
            {
                string extension;
                int ind = 0;
                FileInfo[] file = new DirectoryInfo(path).GetFiles();
                for (int i = 0; i < file.Length; i++)
                {
                    extension = Path.GetExtension(file[i].Name);
                    switch (extension)
                    {
                        case ".txt": { ind = 2; break; }
                        case ".docx": { ind = 3; break; }
                        case ".doc": { ind = 3; break; }
                        case ".pdf": { ind = 1; break; }
                        case ".mp4": { ind = 5; break; }
                        case ".wmv": { ind = 5; break; }
                        case ".rar": { ind = 6; break; }
                        default: { ind = 4; break; }
                    }
                    Text = ($"Path: {path}");
                    ListViewItem itm = listView2.Items.Add(file[i].Name, ind);
                }
            } catch (Exception ex) { }
        }       
    }
}