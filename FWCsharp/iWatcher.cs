using System;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.IO;

namespace FWCsharp
{

    public partial class iWatcher : UserControl
    {
        string watched;
        FileSystemWatcher fsw = new FileSystemWatcher();
        string imageLocation;
        int bclick;
        public iWatcher()
        {
            InitializeComponent();
        }

        public void filewatcher()
        {
            string pattern = @"^(?:[a-zA-Z]\:|\\\\[\w\.]+\\[\w.$]+)\\(?:[\w]+\\)*\w([\w.])+$";
            if (checkBox1.Checked == false)
            {
                if (Regex.IsMatch(txt_path.Text, pattern)==true)
                {
                    if (Directory.Exists(txt_path.Text))
                    {
                        
                        fsw.Path = txt_path.Text;
                        watched = fsw.Path;

                        fsw.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
                          | NotifyFilters.FileName | NotifyFilters.DirectoryName;

                        fsw.Filter = "*.*";

                        fsw.Changed += new FileSystemEventHandler(OnChanged);
                        fsw.Created += new FileSystemEventHandler(OnChanged);
                        fsw.Deleted += new FileSystemEventHandler(OnChanged);

                        fsw.EnableRaisingEvents = true;

                        string[] files = Directory.GetFiles(watched);

                        foreach (var file in files)
                        {
                            if (System.IO.Path.GetExtension(file) == ".png" || System.IO.Path.GetExtension(file) == ".jpg" || System.IO.Path.GetExtension(file) == ".tga")
                            {
                                listBox_activity.Items.Insert(0, Path.GetFileName(file));
                                listBox_activity.SelectedIndex = 0;
                            }
                            else
                                pictureBox1.Image = null; 
                        }

                    }

                }

            }
        }

        private void OnChanged(object source, System.IO.FileSystemEventArgs e)
        {

            try
            {
                if (e.ChangeType == WatcherChangeTypes.Created)
                {
                    this.Invoke((MethodInvoker)(() => listBox_activity.Items.Insert(0, Path.GetFileName(e.FullPath))));
                    this.Invoke((MethodInvoker)(() => listBox_activity.SelectedIndex = 0));
                }

                if (e.ChangeType == WatcherChangeTypes.Deleted)
                {
                    this.Invoke((MethodInvoker)(() => listBox_activity.Items.Remove(Path.GetFileName(e.FullPath))));
                    if (listBox_activity.Items.Count !=0)
                    {
                        this.Invoke((MethodInvoker)(() => listBox_activity.SelectedIndex = 0));
                    }
                 }
            }
            catch
            {

            }
        }


        private void iWatcher_Load(object sender, EventArgs e)
        {
         
        }

        private void btn_browse_Click(object sender, EventArgs e)
        {
            browse(); 
        }

        private void txt_path_TextChanged(object sender, EventArgs e)
        {
            filewatcher();
        }

        private void listBox_activity_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                imageLocation = watched + @"\" + listBox_activity.SelectedItem;
                if (Path.GetExtension(imageLocation) == ".tga")
                {
                    pictureBox1.Image = Paloma.TargaImage.LoadTargaImage(imageLocation);
                }
                else
                    pictureBox1.ImageLocation = imageLocation;
            }
            catch
            {
            }
        }

        public void browse()
        {
            if (bclick == 0)
            {
                btn_browse.Enabled = true;
                listBox_activity.Items.Clear();
                bclick = bclick + 1;
            }
            else
            {
                btn_browse.Enabled = true;
                listBox_activity.Items.Clear();  
                pictureBox1.Image = null;

                if (listBox_activity.Items.Count != 0)
                {
                    fsw.EnableRaisingEvents = false;
                }
            }


            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                this.txt_path.Text = folderBrowserDialog1.SelectedPath;
            }

        }

        public void disable()
        {
            listBox_activity.Items.Clear();
            pictureBox1.Image = null;
            fsw.EnableRaisingEvents = false; 
        }

        public void enable()
        {
            listBox_activity.Items.Clear();
            filewatcher();   
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            try
            {


                if (checkBox1.Checked == true)
                {
                    disable();
                }

                if (checkBox1.Checked == false)
                {
                    enable();
                }
            }
            catch
            {

            }
        }
    }
}
