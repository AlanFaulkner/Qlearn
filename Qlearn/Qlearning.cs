using MathNet.Numerics.Distributions;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
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
        private static Random random = new Random(1);

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
                    
                    GameData[i][Convert.ToInt32(GameData[i][20]) + 11] = (((GameData[i][21] + DiscountRate * Max) + GameData[i][Convert.ToInt32(GameData[i][20]) + 11]) / 2+(DiscountRate-1));
                    
                }
                else
                {
                    GameData[i][Convert.ToInt32(GameData[i][20]) + 11] = ((GameData[i][21] + GameData[i][Convert.ToInt32(GameData[i][20]) + 11]) / 2);
                    
                }
            }

            // Save data to MySQL database
            for (int i = 0; i < GameData.Count; i++)
            {
                string cmd = "INSERT INTO qlearn VALUES(";
                for (int j = 0; j < GameData[i].Count - 2; j++)
                {
                    if (j < GameData[i].Count - 3) { cmd += GameData[i][j].ToString() + ", "; }
                    else { cmd += GameData[i][j].ToString() + ") ON DUPLICATE KEY UPDATE "; }
                }
                for (int j = 11; j < 20; j++)
                {
                    if (j < 19) { cmd += Score[j - 11].ToString() + " = " + GameData[i][j].ToString() + ", "; }
                    else { cmd += Score[j - 11].ToString() + " = " + GameData[i][j].ToString() + ";"; }
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
            List<double> Probibility = new List<double> { };
            for (int i = 0; i < PositionScore.Count; i++)
            {
                Normal Distrubution = new Normal(0, 0.7);
                if (GameState[i] != 0) { Probibility.Add(0); } // eliminate impossible move
                else Probibility.Add(Distrubution.CumulativeDistribution(PositionScore[i]));
            }
            
            double Sum = Probibility.Sum();
            for (int i = 0; i < Probibility.Count(); i++) { Probibility[i] /= Sum; }

            double cumulatedProbability = random.NextDouble();

            for (int i = 0; i < Probibility.Count; i++)
            {
                if ((cumulatedProbability -= Probibility[i]) <= 0)
                    return i;
            }

            throw new InvalidOperationException();
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

        private bool _IsPresent(List<List<double>> Data, List<double> Element)
        {
            //for (int i = 0; i < Data.Count; i++)
            //{
            //    if (Data[i].SequenceEqual(Element) == true) { return true; }
            //}
            return false;
        }

        public List<List<double>> SymetriseResults(List<double> GameState)
        {
            List<List<double>> Symetry = new List<List<double>> { };
            
            List<double> Row1 = new List<double> { };
            Row1.AddRange(GameState);
            if (!_IsPresent(Symetry,Row1)) {Symetry.Add(Row1); }
            
            List<double> Row2 = Rotate(GameState);
            if (!_IsPresent(Symetry, Row2)) { Symetry.Add(Row2); }

            List<double> Row3 = Rotate(Rotate(GameState));
            if (!_IsPresent(Symetry, Row3)) { Symetry.Add(Row3); }

            List<double> Row4 = Rotate(Rotate(Rotate(GameState)));
            if (!_IsPresent(Symetry, Row4)) { Symetry.Add(Row4); }

            List<double> Row5 = HorizantalMirror(GameState);
            if (!_IsPresent(Symetry, Row5)) { Symetry.Add(Row5); }

            List<double> Row6 = VerticalMirror(GameState);
            if (!_IsPresent(Symetry, Row6)) { Symetry.Add(Row6); }

            List<double> Row7 = Rotate(HorizantalMirror(GameState));
            if (!_IsPresent(Symetry, Row7)) { Symetry.Add(Row7); }

            List<double> Row8 = HorizantalMirror(Rotate(GameState));
            if (!_IsPresent(Symetry, Row8)) { Symetry.Add(Row8); }
            
            return Symetry;
        }

        public void SymMove_Set(List<double> ResultsRow)
        {
            // Get GameState from input
            List<double> GameState = new List<double> { };
            for (int i = 2; i < 11; i++) { GameState.Add(ResultsRow[i]); }

            // Get score from input
            List<double> Score = new List<double> { };
            for (int i = 11; i < 20; i++) { Score.Add(ResultsRow[i]); }

            // Test to ascertain if symmetry boards can be found from database

            List<List<double>> PossibileSymmetries = SymetriseResults(GameState);

            for (int a = 0; a < PossibileSymmetries.Count; a++)
            {
                string cmd = "SELECT COUNT(*) FROM qlearn WHERE (";
                for (int b = 0; b < GameState.Count-1; b++) { cmd += Possistion[b]+" = " + PossibileSymmetries[a][b] + " and "; }
                cmd += Possistion[GameState.Count-1] + " = " + GameState[GameState.Count-1] + ");";
                if (OpenConnection() == true)
                {
                    MySqlCommand CMD = new MySqlCommand(cmd, connection);
                    try
                    {
                        int count = Convert.ToInt32(CMD.ExecuteScalar());
                        if (count == 1)
                        {
                            string update = "UPDATE TABLE qlearn SET ";
                            for (int b = 11; b < 20; b++)
                            {
                                if (b < 19) { cmd += Score[b - 11] + " = " + ResultsRow[b] + " and "; }
                                else cmd += Score[b - 11] + " = " + ResultsRow[b] + ");";
                            }
                        }
                    }
                    catch (MySqlException ex) { Console.Write(ex.Message); }
                }
                CloseConnection();
            }

            // if not in database

            string sqlcmd = "INSERT INTO test VALUES(";
            for (int j = 2; j < 11; j++)
            {
                if (j < 10) { sqlcmd += ResultsRow[j].ToString() + ", "; }
                else { sqlcmd += ResultsRow[j].ToString() + ");"; }
            }

            if (OpenConnection() == true)
            {
                MySqlCommand SQLCMD = new MySqlCommand(sqlcmd, connection);
                try { SQLCMD.ExecuteNonQuery(); }
                catch (MySqlException ex) { Console.Write(ex.Message); }
            }
            CloseConnection();

            return;
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

            for (int i = 0; i < 9; i+=3)
            {
                double temp = Rotated[i];
                Rotated[i] = Rotated[i + 2];
                Rotated[i + 2] = temp;
            }

            return Rotated;
        }
    }
}