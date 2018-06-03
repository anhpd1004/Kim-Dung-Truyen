using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BTLDotNet.Controller;
using System.Text.RegularExpressions;
using System.Threading;

namespace BTLDotNet.View
{
    public partial class ResultSearch : Form
    {
        private string input = "";
        private Form home;// to show blur form
        private HomePage homepage;
        private FormBlur blur;
        private FormProgressBar progress;
        private Thread _Thread;
        Model.Stories stories;

        public ResultSearch(Form home, HomePage homepage, string input, Model.Story story)
        {
            this.home = home;
            this.homepage = homepage;
            this.input = input;
            if (story != null)
            {
                stories = new Model.Stories();
                stories.addStory(story);
            }
            else
            {
                stories = Model.MyDatabase.stories;
            }
            InitializeComponent();
            TimKiem();
        }

        public void TimKiem()
        {
            this.CenterToParent();
            blur = new FormBlur(home);
            Show(home);
            progress = new FormProgressBar();
            progress.Show(this);
            _Thread = new Thread(Search);
            _Thread.Start();
        }

        public void Search()
        {
            List<ResultRow> resultMatch = new List<ResultRow>();
            List<ResultRow> results = new List<ResultRow>();

            foreach (Model.Story story in stories.getStories())
            {
                foreach (Model.Chapter chap in story.getChapters())
                {
                    MyRegex regex = new MyRegex();
                    regex.InputString = input;
                    regex.Content = chap.content;
                    Match match = regex.GenerateRegexAllMatch();
                    if (match != null)
                    {
                        int start = match.Index;
                        int len = match.Value.Length;
                        ResultRow item = new ResultRow();
                        item.index_story = story.idt;
                        item.index_chap = story.getChapters().IndexOf(chap);
                        item.start = start;
                        item.len = len;
                        item.resulttext = story.name + " - " + chap.name + ": " + match.Value;
                        resultMatch.Add(item);
                    }
                    else
                    {
                        List<Result> result = regex.FuzzyMethod();
                        foreach (Result rs in result)
                        {
                            int start = rs.begin;
                            int len = rs.end - rs.begin;
                            ResultRow item = new ResultRow();
                            item.index_story = story.idt;
                            item.index_chap = story.getChapters().IndexOf(chap);
                            item.start = start;
                            item.len = len;
                            item.resulttext = story.name + " - " + chap.name + ": (" + rs.numberRhythmsMatch + ")" + chap.content.Substring(start, len);
                            results.Add(item);
                        }
                    }
                }
            }

            listBox1.Invoke((MethodInvoker)delegate () { listBox1.DataSource = resultMatch.Count > 0 ? resultMatch : results; });
            progress.Invoke((MethodInvoker)delegate () { progress.Dispose(); });
        }

        private void button2_Click(object sender, EventArgs e)
        {
            blur.Dispose();
            this.Dispose();
        }

        public class ResultRow
        {
            public int index_story;
            public int index_chap;
            public int start;
            public int len;
            public string resulttext;

            public override string ToString()
            {
                return resulttext;
            }
        }

        public void SelectRow(ResultRow item)
        {
            if (item == null)
            {
                return;
            }
            if (homepage != null)
            {
                homepage.Dispose();
                homepage = new HomePage(homepage.newHome);
            }
            else
            {
                home.Hide();
                homepage = new HomePage((NewHomePage)home);
            }

            homepage.setAuto(item.index_chap, item.start, item.len);
            homepage.Show(item.index_story);
            blur.Dispose();
            Dispose();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ResultRow item = (ResultRow)listBox1.SelectedItem;
            SelectRow(item);
        }
    }
}
