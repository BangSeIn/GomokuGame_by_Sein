using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace GomokuClient
{
    public partial class MultiPlay : Form
    {

        private Thread thread; // Thread for protocol
        private TcpClient TcpClient;// TCP Client
        private NetworkStream stream;

        private const int RectSize = 33;
        private const int EdgeCount = 15;

        private enum Horse { none = 0, BLACK, WHITE };
        private Horse[,] board = new Horse[EdgeCount, EdgeCount];
        private Horse NowPlayer = Horse.BLACK;
        private bool nowTurn;

        private bool Playing;
        private bool entered;
        private bool threading;



        private bool Judge(Horse NowPlayer) //Function judges Victory
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
                for (int j = 4; j < EdgeCount - 4; j++)
                    if (board[i, j] == NowPlayer && board[i + 1, j + 1] == NowPlayer && board[i + 2, j + 2] == NowPlayer &&
                        board[i + 3, j + 3] == NowPlayer && board[i + 4, j + 4] == NowPlayer)
                        return true;
            for (int i = 4; i < EdgeCount; i++) //Y = -X Line
                for (int j = 0; j < EdgeCount - 4; j++)
                    if (board[i, j] == NowPlayer && board[i - 1, j + 1] == NowPlayer && board[i - 2, j + 2] == NowPlayer &&
                        board[i - 3, j + 3] == NowPlayer && board[i - 4, j + 4] == NowPlayer)
                        return true;
            return false;
        }

        private void refresh()
        {
            this.Board.Refresh();
            for (int i = 0; i < EdgeCount; i++)
                for (int j = 0; j < EdgeCount; j++)
                    board[i, j] = Horse.none;
            PlayButton.Enabled = false; 
        }

        private void PlayButton_Click(object sender, EventArgs e)
        {
            if (!Playing)
            {
                refresh();
                Playing = true;
                String message = "[Play]";
                Status.Text = "Now it's " + NowPlayer.ToString() + "'s turn.";
                byte[] buf = Encoding.ASCII.GetBytes(message + this.RoomTextBox.Text);
                stream.Write(buf, 0, buf.Length);
                this.Status.Text = "Waiting until the Opponent's Ready..";
                this.PlayButton.Enabled = false;
            }
           
        }

        public MultiPlay()
        {
            InitializeComponent();
            this.PlayButton.Enabled = false;
            Playing = false;
            entered = false;
            threading = false;
            board = new Horse[EdgeCount, EdgeCount];
            nowTurn = false;
        }

        private void EnterButton_Click(object sender, EventArgs e)
        {
            TcpClient = new TcpClient();
            TcpClient.Connect("127.0.0.1", 9704);
            stream = TcpClient.GetStream();
            thread = new Thread(new ThreadStart(read));
            thread.Start();
            threading = true;

            /* Proceed Room entering */
            string message = "[Enter]";
            byte[] buf = Encoding.ASCII.GetBytes(message + this.RoomTextBox.Text);
            stream.Write(buf, 0, buf.Length);
        }
        /* Get message from server */
        private void read()
        {
            while (true)
            {
                byte[] buf = new byte[1024];
                int bufBytes = stream.Read(buf, 0, buf.Length);
                string message = Encoding.ASCII.GetString(buf, 0, bufBytes);
                /* Connection succeed (Message : [Enter]) */
                if (message.Contains("[Enter]"))
                {
                    this.Status.Text = "Entered into the Room#" + this.RoomTextBox.Text;
                    /* Starting game */
                    this.RoomTextBox.Enabled = false;
                    this.EnterButton.Enabled = false;
                    entered = true;
                }
                /* If the Room is full (message : [Full]) */
                if (message.Contains("[Full]"))
                {
                    this.Status.Text = "The Room is already full.";
                    CloseNetwork();
                }
                /* Game start (message: [Play]{Horse}) */
                if (message.Contains("[Play]"))
                {
                    refresh();
                    string horse = message.Split(']')[1];
                    if (horse.Contains("Black"))
                    {
                        this.Status.Text = "Now it's your turn.";
                        nowTurn = true;
                        NowPlayer = Horse.BLACK;
                    }
                    else
                    {
                        this.Status.Text = "Opponent is playing..";
                        nowTurn = false;
                        NowPlayer = Horse.WHITE;
                    }
                    Playing = true;
                }
                /* If the opponent quit (message: [Exit]) */
                if (message.Contains("[Exit]"))
                {
                    this.Status.Text = "The Opponent left the game.";
                    refresh();
                }
                /* If the opponent set the horse (message: [Put]{X,Y}) */
                if (message.Contains("[Put]"))
                {
                    string position = message.Split(']')[1];
                    int x = Convert.ToInt32(position.Split(',')[0]);
                    int y = Convert.ToInt32(position.Split(',')[1]);
                    Horse enemyPlayer = Horse.none;
                    if (NowPlayer == Horse.BLACK)
                    {
                        enemyPlayer = Horse.WHITE;
                    }
                    else
                    {
                        enemyPlayer = Horse.BLACK;
                    }
                    if (board[x, y] != Horse.none) continue;
                    board[x, y] = enemyPlayer;
                    Graphics g = this.Board.CreateGraphics();
                    if (enemyPlayer == Horse.BLACK)
                    {
                        SolidBrush brush = new SolidBrush(Color.Black);
                        g.FillEllipse(brush, x * RectSize, y * RectSize, RectSize, RectSize);
                    }
                    else
                    {
                        SolidBrush brush = new SolidBrush(Color.White);
                        g.FillEllipse(brush, x * RectSize, y * RectSize, RectSize, RectSize);
                    }
                    if (Judge(enemyPlayer))
                    {
                        Status.Text = "You lost the Game:(";
                        Playing = false;
                        PlayButton.Text = "Restart";
                        PlayButton.Enabled = true;
                    }

                    else
                    {
                        Status.Text = "Now it's your turn.";
                    }
                    nowTurn = true;
                }
            }
        }

        private void Board_MouseDown(object sender, MouseEventArgs e)
        {
            if (!Playing)
            {
                MessageBox.Show("Set the Room Number and click Enter button.\nThen, click Play button to start.");
                return;
            }
            Graphics g = this.Board.CreateGraphics();
            int x = e.X / RectSize;
            int y = e.Y / RectSize;
            // 0 ~ 14 cell
            if (x < 0 || y < 0 || x >= EdgeCount || y >= EdgeCount)
            {
                MessageBox.Show("Sorry, you can't put outisde of the Board :(");
                return;
            }
            if (board[x, y] != Horse.none) return;
            board[x, y] = NowPlayer;
            if (NowPlayer == Horse.BLACK)
            {
                SolidBrush brush = new SolidBrush(Color.Black);
                g.FillEllipse(brush, x * RectSize, y * RectSize, RectSize, RectSize);
            }
            else
            {
                SolidBrush brush = new SolidBrush(Color.White);
                g.FillEllipse(brush, x * RectSize, y * RectSize, RectSize, RectSize);
            }

            /* Send the location of horse */
            string message = "[Put]" + RoomTextBox.Text + "," + x + "," + y;
            byte[] buf = Encoding.ASCII.GetBytes(message);
            stream.Write(buf, 0, buf.Length);

            /* Process judgement */
            if (Judge(NowPlayer))
            {
                Status.Text = "You're the WINNER!";
                Playing = false;
                PlayButton.Text = "Restart";
                PlayButton.Enabled = true;
                return;
            }
            else
            {
                Status.Text = "Opponent is playing..";
            }
            /* Set as Opponent's turn */
            nowTurn = false;
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
            for (int i = RectSize + RectSize / 2; i < RectSize * EdgeCount - RectSize / 2; i += RectSize)
            {
                gp.DrawLine(p, RectSize / 2, i, RectSize * EdgeCount - RectSize / 2, i);
                gp.DrawLine(p, i, RectSize / 2, i, RectSize * EdgeCount - RectSize / 2);

            }
        }

        private void MultiPlayForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            CloseNetwork();
        }
        void CloseNetwork()
        {
            if (threading && thread.IsAlive) thread.Abort();
            if (entered)
            {
                TcpClient.Close();
            }
        }
    }

}
