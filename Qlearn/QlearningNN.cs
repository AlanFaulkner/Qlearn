using System;
using System.Collections.Generic;
using System.Linq;

namespace @this
{
    internal class QlearningNN
    {
        private List<List<double>> GameData = new List<List<double>> { };       // Q-learning update matrix
        private List<double> GameState = new List<double> { 0, 0, 0, 0, 0, 0, 0, 0, 0 }; // Gameboard

        private Random random = new Random();
        private double DiscountRate = 1;
        private int Player = 1;
        private int Move = 0;
        private ANeuralNetwork TicTacToe = new ANeuralNetwork();            // Invoke neural network

        public QlearningNN(List<int> NetworkDescription)
        {
            TicTacToe.Create_Network(NetworkDescription);
            TicTacToe.SetActivationFunction = "Sigmoid";
            TicTacToe.UseActivationFunctionForOutputLayer = true;
        }  // Initialize network

        public int CurrentPlayer
        {
            get { return Player; }
            set { Player = value; }
        }

        public List<double> GameBoard
        {
            get { return GameState; }
            set { GameState = value; }
        }

        public double SetDiscountRate
        {
            get { return DiscountRate; }
            set { DiscountRate = value; }
        }

        public void IncreaseMove()
        {
            Move++;
        }

        public void UpdateGameData(List<double> Row) { GameData.Add(Row); }

        // ####################
        // ## Game functions ##
        // ####################

        public void InitaliseGame()
        {
            for (int i = 0; i < GameState.Count; i++) { GameState[i] = 0; }
            Move = 0; Player = 1;
        }

        public bool GameOver()
        {
            if (GetReward(GameState,Move,true) != 0) { return true; }
            else return false;
        }

        // ###############
        // ## Make Move ##
        // ###############

        public List<double> GetSuggestedMove()
        {
            return TicTacToe.Calculate_Netowrk_Output(GameState);
        }

        private int MakeMove()
        {
            //// Makes a valid move. current move selection is rather crude!
            //List<double> MoveScore = TicTacToe.Calculate_Netowrk_Output(GameState);
            //for (int i = 0; i < MoveScore.Count; i++) { if (GameState[i] != 0) { MoveScore[i] = 0; } } // Remove impossible moves

            //double A = random.NextDouble();

            //if (A < 0.4) { return MoveScore.IndexOf(MoveScore.Max()); }
            //while (true)
            //{
            //    int Move = random.Next(0, 9);
            //    if (GameState[Move] == 0) { return Move; }
            //}

            // Improved Move selection.
            List<double> MoveProbability = TicTacToe.Calculate_Netowrk_Output(GameState);
            for (int i = 0; i < MoveProbability.Count; i++) { if (GameState[i] != 0) { MoveProbability[i] = 0; } } // Remove impossible moves

            // Convert output score to a probability
            double Sum = MoveProbability.Sum();
            for(int i = 0; i < MoveProbability.Count; i++) { MoveProbability[i] /= Sum; }

            // Make move
            double cumulatedProbability = random.NextDouble();

            for (int i = 0; i < MoveProbability.Count; i++)
            {
                if ((cumulatedProbability -= MoveProbability[i]) <= 0)
                    return i;
            }

            throw new InvalidOperationException();
        }

        public void PlayerMove()
        {
            List<double> MoveData = new List<double> { }; // Contains all data about current move
            MoveData.Add(Player);                   // Player ID
            MoveData.Add(Move);                     // Player's Move count
            MoveData.AddRange(GameState);           // Current board configuration

            List<double> StateScore = TicTacToe.Calculate_Netowrk_Output(GameState);       // Get current score for possible solutions to this game state
            //for (int i = 0; i < StateScore.Count; i++) { if (GameState[i] != 0) { StateScore[i] = 1e-2; } } // remove impossible moves
            int NextMove = MakeMove();
            MoveData.AddRange(StateScore);                                          // Existing scores
            MoveData.Add(NextMove);                                                 // Move made

            // Evaluate move
            List<double> NewGameState = new List<double> { };
            NewGameState.AddRange(GameState);
            NewGameState[NextMove] = Player;                                        // update game board with move

            MoveData.Add(GetReward(NewGameState,NextMove,false));                                  // get reward for new state
            if (MoveData.Last() == 1) { GameData[GameData.Count - 1][21] = 1; }     // Update previous move data to reflect game ending in a draw
            if (MoveData.Last() == 2) { GameData[GameData.Count - 1][21] = -2; }    // penalty for loosing
            GameData.Add(MoveData);                                                 // Save move Data

            GameState[NextMove] = Player;                                           // Update game state with move.
            if (Player == 1) { Player = -1; }                                       // Change player
            else { Player = 1; Move++; }
        }

