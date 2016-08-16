using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    class SudokuPuzzle
    {
        SudokuCell[][] _puzzle = new SudokuCell[9][];

        public SudokuPuzzle(int[][] puzzle)
        {
            for (int i = 0; i < 9; i++)
            {
                _puzzle[i] = new SudokuCell[9];
                for (int j = 0; j < 9; j++)
                {
                    int digit = puzzle[i][j];
                    if (validateDigit(digit))
                    {
                        _puzzle[i][j] = new SudokuCell(digit, true);
                    }
                    else
                    {
                        _puzzle[i][j] = new SudokuCell(digit, false);
                    }
                }
            }
        }

        public void setCell(int row, int col, int digit)
        {
            if (validateDigit(row) && validateDigit(col) && validateDigit(digit))
            {
                _puzzle[row - 1][col - 1].setCell(digit);
            }
        }

        public int getCell(int row, int col)
        {
            if (validateDigit(row) && validateDigit(col))
            {
                return _puzzle[row - 1][col - 1].getCell();
            }
            return 0;
        }

        public void setNoteInCell(int row, int col, int note)
        {
            if (validateDigit(row) && validateDigit(col) && validateDigit(note))
            {
                _puzzle[row - 1][col - 1].setNote(note);
            }
        }

        public void deleteNoteInCell(int row, int col, int note)
        {
            if (validateDigit(row) && validateDigit(col) && validateDigit(note))
            {
                _puzzle[row - 1][col - 1].deleteNote(note);
            }
        }

        public void deleteAllNotesInCell(int row, int col)
        {
            if (validateDigit(row) && validateDigit(col))
            {
                _puzzle[row - 1][col - 1].deleteAllNotes();
            }
        }

        public int[] getNoteFromCell(int row, int col)
        {
            if (validateDigit(row) && validateDigit(col))
            {
                return _puzzle[row - 1][col - 1].getNotes();
            }
            return null;
        }

        private bool validateDigit(int num)
        {
            return ((num >= 1) && (num <= 9));
        }

        public void printPuzzle()
        {
            for (int r = 1; r <= 9; r++)
            {
                for (int c = 1; c <= 9; c++)
                {
                    Console.Write(this.getCell(r, c));
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public void printNotes()
        {
            for (int r = 1; r <= 9; r++)
            {
                for (int c = 1; c <= 9; c++)
                {
                    if (this.getCell(r, c) == 0)
                    {
                        int[] notesArr = this.getNoteFromCell(r, c);
                        Console.Write("row = {0}, col = {1}: ", r, c);
                        foreach (int note in notesArr)
                        {
                            Console.Write("{0} ", note);
                        }
                        Console.WriteLine();
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public bool checkCompleted()
        {
            SudokuChecker sc = new SudokuChecker();
            return sc.checkPuzzle(this.getPuzzle());
        }

        public int[][] getPuzzle()
        {
            int[][] puzzle = new int[9][];
            for (int r = 0; r < 9; r++)
            {
                puzzle[r] = new int[9];
                for (int c = 0; c < 9; c++)
                {
                    puzzle[r][c] = this.getCell(r + 1, c + 1);
                }
            }
            return puzzle;
        }

        public void solve()
        {
            //SudokuSolver sv = new SudokuSolver(this);
            //sv.solve();

            SudokuSolver2 sv = new SudokuSolver2(this, _puzzle);
            sv.solve();
        }
    }
}