using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    class SudokuSolver2
    {
        private SudokuPuzzle _sudoku;
        SudokuCell[][] _sudokuCells;
        SudokuCell[][] _cellRowArr = new SudokuCell[9][];
        SudokuCell[][] _cellColArr = new SudokuCell[9][];
        SudokuCell[][] _cellBoxArr = new SudokuCell[9][];
        private bool _hasChanged;

        public SudokuSolver2(SudokuPuzzle sudoku, SudokuCell[][] sudokuCells)
        {
            _sudoku = sudoku;
            _sudokuCells = sudokuCells;

            getCellRowArr();
            getCellColArr();
            getCellBoxArr();
        }

        public void solve()
        {
            while (!_sudoku.checkCompleted())
            {
                _hasChanged = false;
                setNotes();
                setCells();
                if(!_hasChanged)
                {
                    setNotesHiddenSingles();
                    Console.WriteLine("Hidden Single");
                    setCells();
                }
                //_sudoku.printNotes();
                _sudoku.printPuzzle();
                }
        }

        private void setNotes()
        {
            for (int r = 0; r < 9; r++)
            {
                for (int c = 0; c < 9; c++)
                {
                    _sudokuCells[r][c].deleteAllNotes();

                    int[] row = getRow(r);
                    int[] col = getCol(c);
                    int[] box = getBox(r, c);

                    var line = row.Concat(col.Concat(box)).Distinct().ToArray();

                    int[] notes = Enumerable.Range(1, 9).Except(line).ToArray();
                    foreach (int note in notes)
                    {
                        _sudokuCells[r][c].setNote(note);
                    }
                }
            }
        }

        private void setNotesHiddenSingles()
        {
            for (int i = 0; i < 9; i++)
            {
                findHiddenSingle(_cellRowArr[i], i);
                findHiddenSingle(_cellColArr[i], i);
                findHiddenSingle(_cellBoxArr[i], i);
            }
        }

        private void findHiddenSingle(SudokuCell[] cellArr, int index)
        {
            int[][] notes = new int[9][];
            for(int i = 0; i < 9; i++)
            {
                notes[i] = cellArr[i].getNotes();
            }
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
                            cellArr[i].deleteAllNotes();
                            cellArr[i].setNote(num);
                            break;
                        }
                    }
                }
            }
        }

        private void setCells()
        {
            for (int r = 0; r < 9; r++)
            {
                for (int c = 0; c < 9; c++)
                {
                    int[] notes = _sudokuCells[r][c].getNotes();
                    if ((notes.Length == 1) && (notes[0] != 0))
                    {
                        _sudokuCells[r][c].setCell(notes[0]);
                        _hasChanged = true;
                    }
                }
            }
        }

        private int[] getRow(int r)
        {
            int[] row = new int[9];
            for(int i = 0; i < 9; i++)
            {
                row[i] = _cellRowArr[r][i].getCell();
            }
            return row;
        }

        private int[][] getRowNotes(int r)
        {
            int[][] rowNotes = new int[9][];
            for (int i = 0; i < 9; i++)
            {
                rowNotes[i] = _cellRowArr[r][i].getNotes();
            }
            return rowNotes;
        }

        private int[] getCol(int c)
        {
            int[] col = new int[9];
            for(int i = 0; i < 9; i++)
            {
                col[i] = _cellColArr[c][i].getCell();
            }
            return col;
        }

        private int[][] getColNotes(int c)
        {
            int[][] colNotes = new int[9][];
            for (int i = 0; i < 9; i++)
            {
                colNotes[i] = _cellColArr[c][i].getNotes();
            }
            return colNotes;
        }

        private int[] getBox(int r, int c)
        {
            int[] box = new int[9];
            int boxIndex = r / 3 * 3 + c / 3;
            for (int i = 0; i < 9; i++)
            {
                box[i] = _cellBoxArr[boxIndex][i].getCell();
            }
            return box;
        }

        private int[][] getBoxNotes(int boxIndex)
        {
            int[][] boxNotes = new int[9][];
            for (int i = 0; i < 9; i++)
            {
                boxNotes[i] = _cellBoxArr[boxIndex][i].getNotes();
            }
            return boxNotes;
        }

        private void getCellRowArr()
        {
            for(int r = 0; r < 9; r++)
            {
                _cellRowArr[r] = _sudokuCells[r];
            }
        }

        private void getCellColArr()
        {
            for (int c = 0; c < 9; c++)
            {
                _cellColArr[c] = new SudokuCell[9];
                for(int r = 0; r < 9; r++)
                {
                    _cellColArr[c][r] = _sudokuCells[r][c];
                }
            }
        }

        private void getCellBoxArr()
        {
            int arrIndex = 0;
            for(int baseRow = 0; baseRow < 9; baseRow += 3)
            {
                for(int baseCol = 0; baseCol < 9; baseCol += 3)
                {
                    _cellBoxArr[arrIndex] = new SudokuCell[9];
                    int index = 0;
                    for (int r = baseRow; r < baseRow + 3; r++)
                    {
                        for(int c = baseCol; c < baseCol + 3; c++)
                        {
                            _cellBoxArr[arrIndex][index++] = _sudokuCells[r][c];
                        }
                    }
                    arrIndex++;
                }
            }
        }
    }
}
