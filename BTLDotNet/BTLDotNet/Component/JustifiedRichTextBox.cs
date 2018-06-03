using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BTLDotNet.Component
{
    public class JustifiedRichTextBox : RichTextBox
    {
        [DllImport("user32", CharSet = CharSet.Auto)]
        private static extern int SendMessage(HandleRef hWnd, int _msg, int wParam, ref PARAFORMAT2 _pf);

        [DllImport("user32", CharSet = CharSet.Auto)]
        private static extern int SendMessage(HandleRef hWnd, int _msg, int wParam, int lParam);

        public enum TextAlignment
        {
            Left = 1,
            Right,
            Center,
            Justify
        }

        private const int EM_SETEVENTMASK = 1073;
        private const int EM_GETPARAFORMAT = 1085;
        private const int EM_SETPARAFORMAT = 1095;
        private const int EM_SETTYPOGRAPHYOPTIONS = 1226;
        private const int WM_SETREDRAW = 11;
        private const int TO_ADVANCEDTYPOGRAPHY = 0x1;
        private const int PFM_ALIGNMENT = 8;
        private const int SCF_SELECTION = 1;

        [StructLayout(LayoutKind.Sequential)]
        private struct PARAFORMAT2
        {
            //----------------------------------------
            public int cbSize;           // PARAFORMAT
            public uint dwMask;
            public short wNumbering;
            public short wReserved;
            public int dxStartIndent;
            public int dxRightIndent;
            public int dxOffset;
            public short wAlignment;
            public short cTabCount;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public int[] rgxTabs;
            //----------------------------------------
            public int dySpaceBefore;   // PARAFORMAT2
            public int dySpaceAfter;
            public int dyLineSpacing;
            public short sStyle;
            public byte bLineSpacingRule;
            public byte bOutlineLevel;
            public short wShadingWeight;
            public short wShadingStyle;
            public short wNumberingStart;
            public short wNumberingStyle;
            public short wNumberingTab;
            public short wBorderSpace;
            public short wBorderWidth;
            public short wBorders;
        }

        // SelectionAlignment can't be overriden, so step over it
        public new TextAlignment SelectionAlignment
        {
            get
            {
                PARAFORMAT2 _pf = new PARAFORMAT2();
                _pf.cbSize = Marshal.SizeOf(_pf);

                SendMessage(new HandleRef(this, Handle), EM_GETPARAFORMAT, SCF_SELECTION, ref _pf);

                // If 0, defaults to TextAlignment.Left
                if ((_pf.dwMask & PFM_ALIGNMENT) == 0) return TextAlignment.Left;
                return (TextAlignment)_pf.wAlignment;
            }

            set
            {
                PARAFORMAT2 _pf = new PARAFORMAT2();
                _pf.cbSize = Marshal.SizeOf(_pf);
                _pf.dwMask = PFM_ALIGNMENT;
                _pf.wAlignment = (short)value;

                SendMessage(new HandleRef(this, Handle), EM_SETPARAFORMAT, SCF_SELECTION, ref _pf);
            }
        }

        /// Overrides OnHandleCreated to enable RTB advances options
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            // EM_SETTYPOGRAPHYOPTIONS allows to enable RTB (RichEdit) Advanced Typography
            SendMessage(new HandleRef(this, Handle), EM_SETTYPOGRAPHYOPTIONS,
                                                     TO_ADVANCEDTYPOGRAPHY,
                                                     TO_ADVANCEDTYPOGRAPHY);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ResumeLayout(false);

        }
    }
}
