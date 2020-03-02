namespace GomokuClient
{
    partial class SinglePlay
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
            this.Board = new System.Windows.Forms.PictureBox();
            this.PlayButton = new System.Windows.Forms.Button();
            this.Status = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.Board)).BeginInit();
            this.SuspendLayout();
            // 
            // Board
            // 
            this.Board.BackColor = System.Drawing.Color.AntiqueWhite;
            this.Board.Location = new System.Drawing.Point(42, 35);
            this.Board.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Board.Name = "Board";
            this.Board.Size = new System.Drawing.Size(571, 625);
            this.Board.TabIndex = 0;
            this.Board.TabStop = false;
            this.Board.Paint += new System.Windows.Forms.PaintEventHandler(this.Board_Paint);
            this.Board.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Board_MouseDown);
            // 
            // PlayButton
            // 
            this.PlayButton.BackColor = System.Drawing.Color.Black;
            this.PlayButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.PlayButton.Font = new System.Drawing.Font("휴먼옛체", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.PlayButton.ForeColor = System.Drawing.Color.LightSkyBlue;
            this.PlayButton.Location = new System.Drawing.Point(776, 130);
            this.PlayButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.PlayButton.Name = "PlayButton";
            this.PlayButton.Size = new System.Drawing.Size(171, 75);
            this.PlayButton.TabIndex = 1;
            this.PlayButton.Text = "Play";
            this.PlayButton.UseVisualStyleBackColor = false;
            this.PlayButton.Click += new System.EventHandler(this.PlayButton_Click);
            // 
            // Status
            // 
            this.Status.BackColor = System.Drawing.Color.Transparent;
            this.Status.Font = new System.Drawing.Font("굴림", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Status.ForeColor = System.Drawing.Color.White;
            this.Status.Location = new System.Drawing.Point(689, 35);
            this.Status.Name = "Status";
            this.Status.Size = new System.Drawing.Size(343, 38);
            this.Status.TabIndex = 2;
            this.Status.Text = "Press the Play button to start :)";
            this.Status.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SinglePlay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.BackgroundImage = global::GomokuClient.Properties.Resources._522030_BW;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ClientSize = new System.Drawing.Size(1075, 701);
            this.Controls.Add(this.Status);
            this.Controls.Add(this.PlayButton);
            this.Controls.Add(this.Board);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "SinglePlay";
            this.Text = "Gomoku by B611083-SinglePlay";
            ((System.ComponentModel.ISupportInitialize)(this.Board)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox Board;
        private System.Windows.Forms.Button PlayButton;
        private System.Windows.Forms.Label Status;
    }
}