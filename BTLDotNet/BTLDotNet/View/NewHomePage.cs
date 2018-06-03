using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BTLDotNet.View
{
    public partial class NewHomePage : Form
    {
        Settings setting;
        FormBlur formBlur;
        public int size;
        public int volume;

        public NewHomePage()
        {
            InitializeComponent();
            this.AllowTransparency = true;
            this.FormBorderStyle = FormBorderStyle.None;
            this.CenterToScreen();
            size = 0;
            volume = 50;
            this.MouseHover += NewHomePage_MouseHover;
            this.MouseLeave += NewHomePage_MouseLeave;
            wmpAnhHungXaDieu.ClickEvent += wmpAnhHungXaDieu_ClickEvent;
            wmpThanDieuDaiHiep.ClickEvent += wmpThanDieuDaiHiep_ClickEvent;
            wmpLocDinhKi.ClickEvent += wmpLocDinhKi_ClickEvent;
            wmpThienLongBatBo.ClickEvent += wmpThienLongBatBo_ClickEvent;
            wmpTieuNgaoGiangHo.ClickEvent += wmpTieuNgaoGiangHo_ClickEvent;
            wmpYThienDoLongKi.ClickEvent += wmpYThienDoLongKi_ClickEvent;
            picCancel.Click += picCancel_Click;
            picSettings.Click += picSettings_Click;
        }

        void picSettings_Click(object sender, EventArgs e)
        {
            formBlur = new FormBlur(this);

            setting = new Settings(size, volume);
            setting.comboValue += setting_comboValue;
            setting.volumeTrack += setting_volumeTrack;
            setting.ShowDialog(this);
            formBlur.Dispose();
        }

        void setting_volumeTrack(object sender, EventArgs e)
        {
            volume = setting.GetVolume();
            backgroundMusic.settings.volume = volume;
            chemMusic.settings.volume = volume;
        }

        void setting_comboValue(object sender, EventArgs e)
        {
            size = setting.GetSize();
            this.Size = new Size(890 + (int)(0.2 * 907 * size), 570 + (int)(0.2 * 600 * size));
            picCancel.Location = new Point(this.Width - 5 - picCancel.Width, 3);
            picSettings.Location = new Point(this.Width - 5 - picCancel.Width - 7 - picSettings.Width, 2);
            picSearch.Location = new Point(this.Width - 5 - picCancel.Width - 7 - picSettings.Width - 7 - picSearch.Width, 5);
            this.CenterToScreen();
            formBlur.Size = new Size(890 + (int)(0.2 * 907 * size), 570 + (int)(0.2 * 600 * size));
            Size vd = new Size(290 + (int)(0.2 * 290 * size), 200 + (int)(0.2 * 200 * size));
            panel1.Size = vd;
            wmpAnhHungXaDieu.Size = vd;
            panel2.Size = vd;
            wmpThanDieuDaiHiep.Size = vd;
            panel3.Size = vd;
            wmpLocDinhKi.Size = vd;
            panel4.Size = vd;
            wmpThienLongBatBo.Size = vd;
            panel5.Size = vd;
            wmpYThienDoLongKi.Size = vd;
            panel6.Size = vd;
            wmpTieuNgaoGiangHo.Size = vd;
            panel1.Location = new Point(0, 100 + 20 * size);
            wmpAnhHungXaDieu.Location = new Point(0, 0);
            label1.Font = new Font("Microsoft Sans Serif", 10 + size * 2, FontStyle.Bold);
            label1.Location = new Point(0, vd.Height - label1.Height);
            panel2.Location = new Point(vd.Width + 10 + (int)(size * 2), 100 + 20 * size);
            wmpThanDieuDaiHiep.Location = new Point(0, 0);
            label2.Font = new Font("Microsoft Sans Serif", 10 + size * 2, FontStyle.Bold);
            label2.Location = new Point(0, vd.Height - label2.Height);
            panel3.Location = new Point(vd.Width * 2 + 20 + (int)(size * 4), 100 + 20 * size);
            wmpLocDinhKi.Location = new Point(0, 0);
            label3.Font = new Font("Microsoft Sans Serif", 10 + size * 2, FontStyle.Bold);
            label3.Location = new Point(0, vd.Height - label3.Height);
            panel4.Location = new Point(0, 130 + vd.Height + (int)(size * 6) + 20 * size);
            wmpThienLongBatBo.Location = new Point(0, 0);
            label4.Font = new Font("Microsoft Sans Serif", 10 + size * 2, FontStyle.Bold);
            label4.Location = new Point(0, vd.Height - label4.Height);
            panel5.Location = new Point(vd.Width + 10 + (int)(size * 2), 130 + vd.Height + (int)(size * 6) + 20 * size);
            wmpYThienDoLongKi.Location = new Point(0, 0);
            label5.Font = new Font("Microsoft Sans Serif", 10 + size * 2, FontStyle.Bold);
            label5.Location = new Point(0, vd.Height - label5.Height);
            panel6.Location = new Point(vd.Width * 2 + 20 + (int)(size * 4), 130 + vd.Height + (int)(size * 6) + 20 * size);
            wmpTieuNgaoGiangHo.Location = new Point(0, 0);
            label6.Font = new Font("Microsoft Sans Serif", 10 + size * 2, FontStyle.Bold);
            label6.Location = new Point(0, vd.Height - label6.Height);
            kiemXoet.Size = new Size(890 + 178 * size, 520 + 104 * size);
            kiemXoet.Location = new Point(0, 50 + 10 * size);
            label7.Font = new Font("Microsoft Sans Serif", label7.Font.Size + (int)(label7.Font.Size * 0), FontStyle.Bold);
            label7.Location = new Point((this.Width - label7.Width) / 2, 10 + 2 * size);
        }

        void picCancel_Click(object sender, EventArgs e)
        {
            this.Close();
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

        void wmpYThienDoLongKi_ClickEvent(object sender, AxWMPLib._WMPOCXEvents_ClickEvent e)
        {
            chemMusic.Ctlcontrols.play();
            label7.Visible = false;
            kiemXoet.Visible = true;
            kiemXoet.Size = new Size(890 + 178 * size, 520 + 104 * size);
            kiemXoet.Ctlcontrols.play();
            Thread.Sleep(888);
            kiemXoet.Visible = false;
            kiemXoet.Size = new Size(0, 0);
            label7.Visible = true;
        }

        void wmpTieuNgaoGiangHo_ClickEvent(object sender, AxWMPLib._WMPOCXEvents_ClickEvent e)
        {
            chemMusic.Ctlcontrols.play();
            label7.Visible = false;
            kiemXoet.Visible = true;
            kiemXoet.Size = new Size(890 + 178 * size, 520 + 104 * size);
            kiemXoet.Ctlcontrols.play();
            Thread.Sleep(888);
            kiemXoet.Visible = false;
            kiemXoet.Size = new Size(0, 0);
            label7.Visible = true;
        }

        void wmpThienLongBatBo_ClickEvent(object sender, AxWMPLib._WMPOCXEvents_ClickEvent e)
        {
            chemMusic.Ctlcontrols.play();
            label7.Visible = false;
            kiemXoet.Visible = true;
            kiemXoet.Size = new Size(890 + 178 * size, 520 + 104 * size);
            kiemXoet.Ctlcontrols.play();
            Thread.Sleep(888);
            kiemXoet.Visible = false;
            kiemXoet.Size = new Size(0, 0);
            label7.Visible = true;
        }

        void wmpLocDinhKi_ClickEvent(object sender, AxWMPLib._WMPOCXEvents_ClickEvent e)
        {
            chemMusic.Ctlcontrols.play();
            label7.Visible = false;
            kiemXoet.Visible = true;
            kiemXoet.Size = new Size(890 + 178 * size, 520 + 104 * size);
            kiemXoet.Ctlcontrols.play();
            Thread.Sleep(888);
            kiemXoet.Visible = false;
            kiemXoet.Size = new Size(0, 0);
            label7.Visible = true;
        }

        void wmpThanDieuDaiHiep_ClickEvent(object sender, AxWMPLib._WMPOCXEvents_ClickEvent e)
        {
            chemMusic.Ctlcontrols.play();
            label7.Visible = false;
            kiemXoet.Visible = true;
            kiemXoet.Size = new Size(890 + 178 * size, 520 + 104 * size);
            kiemXoet.Ctlcontrols.play();
            Thread.Sleep(888);
            kiemXoet.Visible = false;
            kiemXoet.Size = new Size(0, 0);
            label7.Visible = true;
        }

        void wmpAnhHungXaDieu_ClickEvent(object sender, AxWMPLib._WMPOCXEvents_ClickEvent e)
        {
            chemMusic.Ctlcontrols.play();
            label7.Visible = false;
            kiemXoet.Visible = true;
            kiemXoet.Size = new Size(890 + 178 * size, 520 + 104 * size);
            kiemXoet.Ctlcontrols.play();
            Thread.Sleep(888);
            kiemXoet.Visible = false;
            kiemXoet.Size = new Size(0, 0);
            label7.Visible = true;
        }

        private bool IsContain(Point contain, int w, int h, int x, int y)
        {
            if ((x > contain.X && x < contain.X + w) && (y > contain.Y && y < contain.Y + h))
            {
                return true;
            }
            return false;
        }

        void NewHomePage_MouseLeave(object sender, EventArgs e)
        {
            wmpAnhHungXaDieu.Ctlcontrols.play();
            wmpThanDieuDaiHiep.Ctlcontrols.play();
            wmpLocDinhKi.Ctlcontrols.play();
            wmpThienLongBatBo.Ctlcontrols.play();
            wmpYThienDoLongKi.Ctlcontrols.play();
            wmpTieuNgaoGiangHo.Ctlcontrols.play();
            timer1.Enabled = true;
        }

        void NewHomePage_MouseHover(object sender, EventArgs e)
        {
            wmpAnhHungXaDieu.Ctlcontrols.pause();
            wmpThanDieuDaiHiep.Ctlcontrols.pause();
            wmpLocDinhKi.Ctlcontrols.pause();
            wmpThienLongBatBo.Ctlcontrols.pause();
            wmpYThienDoLongKi.Ctlcontrols.pause();
            wmpTieuNgaoGiangHo.Ctlcontrols.pause();
            timer1.Enabled = false;
        }

        private async void NewHomePage_Load(object sender, EventArgs e)
        {
            var projectPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;

            kiemXoet.URL = Path.Combine(projectPath, @"Resources\kiem_xoet.mp4");
            backgroundMusic.URL = Path.Combine(projectPath, @"Resources\background_music.mp3");
            chemMusic.URL = Path.Combine(projectPath, @"Resources\chem.mp3");
            wmpAnhHungXaDieu.URL = Path.Combine(projectPath, @"Resources\anh_hung_xa_dieu.mp4");
            wmpThanDieuDaiHiep.URL = Path.Combine(projectPath, @"Resources\than_dieu_dai_hiep.mp4");
            wmpLocDinhKi.URL = Path.Combine(projectPath, @"Resources\loc_dinh_ki.mp4");
            wmpThienLongBatBo.URL = Path.Combine(projectPath, @"Resources\thien_long_bat_bo.mp4");
            wmpYThienDoLongKi.URL = Path.Combine(projectPath, @"Resources\y_thien_do_long_ki.mp4");
            wmpTieuNgaoGiangHo.URL = Path.Combine(projectPath, @"Resources\tieu_ngao_giang_ho.mp4");

            backgroundMusic.uiMode = "none";
            kiemXoet.uiMode = "none";
            chemMusic.uiMode = "none";
            backgroundMusic.Visible = false;
            backgroundMusic.settings.volume = volume;
            chemMusic.settings.volume = volume / 2;
            kiemXoet.Visible = false;
            wmpAnhHungXaDieu.uiMode = "none";
            wmpThanDieuDaiHiep.uiMode = "none";
            wmpLocDinhKi.uiMode = "none";
            wmpThienLongBatBo.uiMode = "none";
            wmpYThienDoLongKi.uiMode = "none";
            wmpTieuNgaoGiangHo.uiMode = "none";

            backgroundMusic.settings.autoStart = true;
            chemMusic.settings.autoStart = false;
            wmpAnhHungXaDieu.settings.autoStart = false;
            wmpThanDieuDaiHiep.settings.autoStart = false;
            wmpLocDinhKi.settings.autoStart = false;
            wmpThienLongBatBo.settings.autoStart = false;
            wmpYThienDoLongKi.settings.autoStart = false;
            wmpTieuNgaoGiangHo.settings.autoStart = false;
            kiemXoet.settings.autoStart = false;

            wmpAnhHungXaDieu.settings.mute = true;
            wmpThanDieuDaiHiep.settings.mute = true;
            wmpLocDinhKi.settings.mute = true;
            wmpThienLongBatBo.settings.mute = true;
            wmpYThienDoLongKi.settings.mute = true;
            wmpTieuNgaoGiangHo.settings.mute = true;
            kiemXoet.settings.mute = true;

            backgroundMusic.settings.setMode("loop", true);
            wmpAnhHungXaDieu.settings.setMode("loop", true);
            wmpThanDieuDaiHiep.settings.setMode("loop", true);
            wmpLocDinhKi.settings.setMode("loop", true);
            wmpThienLongBatBo.settings.setMode("loop", true);
            wmpYThienDoLongKi.settings.setMode("loop", true);
            wmpTieuNgaoGiangHo.settings.setMode("loop", true);
            chemMusic.settings.setMode("loop", false);
            kiemXoet.settings.setMode("loop", false);
            timer1.Enabled = true;
            timer2.Enabled = true;

            await Model.MyDatabase.getStories();
        }
        
        private void timer1_Tick(object sender, EventArgs e)
        {
            //1
            if (label1.Location.X + label1.Width < 5)
            {
                label1.Location = new Point(285, label1.Location.Y);
            }
            else
            {
                label1.Location = new Point(label1.Location.X - 1, label1.Location.Y);
            }
            //2
            if (label2.Location.X + label2.Width < 5)
            {
                label2.Location = new Point(285, label2.Location.Y);
            }
            else
            {
                label2.Location = new Point(label2.Location.X - 1, label2.Location.Y);
            }
            //3
            if (label3.Location.X + label3.Width < 5)
            {
                label3.Location = new Point(285, label3.Location.Y);
            }
            else
            {
                label3.Location = new Point(label3.Location.X - 1, label3.Location.Y);
            }
            //4
            if (label4.Location.X + label4.Width < 5)
            {
                label4.Location = new Point(285, label4.Location.Y);
            }
            else
            {
                label4.Location = new Point(label4.Location.X - 1, label4.Location.Y);
            }
            //5
            if (label5.Location.X + label5.Width < 5)
            {
                label5.Location = new Point(285, label5.Location.Y);
            }
            else
            {
                label5.Location = new Point(label5.Location.X - 1, label5.Location.Y);
            }
            //6
            if (label6.Location.X + label6.Width < 5)
            {
                label6.Location = new Point(285, label6.Location.Y);
            }
            else
            {
                label6.Location = new Point(label6.Location.X - 1, label6.Location.Y);
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (label7.Font.Size == 40)
            {
                label7.Font = new Font("Microsoft Sans Serif", 30, FontStyle.Bold);
                label7.ForeColor = Color.DarkRed;
            }
            else
            {
                label7.Font = new Font("Microsoft Sans Serif", 40, FontStyle.Bold);
                label7.ForeColor = Color.Lime;
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            if (Model.MyDatabase.getDone)
            {
                this.Hide();
                new HomePage(this).Show(0);
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {
            if (Model.MyDatabase.getDone)
            {
                this.Hide();
                new HomePage(this).Show(1);
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {
            if (Model.MyDatabase.getDone)
            {
                this.Hide();
                new HomePage(this).Show(2);
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {
            if (Model.MyDatabase.getDone)
            {
                this.Hide();
                new HomePage(this).Show(3);
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {
            if (Model.MyDatabase.getDone)
            {
                this.Hide();
                new HomePage(this).Show(4);
            }
        }

        private void label6_Click(object sender, EventArgs e)
        {
            if (Model.MyDatabase.getDone)
            {
                this.Hide();
                new HomePage(this).Show(5);
            }
        }

        private void picSearch_Click(object sender, EventArgs e)
        {
            if (Model.MyDatabase.getDone)
            {
                InputSearch inpSearch = new InputSearch();
                FormBlur formBlur = new FormBlur(this);

                if (inpSearch.ShowDialog(this) == DialogResult.OK)
                {
                    string text = inpSearch.getInputSearch();
                    ResultSearch result = new ResultSearch(this, null, text, null);
                }
                formBlur.Dispose();
                inpSearch.Dispose();
            }
        }
    }
}
