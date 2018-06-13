using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Media;
using System.Threading;
using System.IO;

namespace BTLDotNet.View
{
    public partial class HomePage : Form
    {
        private bool IsTurnOn = false;
        private int idh;
        private int iStory;//truyện được click từ bên NewHomePage truyền sang
        private Model.Story story;
        public NewHomePage newHome;
        private Thread _Thread;
        private string filesave;
        private int chapautoscroll = -1;
        private int posautoscroll = -1;
        private int poslen = 0;

        public HomePage(NewHomePage page)
        {
            InitializeComponent();
            newHome = page;
        }

        public void setAuto(int chap, int pos, int len)
        {
            chapautoscroll = chap;
            posautoscroll = pos;
            poslen = 0;
        }

        public void Show(int iStory)
        {
            this.iStory = iStory;
            switch (iStory)
            {
                case 0:
                    pictureBox1.BackgroundImage = Properties.Resources.logo_01;
                    break;
                case 1:
                    pictureBox1.BackgroundImage = Properties.Resources.logo_02;
                    break;
                case 2:
                    pictureBox1.BackgroundImage = Properties.Resources.logo_03;
                    break;
                case 3:
                    pictureBox1.BackgroundImage = Properties.Resources.logo_04;
                    break;
                case 4:
                    pictureBox1.BackgroundImage = Properties.Resources.logo_05;
                    break;
                case 5:
                    pictureBox1.BackgroundImage = Properties.Resources.logo_06;
                    break;
            }
            contentchap.Text = "";
            listchap.DataSource = null;
            Show();

            string[] files = Directory.GetFiles(Application.StartupPath);
            foreach (string file in files)
            {
                string[] token = file.Split('\\');
                if (token[token.Length - 1].StartsWith(iStory + "_"))
                {
                    filesave = token[token.Length - 1];
                    break;
                }
            }

            if (chapautoscroll == -1)
            {
                string[] datasave = filesave.Split('_');
                chapautoscroll = int.Parse(datasave[1]);
                posautoscroll = int.Parse(datasave[2]);
            }

            story = Model.MyDatabase.stories.getStories()[iStory];
            if (story != null)
            {
                List<Model.Chapter> list = story.getChapters();
                if (list != null && list.Count > 0)
                {
                    listchap.DataSource = list;
                    listchap.SelectedIndex = chapautoscroll;
                }
            }
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case 0x84:
                    base.WndProc(ref m);
                    if ((int)m.Result == 0x1)
                        m.Result = (IntPtr)0x2;
                    return;
            }

            base.WndProc(ref m);
        }

        private void listchap_SelectedIndexChanged(object sender, EventArgs e)
        {
            _Thread = new Thread(SelectChap);
            _Thread.Start();
        }

        public void SelectChap()
        {
            Model.Chapter chap = null;
            listchap.Invoke((MethodInvoker)delegate ()
            {
                chap = (Model.Chapter)listchap.SelectedItem;
                chapautoscroll = listchap.SelectedIndex;
            });
            if (chap != null)
            {
                idh = chap.idh;
                contentchap.Invoke((MethodInvoker)delegate ()
                {
                    contentchap.Text = chap.content;
                    // contentchap.Text = "kien kiem trumg lung ling lang lieng tuing taing yen yến yên yện yển";
                });
                MarkWrongRhythm(posautoscroll, poslen);
                posautoscroll = 0;
                poslen = 0;
            }
        }

        public void MarkWrongRhythm(int start, int len)
        {
            contentchap.Invoke((MethodInvoker)delegate () { contentchap.Select(0, contentchap.Text.Length); /* contentchap.SelectionStart = 0; */ });
            contentchap.Invoke((MethodInvoker)delegate () { contentchap.SelectionColor = Color.White; });

            string content = "";
            contentchap.Invoke((MethodInvoker)delegate () { content = contentchap.Text; });
            string[] rhythms = Controller.Rhythm.splitRhythm(content);
            int ind = 0;
            long tryparse;
            foreach (var rhythm in rhythms)
            {
                if (!Int64.TryParse(rhythm, out tryparse))
                {
                    if (!rhythm.Equals(string.Empty) && !Controller.MyRule.IsValid(rhythm))
                    {
                        if (content.IndexOf(rhythm, ind) == 0)
                        {
                            // contentchap.Invoke((MethodInvoker)delegate () { contentchap.Select(ind - rhythm.Length, rhythm.Length); });
                            // contentchap.Invoke((MethodInvoker)delegate () { contentchap.SelectionColor = Color.Red; });
                        }
                        do
                        {
                            ind = content.IndexOf(rhythm, ind) + rhythm.Length;
                            contentchap.Invoke((MethodInvoker)delegate () { contentchap.Select(ind - rhythm.Length, rhythm.Length); });
                            char cBehind = content[ind];
                            if (ind >= content.Length || ind <= rhythm.Length)
                            {
                                contentchap.Invoke((MethodInvoker)delegate () { contentchap.SelectionColor = Color.Red; });
                                break;
                            }
                            if (Controller.Rhythm.isSeparator(cBehind))
                            {
                                if (ind > (rhythm.Length + 1))
                                {
                                    char cFront = content[ind - rhythm.Length - 1];
                                    if (Controller.Rhythm.isSeparator(cFront))
                                    {
                                        contentchap.Invoke((MethodInvoker)delegate () { contentchap.SelectionColor = Color.Red; });
                                        break;
                                    }
                                }
                            }
                        } while (content.IndexOf(rhythm, ind) != -1);
                    }
                    else
                    {
                        ind += (rhythm.Length);
                    }
                }
            }

            contentchap.Invoke((MethodInvoker)delegate ()
            {
                contentchap.Select(start, len);
                contentchap.ScrollToCaret();
            });
        }

