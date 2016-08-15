using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    class Program
    {
        static void Main(string[] args)
        {
            bool isContinue = false;

            do
            {
                int[][] puzzleInput = new int[9][];
                for (int i = 0; i < 9; i++)
                {
                    Console.Write("Enter row {0}: ", (i + 1));
                    puzzleInput[i] = new int[9];
                    int input = Int32.Parse(Console.ReadLine());
                    for (int j = 8; j >= 0; j--)
                    {
                        int digit = input % 10;
                        input = input / 10;
                        puzzleInput[i][j] = digit;
                    }
                }
                Console.WriteLine();
                SudokuPuzzle game = new SudokuPuzzle(puzzleInput);
                Console.WriteLine();
                game.solve();
                game.printPuzzle();

                Console.WriteLine("Game completed");
                isContinue = (Int32.Parse(Console.ReadLine()) == 1);
            } while (isContinue);

            Console.ReadKey();
        }
    }
}
