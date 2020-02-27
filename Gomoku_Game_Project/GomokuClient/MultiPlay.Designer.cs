﻿using System;

namespace GomokuClient
{
    partial class MultiPlay
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
            this.EnterButton = new System.Windows.Forms.Button();
            this.RoomTextBox = new System.Windows.Forms.TextBox();
            this.PlayButton = new System.Windows.Forms.Button();
            this.Status = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.Board)).BeginInit();
            this.SuspendLayout();
            // 
            // Board
            // 
            this.Board.BackColor = System.Drawing.Color.AntiqueWhite;
            this.Board.Location = new System.Drawing.Point(37, 28);
            this.Board.Name = "Board";
            this.Board.Size = new System.Drawing.Size(500, 500);
            this.Board.TabIndex = 1;
            this.Board.TabStop = false;
            this.Board.Paint += new System.Windows.Forms.PaintEventHandler(this.Board_Paint);
            this.Board.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Board_MouseDown);
            // 
            // EnterButton
            // 
            this.EnterButton.BackColor = System.Drawing.Color.Black;
            this.EnterButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.EnterButton.Font = new System.Drawing.Font("휴먼옛체", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.EnterButton.ForeColor = System.Drawing.Color.LightSkyBlue;
            this.EnterButton.Location = new System.Drawing.Point(679, 110);
            this.EnterButton.Name = "EnterButton";
            this.EnterButton.Size = new System.Drawing.Size(150, 60);
            this.EnterButton.TabIndex = 2;
            this.EnterButton.Text = "Enter";
            this.EnterButton.UseVisualStyleBackColor = false;
            this.EnterButton.Click += new System.EventHandler(this.EnterButton_Click);
            // 
            // RoomTextBox
            // 
            this.RoomTextBox.Location = new System.Drawing.Point(648, 71);
            this.RoomTextBox.Name = "RoomTextBox";
            this.RoomTextBox.Size = new System.Drawing.Size(209, 21);
            this.RoomTextBox.TabIndex = 3;
            // 
            // PlayButton
            // 
            this.PlayButton.BackColor = System.Drawing.Color.Black;
            this.PlayButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.PlayButton.Font = new System.Drawing.Font("휴먼옛체", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.PlayButton.ForeColor = System.Drawing.Color.LightSkyBlue;
            this.PlayButton.Location = new System.Drawing.Point(679, 214);
            this.PlayButton.Name = "PlayButton";
            this.PlayButton.Size = new System.Drawing.Size(150, 60);
            this.PlayButton.TabIndex = 4;
            this.PlayButton.Text = "Play";
            this.PlayButton.UseVisualStyleBackColor = false;
            this.PlayButton.Click += new System.EventHandler(this.PlayButton_Click);
            // 
            // Status
            // 
            this.Status.Font = new System.Drawing.Font("굴림", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Status.Location = new System.Drawing.Point(601, 28);
            this.Status.Name = "Status";
            this.Status.Size = new System.Drawing.Size(300, 30);
            this.Status.TabIndex = 5;
            this.Status.Text = "Create the Room to enter :)";
            this.Status.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MultiPlay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(941, 561);
            this.Controls.Add(this.Status);
            this.Controls.Add(this.PlayButton);
            this.Controls.Add(this.RoomTextBox);
            this.Controls.Add(this.EnterButton);
            this.Controls.Add(this.Board);
            this.Name = "MultiPlay";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.Board)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void MultiPlay_Load(object sender, EventArgs e)
        {

        }

        #endregion

        private System.Windows.Forms.PictureBox Board;
        private System.Windows.Forms.Button EnterButton;
        private System.Windows.Forms.TextBox RoomTextBox;
        private System.Windows.Forms.Button PlayButton;
        private System.Windows.Forms.Label Status;
    }
}