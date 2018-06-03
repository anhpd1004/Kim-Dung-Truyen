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
    public partial class FormBlur : Form
    {
        private Form parent;

        public FormBlur(Form parent)
        {
            InitializeComponent();
            this.parent = parent;
            Size = new Size(this.parent.Width, this.parent.Height);
            Location = new Point(0, 0);
            BackColor = this.parent.BackColor;
            Show(this.parent);
            this.SizeChanged += FormBlur_SizeChanged;
        }

        void FormBlur_SizeChanged(object sender, EventArgs e)
        {
            this.Location = new Point(0, 0);
            this.CenterToParent();
        }

        private void FormBlur_Load(object sender, EventArgs e)
        {
            this.CenterToParent();
        }
    }
}
