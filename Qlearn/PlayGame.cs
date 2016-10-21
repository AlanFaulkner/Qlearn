using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using @this;

namespace Qlearn
{
    public partial class PlayGame : Form
    {
        static List<int> Net = new List<int> { 9, 27, 9, 9 };
        QlearningNN NN = new QlearningNN(Net);
        

        public PlayGame()
        {
            InitializeComponent();
            PlayerOneSelect.SelectedIndex = 0;
            PlayerTwoSelect.SelectedIndex = 0;
        }

        // Play game button.
        private void PlayButton_Click(object sender, EventArgs e)
        {
            List<Button> Gameboard = new List<Button> { button1, button2, button3, button4, button5, button6, button7, button8, button9 };

            GameSetupBox.Enabled = false;
            NeuralNetBox.Enabled = false;
            TrainNetworkBox.Enabled = false;

            if (NetworkFilename.Text != "") { NN.LoadNetwork(NetworkFilename.Text); } // Loads network from file
            
            for (int i = 0; i < Gameboard.Count; i++)           // Initialize board
            {
                NN.GameBoard[i] = 0;                            // Initialize game board
                Gameboard[i].BackColor = Color.LightGray;       // Blank board
                Gameboard[i].Enabled = true;                    // Board position enabled (Playable)
            }
            NN.SetDiscountRate = 0.7;
            NN.CurrentPlayer = 1;
            SuggestedMove();
            Console.Clear();
            if (PlayerOneSelect.Text == "ANN") { ANN_Move(); }
        }

        // Neural Network Move
        private void ANN_Move()
        {
            List<double> GameDataRow = new List<double> { };
            GameDataRow.Add(NN.CurrentPlayer);
            GameDataRow.Add(0);
            GameDataRow.AddRange(NN.GameBoard);
            List<double> ANNOut = NN.GetSuggestedMove();
            GameDataRow.AddRange(ANNOut);
            int Move = NN.BestMove();
            GameDataRow.Add(Move);
            GameDataRow.Add(NN.GetReward(NN.GameBoard, Move, false));
            NN.UpdateGameData(GameDataRow);
            UpdateBoard();
            EndGame();
        }

        // Human Move.
        private void Board_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            List<double> GameDataRow = new List<double> { };
            GameDataRow.Add(NN.CurrentPlayer);
            GameDataRow.Add(0);
            GameDataRow.AddRange(NN.GameBoard);
            List<double> ANNOut = NN.GetSuggestedMove();
            GameDataRow.AddRange(ANNOut);

            if (NN.CurrentPlayer == 1)
            {
                NN.GameBoard[Convert.ToInt32(button.Tag)] = 1;
                GameDataRow.Add(Convert.ToInt32(button.Tag));
                NN.CurrentPlayer = -1;
                
            }
            else
            {
                NN.GameBoard[Convert.ToInt32(button.Tag)] = -1;
                GameDataRow.Add(Convert.ToInt32(button.Tag));
                NN.CurrentPlayer = 1;
            }
            GameDataRow.Add(NN.GetReward(NN.GameBoard,Convert.ToInt32(button.Tag),false));
            NN.UpdateGameData(GameDataRow);
            UpdateBoard();
            if (!EndGame()) {ANN_Move(); }
        }

        private void SuggestedMove()
        {
            List<Button> GameBoard = new List<Button> { button11, button12, button13, button14, button15, button16, button17, button18, button19 };
            List<double> NNOutput = NN.GetSuggestedMove();
            //red 200,0,0 green 25,175,0

            // ignore positions that are impossible
            double Min = 1;
            double Max = 0;
            
                for (int i = 0; i < NNOutput.Count; i++)
                {
                    if (NN.GameBoard[i] == 0 && NNOutput[i] > Max) { Max = NNOutput[i]; }
                    if (NN.GameBoard[i] == 0 && NNOutput[i] < Min) { Min = NNOutput[i]; }
                }

            double range;
            if (Min == Max) { range = 1; }
            else range = Max - Min;

            //rescale outputs
            for (int i = 0; i < NNOutput.Count; i++) { NNOutput[i] = (175 / range) * (NNOutput[i] - Max) + 175; }

            //set color red bad green good
            for (int i = 0; i < NNOutput.Count; i++)
            {
                if (NN.GameBoard[i] == 0)
                {
                    Color col = new Color();
                    col = Color.FromArgb(200 - Convert.ToInt32(NNOutput[i]), Convert.ToInt32(NNOutput[i]), 0);
                    GameBoard[i].BackColor = col;
                }
                else
                {
                    GameBoard[i].BackColor = Color.LightGray;
                }
            }
        }

