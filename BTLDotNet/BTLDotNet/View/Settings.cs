﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BTLDotNet.View
{
    public partial class Settings : Form
    {
        public event EventHandler comboValue;
        public event EventHandler volumeTrack;
        int size;
        int volume;

        public Settings(int size, int volume)
        {
            InitializeComponent();
            this.AllowTransparency = true;
            this.size = size;
            this.volume = volume;
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

        private void Settings_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = size;

            lbVolume.Text = "(" + volume + ")";
            trackBar1.Value = volume;
        }

        void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            volume = (int)trackBar1.Value;
            lbVolume.Text = "(" + volume + ")";
            if (volumeTrack != null)
            {
                volumeTrack(sender, e);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_MouseHover(object sender, EventArgs e)
        {
            button1.BackColor = Color.Lime;
            Thread.Sleep(800);
            button1.BackColor = Color.DimGray;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {

        }

        public ComboBox GetComboBox()
        {
            return this.comboBox1;
        }

        public int GetSize()
        {
            return size;
        }

        public int GetVolume()
        {
            return this.volume;
        }

        public void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            size = (int)comboBox1.SelectedIndex;
            if (comboValue != null)
            {
                comboValue(sender, e);
            }
            this.CenterToParent();
        }
    }
}