        public void printgamedata(String filename)
        {
            for (int i = 0; i < GameData.Count; i++)
            {
                for (int j = 0; j < GameData[i].Count; j++)
                {
                    string line = Convert.ToString(Math.Round(GameData[i][j], 1));
                    if (j > 10 && j < 20) Console.Write(line.PadLeft(4));
                    else Console.Write(line.PadLeft(3));
                    //Console.Write(Math.Round(GameData[i][j], 2) + " ");
                }
                Console.WriteLine();
            }

            using (System.IO.StreamWriter Out = new System.IO.StreamWriter("../" + filename, false))
            {
                for (int i = 0; i < GameData.Count; i++)
                {
                    for (int j = 0; j < GameData[i].Count; j++)
                    {
                        Out.Write(Math.Round(GameData[i][j], 2) + " ");
                    }
                    Out.WriteLine();
                }
                Out.Flush();
                Out.Close();
            }
        }

        public int BestMove()
        {
            List<double> MoveList = TicTacToe.Calculate_Netowrk_Output(GameState);
            for (int i = 0; i < MoveList.Count; i++) { if (GameState[i] != 0) { MoveList[i] = 0; } }
            GameState[MoveList.IndexOf(MoveList.Max())] = Player;
            if (Player == 1) { Player = -1; }                                       // Change player
            else { Player = 1; Move++; }
            return MoveList.IndexOf(MoveList.Max());
        }

        public void LoadNetwork(string Network)
        {
            TicTacToe.Load_Network(Network);
        }

        public void SaveNetwork(string Filename)
        {
            TicTacToe.Save_Network(Filename);
        }

        public void testinitstate()
        {
            InitaliseGame();
            List<double> Results = TicTacToe.Calculate_Netowrk_Output(GameState);
            for (int i = 0; i < Results.Count; i++) { Console.Write(Math.Round(Results[i], 2) + " "); }
            Console.WriteLine();
        }

        public void UpdateNetwork()
        {
            if (GameData[GameData.Count-1][GameData[0].Count-1] == 2) { GameData[GameData.Count - 2][GameData[0].Count-1] = -2; }
            else if (GameData[GameData.Count-1][GameData[0].Count-1] == 1) { GameData[GameData.Count - 2][GameData[0].Count-1] = 1; }
            

            // update score card with learned values
            for (int i = GameData.Count - 1; i >= 0; i--)
            {
                for (int j = 11; j < 20; j++) { GameData[i][j] = -1 * (Math.Log(1/GameData[i][j]-1)); } // reverse sigmoid function
                if (i < GameData.Count - 2)
                {
                    double Max = 0;
                    for (int j = 11; j < 20; j++) { if (Max < GameData[i + 2][j]) { Max = GameData[i + 2][j]; } }
                    GameData[i][Convert.ToInt32(GameData[i][20]) + 11] += (GameData[i][21] + DiscountRate * Max);
                }
                else
                {
                    GameData[i][Convert.ToInt32(GameData[i][20]) + 11] += (GameData[i][21]);
                }
                for (int j = 11; j < 20; j++) {
                    GameData[i][j] *= 0.99; 
                    GameData[i][j] = (1 / (1 + Math.Exp(-GameData[i][j])));
                    if (GameData[i][j] < 1e-10) { GameData[i][j] = 1e-3; } // hard limit on smallest value to prevent complete decay of network outputs
                } // apply sigmoid function. have included a decay rate of 0.95 this should mean that none selected states reduced over time
            }

            //printgamedata("Update.txt");
            //Console.WriteLine();
            GameData = SymmetriseData();
            //printgamedata("sym.txt");

            // build input and target data sets
            List<List<double>> InputData = new List<List<double>> { };
            List<List<double>> TargetData = new List<List<double>> { };

            for (int i = 0; i < GameData.Count; i++)
            {
                List<double> InputRow = new List<double> { };
                List<double> InputRowInverse = new List<double> { }; // inverted copy of game board i.e. X goes first is same as O going first
                for (int j = 2; j < 11; j++) { InputRow.Add(GameData[i][j]); InputRowInverse.Add(-1*GameData[i][j]); }
                InputData.Add(InputRow);
                InputData.Add(InputRowInverse);

                List<double> TargetRow = new List<double> { };
                for (int j = 11; j < 20; j++) { TargetRow.Add(GameData[i][j]); }
                TargetData.Add(TargetRow);
                TargetData.Add(TargetRow); // score matches moves inverse
            }

            TicTacToe.Back_prop_Stochastic(InputData, TargetData, 0.1, 0.7, 1e-10, 1);
            //TicTacToe.Back_prop_batch(InputData, TargetData, 0.1, 0.7, 1e-10, 1);
            GameData.Clear();
        }

