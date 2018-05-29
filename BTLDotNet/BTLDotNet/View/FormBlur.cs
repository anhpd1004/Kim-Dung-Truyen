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
        public FormBlur()
        {
            InitializeComponent();
        }

        private void FormBlur_Load(object sender, EventArgs e)
        {
            this.CenterToParent();
        }
    }
}
