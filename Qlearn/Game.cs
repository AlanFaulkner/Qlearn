using System;
using System.Collections.Generic;

namespace @this
{
    internal class Game
    {
        private Random random = new Random(1);

        public List<double> GameState = new List<double> { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        public int Turn = 1;

        public void BasicAIMove()
        {
            List<double> Probibility = new List<double> { };
            double NumberOfFreePlaces = 0;
            for (int i = 0; i < GameState.Count; i++) { if (GameState[i] == 0) { NumberOfFreePlaces++; } }
            for (int i = 0; i < GameState.Count; i++)
            {
                if (GameState[i] == 0) { Probibility.Add(1 / NumberOfFreePlaces); }
                else Probibility.Add(0);
            }

            double cumulatedProbability = random.NextDouble();

            for (int i = 0; i < Probibility.Count; i++)
            {
                if ((cumulatedProbability -= Probibility[i]) <= 0)
                {
                    GameState[i] = Turn;
                    return;
                }
            }

            throw new InvalidOperationException();

            // Makes a valid random move for player 2
            //while (true)
            //{
            //    int Move = random.Next(0, 9);
            //    if (GameState[Move] == 0)
            //    {
            //        GameState[Move] = Turn;
            //        break;
            //    }
            //}
        }

        public void MediumAIMove()
        {
            // check to see if its possible to win
            if (Horizantials(true) == true) { return; }
            else if (Verticals(true) == true) { return; }
            else if (Diagnols(true) == true) { return; }

            // block player moves
            else if (Horizantials(false) == true) { return; }
            else if (Verticals(false) == true) { return; }
            else if (Diagnols(false) == true) { return; }
            else BasicAIMove();
        }

        public void HardAIMove()
        {
            if (GameState[4] == 0)
            {
                GameState[4] = Turn;
            }
            else MediumAIMove();
        }

        public double End_Game()
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
                if (Horizantal[i] == -3 || Vertical[i] == -3) { return -1; }
                else if (Horizantal[i] == 3 || Vertical[i] == 3) { return 1; }
            }
            if (D1 == -3 || D2 == -3) { return -1; }
            else if (D1 == 3 || D2 == 3) { return 1; }
            if (sum == 0) { return 0.5; }

            return -0.1;
        }

        private bool Horizantials(bool Action)
        // if action true win if false block
        {
            int[] horizantials = { 0, 0, 0 };
            horizantials[0] = (int)GameState[0] + (int)GameState[1] + (int)GameState[2];
            horizantials[1] = (int)GameState[3] + (int)GameState[4] + (int)GameState[5];
            horizantials[2] = (int)GameState[6] + (int)GameState[7] + (int)GameState[8];

            if (Action == true)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (horizantials[j] == 2 * Turn)
                    {
                        for (int i = 0; i < 3; i++) { if (GameState[3 * j + i] == 0) { GameState[3 * j + i] = Turn; return true; } }
                    }
                }
            }
            else if (Action == false)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (horizantials[j] == -2 * Turn)
                    {
                        for (int i = 0; i < 3; i++) { if (GameState[3 * j + i] == 0) { GameState[3 * j + i] = Turn; return true; } }
                    }
                }
            }

            return false;
        }

        private bool Verticals(bool Action)
        {
            int[] verticals = { 0, 0, 0 };
            verticals[0] = (int)GameState[0] + (int)GameState[3] + (int)GameState[6];
            verticals[1] = (int)GameState[1] + (int)GameState[4] + (int)GameState[7];
            verticals[2] = (int)GameState[2] + (int)GameState[5] + (int)GameState[8];

            if (Action == true)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (verticals[i] == 2 * Turn)
                    {
                        for (int j = 0; j < 3; j++) { if (GameState[3 * j + i] == 0) { GameState[3 * j + i] = Turn; return true; } }
                    }
                }
            }
            else if (Action == false)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (verticals[i] == -2 * Turn)
                    {
                        for (int j = 0; j < 3; j++) { if (GameState[3 * j + i] == 0) { GameState[3 * j + i] = Turn; return true; } }
                    }
                }
            }

            return false;
        }

        private bool Diagnols(bool Action)
        {
            int[] diagnols = { 0, 0 };
            diagnols[0] = (int)GameState[0] + (int)GameState[4] + (int)GameState[8];
            diagnols[1] = (int)GameState[2] + (int)GameState[4] + (int)GameState[6];

            if (Action == true)
            {
                if (diagnols[0] == 2 * Turn)
                {
                    for (int i = 0; i < 9; i = i + 4) { if (GameState[i] == 0) { GameState[i] = Turn; return true; } }
                }
                else if (diagnols[1] == 2 * Turn)
                {
                    for (int i = 2; i < 7; i = i + 2) { if (GameState[i] == 0) { GameState[i] = Turn; return true; } }
                }
            }
            else if (Action == false)
            {
                if (diagnols[0] == -2 * Turn)
                {
                    for (int i = 0; i < 9; i = i + 4) { if (GameState[i] == 0) { GameState[i] = Turn; return true; } }
                }
                else if (diagnols[1] == -2 * Turn)
                {
                    for (int i = 2; i < 7; i = i + 2) { if (GameState[i] == 0) { GameState[i] = Turn; return true; } }
                }
            }

            return false;
        }
    }
}