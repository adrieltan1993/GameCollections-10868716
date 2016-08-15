using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    class SudokuCell
    {
        int _digit = 0;

        public SudokuCell(int digit, bool isDefault)
        {
            _digit = digit;
        }

        public void setCell(int digit)
        {
            _digit = digit;
        }

        public int getCell()
        {
            return _digit;
        }
    }
}