        public double GetReward(List<double> GameState, int Move, bool EndGame)
        {
            // returns 0 if game is not over, 2 for a win 1.5 for a draw -3 for loss

            // No Moves
            int sum = 0;

            for (int i = 0; i < GameState.Count; i++) { if (GameState[i] == 0) { sum++; } }

            // Horizontals
            List<int> Horizantal = new List<int> { 0, 0, 0 };
            Horizantal[0] = (int)GameState[0] + (int)GameState[1] + (int)GameState[2];
            Horizantal[1] = (int)GameState[3] + (int)GameState[4] + (int)GameState[5];
            Horizantal[2] = (int)GameState[6] + (int)GameState[7] + (int)GameState[8];

            // Verticals
            List<int> Vertical = new List<int> { 0, 0, 0 };
            Vertical[0] = (int)GameState[0] + (int)GameState[3] + (int)GameState[6];
            Vertical[1] = (int)GameState[1] + (int)GameState[4] + (int)GameState[7];
            Vertical[2] = (int)GameState[2] + (int)GameState[5] + (int)GameState[8];

            // Diagonals
            int D1 = (int)GameState[0] + (int)GameState[4] + (int)GameState[8];
            int D2 = (int)GameState[2] + (int)GameState[4] + (int)GameState[6];

            for (int i = 0; i < 3; i++)
            {
                if (Math.Abs(Horizantal[i]) == 3 || Math.Abs(Vertical[i]) == 3) { return 2; }
                
            }
            if (Math.Abs(D1) == 3 || Math.Abs(D2) == 3) { return 2; }
            
            if (sum == 0) { return 1; }

            // bonus reward for blocking
            if (EndGame == false)
            {
                if (_Blocking(Move)) { return 0.1; }
            }

            return 0;
        }

