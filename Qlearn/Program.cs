using Qlearn;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace @this
{
    internal class Program
    {
        public static List<double> GameState = new List<double> { 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        public static void Main(string[] args)
        {
            //Qlearning Qlearn = new Qlearning("tictactoe");
            //Game TicTacToe = new Game();

            //for (int i = 1; i <= 1000; i++)
            //{
            //    // Re-initialize game board
            //    for (int j = 0; j < TicTacToe.GameState.Count; j++) { TicTacToe.GameState[j] = 0; }
            //    TicTacToe.Turn = 1;
            //    int GameMove = 0;
            //    while (true)
            //    {
            //        //player one move
            //        int Move = Qlearn.PlayerMove(TicTacToe.GameState, TicTacToe.Turn, GameMove);
            //        TicTacToe.GameState[Move] = TicTacToe.Turn;
            //        if (TicTacToe.End_Game() != -0.1) { break; }

            //        TicTacToe.Turn = -1;

            //        // player 2
            //        Move = Qlearn.PlayerMove(TicTacToe.GameState, TicTacToe.Turn, GameMove);
            //        TicTacToe.GameState[Move] = TicTacToe.Turn;
            //        if (TicTacToe.End_Game() != -0.1) { break; }
            //        TicTacToe.Turn = 1;

            //        GameMove++;
            //    }

            //    Qlearn.FinalScore(i);
            //    //PrintList(Qlearn.GameData);

            //    int BestMove = Qlearn.BestMove();
            //    if (i % 100 == 0) { Console.Write("Games Complete: " + i + "\t Best Move: " + BestMove + Environment.NewLine); Qlearn.DecreaseDiscount(); }
            //    Qlearn.GameData.Clear();
            //}

            //ANeuralNetwork test = new ANeuralNetwork();
            //List<int> Net = new List<int> { 9, 81, 36, 9 };

            //QlearningNN NN = new QlearningNN(Net);
            //Console.Write("Training has begun...!\n\n");
            //for (int i = 0; i < 100000; i++)
            //{
            //    NN.InitaliseGame();
            //    while (true)
            //    {
            //        NN.PlayerMove();
            //        if (NN.GameOver()) { break; };
            //    }
            //    //NN.printgamedata();
            //    NN.UpdateNetwork();
            //    if (i % 100 == 0) { Console.Write("Number of Games completed: " + i + Environment.NewLine); }
            //}

            //NN.InitaliseGame();
            //while (true)
            //{
            //    NN.BestMove();
            //    if (NN.GameOver()) { break; }
            //}

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new PlayGame());
        }
    }
}