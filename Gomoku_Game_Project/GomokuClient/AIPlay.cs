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
    public partial class AIPlay : Form
    {
        private const int rectSize = 33;
        private const int edgeCount = 15;

        private enum Horse { none = 0, BLACK, WHITE };
        private Horse[,] board = new Horse[edgeCount, edgeCount];
        private Horse nowPlayer = Horse.BLACK;
        private Horse aiPlayer, userPlayer;

        private int targetX, targetY;
        private int limit = 1;
        private bool playing = false;

        private bool judge()
        {
            for (int i = 0; i < edgeCount - 4; i++) // Row
                for (int j = 0; j < edgeCount; j++)
                    if (board[i, j] == nowPlayer && board[i + 1, j] == nowPlayer && board[i + 2, j] == nowPlayer &&
                        board[i + 3, j] == nowPlayer && board[i + 4, j] == nowPlayer)
                        return true;
            for (int i = 0; i < edgeCount; i++) // Column
                for (int j = 4; j < edgeCount; j++)
                    if (board[i, j] == nowPlayer && board[i, j - 1] == nowPlayer && board[i, j - 2] == nowPlayer &&
                        board[i, j - 3] == nowPlayer && board[i, j - 4] == nowPlayer)
                        return true;
            for (int i = 0; i < edgeCount - 4; i++) // Y = X line
                for (int j = 0; j < edgeCount - 4; j++)
                    if (board[i, j] == nowPlayer && board[i + 1, j + 1] == nowPlayer && board[i + 2, j + 2] == nowPlayer &&
                        board[i + 3, j + 3] == nowPlayer && board[i + 4, j + 4] == nowPlayer)
                        return true;
            for (int i = 4; i < edgeCount; i++) // Y = -X line
                for (int j = 0; j < edgeCount - 4; j++)
                    if (board[i, j] == nowPlayer && board[i - 1, j + 1] == nowPlayer && board[i - 2, j + 2] == nowPlayer &&
                        board[i - 3, j + 3] == nowPlayer && board[i - 4, j + 4] == nowPlayer)
                        return true;
            return false;
        }


        private void refresh()
        {
            // Initiate whole Board information
            for (int i = 0; i < edgeCount; i++)
                for (int j = 0; j < edgeCount; j++)
                    board[i, j] = Horse.none;
            this.Board.Refresh();
            // Choose the first between AI and Player
            int rand = new Random().Next(1, 3);
            if (rand == 1) aiPlayer = Horse.BLACK;
            else aiPlayer = Horse.WHITE;
            userPlayer = ((aiPlayer == Horse.BLACK) ? Horse.WHITE : Horse.BLACK);
            nowPlayer = Horse.BLACK;
            if (nowPlayer == userPlayer) Status.Text = "Now it's your turn.";
            else Status.Text = "AI is playing..";
        }

        private void PlayButton_Click(object sender, EventArgs e)
        {
            if (!playing)
            {
                refresh();
                playing = true;
                PlayButton.Text = "Restart";
            }
            else
            {
                refresh();
            }
            if (nowPlayer == aiPlayer)
            {
                AI_attack();
            }
        }

        public AIPlay()
        {
            InitializeComponent();
        }

        private void Board_Paint(object sender, PaintEventArgs e)
        {
            Graphics gp = e.Graphics;
            Color lineColor = Color.Black; // Line color of board
            Pen p = new Pen(lineColor, 2);
            gp.DrawLine(p, rectSize / 2, rectSize / 2, rectSize / 2, rectSize * edgeCount - rectSize / 2); // 좌측
            gp.DrawLine(p, rectSize / 2, rectSize / 2, rectSize * edgeCount - rectSize / 2, rectSize / 2); // 상측
            gp.DrawLine(p, rectSize / 2, rectSize * edgeCount - rectSize / 2, rectSize * edgeCount - rectSize / 2, rectSize * edgeCount - rectSize / 2); // 하측
            gp.DrawLine(p, rectSize * edgeCount - rectSize / 2, rectSize / 2, rectSize * edgeCount - rectSize / 2, rectSize * edgeCount - rectSize / 2); // 우측
            p = new Pen(lineColor, 1);
            // Draw cross line moving diagnoally
            for (int i = rectSize + rectSize / 2; i < rectSize * edgeCount - rectSize / 2; i += rectSize)
            {
                gp.DrawLine(p, rectSize / 2, i, rectSize * edgeCount - rectSize / 2, i);
                gp.DrawLine(p, i, rectSize / 2, i, rectSize * edgeCount - rectSize / 2);
            }
        }

        private void AI_attack()
        {
            AlphaBetaPruning(0, -1000000, 1000000);
            board[targetX, targetY] = aiPlayer;
            Graphics g = this.Board.CreateGraphics();
            if (nowPlayer == Horse.BLACK)
            {
                SolidBrush brush = new SolidBrush(Color.Black);
                g.FillEllipse(brush, targetX * rectSize, targetY * rectSize, rectSize, rectSize);
            }
            else
            {
                SolidBrush brush = new SolidBrush(Color.White);
                g.FillEllipse(brush, targetX * rectSize, targetY * rectSize, rectSize, rectSize);
            }
            if (judge())
            {
                if (nowPlayer == aiPlayer) Status.Text = "You lost the Game:(";
                else Status.Text = "You're the WINNER!";
                playing = false;
                PlayButton.Text = "Restart";
            }
            else
            {
                nowPlayer = ((nowPlayer == Horse.BLACK) ? Horse.WHITE : Horse.BLACK);
            }
        }

        private void Board_MouseDown(object sender, MouseEventArgs e)
        {
            if (nowPlayer == aiPlayer) return;
            if (!playing)
            {
                MessageBox.Show("You should click Play button to start.");
                return;
            }
            Graphics g = this.Board.CreateGraphics();
            int x = e.X / rectSize;
            int y = e.Y / rectSize;
            if (x < 0 || y < 0 || x >= edgeCount || y >= edgeCount)
            {
                MessageBox.Show("Sorry, you can't put outisde of the Board :(");
                return;
            }
            if (board[x, y] != Horse.none) return;
            board[x, y] = nowPlayer;
            if (nowPlayer == Horse.BLACK)
            {
                SolidBrush brush = new SolidBrush(Color.Black);
                g.FillEllipse(brush, x * rectSize, y * rectSize, rectSize, rectSize);
            }
            else
            {
                SolidBrush brush = new SolidBrush(Color.White);
                g.FillEllipse(brush, x * rectSize, y * rectSize, rectSize, rectSize);
            }
            if (judge())
            {
                if (nowPlayer == aiPlayer) Status.Text = "You lost the Game:(";
                else Status.Text = "You're the WINNER!";
                playing = false;
                PlayButton.Text = "Restart";
            }
            else
            {
                nowPlayer = ((nowPlayer == Horse.BLACK) ? Horse.WHITE : Horse.BLACK);
                AI_attack();
            }
        }

        //All of 20 ways to win the game(make gomoku)
        public bool make5mok1(int x, int y)
        {
            try
            {
                for (int i = y; i < y + 5; i++)
                {
                    if (board[x, i] != board[x, y]) return false;
                }
            }
            catch (IndexOutOfRangeException)
            {
                return false;
            }
            return true;
        }

        public bool make5mok2(int x, int y)
        {
            try
            {
                for (int i = x, j = y; i < x + 5; i++, j--)
                {
                    if (board[i, j] != board[x, y]) return false;
                }
            }
            catch (IndexOutOfRangeException)
            {
                return false;
            }
            return true;
        }

        public bool make5mok3(int x, int y)
        {
            try
            {
                for (int i = x; i < x + 5; i++)
                {
                    if (board[i, y] != board[x, y]) return false;
                }
            }
            catch (IndexOutOfRangeException)
            {
                return false;
            }
            return true;
        }

        public bool make5mok4(int x, int y)
        {
            try
            {
                for (int i = x, j = y; i < x + 5; i++, j++)
                {
                    if (board[i, j] != board[x, y]) return false;
                }
            }
            catch (IndexOutOfRangeException)
            {
                return false;
            }
            return true;
        }

        public bool make5mok5(int x, int y)
        {
            try
            {
                for (int i = y; i > y - 5; i--)
                {
                    if (board[x, i] != board[x, y]) return false;
                }
            }
            catch (IndexOutOfRangeException)
            {
                return false;
            }
            return true;
        }

        public bool make5mok6(int x, int y)
        {
            try
            {
                for (int i = x, j = y; i > x - 5; i--, j++)
                {
                    if (board[i, j] != board[x, y]) return false;
                }
            }
            catch (IndexOutOfRangeException)
            {
                return false;
            }
            return true;
        }

        public bool make5mok7(int x, int y)
        {
            try
            {
                for (int i = x; i > x - 5; i--)
                {
                    if (board[i, y] != board[x, y]) return false;
                }
            }
            catch (IndexOutOfRangeException)
            {
                return false;
            }
            return true;
        }

        public bool make5mok8(int x, int y)
        {
            try
            {
                for (int i = x, j = y; i > x - 5; i--, j--)
                {
                    if (board[i, j] != board[x, y]) return false;
                }
            }
            catch (IndexOutOfRangeException)
            {
                return false;
            }
            return true;
        }

        public bool make5mok9(int x, int y)
        {
            try
            {
                for (int i = y - 1; i < y + 4; i++)
                {
                    if (board[x, i] != board[x, y]) return false;
                }
            }
            catch (IndexOutOfRangeException)
            {
                return false;
            }
            return true;
        }

        public bool make5mok10(int x, int y)
        {
            try
            {
                for (int i = x - 1, j = y + 1; i < x + 4; i++, j--)
                {
                    if (board[i, j] != board[x, y]) return false;
                }
            }
            catch (IndexOutOfRangeException)
            {
                return false;
            }
            return true;
        }

        public bool make5mok11(int x, int y)
        {
            try
            {
                for (int i = x - 1; i < x + 4; i++)
                {
                    if (board[i, y] != board[x, y]) return false;
                }
            }
            catch (IndexOutOfRangeException)
            {
                return false;
            }
            return true;
        }

        public bool make5mok12(int x, int y)
        {
            try
            {
                for (int i = x - 1, j = y - 1; i < x + 4; i++, j++)
                {
                    if (board[i, j] != board[x, y]) return false;
                }
            }
            catch (IndexOutOfRangeException)
            {
                return false;
            }
            return true;
        }

        public bool make5mok13(int x, int y)
        {
            try
            {
                for (int i = y + 1; i > y - 4; i--)
                {
                    if (board[x, i] != board[x, y]) return false;
                }
            }
            catch (IndexOutOfRangeException)
            {
                return false;
            }
            return true;
        }

        public bool make5mok14(int x, int y)
        {
            try
            {
                for (int i = x + 1, j = y - 1; i > x - 4; i--, j++)
                {
                    if (board[i, j] != board[x, y]) return false;
                }
            }
            catch (IndexOutOfRangeException)
            {
                return false;
            }
            return true;
        }

        public bool make5mok15(int x, int y)
        {
            try
            {
                for (int i = x + 1; i > x - 4; i--)
                {
                    if (board[i, y] != board[x, y]) return false;
                }
            }
            catch (IndexOutOfRangeException)
            {
                return false;
            }
            return true;
        }

        public bool make5mok16(int x, int y)
        {
            try
            {
                for (int i = x + 1, j = y + 1; i > x - 4; i--, j--)
                {
                    if (board[i, j] != board[x, y]) return false;
                }
            }
            catch (IndexOutOfRangeException)
            {
                return false;
            }
            return true;
        }

        public bool make5mok17(int x, int y)
        {
            try
            {
                for (int i = y - 2; i < y + 3; i++)
                {
                    if (board[x, i] != board[x, y]) return false;
                }
            }
            catch (IndexOutOfRangeException)
            {
                return false;
            }
            return true;
        }

        public bool make5mok18(int x, int y)
        {
            try
            {
                for (int i = x - 2; i < x + 3; i++)
                {
                    if (board[i, y] != board[x, y]) return false;
                }
            }
            catch (IndexOutOfRangeException)
            {
                return false;
            }
            return true;
        }

        public bool make5mok19(int x, int y)
        {
            try
            {
                for (int i = y + 2; i > y - 3; i--)
                {
                    if (board[x, i] != board[x, y]) return false;
                }
            }
            catch (IndexOutOfRangeException)
            {
                return false;
            }
            return true;
        }

        public bool make5mok20(int x, int y)
        {
            try
            {
                for (int i = x + 2; i > x - 3; i--)
                {
                    if (board[i, y] != board[x, y]) return false;
                }
            }
            catch (IndexOutOfRangeException)
            {
                return false;
            }
            return true;
        }

        //Judging all of the ways to make 3-horses-in-one-line
        /*
           - Basically, set count = 2 when three horses are in one line.
           - After that, count -= 1 if one of the side is blocked.
           - ex) (Blocked)->// (Horse)->OO0
           - //OOO// : return 0;     //OOO : return 1;      OOO : return 2
           - If it is not three-horses-in-one-line, return -1. 
        */

        public int make3mok1(int x, int y)
        {
            int count = -1;
            try
            {
                if (board[x, y] == board[x, y + 1] && board[x, y] == board[x, y + 2])
                {
                    count = 2;
                    // if 4-horses
                    if (y < edgeCount - 3 && board[x, y] == board[x, y + 3]) return -1;
                    if (y > 0 && board[x, y] == board[x, y - 1]) return -1;
                    // if not 4-horses, check there is a blocked side
                    if (y == edgeCount - 3 || board[x, y + 3] != 0) count--;
                    if (y == 0 || board[x, y - 1] != 0) count--;
                }
            }
            catch (IndexOutOfRangeException)
            {
                return -1;
            }
            return count;
        }

        public int make3mok2(int x, int y)
        {
            int count = -1;
            try
            {
                if (board[x, y + 1] == 0 && board[x, y] == board[x, y + 2] && board[x, y] == board[x, y + 3])
                {
                    count = 2;
                    // if 4-horses
                    if (y < edgeCount - 4 && board[x, y] == board[x, y + 4]) return -1;
                    if (y > 0 && board[x, y] == board[x, y - 1]) return -1;
                    // if not, check there is blocked side
                    if (y == edgeCount - 4 || board[x, y + 4] != 0) count--;
                    if (y == 0 || board[x, y - 1] != 0) count--;
                }
            }
            catch (IndexOutOfRangeException)
            {
                return -1;
            }
            return count;
        }

        public int make3mok3(int x, int y)
        {
            int count = -1;
            try
            {
                if (board[x, y + 2] == 0 && board[x, y] == board[x, y + 1] && board[x, y] == board[x, y + 3])
                {
                    count = 2;
                    // if 4-horses
                    if (y < edgeCount - 4 && board[x, y] == board[x, y + 4]) return -1;
                    if (y > 0 && board[x, y] == board[x, y - 1]) return -1;
                    // if not, check there is blocked side
                    if (y == edgeCount - 4 || board[x, y + 4] != 0) count--;
                    if (y == 0 || board[x, y - 1] != 0) count--;
                }
            }
            catch (IndexOutOfRangeException)
            {
                return -1;
            }
            return count;
        }

        public int make3mok4(int x, int y)
        {
            int count = -1;
            try
            {
                if (board[x, y] == board[x - 1, y + 1] && board[x, y] == board[x - 2, y + 2])
                {
                    count = 2;
                    // if 4-horses
                    if (x > 2 && y < edgeCount - 3 && board[x, y] == board[x - 3, y + 3]) return -1;
                    if (x < edgeCount - 1 && y > 0 && board[x, y] == board[x + 1, y - 1]) return -1;
                    // if not, check there is blocked side
                    if (x == 2 || y == edgeCount - 3 || board[x - 3, y + 3] != 0) count--;
                    if (x == edgeCount - 1 || y == 0 || board[x + 1, y - 1] != 0) count--;
                }
            }
            catch (IndexOutOfRangeException)
            {
                return -1;
            }
            return count;
        }

        public int make3mok5(int x, int y)
        {
            int count = -1;
            try
            {
                if (board[x - 1, y + 1] == 0 && board[x, y] == board[x - 2, y + 2] && board[x, y] == board[x - 3, y + 3])
                {
                    count = 2;
                    // if 4-horses
                    if (x > 3 && y < edgeCount - 4 && board[x, y] == board[x - 4, y + 4]) return -1;
                    if (x < edgeCount - 1 && y > 0 && board[x, y] == board[x + 1, y - 1]) return -1;
                    // if not, check there is blocked side
                    if (x == 3 || y == edgeCount - 4 || board[x - 4, y + 4] != 0) count--;
                    if (x == edgeCount - 1 || y == 0 || board[x + 1, y - 1] != 0) count--;
                }
            }
            catch (IndexOutOfRangeException)
            {
                return -1;
            }
            return count;
        }

        public int make3mok6(int x, int y)
        {
            int count = -1;
            try
            {
                if (board[x - 2, y + 2] == 0 && board[x, y] == board[x - 1, y + 1] && board[x, y] == board[x - 3, y + 3])
                {
                    count = 2;
                    // if 4-horses
                    if (x > 3 && y < edgeCount - 4 && board[x, y] == board[x - 4, y + 4]) return -1;
                    if (x < edgeCount - 1 && y > 0 && board[x, y] == board[x + 1, y - 1]) return -1;
                    // it not, check there is blocekd side
                    if (x == 3 || y == edgeCount - 4 || board[x - 4, y + 4] != 0) count--;
                    if (x == edgeCount - 1 || y == 0 || board[x + 1, y - 1] != 0) count--;
                }
            }
            catch (IndexOutOfRangeException)
            {
                return -1;
            }
            return count;
        }

        public int make3mok7(int x, int y)
        {
            int count = -1;
            try
            {
                if (board[x, y] == board[x - 1, y] && board[x - 2, y] == board[x, y])
                {
                    count = 2;
                    // if 4-horses
                    if (x < edgeCount - 1 && board[x, y] == board[x + 1, y]) return -1;
                    if (x > 2 && board[x, y] == board[x - 3, y]) return -1;
                    // if not, check there is blocked side
                    if (x == edgeCount - 1 || board[x + 1, y] != 0) count--;
                    if (x == 2 || board[x - 3, y] != 0) count--;
                }
            }
            catch (IndexOutOfRangeException)
            {
                return -1;
            }
            return count;
        }

        public int make3mok8(int x, int y)
        {
            int count = -1;
            try
            {
                if (board[x - 1, y] == 0 && board[x, y] == board[x - 2, y] && board[x - 3, y] == board[x, y])
                {
                    count = 2;
                    // if 4-horses
                    if (x < edgeCount - 1 && board[x, y] == board[x + 1, y]) return -1;
                    if (x > 3 && board[x, y] == board[x - 4, y]) return -1;
                    // if not, check there is blocked side
                    if (x == edgeCount - 1 || board[x + 1, y] != 0) count--;
                    if (x == 3 || board[x - 4, y] != 0) count--;
                }
            }
            catch (IndexOutOfRangeException)
            {
                return -1;
            }
            return count;
        }

        public int make3mok9(int x, int y)
        {
            int count = -1;
            try
            {
                if (board[x - 2, y] == 0 && board[x, y] == board[x - 1, y] && board[x - 3, y] == board[x, y])
                {
                    count = 2;
                    // if 4-horses
                    if (x < edgeCount - 1 && board[x, y] == board[x + 1, y]) return -1;
                    if (x > 3 && board[x, y] == board[x - 4, y]) return -1;
                    // if not, check there is blocked side
                    if (x == edgeCount - 1 || board[x + 1, y] != 0) count--;
                    if (x == 3 || board[x - 4, y] != 0) count--;
                }
            }
            catch (IndexOutOfRangeException)
            {
                return -1;
            }
            return count;
        }

        public int make3mok10(int x, int y)
        {
            int count = -1;
            try
            {
                if (board[x, y] == board[x - 1, y - 1] && board[x, y] == board[x - 2, y - 2])
                {
                    count = 2;
                    // if 4-horses
                    if (x > 2 && y > 2 && board[x, y] == board[x - 3, y - 3]) return -1;
                    if (x < edgeCount - 1 && y < edgeCount - 1 && board[x, y] == board[x + 1, y + 1]) return -1;
                    // if not, check there is blocked side
                    if (x == 2 || y == 2 || board[x - 3, y - 3] != 0) count--;
                    if (x == edgeCount - 1 || y == edgeCount - 1 || board[x + 1, y + 1] != 0) count--;
                }
            }
            catch (IndexOutOfRangeException)
            {
                return -1;
            }
            return count;
        }

        public int make3mok11(int x, int y)
        {
            int count = -1;
            try
            {
                if (board[x - 1, y - 1] == 0 && board[x, y] == board[x - 2, y - 2] && board[x, y] == board[x - 3, y - 3])
                {
                    count = 2;
                    // if 4-horses
                    if (x > 3 && y > 3 && board[x, y] == board[x - 4, y - 4]) return -1;
                    if (x < edgeCount - 1 && y < edgeCount - 1 && board[x, y] == board[x + 1, y + 1]) return -1;
                    // if not, check there is blocked side
                    if (x == 3 || y == 3 || board[x - 4, y - 4] != 0) count--;
                    if (x == edgeCount - 1 || y == edgeCount - 1 || board[x + 1, y + 1] != 0) count--;
                }
            }
            catch (IndexOutOfRangeException)
            {
                return -1;
            }
            return count;
        }

        public int make3mok12(int x, int y)
        {
            int count = -1;
            try
            {
                if (board[x - 2, y - 2] == 0 && board[x, y] == board[x - 1, y - 1] && board[x, y] == board[x - 3, y - 3])
                {
                    count = 2;
                    // if 4-horses
                    if (x > 3 && y > 3 && board[x, y] == board[x - 4, y - 4]) return -1;
                    if (x < edgeCount - 1 && y < edgeCount - 1 && board[x, y] == board[x + 1, y + 1]) return -1;
                    // if not, check there is blocked side
                    if (x == 3 || y == 3 || board[x - 4, y - 4] != 0) count--;
                    if (x == edgeCount - 1 || y == edgeCount - 1 || board[x + 1, y + 1] != 0) count--;
                }
            }
            catch (IndexOutOfRangeException)
            {
                return -1;
            }
            return count;
        }

        //Judge all the ways to make four-horses-in-one-line
        /*
         - Basically, set count = 2 if it is 4-horses-in-one-line.
         - Calculate as same as 3-horses-in-one-line. 
         - if it is not 4-horses, return -1.
         */

        public int make4mok1(int x, int y)
        {
            int count = -1;
            try
            {
                if (board[x, y] == board[x, y + 1] && board[x, y] == board[x, y + 2] && board[x, y] == board[x, y + 3])
                {
                    count = 2;
                    // check there is blocked side
                    if (y == edgeCount - 4 || board[x, y + 4] != 0) count--;
                    if (y == 0 || board[x, y - 1] != 0) count--;
                }
            }
            catch (IndexOutOfRangeException)
            {
                return -1;
            }
            return count;
        }

        public int make4mok2(int x, int y)
        {
            int count = -1;
            try
            {
                if (board[x, y + 1] == 0 && board[x, y] == board[x, y + 2] && board[x, y] == board[x, y + 3] && board[x, y] == board[x, y + 4]) count = 1;
            }
            catch (IndexOutOfRangeException)
            {
                return -1;
            }
            return count;
        }

        public int make4mok3(int x, int y)
        {
            int count = -1;
            try
            {
                if (board[x, y + 2] == 0 && board[x, y] == board[x, y + 1] && board[x, y] == board[x, y + 3] && board[x, y] == board[x, y + 4]) count = 1;
            }
            catch (IndexOutOfRangeException)
            {
                return -1;
            }
            return count;
        }

        public int make4mok4(int x, int y)
        {
            int count = -1;
            try
            {
                if (board[x, y + 3] == 0 && board[x, y] == board[x, y + 1] && board[x, y] == board[x, y + 2] && board[x, y] == board[x, y + 4]) count = 1;
            }
            catch (IndexOutOfRangeException)
            {
                return -1;
            }
            return count;
        }

        public int make4mok5(int x, int y)
        {
            int count = -1;
            try
            {
                if (board[x, y] == board[x - 1, y + 1] && board[x, y] == board[x - 2, y + 2] && board[x, y] == board[x - 3, y + 3])
                {
                    count = 2;
                    // check there is blocked side
                    if (x == edgeCount - 1 || y == 0 || board[x + 1, y - 1] != 0) count--;
                    if (x == 3 || y == edgeCount - 4 || board[x - 4, y + 4] != 0) count--;
                }
            }
            catch (IndexOutOfRangeException)
            {
                return -1;
            }
            return count;
        }

        public int make4mok6(int x, int y)
        {
            int count = -1;
            try
            {
                if (board[x - 1, y + 1] == 0 && board[x, y] == board[x - 2, y + 2] && board[x, y] == board[x - 3, y + 3] && board[x, y] == board[x - 4, y + 4]) count = 1;
            }
            catch (IndexOutOfRangeException)
            {
                return -1;
            }
            return count;
        }

        public int make4mok7(int x, int y)
        {
            int count = -1;
            try
            {
                if (board[x - 2, y + 2] == 0 && board[x, y] == board[x - 1, y + 1] && board[x, y] == board[x - 3, y + 3] && board[x, y] == board[x - 4, y + 4]) count = 1;
            }
            catch (IndexOutOfRangeException)
            {
                return -1;
            }
            return count;
        }

        public int make4mok8(int x, int y)
        {
            int count = -1;
            try
            {
                if (board[x - 3, y + 3] == 0 && board[x, y] == board[x - 1, y + 1] && board[x, y] == board[x - 2, y + 2] && board[x, y] == board[x - 4, y + 4]) count = 1;
            }
            catch (IndexOutOfRangeException)
            {
                return -1;
            }
            return count;
        }

        public int make4mok9(int x, int y)
        {
            int count = -1;
            try
            {
                if (board[x, y] == board[x - 1, y] && board[x, y] == board[x - 2, y] && board[x - 3, y] == board[x, y])
                {
                    count = 2;
                    // Check there is blocked side
                    if (x == edgeCount - 1 || board[x + 1, y] != 0) count--;
                    if (x == 3 || board[x - 4, y] != 0) count--;
                }
            }
            catch (IndexOutOfRangeException)
            {
                return -1;
            }
            return count;
        }

        public int make4mok10(int x, int y)
        {
            int count = -1;
            try
            {
                if (board[x - 1, y] == 0 && board[x, y] == board[x - 2, y] && board[x, y] == board[x - 3, y] && board[x - 4, y] == board[x, y]) count = 1;
            }
            catch (IndexOutOfRangeException)
            {
                return -1;
            }
            return count;
        }

        public int make4mok11(int x, int y)
        {
            int count = -1;
            try
            {
                if (board[x - 2, y] == 0 && board[x, y] == board[x - 1, y] && board[x, y] == board[x - 3, y] && board[x - 4, y] == board[x, y]) count = 1;
            }
            catch (IndexOutOfRangeException)
            {
                return -1;
            }
            return count;
        }

        public int make4mok12(int x, int y)
        {
            int count = -1;
            try
            {
                if (board[x - 3, y] == 0 && board[x, y] == board[x - 2, y] && board[x, y] == board[x - 1, y] && board[x - 4, y] == board[x, y]) count = 1;
            }
            catch (IndexOutOfRangeException)
            {
                return -1;
            }
            return count;
        }

        public int make4mok13(int x, int y)
        {
            int count = -1;
            try
            {
                if (board[x, y] == board[x - 1, y - 1] && board[x, y] == board[x - 2, y - 2] && board[x, y] == board[x - 3, y - 3])
                {
                    count = 2;
                    // 4목 주변으로 닫혔는지 확인
                    if (x == edgeCount - 1 || y == edgeCount - 1 || board[x + 1, y + 1] != 0) count--;
                    if (x == 3 || y == 3 || board[x - 4, y - 4] != 0) count--;
                }
            }
            catch (IndexOutOfRangeException)
            {
                return -1;
            }
            return count;
        }

        public int make4mok14(int x, int y)
        {
            int count = -1;
            try
            {
                if (board[x - 1, y - 1] == 0 && board[x, y] == board[x - 2, y - 2] && board[x, y] == board[x - 3, y - 3] && board[x, y] == board[x - 4, y - 4]) count = 1;
            }
            catch (IndexOutOfRangeException)
            {
                return -1;
            }
            return count;
        }

        public int make4mok15(int x, int y)
        {
            int count = -1;
            try
            {
                if (board[x - 2, y - 2] == 0 && board[x, y] == board[x - 1, y - 1] && board[x, y] == board[x - 3, y - 3] && board[x, y] == board[x - 4, y - 4]) count = 1;
            }
            catch (IndexOutOfRangeException)
            {
                return -1;
            }
            return count;
        }

        public int make4mok16(int x, int y)
        {
            int count = -1;
            try
            {
                if (board[x - 3, y - 3] == 0 && board[x, y] == board[x - 1, y - 1] && board[x, y] == board[x - 2, y - 2] && board[x, y] == board[x - 4, y - 4]) count = 1;
            }
            catch (IndexOutOfRangeException)
            {
                return -1;
            }
            return count;
        }


        //Judge all the ways to make 2-horses-in-one-line
        /*
         - Basically, Set as same as 3-horses and 4-horses-in-one-line.
         - One-side blocked 2-horses return 1 , Both-side blocked 3 horses return 0
         */

        public int make2mok1(int x, int y)
        {
            int count = -1;
            try
            {
                if (board[x, y] == board[x, y + 1])
                {
                    count = 2;
                    // if 3-horses
                    if (y < edgeCount - 2 && board[x, y] == board[x, y + 2]) return -1;
                    if (y > 0 && board[x, y] == board[x, y - 1]) return -1;
                    // if not, check there is blocked side
                    if (y == edgeCount - 2 || board[x, y + 2] != 0) count--;
                    if (y == 0 || board[x, y - 1] != 0) count--;
                }
            }
            catch (IndexOutOfRangeException)
            {
                return -1;
            }
            return count;
        }


        public int make2mok2(int x, int y)
        {
            int count = -1;
            try
            {
                if (board[x, y] == board[x - 1, y + 1])
                {
                    count = 2;
                    // if 3-horses
                    if (x > 1 && y < edgeCount - 2 && board[x, y] == board[x - 2, y + 2]) return -1;
                    if (x < edgeCount - 1 && y > 0 && board[x, y] == board[x + 1, y - 1]) return -1;
                    // if not, check there is blocked side
                    if (x == 1 || y == edgeCount - 2 || board[x - 2, y + 2] != 0) count--;
                    if (x == edgeCount - 1 || y == 0 || board[x + 1, y - 1] != 0) count--;
                }
            }
            catch (IndexOutOfRangeException)
            {
                return -1;
            }
            return count;
        }

        public int make2mok3(int x, int y)
        {
            int count = -1;
            try
            {
                if (board[x, y] == board[x - 1, y])
                {
                    count = 2;
                    // if 3-horses
                    if (x > 1 && board[x, y] == board[x - 2, y]) return -1;
                    if (x < edgeCount - 1 && board[x, y] == board[x + 1, y]) return -1;
                    // if not, check there is blocked side
                    if (x == 1 || board[x - 2, y] != 0) count--;
                    if (x == edgeCount - 1 || board[x + 1, y] != 0) count--;
                }
            }
            catch (IndexOutOfRangeException)
            {
                return -1;
            }
            return count;
        }

        public int make2mok4(int x, int y)
        {
            int count = -1;
            try
            {
                if (board[x, y] == board[x - 1, y - 1])
                {
                    count = 2;
                    // if 3-horses
                    if (x > 1 && y > 1 && board[x, y] == board[x - 2, y - 2]) return -1;
                    if (x < edgeCount - 1 && y < edgeCount - 1 && board[x, y] == board[x + 1, y + 1]) return -1;
                    // if not, check there is blocked side
                    if (x == 1 || y == 1 || board[x - 2, y - 2] != 0) count--;
                    if (x == edgeCount - 1 || y == edgeCount - 1 || board[x + 1, y + 1] != 0) count--;
                }
            }
            catch (IndexOutOfRangeException)
            {
                return -1;
            }
            return count;
        }


        //Judging each decision's value of this turn and Comparing part
        private int evaluate(Horse horse)
        {
            //variable that checks all of the situations of putting horse
            int sum = 0;
            int open3 = 0;
            int close3 = 0;
            int half3 = 0;
            int open4 = 0;
            int close4 = 0;
            int half4 = 0;
            int open2 = 0;
            int close2 = 0;
            int half2 = 0;
            int count;

            // Check the Board
            for (int i = 0; i < edgeCount; i++)
            {
                for (int j = 0; j < edgeCount; j++)
                {
                    // If it is player's horse
                    if (board[i, j] == horse)
                    {
                        // if 5-horses is made
                        if (make5mok1(i, j) || make5mok2(i, j) || make5mok3(i, j) || make5mok4(i, j) || make5mok5(i, j) ||
                                make5mok6(i, j) || make5mok7(i, j) || make5mok8(i, j) || make5mok9(i, j) || make5mok10(i, j) ||
                                make5mok11(i, j) || make5mok12(i, j) || make5mok13(i, j) || make5mok14(i, j) || make5mok15(i, j) ||
                                make5mok16(i, j) || make5mok17(i, j) || make5mok18(i, j) || make5mok19(i, j) || make5mok20(i, j))
                        {
                            return 1000000;
                        }

                        //Check the situations if only 4-horses is made

                        count = make4mok1(i, j);
                        if (count == 2) open4++;
                        else if (count == 1) half4++;
                        else if (count == 0) close4++;

                        count = make4mok2(i, j);
                        if (count == 2) open4++;
                        else if (count == 1) half4++;
                        else if (count == 0) close4++;

                        count = make4mok3(i, j);
                        if (count == 2) open4++;
                        else if (count == 1) half4++;
                        else if (count == 0) close4++;

                        count = make4mok4(i, j);
                        if (count == 2) open4++;
                        else if (count == 1) half4++;
                        else if (count == 0) close4++;

                        count = make4mok5(i, j);
                        if (count == 2) open4++;
                        else if (count == 1) half4++;
                        else if (count == 0) close4++;

                        count = make4mok6(i, j);
                        if (count == 2) open4++;
                        else if (count == 1) half4++;
                        else if (count == 0) close4++;

                        count = make4mok7(i, j);
                        if (count == 2) open4++;
                        else if (count == 1) half4++;
                        else if (count == 0) close4++;

                        count = make4mok8(i, j);
                        if (count == 2) open4++;
                        else if (count == 1) half4++;
                        else if (count == 0) close4++;

                        count = make4mok9(i, j);
                        if (count == 2) open4++;
                        else if (count == 1) half4++;
                        else if (count == 0) close4++;

                        count = make4mok10(i, j);
                        if (count == 2) open4++;
                        else if (count == 1) half4++;
                        else if (count == 0) close4++;

                        count = make4mok11(i, j);
                        if (count == 2) open4++;
                        else if (count == 1) half4++;
                        else if (count == 0) close4++;

                        count = make4mok12(i, j);
                        if (count == 2) open4++;
                        else if (count == 1) half4++;
                        else if (count == 0) close4++;

                        count = make4mok13(i, j);
                        if (count == 2) open4++;
                        else if (count == 1) half4++;
                        else if (count == 0) close4++;

                        count = make4mok14(i, j);
                        if (count == 2) open4++;
                        else if (count == 1) half4++;
                        else if (count == 0) close4++;

                        count = make4mok15(i, j);
                        if (count == 2) open4++;
                        else if (count == 1) half4++;
                        else if (count == 0) close4++;

                        count = make4mok16(i, j);
                        if (count == 2) open4++;
                        else if (count == 1) half4++;
                        else if (count == 0) close4++;

                        //Check the situations if only 3-horses is made

                        count = make3mok1(i, j);
                        if (count == 2) open3++;
                        else if (count == 1) half3++;
                        else if (count == 0) close3++;

                        count = make3mok2(i, j);
                        if (count == 2) open3++;
                        else if (count == 1) half3++;
                        else if (count == 0) close3++;

                        count = make3mok3(i, j);
                        if (count == 2) open3++;
                        else if (count == 1) half3++;
                        else if (count == 0) close3++;

                        count = make3mok4(i, j);
                        if (count == 2) open3++;
                        else if (count == 1) half3++;
                        else if (count == 0) close3++;

                        count = make3mok5(i, j);
                        if (count == 2) open3++;
                        else if (count == 1) half3++;
                        else if (count == 0) close3++;

                        count = make3mok6(i, j);
                        if (count == 2) open3++;
                        else if (count == 1) half3++;
                        else if (count == 0) close3++;

                        count = make3mok7(i, j);
                        if (count == 2) open3++;
                        else if (count == 1) half3++;
                        else if (count == 0) close3++;

                        count = make3mok8(i, j);
                        if (count == 2) open3++;
                        else if (count == 1) half3++;
                        else if (count == 0) close3++;

                        count = make3mok9(i, j);
                        if (count == 2) open3++;
                        else if (count == 1) half3++;
                        else if (count == 0) close3++;

                        count = make3mok10(i, j);
                        if (count == 2) open3++;
                        else if (count == 1) half3++;
                        else if (count == 0) close3++;

                        count = make3mok11(i, j);
                        if (count == 2) open3++;
                        else if (count == 1) half3++;
                        else if (count == 0) close3++;

                        count = make3mok12(i, j);
                        if (count == 2) open3++;
                        else if (count == 1) half3++;
                        else if (count == 0) close3++;

                        //Check the situations if only 2-horses is made

                        count = make2mok1(i, j);
                        if (count == 2) open2++;
                        else if (count == 1) half2++;
                        else if (count == 0) close2++;

                        count = make2mok2(i, j);
                        if (count == 2) open2++;
                        else if (count == 1) half2++;
                        else if (count == 0) close2++;

                        count = make2mok3(i, j);
                        if (count == 2) open2++;
                        else if (count == 1) half2++;
                        else if (count == 0) close2++;

                        count = make2mok4(i, j);
                        if (count == 2) open2++;
                        else if (count == 1) half2++;
                        else if (count == 0) close2++;

                        //Set Gas Point as much as the horse is closer between center point
                        
                        int middle = edgeCount / 2;
                        if (i > middle)
                        {
                            sum += 500 - ((i - middle) * 20);
                        }
                        else
                        {
                            sum += 500 - (middle - i) * 20;
                        }
                        if (j > middle)
                        {
                            sum += 500 - ((j - middle) * 20);
                        }
                        else
                        {
                            sum += 500 - (middle - j) * 20;
                        }
                    }
                }
            }

            //return Sum of Gas Point + sum
            sum += open4 * 200000;
            sum += half4 * 15000;
            sum += close4 * 1500;
            sum += open3 * 4000;
            sum += half3 * 1500;
            sum += close3 * 300;
            sum += open2 * 1500;
            sum += half2 * 300;
            sum += close2 * 50;
            return sum;
        }

        // AlphaBetaPruning algorithm function
        int AlphaBetaPruning(int level, int alpha, int beta)
        {
            // if approached at limit(deepest depth)
            if (level == limit)
            {
                // return (AI's evaluation value)-(Player's evaluation value)
                return evaluate(aiPlayer) - evaluate(userPlayer);
            }
            // If it's MAX part
            if (level % 2 == 0)
            {
                // Set smallest value as max
                int max = -1000000;
                // Set find = 1 when finished searching
                int find = 0;
                // 전체 바둑판을 모두 탐색
                for (int i = 0; i < edgeCount; i++)
                {
                    for (int j = 0; j < edgeCount; j++)
                    {
                        // Enable to put horse in current location
                        if (board[i, j] == 0)
                        {
                            // Set tempararily put the horse
                            board[i, j] = aiPlayer;
                            // Recursive calling
                            int e = AlphaBetaPruning(level + 1, alpha, beta);
                            // Set does not put the horse
                            board[i, j] = 0;
                            // If found more efficient way
                            if (max < e)
                            {
                                // Set target as that value
                                max = e;
                                if (level == 0)
                                {
                                    targetX = i;
                                    targetY = j;
                                }
                            }
                            // Renew 'alpha'
                            if (alpha < max)
                            {
                                alpha = max;
                                // If alpha is already bigger then beta, don't have to see node anymore
                                if (alpha >= beta) find = 1;
                            }
                        }
                        if (find == 1) break;
                    }
                    if (find == 1) break;
                }
                return max;
            }
            // If it's MIN part
            else
            {
                // Set biggest value as min
                int min = 1000000;
                // Set find = 1 when finishing search
                int find = 0;
                // Search all of the board
                for (int i = 0; i < edgeCount; i++)
                {
                    for (int j = 0; j < edgeCount; j++)
                    {
                        // Enable to put in current location
                        if (board[i, j] == 0)
                        {
                            // Set tempararily put the horse
                            board[i, j] = userPlayer;
                            // Recursive call
                            int e = AlphaBetaPruning(level + 1, alpha, beta);
                            // Set does not put the horse
                            board[i, j] = 0;
                            // If found smaller value
                            if (min > e) min = e;
                            // Renew 'beta'
                            if (beta > min)
                            {
                                beta = min;
                                // If alpha is already bigger then beta, don't have to see node anymore
                                if (alpha >= beta) find = 1;
                            }
                        }
                        if (find == 1) break;
                    }
                    if (find == 1) break;
                }
                return min;
            }
        }


    }
}



