using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GomokuClient
{
    public partial class StartMenu : Form
    {
        public StartMenu()
        {
            InitializeComponent();
        }


        private void SinglePlayButton_Click(object sender, EventArgs e)
        {
            Hide();
            SinglePlay singlePlay = new SinglePlay();
            singlePlay.FormClosed += new FormClosedEventHandler(Back2_Menu);
            singlePlay.Show();

        }

        void Back2_Menu(object sender, FormClosedEventArgs e)
        {
            Show();
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void MultiPlayButton_Click(object sender, EventArgs e)
        {
            Hide();
            MultiPlay multiPlay = new MultiPlay();
            multiPlay.FormClosed += new FormClosedEventHandler(Back2_Menu);
            multiPlay.Show();
        }
    }
}
