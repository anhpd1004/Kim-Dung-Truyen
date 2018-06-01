using BTLDotNet.Controller;
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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            string s = "Chính phủ Papua New Guinea vừa ra quyết định sẽ chặn hoàn toàn Facebook trong vòng một tháng. Bộ trưởng Truyền thông của nước này, ông Sam Basil, cho biết khoảng thời gian một tháng là để các cơ quan điều tra, nghiên cứu người dùng và tìm ra cách khắc phục các vấn đề bao gồm tin giả, người dùng giả và lan truyền nội dung khiêu dâm.";
            s += "Chính phủ Papua New Guinea vừa ra quyết định sẽ chặn hoàn toàn Facebook trong vòng một tháng. Bộ trưởng Truyền thông của nước này, ông Sam Basil, cho biết khoảng thời gian một tháng là để các cơ quan điều tra, nghiên cứu người dùng và tìm ra cách khắc phục các vấn đề bao gồm tin giả, người dùng giả và lan truyền nội dung khiêu dâm.\n\t";
            s += "Chính phủ Papua New Guinea vừa ra quyết định sẽ chặn hoàn toàn Facebook trong vòng một tháng. Bộ trưởng Truyền thông của nước này, ông Sam Basil, cho biết khoảng thời gian một tháng là để các cơ quan điều tra, nghiên cứu người dùng và tìm ra cách khắc phục các vấn đề bao gồm tin giả, người dùng giả và lan truyền nội dung khiêu dâm.";
            justifiedRichTextBox1.Text = s;
            justifiedRichTextBox1.SelectionAlignment = JustifiedRichTextBox.TextAlignment.Justify;
        }
    }
}
