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
            setNotes();
            setCells();
            while (!_sudoku.checkCompleted())
            {
                _hasChanged = false;
                updateNotes();
                setCells();

                if (!_hasChanged)
                {
                    setNotesHiddenSingles();
                    setNotesLockedCandidates();
                    setNotesNakedPair();
                    setCells();
                }
                //if (!_hasChanged)
                //{
                //    Console.WriteLine("Hidden Single");
                //    setNotesHiddenSingles();
                //    setCells();
                //}
                //if (!_hasChanged)
                //{
                //    Console.WriteLine("Locked Candidate");
                //    setNotesLockedCandidates();
                //    setCells();
                //}
                //if (!_hasChanged)
                //{
                //    //_sudoku.printNotes();
                //    Console.WriteLine("Naked Pair");
                //    setNotesNakedPair();
                //    setCells();
                //    //_sudoku.printNotes();
                //}
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

        private void updateNotes()
        {
            for (int r = 0; r < 9; r++)
            {
                for (int c = 0; c < 9; c++)
                {
                    int[] row = getRow(r);
                    int[] col = getCol(c);
                    int[] box = getBox(r, c);

                    var line = row.Concat(col.Concat(box)).Distinct().Except(new int[] { 0 }).ToArray();

                    foreach (int num in line)
                    {
                        _sudokuCells[r][c].deleteNote(num);
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

        private void setNotesLockedCandidates()
        {
            for (int i = 0; i < 9; i++)
            {
                findLockedCandidates(_cellBoxArr[i], i);
            }
        }

        private void findLockedCandidates(SudokuCell[] cellArr, int boxNum)
        {
            int[][] notes = getNotes(cellArr);
            int baseRow = boxNum / 3 * 3;
            int baseCol = boxNum % 3 * 3;

            findLockedCandidatesHorizontal(notes, baseRow, baseCol);
            findLockedCandidatesVertical(notes, baseRow, baseCol);
        }

        private void findLockedCandidatesHorizontal(int[][] notes, int baseRow, int baseCol)
        {
            int[][] horizontals = new int[3][];

            for (int i = 0; i < 3; i++)
            {
                int index = i * 3;
                horizontals[i] = notes[index].Concat(notes[index + 1].Concat(notes[index + 2])).Except(new int[] { 0 }).Distinct().ToArray();
            }

            for (int i = 0; i < 3; i++)
            {
                int[] uniqueArr = horizontals[i].Except(horizontals[(i + 1) % 3].Concat(horizontals[(i + 2) % 3])).ToArray();
                if (uniqueArr.Count() != 0)
                {
                    foreach (int num in uniqueArr)
                    {
                        for (int c = 0; c < 9; c++)
                        {
                            if (!((c == baseCol) || (c == baseCol + 1) || (c == baseCol + 2)))
                            {
                                _sudokuCells[baseRow + i][c].deleteNote(num);
                            }
                        }
                    }
                }
            }
        }

        private void findLockedCandidatesVertical(int[][] notes, int baseRow, int baseCol)
        {
            int[][] verticals = new int[3][];

            for (int i = 0; i < 3; i++)
            {
                verticals[i] = notes[i].Concat(notes[i + 3].Concat(notes[i + 6])).Distinct().Except(new int[] { 0 }).ToArray();
            }

            for (int i = 0; i < 3; i++)
            {
                int[] uniqueArr = verticals[i].Except(verticals[(i + 1) % 3].Concat(verticals[(i + 2) % 3])).ToArray();
                if (uniqueArr.Count() != 0)
                {
                    foreach (int num in uniqueArr)
                    {
                        for (int r = 0; r < 9; r++)
                        {
                            if (!((r == baseRow) || (r == baseRow + 1) || (r == baseRow + 2)))
                            {
                                _sudokuCells[r][baseCol + i].deleteNote(num);
                            }
                        }
                    }
                }
            }
        }

        private void setNotesNakedPair()
        {
            for (int i = 0; i < 9; i++)
            {
                findNakedPair(_cellRowArr[i]);
                findNakedPair(_cellColArr[i]);
                findNakedPair(_cellBoxArr[i]);
            }
        }

        private void findNakedPair(SudokuCell[] cellArr)
        {
            int[][] notes = getNotes(cellArr);

            for (int i = 0; i < 8; i++)
            {
                if (notes[i].Count() == 2)
                {
                    for (int j = i + 1; j < 9; j++)
                    {
                        if ((notes[j].Count() == 2) && (notes[i].SequenceEqual(notes[j])))
                        {
                            for (int index = 0; index < 9; index++)
                            {
                                if ((index != i) && (index != j))
                                {
                                    foreach (int num in notes[i])
                                    {
                                        cellArr[index].deleteNote(num);
                                    }
                                }
                            }
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

        private int[][] getNotes(SudokuCell[] cellArr)
        {
            int[][] notes = new int[9][];
            for (int i = 0; i < 9; i++)
            {
                notes[i] = cellArr[i].getNotes();
            }
            return notes;
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
