namespace BTLDotNet.View
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.justifiedRichTextBox1 = new BTLDotNet.Controller.JustifiedRichTextBox();
            this.SuspendLayout();
            // 
            // justifiedRichTextBox1
            // 
            this.justifiedRichTextBox1.Location = new System.Drawing.Point(12, 64);
            this.justifiedRichTextBox1.Name = "justifiedRichTextBox1";
            this.justifiedRichTextBox1.SelectionAlignment = BTLDotNet.Controller.JustifiedRichTextBox.TextAlignment.Left;
            this.justifiedRichTextBox1.Size = new System.Drawing.Size(820, 303);
            this.justifiedRichTextBox1.TabIndex = 0;
            this.justifiedRichTextBox1.Text = "";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(856, 420);
            this.Controls.Add(this.justifiedRichTextBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private Controller.JustifiedRichTextBox justifiedRichTextBox1;

    }
}