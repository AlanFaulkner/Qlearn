namespace Qlearn
{
    partial class PlayGame
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
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.PlayButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.PlayerOneSelect = new System.Windows.Forms.ComboBox();
            this.PlayerTwoSelect = new System.Windows.Forms.ComboBox();
            this.button11 = new System.Windows.Forms.Button();
            this.button12 = new System.Windows.Forms.Button();
            this.button13 = new System.Windows.Forms.Button();
            this.button14 = new System.Windows.Forms.Button();
            this.button15 = new System.Windows.Forms.Button();
            this.button16 = new System.Windows.Forms.Button();
            this.button17 = new System.Windows.Forms.Button();
            this.button18 = new System.Windows.Forms.Button();
            this.button19 = new System.Windows.Forms.Button();
            this.NetworkFilename = new System.Windows.Forms.TextBox();
            this.RetrainNetwork = new System.Windows.Forms.CheckBox();
            this.UpdateBox = new System.Windows.Forms.CheckBox();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.TrainButton = new System.Windows.Forms.Button();
            this.NumberOfGames = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.SuggestMove = new System.Windows.Forms.GroupBox();
            this.NeuralNetBox = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.GameSetupBox = new System.Windows.Forms.GroupBox();
            this.TrainNetworkBox = new System.Windows.Forms.GroupBox();
            this.GameBoard = new System.Windows.Forms.GroupBox();
            this.TrainNeuralNetowrk = new System.ComponentModel.BackgroundWorker();
            this.SuggestMove.SuspendLayout();
            this.NeuralNetBox.SuspendLayout();
            this.GameSetupBox.SuspendLayout();
            this.TrainNetworkBox.SuspendLayout();
            this.GameBoard.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(4, 20);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(40, 40);
            this.button1.TabIndex = 0;
            this.button1.Tag = "0";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Board_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(50, 20);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(40, 40);
            this.button2.TabIndex = 1;
            this.button2.Tag = "1";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.Board_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(96, 20);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(40, 40);
            this.button3.TabIndex = 2;
            this.button3.Tag = "2";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.Board_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(4, 66);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(40, 40);
            this.button4.TabIndex = 3;
            this.button4.Tag = "3";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.Board_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(50, 66);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(40, 40);
            this.button5.TabIndex = 4;
            this.button5.Tag = "4";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.Board_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(96, 66);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(40, 40);
            this.button6.TabIndex = 5;
            this.button6.Tag = "5";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.Board_Click);
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(4, 112);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(40, 40);
            this.button7.TabIndex = 6;
            this.button7.Tag = "6";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.Board_Click);
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(50, 112);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(40, 40);
            this.button8.TabIndex = 7;
            this.button8.Tag = "7";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.Board_Click);
            // 
            // button9
            // 
            this.button9.Location = new System.Drawing.Point(96, 112);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(40, 40);
            this.button9.TabIndex = 8;
            this.button9.Tag = "8";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.Board_Click);
            // 
            // PlayButton
            // 
            this.PlayButton.Location = new System.Drawing.Point(22, 125);
            this.PlayButton.Name = "PlayButton";
            this.PlayButton.Size = new System.Drawing.Size(74, 24);
            this.PlayButton.TabIndex = 9;
            this.PlayButton.Text = "Play";
            this.PlayButton.UseVisualStyleBackColor = true;
            this.PlayButton.Click += new System.EventHandler(this.PlayButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Player 1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 61);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Player 2";
            // 
            // PlayerOneSelect
            // 
            this.PlayerOneSelect.FormattingEnabled = true;
            this.PlayerOneSelect.Items.AddRange(new object[] {
            "Human",
            "ANN"});
            this.PlayerOneSelect.Location = new System.Drawing.Point(10, 33);
            this.PlayerOneSelect.Name = "PlayerOneSelect";
            this.PlayerOneSelect.Size = new System.Drawing.Size(99, 21);
            this.PlayerOneSelect.TabIndex = 12;
            // 
            // PlayerTwoSelect
            // 
            this.PlayerTwoSelect.FormattingEnabled = true;
            this.PlayerTwoSelect.Items.AddRange(new object[] {
            "Human",
            "ANN"});
            this.PlayerTwoSelect.Location = new System.Drawing.Point(10, 77);
            this.PlayerTwoSelect.Name = "PlayerTwoSelect";
            this.PlayerTwoSelect.Size = new System.Drawing.Size(99, 21);
            this.PlayerTwoSelect.TabIndex = 13;
            // 
            // button11
            // 
            this.button11.Enabled = false;
            this.button11.Location = new System.Drawing.Point(6, 16);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(30, 30);
            this.button11.TabIndex = 22;
            this.button11.Tag = "0";
            this.button11.UseVisualStyleBackColor = true;
            // 
            // button12
            // 
            this.button12.Enabled = false;
            this.button12.Location = new System.Drawing.Point(42, 16);
            this.button12.Name = "button12";
            this.button12.Size = new System.Drawing.Size(30, 30);
            this.button12.TabIndex = 21;
            this.button12.Tag = "1";
            this.button12.UseVisualStyleBackColor = true;
            // 
            // button13
            // 
            this.button13.Enabled = false;
            this.button13.Location = new System.Drawing.Point(78, 16);
            this.button13.Name = "button13";
            this.button13.Size = new System.Drawing.Size(30, 30);
            this.button13.TabIndex = 20;
            this.button13.Tag = "2";
            this.button13.UseVisualStyleBackColor = true;
            // 
            // button14
            // 
            this.button14.Enabled = false;
            this.button14.Location = new System.Drawing.Point(6, 52);
            this.button14.Name = "button14";
            this.button14.Size = new System.Drawing.Size(30, 30);
            this.button14.TabIndex = 19;
            this.button14.Tag = "3";
            this.button14.UseVisualStyleBackColor = true;
            // 
            // button15
            // 
            this.button15.Enabled = false;
            this.button15.Location = new System.Drawing.Point(42, 52);
            this.button15.Name = "button15";
            this.button15.Size = new System.Drawing.Size(30, 30);
            this.button15.TabIndex = 18;
            this.button15.Tag = "4";
            this.button15.UseVisualStyleBackColor = true;
            // 
            // button16
            // 
            this.button16.Enabled = false;
            this.button16.Location = new System.Drawing.Point(78, 52);
            this.button16.Name = "button16";
            this.button16.Size = new System.Drawing.Size(30, 30);
            this.button16.TabIndex = 17;
            this.button16.Tag = "5";
            this.button16.UseVisualStyleBackColor = true;
            // 
            // button17
            // 
            this.button17.Enabled = false;
            this.button17.Location = new System.Drawing.Point(6, 88);
            this.button17.Name = "button17";
            this.button17.Size = new System.Drawing.Size(30, 30);
            this.button17.TabIndex = 16;
            this.button17.Tag = "6";
            this.button17.UseVisualStyleBackColor = true;
            // 
            // button18
            // 
            this.button18.Enabled = false;
            this.button18.Location = new System.Drawing.Point(42, 88);
            this.button18.Name = "button18";
            this.button18.Size = new System.Drawing.Size(30, 30);
            this.button18.TabIndex = 15;
            this.button18.Tag = "7";
            this.button18.UseVisualStyleBackColor = true;
            // 
            // button19
            // 
            this.button19.Enabled = false;
            this.button19.Location = new System.Drawing.Point(78, 88);
            this.button19.Name = "button19";
            this.button19.Size = new System.Drawing.Size(30, 30);
            this.button19.TabIndex = 14;
            this.button19.Tag = "8";
            this.button19.UseVisualStyleBackColor = true;
            // 
            // NetworkFilename
            // 
            this.NetworkFilename.Location = new System.Drawing.Point(64, 19);
            this.NetworkFilename.Name = "NetworkFilename";
            this.NetworkFilename.Size = new System.Drawing.Size(113, 20);
            this.NetworkFilename.TabIndex = 25;
            this.NetworkFilename.Text = "Network_Trained.net";
            // 
            // RetrainNetwork
            // 
            this.RetrainNetwork.AutoSize = true;
            this.RetrainNetwork.Location = new System.Drawing.Point(12, 56);
            this.RetrainNetwork.Name = "RetrainNetwork";
            this.RetrainNetwork.Size = new System.Drawing.Size(67, 17);
            this.RetrainNetwork.TabIndex = 27;
            this.RetrainNetwork.Text = "Re-Train";
            this.RetrainNetwork.UseVisualStyleBackColor = true;
            // 
            // UpdateBox
            // 
            this.UpdateBox.AutoSize = true;
            this.UpdateBox.Location = new System.Drawing.Point(9, 104);
            this.UpdateBox.Name = "UpdateBox";
            this.UpdateBox.Size = new System.Drawing.Size(104, 17);
            this.UpdateBox.TabIndex = 28;
            this.UpdateBox.Text = "Update Network";
            this.UpdateBox.UseVisualStyleBackColor = true;
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(12, 81);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(165, 23);
            this.progressBar.TabIndex = 29;
            this.progressBar.Visible = false;
            // 
            // TrainButton
            // 
            this.TrainButton.Location = new System.Drawing.Point(102, 52);
            this.TrainButton.Name = "TrainButton";
            this.TrainButton.Size = new System.Drawing.Size(75, 23);
            this.TrainButton.TabIndex = 30;
            this.TrainButton.Text = "Train";
            this.TrainButton.UseVisualStyleBackColor = true;
            this.TrainButton.Click += new System.EventHandler(this.TrainButton_Click);
            // 
            // NumberOfGames
            // 
            this.NumberOfGames.Location = new System.Drawing.Point(124, 24);
            this.NumberOfGames.Name = "NumberOfGames";
            this.NumberOfGames.Size = new System.Drawing.Size(53, 20);
            this.NumberOfGames.TabIndex = 31;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 27);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(90, 13);
            this.label5.TabIndex = 32;
            this.label5.Text = "Number of games";
            // 
            // SuggestMove
            // 
            this.SuggestMove.Controls.Add(this.button11);
            this.SuggestMove.Controls.Add(this.button12);
            this.SuggestMove.Controls.Add(this.button13);
            this.SuggestMove.Controls.Add(this.button14);
            this.SuggestMove.Controls.Add(this.button15);
            this.SuggestMove.Controls.Add(this.button16);
            this.SuggestMove.Controls.Add(this.button17);
            this.SuggestMove.Controls.Add(this.button18);
            this.SuggestMove.Controls.Add(this.button19);
            this.SuggestMove.Location = new System.Drawing.Point(194, 12);
            this.SuggestMove.Name = "SuggestMove";
            this.SuggestMove.Size = new System.Drawing.Size(115, 127);
            this.SuggestMove.TabIndex = 33;
            this.SuggestMove.TabStop = false;
            this.SuggestMove.Text = "Suggested Move";
            // 
            // NeuralNetBox
            // 
            this.NeuralNetBox.Controls.Add(this.label3);
            this.NeuralNetBox.Controls.Add(this.NetworkFilename);
            this.NeuralNetBox.Location = new System.Drawing.Point(151, 165);
            this.NeuralNetBox.Name = "NeuralNetBox";
            this.NeuralNetBox.Size = new System.Drawing.Size(196, 45);
            this.NeuralNetBox.TabIndex = 34;
            this.NeuralNetBox.TabStop = false;
            this.NeuralNetBox.Text = "Neural Network";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 23);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 13);
            this.label3.TabIndex = 33;
            this.label3.Text = "Filename";
            // 
            // GameSetupBox
            // 
            this.GameSetupBox.Controls.Add(this.UpdateBox);
            this.GameSetupBox.Controls.Add(this.PlayerTwoSelect);
            this.GameSetupBox.Controls.Add(this.PlayerOneSelect);
            this.GameSetupBox.Controls.Add(this.label2);
            this.GameSetupBox.Controls.Add(this.label1);
            this.GameSetupBox.Controls.Add(this.PlayButton);
            this.GameSetupBox.Location = new System.Drawing.Point(16, 165);
            this.GameSetupBox.Name = "GameSetupBox";
            this.GameSetupBox.Size = new System.Drawing.Size(120, 155);
            this.GameSetupBox.TabIndex = 35;
            this.GameSetupBox.TabStop = false;
            this.GameSetupBox.Text = "Game Setup";
            // 
            // TrainNetworkBox
            // 
            this.TrainNetworkBox.Controls.Add(this.label5);
            this.TrainNetworkBox.Controls.Add(this.NumberOfGames);
            this.TrainNetworkBox.Controls.Add(this.TrainButton);
            this.TrainNetworkBox.Controls.Add(this.progressBar);
            this.TrainNetworkBox.Controls.Add(this.RetrainNetwork);
            this.TrainNetworkBox.Location = new System.Drawing.Point(151, 211);
            this.TrainNetworkBox.Name = "TrainNetworkBox";
            this.TrainNetworkBox.Size = new System.Drawing.Size(196, 109);
            this.TrainNetworkBox.TabIndex = 34;
            this.TrainNetworkBox.TabStop = false;
            this.TrainNetworkBox.Text = "Auto Train Network";
            // 
            // GameBoard
            // 
            this.GameBoard.Controls.Add(this.button9);
            this.GameBoard.Controls.Add(this.button8);
            this.GameBoard.Controls.Add(this.button7);
            this.GameBoard.Controls.Add(this.button6);
            this.GameBoard.Controls.Add(this.button5);
            this.GameBoard.Controls.Add(this.button4);
            this.GameBoard.Controls.Add(this.button3);
            this.GameBoard.Controls.Add(this.button2);
            this.GameBoard.Controls.Add(this.button1);
            this.GameBoard.Location = new System.Drawing.Point(5, 4);
            this.GameBoard.Name = "GameBoard";
            this.GameBoard.Size = new System.Drawing.Size(142, 160);
            this.GameBoard.TabIndex = 36;
            this.GameBoard.TabStop = false;
            this.GameBoard.Text = "GameBoard";
            // 
            // TrainNeuralNetowrk
            // 
            this.TrainNeuralNetowrk.WorkerReportsProgress = true;
            this.TrainNeuralNetowrk.DoWork += new System.ComponentModel.DoWorkEventHandler(this.TrainNeuralNetowrk_DoWork);
            this.TrainNeuralNetowrk.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.TrainNeuralNetwork_ReportProgress);
            this.TrainNeuralNetowrk.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.TrainNeuralNetwork_Complete);
            // 
            // PlayGame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(359, 335);
            this.Controls.Add(this.GameBoard);
            this.Controls.Add(this.TrainNetworkBox);
            this.Controls.Add(this.GameSetupBox);
            this.Controls.Add(this.NeuralNetBox);
            this.Controls.Add(this.SuggestMove);
            this.Name = "PlayGame";
            this.Text = "Tic-Tac-Toe";
            this.SuggestMove.ResumeLayout(false);
            this.NeuralNetBox.ResumeLayout(false);
            this.NeuralNetBox.PerformLayout();
            this.GameSetupBox.ResumeLayout(false);
            this.GameSetupBox.PerformLayout();
            this.TrainNetworkBox.ResumeLayout(false);
            this.TrainNetworkBox.PerformLayout();
            this.GameBoard.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Button PlayButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox PlayerOneSelect;
        private System.Windows.Forms.ComboBox PlayerTwoSelect;
        private System.Windows.Forms.Button button11;
        private System.Windows.Forms.Button button12;
        private System.Windows.Forms.Button button13;
        private System.Windows.Forms.Button button14;
        private System.Windows.Forms.Button button15;
        private System.Windows.Forms.Button button16;
        private System.Windows.Forms.Button button17;
        private System.Windows.Forms.Button button18;
        private System.Windows.Forms.Button button19;
        private System.Windows.Forms.TextBox NetworkFilename;
        private System.Windows.Forms.CheckBox RetrainNetwork;
        private System.Windows.Forms.CheckBox UpdateBox;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Button TrainButton;
        private System.Windows.Forms.TextBox NumberOfGames;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox SuggestMove;
        private System.Windows.Forms.GroupBox NeuralNetBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox GameSetupBox;
        private System.Windows.Forms.GroupBox TrainNetworkBox;
        private System.Windows.Forms.GroupBox GameBoard;
        private System.ComponentModel.BackgroundWorker TrainNeuralNetowrk;
    }
}