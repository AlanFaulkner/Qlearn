using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace @this
{
    internal class Qlearning
    {
        private MySqlConnection connection;
        private string server;
        private string database;
        private string uid;
        private string password;

        private List<string> Score = new List<string> { "S1", "S2", "S3", "S4", "S5", "S6", "S7", "S8", "S9" };
        private List<string> Possistion = new List<string> { "ul", "um", "ur", "ml", "mm", "mr", "ll", "lm", "lr" };
        public List<List<double>> GameData = new List<List<double>> { };

        private double DiscountRate = 1;
        private static Random random = new Random(2);

        private StreamWriter writetext = new StreamWriter("Analysis.txt");

        public Qlearning(string Database)
        {
            Initialize(Database);
        }

        public List<double> GetCurrentScoreFromDatabase(List<double> Gamestate)
        {
            List<double> Moves = new List<double> { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            string Table = "Qlearn";
            string cmd = "SELECT * From " + Table + " WHERE ";
            for (int i = 0; i < Gamestate.Count; i++)
            {
                if (i < Gamestate.Count - 1)
                {
                    cmd += Possistion[i] + " = " + Gamestate[i].ToString() + " AND ";
                }
                else cmd += Possistion[i] + " = " + Gamestate[i].ToString() + ";";
            }

            if (this.OpenConnection() == true)
            {
                MySqlCommand CMD = new MySqlCommand(cmd, connection);
                try
                {
                    CMD.ExecuteNonQuery();
                    MySqlDataReader Reader = CMD.ExecuteReader();

                    while (Reader.Read())
                    {
                        for (int i = 0; i < Moves.Count; i++) { Moves[i] = Reader.GetDouble(Score[i]); }
                    }
                }
                catch (MySqlException ex) { Console.Write(ex.Message); }
            }
            this.CloseConnection();

            return Moves;
        }

        public int PlayerMove(List<double> GameState, int Player, int Move)
        {
            List<double> MoveData = new List<double> { }; // Contains all data about current move
            MoveData.Add(Player);                   // Player ID
            MoveData.Add(Move);                     // Player's Move count
            MoveData.AddRange(GameState);           // Current board configuration

            List<double> StateScore = GetCurrentScoreFromDatabase(GameState);       // Get current score for possible solutions to this game state
            int NextMove = GenerateMove(StateScore, GameState);
            MoveData.AddRange(StateScore);          // Existing scores
            MoveData.Add(NextMove);                 // Move made

            // Evaluate move
            List<double> NewGameState = new List<double> { };
            NewGameState.AddRange(GameState);
            NewGameState[NextMove] = Player;        // update game board with move

            MoveData.Add(GetReward(NewGameState));  // get reward for new state
            if (MoveData.Last() == 1) { GameData[GameData.Count - 1][21] = 1; } // Update previous move data to reflect game ending in a draw
            if (MoveData.Last() == 2) { GameData[GameData.Count - 1][21] = -2; }  // penalty for loosing
            GameData.Add(MoveData);                 // Save move Data

            return NextMove;
        }

        public void FinalScore(int GameNumber)
        {
            // update score card with learned values
            for (int i = GameData.Count - 1; i >= 0; i--)
            {
                if (i < GameData.Count - 2)
                {
                    double Max = 0;
                    for (int j = 11; j < 20; j++) { if (Max < GameData[i + 2][j]) { Max = GameData[i + 2][j]; } }

                    //GameData[i][Convert.ToInt32(GameData[i][20]) + 11] = (((GameData[i][21] + DiscountRate * Max) + GameData[i][Convert.ToInt32(GameData[i][20]) + 11])/2);
                    //GameData[i][Convert.ToInt32(GameData[i][20]) + 11] += 0.2*(GameData[i][21] + DiscountRate * Max + GameData[i][Convert.ToInt32(GameData[i][20]) + 11]);
                    GameData[i][Convert.ToInt32(GameData[i][20]) + 11] += (GameData[i][21] + DiscountRate * Max);
                }
                else
                {
                    GameData[i][Convert.ToInt32(GameData[i][20]) + 11] += (GameData[i][21]);
                }
            }

            
            // apply symmetry averaging
            List<List<double>> SymmetrisedData = SymmetriseData();
            string Result = "";
            for (int i = 11; i < 20; i++) { Result += SymmetrisedData[0][i] + "\t"; }
            writetext.WriteLine(Result); // hard copy of data

            // Save data to MySQL database
            for (int i = 0; i < SymmetrisedData.Count; i++)
            {
                string cmd = "INSERT INTO qlearn VALUES(";
                for (int j = 0; j < SymmetrisedData[i].Count; j++)
                {
                    if (j < SymmetrisedData[i].Count - 1) { cmd += SymmetrisedData[i][j].ToString() + ", "; }
                    else { cmd += SymmetrisedData[i][j].ToString() + ") ON DUPLICATE KEY UPDATE "; }
                }
                for (int j = 11; j < 20; j++)
                {
                    if (j < 19) { cmd += Score[j - 11].ToString() + " = " + SymmetrisedData[i][j].ToString() + ", "; }
                    else { cmd += Score[j - 11].ToString() + " = " + SymmetrisedData[i][j].ToString() + ";"; }
                }

                if (OpenConnection() == true)
                {
                    MySqlCommand CMD = new MySqlCommand(cmd, connection);
                    try { CMD.ExecuteNonQuery(); }
                    catch (MySqlException ex) { Console.Write(ex.Message); }
                }
                CloseConnection();
            }

            //GameData.Clear();
        }

        public List<List<double>> SymmetriseData()
        {
            // the resultant symmetrical data
            List<List<double>> SymetricallyUnique = new List<List<double>> { };

            for (int i = 0; i < GameData.Count(); i++)
            {
                List<double> GameState = new List<double> { }; // Games state information
                List<double> ScoreData = new List<double> { }; // Score information
                // get data.
                for (int j = 2; j < 20; j++)
                {
                    if (j < 11) { GameState.Add(GameData[i][j]); }
                    else ScoreData.Add(GameData[i][j]);
                }

                // Get all possible symmetric configurations
                List<List<double>> SymmertricCongfigurations = new List<List<double>> { };
                List<double> FirstRow = new List<double> { };
                for (int j = 0; j < 20; j++) { FirstRow.Add(GameData[i][j]); }
                SymmertricCongfigurations.Add(FirstRow);

                // Rotate 90 degrees clockwise
                List<double> Row1 = new List<double> { };
                List<double> Row1A = Rotate(GameState);
                List<double> Row1B = Rotate(ScoreData);
                Row1.Add(GameData[i][0]);
                Row1.Add(GameData[i][1]);
                Row1.AddRange(Row1A);
                Row1.AddRange(Row1B);
                SymmertricCongfigurations.Add(Row1);

                // Rotate 180 degrees clockwise
                List<double> Row2 = new List<double> { };
                List<double> Row2A = Rotate(Rotate(GameState));
                List<double> Row2B = Rotate(Rotate(ScoreData));
                Row2.Add(GameData[i][0]);
                Row2.Add(GameData[i][1]);
                Row2.AddRange(Row2A);
                Row2.AddRange(Row2B);
                SymmertricCongfigurations.Add(Row2);

                // Rotate 270 degrees clockwise
                List<double> Row3 = new List<double> { };
                List<double> Row3A = Rotate(Rotate(Rotate(GameState)));
                List<double> Row3B = Rotate(Rotate(Rotate(ScoreData)));
                Row3.Add(GameData[i][0]);
                Row3.Add(GameData[i][1]);
                Row3.AddRange(Row3A);
                Row3.AddRange(Row3B);
                SymmertricCongfigurations.Add(Row3);

                // Mirror Horizontally
                List<double> Row4 = new List<double> { };
                List<double> Row4A = HorizantalMirror(GameState);
                List<double> Row4B = HorizantalMirror(ScoreData);
                Row4.Add(GameData[i][0]);
                Row4.Add(GameData[i][1]);
                Row4.AddRange(Row4A);
                Row4.AddRange(Row4B);
                SymmertricCongfigurations.Add(Row4);

                // Mirror Vertically
                List<double> Row5 = new List<double> { };
                List<double> Row5A = VerticalMirror(GameState);
                List<double> Row5B = VerticalMirror(ScoreData);
                Row5.Add(GameData[i][0]);
                Row5.Add(GameData[i][1]);
                Row5.AddRange(Row5A);
                Row5.AddRange(Row5B);
                SymmertricCongfigurations.Add(Row5);

                // Mirror left diagonal
                List<double> Row6 = new List<double> { };
                List<double> Row6A = Rotate(HorizantalMirror(GameState));
                List<double> Row6B = Rotate(HorizantalMirror(ScoreData));
                Row6.Add(GameData[i][0]);
                Row6.Add(GameData[i][1]);
                Row6.AddRange(Row6A);
                Row6.AddRange(Row6B);
                SymmertricCongfigurations.Add(Row6);

                // Mirror Right diagonal
                List<double> Row7 = new List<double> { };
                List<double> Row7A = HorizantalMirror(Rotate(GameState));
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
                    for (int b = 11; b < 20; b++)
                    {
                        if (Data[i][b] != 0 && Element[b] != 0) { Data[i][b] = (Data[i][b] + Element[b]) / 2; }
                        else Data[i][b] += Element[b];
                    }
                    return true;
                }
            }
            return false;
        }

        public int BestMove()
        {
            // returns current best first position.
            List<double> Max = new List<double> { 0, 0 };
            for (int i = 11; i < 20; i++) { if (Max[1] < GameData[0][i]) { Max[0] = i - 11; Max[1] = GameData[0][i]; } }

            return Convert.ToInt32(Max[0]);
        }

        public void DecreaseDiscount()
        {
            if (this.DiscountRate > 0.01) { this.DiscountRate -= 0.01; }
        }

        private double GetReward(List<double> GameState)
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
                if (Horizantal[i] == -3 || Vertical[i] == -3) { return -2; }
                else if (Horizantal[i] == 3 || Vertical[i] == 3) { return 2; }
            }
            if (D1 == -3 || D2 == -3) { return -2; }
            else if (D1 == 3 || D2 == 3) { return 2; }
            if (sum == 0) { return 1; }

            return 0;
        }

        private static int GenerateMove(List<double> PositionScore, List<double> GameState)
        {
            //double I = 0; // number of possible moves
            //double J = 0; // number of moves accounted for

            //for (int i = 0; i < GameState.Count; i++)
            //{
            //    if (GameState[i] == 0) { I++; }
            //    if (PositionScore[i] == 0 && GameState[i] == 0) { J++; }
            //}

            //List<double> Probibility = new List<double> { };

            //for (int i = 0; i < PositionScore.Count; i++)
            //{
            //    if ( PositionScore[i] != 0) { Probibility.Add(PositionScore[i]); }
            //    else if (PositionScore[i] == 0 && GameState[i] == 0) {
            //        if (I ==1 || J==1){ Probibility.Add(1); }
            //        else Probibility.Add((1 / I) + (1 / J) * (PositionScore[i] - (1 / I)));
            //    }
            //    else Probibility.Add(0);
            //}

            //// renormalise

            //double Sum = Probibility.Sum();
            //for (int i = 0; i < Probibility.Count; i++) { Probibility[i] /= Sum; }

            //double cumulatedProbability = random.NextDouble();

            //for (int i = 0; i < Probibility.Count; i++)
            //{
            //    if ((cumulatedProbability -= Probibility[i]) <= 0)
            //        return i;
            //}

            //throw new InvalidOperationException();

            // kinda of crude as the 0.5 is arbitrary and makes not reference at all to current probabilities.
            double A = random.NextDouble();
            if (A < 0.2 && GameState[PositionScore.IndexOf(PositionScore.Max())] == 0) { return PositionScore.IndexOf(PositionScore.Max()); }
            else
            {
                while (true)
                {
                    int Move = random.Next(0, PositionScore.Count);
                    if (GameState[Move] == 0) { return Move; }
                }
            }
        }

        private void Initialize(string Database)
        {
            server = "localhost";
            database = Database;
            uid = "root";
            password = "password";
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

            connection = new MySqlConnection(connectionString);
        }

        // Open connection to database
        private bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                //The two most common error numbers when connecting are as follows:
                //0: Cannot connect to server.
                //1045: Invalid user name and/or password.
                switch (ex.Number)
                {
                    case 0:
                        Console.Write("Cannot connect to server.  Contact administrator");
                        break;

                    case 1045:
                        Console.Write("Invalid username/password, please try again");
                        break;
                }
                return false;
            }
        }

        // Close connection
        private bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                Console.Write(ex.Message);
                return false;
            }
        }

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