        private void contentchap_MouseMove(object sender, MouseEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(contentchap.Text))
            {
                //get index of nearest character
                var index = contentchap.GetCharIndexFromPosition(e.Location);
                //check if mouse is above a word (non-whitespace character)
                if (!Controller.Rhythm.isSeparator(contentchap.Text[index]))
                {
                    var start = index;
                    while (start > 0 && !Controller.Rhythm.isSeparator(contentchap.Text[start - 1]))
                        start--;
                    var end = index;
                    while (end < contentchap.Text.Length - 1 && !Controller.Rhythm.isSeparator(contentchap.Text[end + 1]))
                        end++;
                    string strHover = contentchap.Text.Substring(start, end - start + 1);
                    long tryparse;
                    if (!Int64.TryParse(strHover, out tryparse))
                    {
                        if (!strHover.Equals(string.Empty) && !Controller.MyRule.IsValid(strHover))
                        {
                            // hien tooltip
                            toolTip1.Show(Controller.MyRule.Explain(strHover), contentchap, e.Location.X + 10, e.Location.Y + 5);
                        }
                        else
                        {
                            toolTip1.Hide(contentchap);
                            // an tooltip
                        }
                    }
                    else
                    {
                        toolTip1.Hide(contentchap);
                        // An tooltip
                    }
                }
            }
        }

        private void contentchap_MouseDown(object sender, MouseEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(contentchap.Text))
            {
                //get index of nearest character
                var index = contentchap.GetCharIndexFromPosition(e.Location);
                //check if mouse is above a word (non-whitespace character)
                if (!Controller.Rhythm.isSeparator(contentchap.Text[index]))
                {
                    var start = index;
                    while (start > 0 && !Controller.Rhythm.isSeparator(contentchap.Text[start - 1]))
                        start--;
                    var end = index;
                    while (end < contentchap.Text.Length - 1 && !Controller.Rhythm.isSeparator(contentchap.Text[end + 1]))
                        end++;
                    string strHover = contentchap.Text.Substring(start, end - start + 1);
                    long tryparse;
                    if (!Int64.TryParse(strHover, out tryparse))
                    {
                        if (!strHover.Equals(string.Empty) && !Controller.MyRule.IsValid(strHover))
                        {
                            DialogResult dialogResult;
                            switch (e.Button)
                            {
                                case MouseButtons.Right:

                                    dialogResult = MessageBox.Show("Bạn có muốn xóa từ này không?", "", MessageBoxButtons.YesNo);
                                    if (dialogResult == DialogResult.Yes)
                                    {
                                        MessageBox.Show("Đã xóa từ");
                                    }
                                    break;
                                case MouseButtons.Left:
                                    dialogResult = MessageBox.Show("Bạn có muốn sửa từ này không?", "", MessageBoxButtons.YesNo);
                                    if (dialogResult == DialogResult.Yes)
                                    {
                                        Form form = new EditErrorWord(iStory, idh, start, end, contentchap.Text, strHover);
                                        DialogResult result = form.ShowDialog(this);
                                        if (result == DialogResult.OK)
                                        {
                                            Model.Chapter chap = (Model.Chapter)listchap.SelectedItem;
                                            chap.content = Model.MyDatabase.getContentChap(idh);

                                            contentchap.Text = chap.content.Replace("\r\n", "\n");

                                            _Thread = new Thread(() => MarkWrongRhythm(start, end - start + 1));
                                            _Thread.Start();
                                        }
                                    }
                                    break;
                            }
                        }
                    }
                }
            }

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            SaveFile();

            newHome.Dispose();
            this.Dispose();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            SaveFile();

            newHome.Show();
            this.Hide();
        }

        public void SaveFile()
        {
            filesave = Application.StartupPath + @"\" + filesave;
            File.Delete(filesave);
            Point P = new Point(0, 0);
            int CharIndex = contentchap.GetCharIndexFromPosition(P);
            filesave = Application.StartupPath + @"\" + iStory + "_" + chapautoscroll + "_" + CharIndex;
            File.Create(filesave);
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            InputSearch inpSearch = new InputSearch();
            FormBlur formBlur = new FormBlur(this);

            if (inpSearch.ShowDialog(this) == DialogResult.OK)
            {
                string text = inpSearch.getInputSearch();
                ResultSearch result = new ResultSearch(this, this, text, story);
            }
            formBlur.Dispose();
            inpSearch.Dispose();
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            if (!IsTurnOn)
            {
                this.BackColor = Color.FromArgb(111, 111, 112);
                IsTurnOn = true;
            }
            else
            {
                this.BackColor = Color.Black;
                IsTurnOn = false;
            }
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            contextMenuStrip1.Show(pictureBox5, pictureBox5.Location);
        }

        private void ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            contentchap.BackColor = panel2.BackColor = (sender as ToolStripMenuItem).BackColor;
        }
    }
}