        private void UpdateBoard()
        {
            List<Button> GameBoard = new List<Button> { button1, button2, button3, button4, button5, button6, button7, button8, button9 };

            for (int i = 0; i < NN.GameBoard.Count; i++)
            {
                if (NN.GameBoard[i] == -1)
                {
                    GameBoard[i].BackColor = Color.Blue;
                    GameBoard[i].Enabled = false;
                }
                else if (NN.GameBoard[i] == 1)
                {
                    GameBoard[i].BackColor = Color.Red;
                    GameBoard[i].Enabled = false;
                }
            }
        }

        private bool EndGame()
        {
            List<Button> Gameboard = new List<Button> { button1, button2, button3, button4, button5, button6, button7, button8, button9 };
            List<Button> Hints = new List<Button> { button11, button12, button13, button14, button15, button16, button17, button18, button19 };

            if (NN.GameOver())
            {
                
                for (int i = 0; i < Gameboard.Count; i++)
                {
                    Gameboard[i].Enabled = false;
                    Hints[i].BackColor = Color.LightGray;
                } // disable game board and clear suggest moves.
                MessageBox.Show("Game Over");
                GameSetupBox.Enabled = true;
                NeuralNetBox.Enabled = true;
                TrainNetworkBox.Enabled = true;
                NN.printgamedata("inital.txt");
                Console.WriteLine();
                if (UpdateBox.Checked) { Console.Write("Updating\n"); NN.UpdateNetwork(); }
                NetworkFilename.Text = "Network_Trained.net";
                
                
                return true;
            }
            SuggestedMove();
            return false;
        }

        private void TrainButton_Click(object sender, EventArgs e)
        {
            // validate number of games
            if (NumberOfGames.Text == "") { MessageBox.Show("Enter number of games to train on!"); return; }
            
            // Disable game play
            GameSetupBox.Enabled = false;
            NeuralNetBox.Enabled = false;
            TrainNetworkBox.Enabled = false;
            GameBoard.Enabled = false;
            SuggestMove.Enabled = false;
            progressBar.Visible = true;

            List<string> Parameters = new List<string> { };
            if (NetworkFilename.Text == "") { Parameters.Add("None"); }
            else Parameters.Add(NetworkFilename.Text.ToString());
            Parameters.Add(NumberOfGames.Text.ToString());
            if (RetrainNetwork.Checked) { Parameters.Add("1"); }
            else Parameters.Add("0");
            TrainNeuralNetowrk.RunWorkerAsync(Parameters);
        }

        private void TrainNeuralNetowrk_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            List<string> Parameters = e.Argument as List<string>; // Get parameters: filename,Number of games, retrain
            
            ANeuralNetwork test = new ANeuralNetwork();
            List<int> Net = new List<int> { 9, 81, 36, 9 };

            QlearningNN NN = new QlearningNN(Net);

            if(Parameters[1]=="1") NN.LoadNetwork(Parameters[0]); // loads an existing network - allows continued training of a network

            Console.Write("Training has begun...!\n\n");
            for (int i = 0; i < Convert.ToInt32(Parameters[1]); i++)
            {
                NN.InitaliseGame();
                while (true)
                {
                    NN.PlayerMove();
                    if (NN.GameOver()) { break; };
                }

                NN.UpdateNetwork(); 
                if (i % 100 == 0) {Console.Write("Number of Games completed: " + i + Environment.NewLine);}
                double Percent = (i + 1) / Convert.ToDouble(Parameters[1]) * 100;
                TrainNeuralNetowrk.ReportProgress((int)Percent);
            }
            
            if (Parameters[0] != "" && Parameters[2] == "0") { NN.SaveNetwork(Parameters[0]); }
        }

        private void TrainNeuralNetwork_ReportProgress(object sender, ProgressChangedEventArgs e)
        {
            // Update progress bar
            progressBar.Value = e.ProgressPercentage;
        }

        private void TrainNeuralNetwork_Complete(object sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show("Training Compete!");
            GameSetupBox.Enabled = true;
            TrainNetworkBox.Enabled = true;
            NeuralNetBox.Enabled = true;
            
            progressBar.Visible = false;
        }
    }
}