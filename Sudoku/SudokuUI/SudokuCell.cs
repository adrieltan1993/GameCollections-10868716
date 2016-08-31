using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuUI
{
    class SudokuCell
    {
        int _digit = 0;
        bool[] _notes = new bool[] { false, false, false, false, false, false, false, false, false };
        bool _isLocked;

        public SudokuCell(int digit, bool isLocked)
        {
            _digit = digit;
            _isLocked = isLocked;
        }

        public void setCell(int digit)
        {
            if(!_isLocked)
            {
                _digit = digit;
                this.deleteAllNotes();
            }
        }

        public int getCell()
        {
            return _digit;
        }

        public void setNote(int noteDigit)
        {
            if(!_isLocked)
            {
                _notes[noteDigit - 1] = true;
            }
        }

        public void deleteNote(int noteDigit)
        {
            if (!_isLocked)
            {
                _notes[noteDigit - 1] = false;
            }
        }

        public void deleteAllNotes()
        {
            if (!_isLocked)
            {
                _notes = new bool[] { false, false, false, false, false, false, false, false, false };
            }
        }

        public int[] getNotes()
        {
            if (!_isLocked && (_digit == 0))
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
            else
            {
                return new int[] { 0 };
            }
        }
    }
}