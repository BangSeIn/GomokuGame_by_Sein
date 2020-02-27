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
    public partial class SinglePlay : Form
    {
        private const int RectSize = 33;
        private const int EdgeCount = 15;

        private enum Horse {  none = 0, BLACK, WHITE };
        private Horse[,] board = new Horse[EdgeCount, EdgeCount];
        private Horse NowPlayer = Horse.BLACK;

        private bool Playing = false; //Judges game is now playing or not

        private bool Judge() //Function judges Victory
        {
            for (int i = 0; i < EdgeCount - 4; i++) //Row
                for (int j = 0; j < EdgeCount; j++)
                    if (board[i, j] == NowPlayer && board[i + 1, j] == NowPlayer && board[i + 2, j] == NowPlayer &&
                        board[i + 3, j] == NowPlayer && board[i + 4, j] == NowPlayer)
                        return true;
            for (int i = 0; i < EdgeCount; i++) //Column
                for (int j = 0; j < EdgeCount; j++)
                    if (board[i, j] == NowPlayer && board[i, j - 1] == NowPlayer && board[i, j - 2] == NowPlayer &&
                        board[i, j - 3] == NowPlayer && board[i, j - 4] == NowPlayer)
                        return true;
            for (int i = 0; i < EdgeCount - 4; i++) // Y = X Line
                for (int j = 4; j < EdgeCount-4; j++)
                    if (board[i, j] == NowPlayer && board[i + 1, j+1] == NowPlayer && board[i + 2, j+2] == NowPlayer &&
                        board[i + 3, j+3] == NowPlayer && board[i + 4, j+4] == NowPlayer)
                        return true;
            for (int i = 4; i < EdgeCount; i++) //Y = -X Line
                for (int j = 0; j < EdgeCount-4; j++)
                    if (board[i, j] == NowPlayer && board[i - 1, j+1] == NowPlayer && board[i - 2, j+2] == NowPlayer &&
                        board[i - 3, j+3] == NowPlayer && board[i - 4, j+4] == NowPlayer)
                        return true;
            return false;
        }

        private void refresh()
        {
            this.Board.Refresh();
            for (int i = 0; i < EdgeCount; i++)
                for (int j = 0; j < EdgeCount; j++)
                    board[i, j] = Horse.none;
        }

        private void PlayButton_Click(object sender, EventArgs e)
        {
            if(!Playing)
            {
                refresh();
                Playing = true;
                PlayButton.Text = "Play";
                Status.Text = "Now it's "+NowPlayer.ToString() + "'s turn.";
            }
            else
            {
                refresh();
                Status.Text = "The Game was restarted.";
            }
        }
        public SinglePlay()
        {
            InitializeComponent();  
        }

        private void Board_MouseDown(object sender, MouseEventArgs e)
        {   
            if(!Playing)
            {
                MessageBox.Show("You should click Play button to start.");
                return;
            }
            Graphics g = this.Board.CreateGraphics();
            int x = e.X / RectSize;
            int y = e.Y / RectSize;
            // 0 ~ 14 cell
            if(x<0||y<0||x>=EdgeCount||y>=EdgeCount)
            {
                MessageBox.Show("Sorry, you can't put outisde of the Board :(");
                return;
            }
            if (board[x, y] != Horse.none) return;
            board[x, y] = NowPlayer;
            if(NowPlayer == Horse.BLACK)
            {
                SolidBrush brush = new SolidBrush(Color.Black);
                g.FillEllipse(brush, x * RectSize, y * RectSize, RectSize, RectSize);
            }
            else
            {
                SolidBrush brush = new SolidBrush(Color.White);
                g.FillEllipse(brush, x * RectSize, y * RectSize, RectSize, RectSize);
            }
            
            
            if(NowPlayer==Horse.BLACK)
            {
                SolidBrush brush = new SolidBrush(Color.Black);
                g.FillEllipse(brush, x * RectSize, y * RectSize, RectSize, RectSize);

            }
            else
            {
                SolidBrush brush = new SolidBrush(Color.White);
                g.FillEllipse(brush, x * RectSize, y * RectSize, RectSize, RectSize);

            }
            if (Judge()) //if Player won the game
            {
                Status.Text = "Player "+NowPlayer.ToString() + " is the WINNER!";
                Playing = false;
                PlayButton.Text = "Play";
            }
            else
            {
                NowPlayer = ((NowPlayer == Horse.BLACK) ? Horse.WHITE : Horse.BLACK);
                Status.Text = "Now it's " + NowPlayer.ToString() + "'s turn.";
            }
        }

        private void Board_Paint(object sender, PaintEventArgs e)
        {
            Graphics gp = e.Graphics;
            Color lineColor = Color.Black; //Color of the Board Line
            Pen p = new Pen(lineColor, 2);
            gp.DrawLine(p, RectSize / 2, RectSize / 2, RectSize / 2, RectSize * EdgeCount - RectSize / 2);
            gp.DrawLine(p, RectSize / 2, RectSize / 2, RectSize * EdgeCount - RectSize / 2, RectSize / 2);
            gp.DrawLine(p, RectSize / 2, RectSize * EdgeCount - RectSize / 2, RectSize * EdgeCount - RectSize / 2, RectSize * EdgeCount - RectSize / 2);
            gp.DrawLine(p, RectSize * EdgeCount - RectSize / 2, RectSize / 2, RectSize * EdgeCount - RectSize / 2, RectSize * EdgeCount - RectSize / 2);
            p = new Pen(lineColor, 1);

            //Draw Cross lines with moving diagonally
            for(int i = RectSize+RectSize/2;i< RectSize * EdgeCount - RectSize / 2;i+=RectSize)
            {
                gp.DrawLine(p, RectSize / 2, i, RectSize * EdgeCount - RectSize / 2, i);
                gp.DrawLine(p, i,RectSize / 2, i, RectSize * EdgeCount - RectSize / 2);

            }
        }

    }
}
