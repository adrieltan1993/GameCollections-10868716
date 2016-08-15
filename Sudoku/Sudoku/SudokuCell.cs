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
        bool[] _notes = new bool[] { false, false, false, false, false, false, false, false, false };

        public SudokuCell(int digit, bool isDefault)
        {
            _digit = digit;
        }

        public void setCell(int digit)
        {
            _digit = digit;
            this.deleteAllNotes();
        }

        public int getCell()
        {
            return _digit;
        }

        public void setNote(int noteDigit)
        {
            _notes[noteDigit - 1] = true;
        }

        public void deleteNote(int noteDigit)
        {
            _notes[noteDigit - 1] = false;
        }

        public void deleteAllNotes()
        {
            _notes = new bool[] { false, false, false, false, false, false, false, false, false };
        }

        public int[] getNotes()
        {
            List<int> notes = new List<int>();
            for (int i = 0; i < 9; i++)
            {
                if (_notes[i])
                {
                    notes.Add(i + 1);
                }
            }

            return notes.ToArray();
        }
    }
}