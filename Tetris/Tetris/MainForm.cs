using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tetris
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.tetrisControl1.onFocusChanged += new Action(TetrisControl_FocusChanged);
        }


        protected override bool ProcessCmdKey(ref Message msg, Keys keyData) {
            this.tetrisControl1.TetrisControl_KeyDown(null, keyData);
            return base.ProcessCmdKey(ref msg, keyData);
        }

        public void TetrisControl_FocusChanged()
        {
            this.tetrisControl1.Focus();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.tetrisControl1.End();
            Thread.Sleep(500);
        }
    }
}
