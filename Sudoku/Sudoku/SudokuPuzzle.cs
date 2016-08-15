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

        private bool validateDigit(int num)
        {
            return ((num >= 1) && (num <= 9));
        }
    }
}