        private bool _Blocking(int Move)
        {
            List<double> LocalGamesState = new List<double> { };
            LocalGamesState.AddRange(GameState);
            LocalGamesState[Move] = 0;

            List<List<int>> Results = new List<List<int>> { };
            for (int i=0; i < 8; i++) { List<int> Row = new List<int> { 0, 0 };Results.Add(Row); }
            
            // Horizontals
            Results[0][0] = (int)GameState[0] + (int)GameState[1] + (int)GameState[2];
            Results[0][1] = (int)LocalGamesState[0] + (int)LocalGamesState[1] + (int)LocalGamesState[2];
            Results[1][0] = (int)GameState[3] + (int)GameState[4] + (int)GameState[5];
            Results[1][1] = (int)LocalGamesState[3] + (int)LocalGamesState[4] + (int)LocalGamesState[5];
            Results[2][0] = (int)GameState[6] + (int)GameState[7] + (int)GameState[8];
            Results[2][1] = (int)LocalGamesState[6] + (int)LocalGamesState[7] + (int)LocalGamesState[8];
            
            // Verticals
            Results[3][0] = (int)GameState[0] + (int)GameState[3] + (int)GameState[6];
            Results[3][1] = (int)LocalGamesState[0] + (int)LocalGamesState[3] + (int)LocalGamesState[6];
            Results[4][0] = (int)GameState[1] + (int)GameState[4] + (int)GameState[7];
            Results[4][1] = (int)LocalGamesState[1] + (int)LocalGamesState[4] + (int)LocalGamesState[7];
            Results[5][0] = (int)GameState[2] + (int)GameState[5] + (int)GameState[8];
            Results[5][1] = (int)LocalGamesState[2] + (int)LocalGamesState[5] + (int)LocalGamesState[8];

            // Diagonals
            Results[6][0] = (int)GameState[0] + (int)GameState[4] + (int)GameState[8];
            Results[6][1] = (int)LocalGamesState[0] + (int)LocalGamesState[4] + (int)LocalGamesState[8];
            Results[7][0] = (int)GameState[2] + (int)GameState[4] + (int)GameState[6];
            Results[7][1] = (int)LocalGamesState[2] + (int)LocalGamesState[4] + (int)LocalGamesState[6];

            for (int i=0;i<8; i++)
            {
                if (Math.Abs(Results[i][0]) == 1 && Math.Abs(Results[i][1]) == 2) { return true; }
            }
            return false;
        }

        public List<List<double>> SymmetriseData()
        {
            // the resultant symmetrical data
            List<List<double>> SymetricallyUnique = new List<List<double>> { };

            for (int i = 0; i < GameData.Count(); i++)
            {
                List<double> BoardState = new List<double> { }; // Games state information
                List<double> ScoreData = new List<double> { }; // Score information
                // get data.
                for (int j = 2; j < 20; j++)
                {
                    if (j < 11) { BoardState.Add(GameData[i][j]); }
                    else ScoreData.Add(GameData[i][j]);
                }

                // Get all possible symmetric configurations
                List<List<double>> SymmertricCongfigurations = new List<List<double>> { };
                List<double> FirstRow = new List<double> { };
                for (int j = 0; j < 20; j++) { FirstRow.Add(GameData[i][j]); }
                SymmertricCongfigurations.Add(FirstRow);

                // Rotate 90 degrees clockwise
                List<double> Row1 = new List<double> { };
                List<double> Row1A = Rotate(BoardState);
                List<double> Row1B = Rotate(ScoreData);
                Row1.Add(GameData[i][0]);
                Row1.Add(GameData[i][1]);
                Row1.AddRange(Row1A);
                Row1.AddRange(Row1B);
                SymmertricCongfigurations.Add(Row1);

                // Rotate 180 degrees clockwise
                List<double> Row2 = new List<double> { };
                List<double> Row2A = Rotate(Rotate(BoardState));
                List<double> Row2B = Rotate(Rotate(ScoreData));
                Row2.Add(GameData[i][0]);
                Row2.Add(GameData[i][1]);
                Row2.AddRange(Row2A);
                Row2.AddRange(Row2B);
                SymmertricCongfigurations.Add(Row2);

                // Rotate 270 degrees clockwise
                List<double> Row3 = new List<double> { };
                List<double> Row3A = Rotate(Rotate(Rotate(BoardState)));
                List<double> Row3B = Rotate(Rotate(Rotate(ScoreData)));
                Row3.Add(GameData[i][0]);
                Row3.Add(GameData[i][1]);
                Row3.AddRange(Row3A);
                Row3.AddRange(Row3B);
                SymmertricCongfigurations.Add(Row3);

                // Mirror Horizontally
                List<double> Row4 = new List<double> { };
                List<double> Row4A = HorizantalMirror(BoardState);
                List<double> Row4B = HorizantalMirror(ScoreData);
                Row4.Add(GameData[i][0]);
                Row4.Add(GameData[i][1]);
                Row4.AddRange(Row4A);
                Row4.AddRange(Row4B);
                SymmertricCongfigurations.Add(Row4);

                // Mirror Vertically
                List<double> Row5 = new List<double> { };
                List<double> Row5A = VerticalMirror(BoardState);
                List<double> Row5B = VerticalMirror(ScoreData);
                Row5.Add(GameData[i][0]);
                Row5.Add(GameData[i][1]);
                Row5.AddRange(Row5A);
                Row5.AddRange(Row5B);
                SymmertricCongfigurations.Add(Row5);

                // Mirror left diagonal
                List<double> Row6 = new List<double> { };
                List<double> Row6A = Rotate(HorizantalMirror(BoardState));
                List<double> Row6B = Rotate(HorizantalMirror(ScoreData));
                Row6.Add(GameData[i][0]);
                Row6.Add(GameData[i][1]);
                Row6.AddRange(Row6A);
                Row6.AddRange(Row6B);
                SymmertricCongfigurations.Add(Row6);

                // Mirror Right diagonal
                List<double> Row7 = new List<double> { };
                List<double> Row7A = HorizantalMirror(Rotate(BoardState));
                List<double> Row7B = HorizantalMirror(Rotate(ScoreData));
                Row7.Add(GameData[i][0]);
                Row7.Add(GameData[i][1]);
                Row7.AddRange(Row7A);
                Row7.AddRange(Row7B);
                SymmertricCongfigurations.Add(Row7);

                // average score data over symmetrically equivalent positions while keeping unique states
                // this means that on symmetric transformation game state remains the same

                for (int a = 0; a < SymmertricCongfigurations.Count; a++)
                {
                    List<double> Row = new List<double> { };
                    Row.AddRange(SymmertricCongfigurations[a]); // local copy of row
                    Row.Add(1);

                    // check to see if row is in unique list if not add scores
                    if (!_IsPresent(SymetricallyUnique, Row))
                    {
                        SymetricallyUnique.Add(Row);
                    }
                }
            }

            return SymetricallyUnique;
        }

