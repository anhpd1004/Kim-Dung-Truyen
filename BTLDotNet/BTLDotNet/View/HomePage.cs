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

namespace BTLDotNet.View
{
    public partial class HomePage : Form
    {
        private bool IsTurnOn = false;
        private int idh;
        private int iStory;//truyện được click từ bên NewHomePage truyền sang
        private Model.Story story;
        private NewHomePage newHome;

        public HomePage(int iStory, NewHomePage page)
        {
            InitializeComponent();
            newHome = page;
            this.iStory = iStory;
        }

        private void HomePage_Load(object sender, EventArgs e)
        {
            contentchap.Text = "";
            story = Model.MyDatabase.stories.getStories()[iStory];
            List<Model.Chapter> list = story.getChapters();
            listchap.DataSource = list;
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
            Model.Chapter chap = (Model.Chapter)listchap.SelectedItem;
            idh = chap.idh;
            contentchap.Text = chap.content.Replace("\r\n", "\n");
            MarkWrongRhythm();

            contentchap.SelectionStart = 0;
            contentchap.SelectionIndent = 100;
            contentchap.SelectionHangingIndent = -70;
            contentchap.SelectionRightIndent = 30;
        }

        public void MarkWrongRhythm()
        {
            contentchap.SelectionStart = 0;
            contentchap.SelectionColor = Color.Black;

            string content = contentchap.Text.Replace("\r\n", "\n");
            string[] rhythms = Controller.Rhythm.splitRhythm(content);
            int ind = 0;
            long tryparse;
            foreach (var rhythm in rhythms)
            {
                if (!Int64.TryParse(rhythm, out tryparse))
                {
                    if (!rhythm.Equals(string.Empty) && !Controller.MyRule.IsValid(rhythm))
                    {
                        do
                        {
                            ind = content.IndexOf(rhythm, ind) + rhythm.Length;
                            contentchap.Select(ind - rhythm.Length, rhythm.Length);
                            char cBehind = content[ind];
                            if (Controller.Rhythm.isSeparator(cBehind))
                            {
                                if (ind > (rhythm.Length + 1))
                                {
                                    char cFront = content[ind - rhythm.Length - 1];
                                    if (Controller.Rhythm.isSeparator(cFront))
                                    {
                                        contentchap.SelectionColor = Color.Red;
                                        break;
                                    }
                                }
                            }
                        } while (true);
                    }
                    else
                    {
                        ind += (rhythm.Length);
                    }
                }
            }
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
                                            MarkWrongRhythm();
                                            contentchap.Select(start, end - start + 1);
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
            newHome.Dispose();
            this.Dispose();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            newHome.Show();
            this.Hide();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            InputSearch inpSearch = new InputSearch();
            FormBlur formBlur = new FormBlur();
            formBlur.Size = new Size(this.Width, this.Height);
            formBlur.Location = new Point(0, 0);
            formBlur.BackColor = this.BackColor;
            formBlur.Show(this);
            if (inpSearch.ShowDialog(this) == DialogResult.OK)
            {
                string text = inpSearch.getInputSearch();
                ResultSearch result = new ResultSearch(this, text);
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
