using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BTLDotNet.View
{
    public partial class InputSearch : Form
    {
        public InputSearch()
        {
            InitializeComponent();
            textBox1.Focus();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        public string getInputSearch()
        {
            return textBox1.Text;
        }

        private void InputSearch_Load(object sender, EventArgs e)
        {
            textBox1.Focus();
            this.AutoScroll = true;
            this.AutoSize = false;
        }
    }
}