        private bool _IsPresent(List<List<double>> Data, List<double> Element)
        {
            for (int i = 0; i < Data.Count; i++) // only mataches on gamestate not score
            {
                List<double> MatchKey = new List<double> { };
                for (int j = 2; j < 11; j++) { MatchKey.Add(Data[i][j]); }

                List<double> Key = new List<double> { };
                for (int b = 2; b < 11; b++) { Key.Add(Element[b]); } // key is Game state used for matching

                if (MatchKey.SequenceEqual(Key) == true)
                {
                    Data[i][20]++;
                    for (int b = 11; b < 20; b++)
                    {
                        Data[i][b] = (Data[i][20] - 1) / Data[i][20] * Data[i][b] + Element[b] / Data[i][20];
                    }
                    return true;
                }
            }
            return false;
        }

        // Game board symmetry operations
        private List<double> Rotate(List<double> GameState)
        {
            List<double> Rotated = new List<double> { };

            List<List<double>> Board = new List<List<double>> {
                new List<double> { 0, 0, 0 },
                new List<double> { 0, 0, 0 },
                new List<double> { 0, 0, 0 },
            };

            // convert to 2d board rep
            int a = 0;
            int b = 0;
            for (int i = 0; i < GameState.Count; i++)
            {
                Board[a][b] = GameState[i];
                b++;
                if ((i + 1) % 3 == 0) { a++; b = 0; }
            }

            // convert 2d board back to 1d list
            for (int i = 0; i < Board.Count; i++)
            {
                for (int j = 0; j < Board.Count; j++)
                {
                    Rotated.Add(Board[(Board.Count - 1) - j][i]);
                }
            }
            return Rotated;
        }

        private List<double> VerticalMirror(List<double> GameState)
        {
            List<double> Rotated = new List<double> { };
            Rotated.AddRange(GameState);

            for (int i = 0; i < 3; i++)
            {
                double temp = Rotated[i];
                Rotated[i] = Rotated[i + 6];
                Rotated[i + 6] = temp;
            }

            return Rotated;
        }

        private List<double> HorizantalMirror(List<double> GameState)
        {
            List<double> Rotated = new List<double> { };
            Rotated.AddRange(GameState);

            for (int i = 0; i < 9; i += 3)
            {
                double temp = Rotated[i];
                Rotated[i] = Rotated[i + 2];
                Rotated[i + 2] = temp;
            }

            return Rotated;
        }
    }
}