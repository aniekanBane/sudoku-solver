using System;
using System.Linq;

namespace SudokuSolver
{
    class Program
    {
        /// <summary>Finds next row, column on the puzzle that is empty --> repr -1</summary>
        /// <param name="puzzle">Jagged array where each array is a row in the puzzle</param>
        /// <returns>tuple: (row, col) or (null, null) if there's none</returns>
        public static (int?, int?) Find(int[][] puzzle)
        {
            for (int r = 0; r < 9; r++)
            {
                for (int c = 0; c < 9; c++)
                {
                    if (puzzle[r][c] == -1)
                        return (r, c); // return position of empty space
                }
            }
            // no spaces in puzzle are empty
            return (null, null);
        }

        /// <summary>Figures out if the guess at the row/col is valid</summary>
        /// <param name="puzzle"></param>
        /// <param name="guess">guess in the range 1-9</param>
        /// <param name="row">x index</param>
        /// <param name="col">y index</param>
        /// <returns>True is guess is valid and vice versa</returns>
        public static bool IsValid(int[][] puzzle, int guess, int row, int col)
        {
            int[] row_vals = puzzle[row];
            if (row_vals.Contains(guess))
                return false;

            var col_vals = (from num in new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 }
                            select puzzle[num][col]).ToArray();
            if (col_vals.Contains(guess))
                return false;

            /*
             * the square
             * get where ther 3x3 matrix starts and then iterate
             * over the 3 values in the row/column
             */
            int rs = row / 3 * 3;
            int cs = col / 3 * 3;

            for (int r = rs; r < rs + 3; r++)
            {
                for (int c = cs; c < cs + 3; c++)
                {
                    if (puzzle[r][c] == guess)
                        return false;
                }
            }
            // guess not present in puzzle
            return true;
        }

        /// <summary>
        /// Solve sudoku using backtracking technique,
        /// mutates puzzle to be the solution (if solution exists)
        /// </summary>
        /// <param name="puzzle"></param>
        /// <returns>True if puzzle has been solved and vice versa</returns>
        public static bool Solve(ref int[][] puzzle)
        {
            // choose somewhere to make a guess
            var (row, col) = Find(puzzle);

            // nowhere left? we're done
            if (row == null)
                return true;

            // not finished? make guess between 1-9
            for (int guess = 1; guess < 10; guess++)
            {
                // check if guess is valid
                if (IsValid(puzzle, guess, row.Value, col.Value))
                {
                    puzzle[row.Value][col.Value] = guess;

                    if (Solve(ref puzzle))
                        return true;
                }
                // if not valid or puzzle not solved
                puzzle[row.Value][col.Value] = -1; // reset guess
            }
            //// unsolvable
            return false;
        }

        static void Main(string[] args)
        {
            //Example board
            int[][] board = new int[9][];

            board[0] = new int[9] { 3, 9, -1, -1, 5, -1, -1, -1, -1 };
            board[1] = new int[9] { -1, -1, -1, 2, -1, -1, -1, -1, 5 };
            board[2] = new int[9] { -1, -1, -1, 7, 1, 9, -1, 8, -1 };
            board[3] = new int[9] { -1, 5, -1, -1, 6, 8, -1, -1, -1 };
            board[4] = new int[9] { 2, -1, 6, -1, -1, 3, -1, -1, -1 };
            board[5] = new int[9] { -1, -1, -1, -1, -1, -1, -1, -1, 4 };
            board[6] = new int[9] { 5, -1, -1, -1, -1, -1, -1, -1, -1 };
            board[7] = new int[9] { 6, 7, -1, -1, -1, 5, -1, 4, -1 };
            board[8] = new int[9] { -1, -1, 9, -1, -1, -1, 2, -1, -1 };

            Console.WriteLine(Solve(ref board));
            Console.WriteLine();

            for (int i = 0; i < board.Length; i++)
            {
                for (int j = 0; j < board[i].Length; j++)
                    Console.Write(board[i][j] + " ");
                Console.WriteLine();
            }
        }
    }
}

