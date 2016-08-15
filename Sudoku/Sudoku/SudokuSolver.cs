using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    class SudokuSolver
    {
        private SudokuPuzzle _sudoku;
        private bool _hasChanged;

        public SudokuSolver(SudokuPuzzle sudoku)
        {
            _sudoku = sudoku;
        }

        public void solve()
        {
            while (!_sudoku.checkCompleted())
            {
                _hasChanged = false;
                setNotes();
                setCells();
                if (!_hasChanged)
                {
                    setNotesAdvanced();
                    setCells();
                }
            }
        }

        private void setNotes()
        {
            int[][] puzzle = _sudoku.getPuzzle();

            for (int r = 1; r <= 9; r++)
            {
                for (int c = 1; c <= 9; c++)
                {
                    if (_sudoku.getCell(r, c) == 0)
                    {
                        _sudoku.deleteAllNotesInCell(r, c);

                        int[] row = getRow(puzzle, r);
                        int[] col = getCol(puzzle, c);
                        int[] box = getBox(puzzle, r, c);
                        var line = row.Concat(col.Concat(box)).Distinct().ToArray();

                        int[] notes = Enumerable.Range(1, 9).Except(line).ToArray();
                        foreach (int note in notes)
                        {
                            _sudoku.setNoteInCell(r, c, note);
                            _hasChanged = true;
                        }
                    }

                }
            }
        }

        private void setNotesAdvanced()
        {
            for (int i = 1; i <= 9; i++)
            {
                int[][] rowNotes = getRowNotes(i);
                findHiddenSingle(rowNotes, i, "row");
                int[][] colNotes = getColNotes(i);
                findHiddenSingle(colNotes, i, "col");
                int[][] boxNotes = getBoxNotes(i);
                findHiddenSingle(boxNotes, i, "box");
            }
        }

        private void setCells()
        {
            for (int r = 1; r <= 9; r++)
            {
                for (int c = 1; c <= 9; c++)
                {
                    int[] notes = _sudoku.getNoteFromCell(r, c);
                    if ((notes.Length == 1) && (notes[0] != 0))
                    {
                        _sudoku.setCell(r, c, notes[0]);
                    }
                }
            }
        }

        private int[] getRow(int[][] puzzle, int r)
        {
            return puzzle[r - 1];
        }

        private int[] getCol(int[][] puzzle, int c)
        {
            int[] col = new int[9];
            for (int i = 0; i < 9; i++)
            {
                col[i] = puzzle[i][c - 1];
            }
            return col;
        }

        private int[] getBox(int[][] puzzle, int r, int c)
        {
            int[] box = new int[9];
            int row = (r - 1) / 3 * 3;
            int col = (c - 1) / 3 * 3;

            for (int i = row; i < row + 3; i++)
            {
                for (int j = col; j < col + 3; j++)
                {
                    box[i % 3 * 3 + j % 3] = puzzle[i][j];
                }
            }
            return box;
        }

        private int[][] getRowNotes(int r)
        {
            int[][] rowNotes = new int[9][];

            for (int c = 1; c <= 9; c++)
            {
                rowNotes[c - 1] = _sudoku.getNoteFromCell(r, c);
            }
            return rowNotes;
        }

        private int[][] getColNotes(int c)
        {
            int[][] colNotes = new int[9][];

            for (int r = 1; r <= 9; r++)
            {
                colNotes[r - 1] = _sudoku.getNoteFromCell(r, c);
            }
            return colNotes;
        }

        private int[][] getBoxNotes(int boxNum)
        {
            int[][] boxNotes = new int[9][];
            int row = (boxNum - 1) / 3 * 3 + 1;
            int col = (boxNum - 1) % 3 * 3 + 1;

            for (int r = row; r < row + 3; r++)
            {
                for (int c = col; c < col + 3; c++)
                {
                    boxNotes[(r - row) * 3 + (c - col)] = _sudoku.getNoteFromCell(r, c);
                }
            }

            return boxNotes;
        }

        private void findHiddenSingle(int[][] notes, int index, String type)
        {
            int[] allNotes = notes.SelectMany(i => i).Where(n => n != 0).OrderBy(n => n).ToArray();
            int[] uniqueArr = allNotes.Distinct().ToArray();

            foreach (int num in uniqueArr)
            {
                int count = allNotes.Where(n => n == num).Count();
                if (count == 1)
                {
                    for (int i = 0; i < 9; i++)
                    {
                        if (notes[i].Contains(num))
                        {
                            switch (type)
                            {
                                case ("row"):
                                    _sudoku.deleteAllNotesInCell(index, i + 1);
                                    _sudoku.setNoteInCell(index, i + 1, num);
                                    Console.WriteLine("<ROW> Hidden single in row {0}, col {1}, num {2}", index, i + 1, num);
                                    break;
                                case ("col"):
                                    _sudoku.deleteAllNotesInCell(i + 1, index);
                                    _sudoku.setNoteInCell(i + 1, index, num);
                                    Console.WriteLine("<COL> Hidden single in row {0}, col {1}, num {2}", i + 1, index, num);
                                    break;
                                case ("box"):
                                    _sudoku.deleteAllNotesInCell(((index - 1) / 3 * 3 + 1) + i / 3, ((index - 1) % 3 * 3 + 1) + i % 3);
                                    _sudoku.setNoteInCell(((index - 1) / 3 * 3 + 1) + i / 3, ((index - 1) % 3 * 3 + 1) + i % 3, num);
                                    Console.WriteLine("<BOX> Hidden single in row {0}, col {1}, num {2}", ((index - 1) / 3 * 3 + 1) + i / 3, ((index - 1) % 3 * 3 + 1) + i % 3, num);
                                    break;
                            }
                        }
                    }
                }
            }
        }
    }
}
