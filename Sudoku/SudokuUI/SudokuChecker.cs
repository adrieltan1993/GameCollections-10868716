using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuUI
{
    class SudokuChecker
    {
        private int[][] _puzzle;

        public SudokuChecker() { }

        public bool checkPuzzle(int[][] puzzle)
        {
            _puzzle = puzzle;
            bool isCorrect = true;

            for (int i = 0; i < 9; i++)
            {
                int[] row = getRow(i);
                int[] column = getColumn(i);

                if (!checkLine(row) || !checkLine(column))
                {
                    isCorrect = false;
                    break;
                }
            }

            return isCorrect;
        }

        private bool checkLine(int[] line)
        {
            return ((line.Distinct().Count() == 9) && !line.Any(i => i == 0));
        }

        private int[] getRow(int rowNum)
        {
            return _puzzle[rowNum];
        }

        private int[] getColumn(int colNum)
        {
            int[] column = new int[9];

            for (int row = 0; row < 9; row++)
            {
                column[row] = _puzzle[row][colNum];
            }

            return column;
        }
    }
}
