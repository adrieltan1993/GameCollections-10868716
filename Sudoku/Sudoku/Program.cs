using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    class Program
    {
        const String INVALID_INPUT_MESSAGE = "!!! Invalid Input !!!";
        const String CHOOSE_OPTION_MESSAGE = "Choose from the following options: ";
        const String OPTION_ONE_MESSAGE = "1. Solve puzzle completely";
        const String OPTION_TWO_MESSAGE = "2. Solve one cell only";
        const String OPTION_THREE_MESSAGE = "3. Show notes";
        const String OPTION_FOUR_MESSAGE = "4. Show Puzzle";
        const String OPTION_FIVE_MESSAGE = "5. Enter more cells";
        const String OPTION_ZERO_MESSAGE = "0. Quit";
        const String TO_CONTINUE_MESSAGE = "To solve another puzzle, Press 1";
        const String EXIT_MESSAGE = "--- Press any key to Exit ---";


        static void Main(string[] args)
        {
            bool isContinue = false;

            do
            {
                int[][] puzzleInput = new int[9][];
                bool hasFailed = false;
                for(int i = 0; i < 9; i++)
                {
                    Console.Write("Enter row {0}: ", (i + 1));
                    puzzleInput[i] = new int[9];
                    try
                    {
                        int input = Int32.Parse(Console.ReadLine());
                        for (int j = 8; j >= 0; j--)
                        {
                            int digit = input % 10;
                            input = input / 10;
                            puzzleInput[i][j] = digit;
                        }
                    }
                    catch
                    {
                        Console.WriteLine(INVALID_INPUT_MESSAGE);
                        hasFailed = true;
                        break;
                    }
                }
                Console.WriteLine();
                if(!hasFailed)
                {
                    SudokuPuzzle game = new SudokuPuzzle(puzzleInput);
                    Console.WriteLine();

                    bool isQuit = false;
                    do
                    {
                        Console.WriteLine(CHOOSE_OPTION_MESSAGE);
                        Console.WriteLine(OPTION_ONE_MESSAGE);
                        Console.WriteLine(OPTION_TWO_MESSAGE);
                        Console.WriteLine(OPTION_THREE_MESSAGE);
                        Console.WriteLine(OPTION_FOUR_MESSAGE);
                        Console.WriteLine(OPTION_FIVE_MESSAGE);
                        Console.WriteLine(OPTION_ZERO_MESSAGE);

                        int input = -1;
                        try
                        {
                            input = Int32.Parse(Console.ReadLine());
                            Console.WriteLine();
                        }
                        catch
                        {
                            Console.WriteLine(INVALID_INPUT_MESSAGE);
                            Console.WriteLine();
                            continue;
                        }
                        switch (input)
                        {
                            case 1:
                                game.solve(false);
                                game.printPuzzle();
                                break;
                            case 2:
                                game.solve(true);
                                game.printPuzzle();
                                break;
                            case 3:
                                game.printNotes();
                                break;
                            case 4:
                                game.printPuzzle();
                                break;
                            case 5:
                                enterMoreCells(game);
                                game.printPuzzle();
                                break;
                            case 0:
                                isQuit = true;
                                break;
                            default:
                                Console.WriteLine(INVALID_INPUT_MESSAGE);
                                Console.WriteLine();
                                continue;
                        }
                    } while (!game.checkCompleted() && !isQuit);
                }


                Console.WriteLine(TO_CONTINUE_MESSAGE);
                try
                {
                    isContinue = (Int32.Parse(Console.ReadLine()) == 1);
                }
                catch
                {
                    isContinue = false;
                }
            } while (isContinue);

            Console.WriteLine(EXIT_MESSAGE);
            Console.ReadKey();
        }

        private static void enterMoreCells(SudokuPuzzle game)
        {
            bool continueInput = true;
            do
            {
                try
                {
                    Console.WriteLine("Enter 0 to stop input");
                    Console.Write("Enter <ROW><COLUMN><DIGIT> (without spaces): ");
                    int input = Int32.Parse(Console.ReadLine());
                    Console.WriteLine();

                    if(input != 0)
                    {
                        int digit = input % 10;
                        input = input / 10;
                        int col = input % 10;
                        input = input / 10;
                        int row = input % 10;
                        game.setCell(row, col, digit);
                    }
                    else
                    {
                        continueInput = false;
                    }
                }
                catch
                {
                    Console.WriteLine(INVALID_INPUT_MESSAGE);
                }
            } while (continueInput && !game.checkCompleted());
        }
    }
}
