using MathNet.Numerics.Distributions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace @this
{
    internal class Program
    {
        private static Random random = new Random(1);

        public static int GeneratedWeighted_RND(List<double> Items)
        {
            List<double> Probibility = new List<double> { };
            for (int i = 0; i < Items.Count; i++)
            {
                Normal Distrubution = new Normal(0, 0.7);
                Probibility.Add(Distrubution.CumulativeDistribution(Items[i]));
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

        public static void PrintList(List<List<double>> List)
        {
            for (int i = 0; i < List.Count; i++)
            {
                for (int j = 0; j < List[i].Count; j++)
                {
                    Console.Write(Math.Round(List[i][j], 2).ToString().PadLeft(3));
                    if (j == 1 || j == 10 || j == 19) { Console.Write(" |"); }
                }
                Console.Write(Environment.NewLine);
            }
            Console.Write(Environment.NewLine);
        }

        public static void Main(string[] args)
        {
            List<double> state = new List<double> { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            Qlearning Qlearn = new Qlearning("tictactoe");
            Game TicTacToe = new Game();

            List<double> t1 = new List<double> { };
            List<double> t2 = new List<double> { };

            for (int i = 1; i <= 1; i++)
            {
                TicTacToe.GameState.Clear();
                TicTacToe.GameState.AddRange(state);
                TicTacToe.Turn = 1;
                int GameMove = 0;
                while (true)
                {
                    //player one move
                    int Move = Qlearn.PlayerMove(TicTacToe.GameState, TicTacToe.Turn, GameMove);
                    TicTacToe.GameState[Move] = TicTacToe.Turn;
                    if (TicTacToe.End_Game() != -0.1) { break; }

                    TicTacToe.Turn = -1;

                    // player 2
                    Move = Qlearn.PlayerMove(TicTacToe.GameState, TicTacToe.Turn, GameMove);
                    TicTacToe.GameState[Move] = TicTacToe.Turn;
                    if (TicTacToe.End_Game() != -0.1) { break; }
                    TicTacToe.Turn = 1;

                    GameMove++;
                }

                Qlearn.FinalScore(i);
                PrintList(Qlearn.GameData);
                
                for (int s = 2; s < 11; s++) { t1.Add(Qlearn.GameData[2][s]); }
                for (int s = 11; s < 20; s++) { t2.Add(Qlearn.GameData[2][s]); }

                int BestMove = Qlearn.BestMove();
                if (i % 100 == 0) { Console.Write("Games Complete: " + i + "\t Best Move: " + BestMove + Environment.NewLine); Qlearn.DecreaseDiscount(); }
                Qlearn.GameData.Clear();
            }

            
            List<List<double>> New = Qlearn.SymetriseResults(t1);
            
            PrintList(New);
            Console.WriteLine();
            List<List<double>> new2 = Qlearn.SymetriseResults(t2);
            PrintList(new2);

            //Qlearn.SymMove_Set(state);
        }
    }
}