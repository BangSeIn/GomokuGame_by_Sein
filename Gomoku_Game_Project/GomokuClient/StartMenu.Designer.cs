namespace GomokuClient
{
    partial class StartMenu
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.SinglePlayButton = new System.Windows.Forms.Button();
            this.ExitButton = new System.Windows.Forms.Button();
            this.NameofGame = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.MultiPlayButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // SinglePlayButton
            // 
            this.SinglePlayButton.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.SinglePlayButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.SinglePlayButton.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.SinglePlayButton.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.SinglePlayButton.Location = new System.Drawing.Point(355, 268);
            this.SinglePlayButton.Name = "SinglePlayButton";
            this.SinglePlayButton.Size = new System.Drawing.Size(150, 60);
            this.SinglePlayButton.TabIndex = 0;
            this.SinglePlayButton.Text = "Single Play";
            this.SinglePlayButton.UseVisualStyleBackColor = false;
            this.SinglePlayButton.Click += new System.EventHandler(this.SinglePlayButton_Click);
            // 
            // ExitButton
            // 
            this.ExitButton.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ExitButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ExitButton.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ExitButton.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.ExitButton.Location = new System.Drawing.Point(355, 450);
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.Size = new System.Drawing.Size(150, 60);
            this.ExitButton.TabIndex = 2;
            this.ExitButton.Text = "Exit";
            this.ExitButton.UseVisualStyleBackColor = false;
            this.ExitButton.Click += new System.EventHandler(this.ExitButton_Click);
            // 
            // NameofGame
            // 
            this.NameofGame.AutoSize = true;
            this.NameofGame.BackColor = System.Drawing.Color.Transparent;
            this.NameofGame.Font = new System.Drawing.Font("Tahoma", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NameofGame.ForeColor = System.Drawing.Color.Black;
            this.NameofGame.Location = new System.Drawing.Point(220, 111);
            this.NameofGame.Name = "NameofGame";
            this.NameofGame.Size = new System.Drawing.Size(417, 48);
            this.NameofGame.TabIndex = 3;
            this.NameofGame.Text = "Gomoku Game(Omok)";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("굴림", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.ForeColor = System.Drawing.Color.MidnightBlue;
            this.label1.Location = new System.Drawing.Point(574, 175);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(139, 21);
            this.label1.TabIndex = 4;
            this.label1.Text = "made by Sein";
            // 
            // MultiPlayButton
            // 
            this.MultiPlayButton.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.MultiPlayButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.MultiPlayButton.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.MultiPlayButton.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.MultiPlayButton.Location = new System.Drawing.Point(355, 356);
            this.MultiPlayButton.Name = "MultiPlayButton";
            this.MultiPlayButton.Size = new System.Drawing.Size(150, 60);
            this.MultiPlayButton.TabIndex = 5;
            this.MultiPlayButton.Text = "Multi Play";
            this.MultiPlayButton.UseVisualStyleBackColor = false;
            this.MultiPlayButton.Click += new System.EventHandler(this.MultiPlayButton_Click);
            // 
            // StartMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.BackgroundImage = global::GomokuClient.Properties.Resources._522030_BW;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ClientSize = new System.Drawing.Size(884, 561);
            this.Controls.Add(this.MultiPlayButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.NameofGame);
            this.Controls.Add(this.ExitButton);
            this.Controls.Add(this.SinglePlayButton);
            this.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.Name = "StartMenu";
            this.Text = "Gomoku by B611083";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button SinglePlayButton;
        private System.Windows.Forms.Button ExitButton;
        private System.Windows.Forms.Label NameofGame;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button MultiPlayButton;
    }
}

