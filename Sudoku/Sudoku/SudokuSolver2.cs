using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    class SudokuSolver2
    {
        const String NAKED_SINGLE_MESSAGE = "Naked Single";
        const String HIDDEN_SINGLE_MESSAGE = "Hidden Single";
        const String HIDDEN_PAIR_MESSAGE = "Hidden Pair";
        const String NAKED_PAIR_MESSAGE = "Naked Pair";
        const String NAKED_TRIPLE_MESSAGE = "Naked Triple";
        const String LOCKED_CANDIDATE_ROW_MESSAGE = "Locked Candidate R";
        const String LOCKED_CANDIDATE_COL_MESSAGE = "Locked Candidate C";


        private SudokuPuzzle _sudoku;
        SudokuCell[][] _sudokuCells;
        SudokuCell[][] _cellRowArr = new SudokuCell[9][];
        SudokuCell[][] _cellColArr = new SudokuCell[9][];
        SudokuCell[][] _cellBoxArr = new SudokuCell[9][];
        private bool _hasChanged;
        private bool _isOneByOne = false;
        private bool _foundHint;

        public SudokuSolver2(SudokuPuzzle sudoku, SudokuCell[][] sudokuCells)
        {
            _sudoku = sudoku;
            _sudokuCells = sudokuCells;

            getCellRowArr();
            getCellColArr();
            getCellBoxArr();

            setNotes();
        }

        public void solve(bool isOneByOne)
        {
            _isOneByOne = isOneByOne;
            setNotes();
            while (!_sudoku.checkCompleted())
            {
                _hasChanged = false;
                _foundHint = false;
                updateNotes();
                setCells();
                if (_isOneByOne && _hasChanged)
                {
                    Console.WriteLine(NAKED_SINGLE_MESSAGE);
                }
                if (!_hasChanged)
                {
                    setNotesHiddenSingles();
                    setCells();
                }
                if (!_hasChanged)
                {
                    setNotesLockedCandidates();
                    setCells();
                }
                if(!_hasChanged)
                {
                    setNotesNakedPair();
                    setCells();
                }
                if(!_hasChanged)
                {
                    setNotesHiddenPair();
                    setCells();
                }
                if (!_hasChanged)
                {
                    setNotesNakedTriple();
                    setCells();
                }
                if(_isOneByOne && _hasChanged)
                {
                    return;
                }
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
                    if(_sudokuCells[r][c].getCell() == 0)
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
        }

        private void setNotesHiddenSingles()
        {
            for (int i = 0; i < 9; i++)
            {
                findHiddenSingle(_cellRowArr[i], i);
                if(_foundHint) { return; }
                findHiddenSingle(_cellColArr[i], i);
                if (_foundHint) { return; }
                findHiddenSingle(_cellBoxArr[i], i);
                if (_foundHint) { return; }
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
                            if (_isOneByOne)
                            {
                                Console.WriteLine(HIDDEN_SINGLE_MESSAGE);
                                _foundHint = true;
                                return;
                            }
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
                if (_foundHint) { return; }
                findLockedCandidatesRow(_cellRowArr[i], i);
                if (_foundHint) { return; }
                findLockedCandidatesCol(_cellColArr[i], i);
                if (_foundHint) { return; }
            }
        }

        private void findLockedCandidates(SudokuCell[] cellArr, int boxNum)
        {
            int[][] notes = getNotes(cellArr);
            int baseRow = boxNum / 3 * 3;
            int baseCol = boxNum % 3 * 3;

            findLockedCandidatesHorizontal(notes, baseRow, baseCol);
            if (_foundHint) { return; }
            findLockedCandidatesVertical(notes, baseRow, baseCol);
            if (_foundHint) { return; }
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
                                if (_sudokuCells[baseRow + i][c].getCell() == 0 && _sudokuCells[baseRow + i][c].getNotes().Contains(num))
                                {
                                    _sudokuCells[baseRow + i][c].deleteNote(num);
                                    _foundHint = true;
                                }
                            }
                        }
                        if (_isOneByOne && _foundHint)
                        {
                            Console.WriteLine(LOCKED_CANDIDATE_ROW_MESSAGE);
                            return;
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
                                if (_sudokuCells[r][baseCol + i].getCell() == 0 && _sudokuCells[r][baseCol + i].getNotes().Contains(num))
                                {
                                    _sudokuCells[r][baseCol + i].deleteNote(num);
                                    _foundHint = true;
                                }
                            }
                        }
                        if (_isOneByOne && _foundHint)
                        {
                            Console.WriteLine(LOCKED_CANDIDATE_COL_MESSAGE);
                            return;
                        }
                    }
                }
            }
        }

        private void findLockedCandidatesRow(SudokuCell[] cellArr, int rowNum)
        {
            int[][] notes = getNotes(cellArr);
            int baseRow = rowNum / 3 * 3;

            int[][] parts = new int[3][];
            for (int i = 0; i < 3; i++)
            {
                int index = i * 3;
                parts[i] = notes[index].Concat(notes[index + 1].Concat(notes[index + 2])).Distinct().Except(new int[] { 0 }).ToArray();
            }

            for (int i = 0; i < 3; i++)
            {
                int[] uniqueArr = parts[i].Except(parts[(i + 1) % 3].Concat(parts[(i + 2) % 3])).ToArray();
                if (uniqueArr.Count() != 0)
                {
                    int baseCol = i * 3;
                    foreach (int num in uniqueArr)
                    {
                        for(int r = baseRow; r < baseRow + 3; r++)
                        {
                            if(r != rowNum)
                            {
                                if (_sudokuCells[r][baseCol].getCell() == 0 && _sudokuCells[r][baseCol].getNotes().Contains(num))
                                {
                                    _sudokuCells[r][baseCol].deleteNote(num);
                                    _foundHint = true;
                                }
                                if (_sudokuCells[r][baseCol + 1].getCell() == 0 && _sudokuCells[r][baseCol + 1].getNotes().Contains(num))
                                {
                                    _sudokuCells[r][baseCol + 1].deleteNote(num);
                                    _foundHint = true;
                                }
                                if (_sudokuCells[r][baseCol + 2].getCell() == 0 && _sudokuCells[r][baseCol + 2].getNotes().Contains(num))
                                {
                                    _sudokuCells[r][baseCol + 2].deleteNote(num);
                                    _foundHint = true;
                                }
                            }
                        }
                        if (_isOneByOne && _foundHint)
                        {
                            Console.WriteLine(LOCKED_CANDIDATE_ROW_MESSAGE);
                            return;
                        }
                    }
                }
            }
        }

        private void findLockedCandidatesCol(SudokuCell[] cellArr, int colNum)
        {
            int[][] notes = getNotes(cellArr);
            int baseCol = colNum / 3 * 3;

            int[][] parts = new int[3][];
            for (int i = 0; i < 3; i++)
            {
                int index = i * 3;
                parts[i] = notes[index].Concat(notes[index + 1].Concat(notes[index + 2])).Distinct().Except(new int[] { 0 }).ToArray();
            }

            for (int i = 0; i < 3; i++)
            {
                int[] uniqueArr = parts[i].Except(parts[(i + 1) % 3].Concat(parts[(i + 2) % 3])).ToArray();
                if (uniqueArr.Count() != 0)
                {
                    int baseRow = i * 3;
                    foreach (int num in uniqueArr)
                    {
                        for (int c = baseCol; c < baseCol + 3; c++)
                        {
                            if (c != colNum)
                            {
                                if(_sudokuCells[baseRow][c].getCell() == 0 && _sudokuCells[baseRow][c].getNotes().Contains(num))
                                {
                                    _sudokuCells[baseRow][c].deleteNote(num);
                                    _foundHint = true;
                                }
                                if (_sudokuCells[baseRow + 1][c].getCell() == 0 && _sudokuCells[baseRow + 1][c].getNotes().Contains(num))
                                {
                                    _sudokuCells[baseRow + 1][c].deleteNote(num);
                                    _foundHint = true;
                                }
                                if (_sudokuCells[baseRow + 2][c].getCell() == 0 && _sudokuCells[baseRow + 2][c].getNotes().Contains(num))
                                {
                                    _sudokuCells[baseRow + 2][c].deleteNote(num);
                                    _foundHint = true;
                                }
                            }
                        }
                        if (_isOneByOne && _foundHint)
                        {
                            Console.WriteLine(LOCKED_CANDIDATE_COL_MESSAGE);
                            return;
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
                if (_foundHint) { return; }
                findNakedPair(_cellColArr[i]);
                if (_foundHint) { return; }
                findNakedPair(_cellBoxArr[i]);
                if (_foundHint) { return; }
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
                                        if(cellArr[index].getCell() == 0 && cellArr[index].getNotes().Contains(num))
                                        {
                                            cellArr[index].deleteNote(num);
                                            _foundHint = true;
                                        }
                                    }
                                }
                            }
                            if (_isOneByOne && _foundHint)
                            {
                                Console.WriteLine(NAKED_PAIR_MESSAGE);
                            }
                            return;
                        }
                    }
                }
            }
        }

        private void setNotesHiddenPair()
        {
            for (int i = 0; i < 9; i++)
            {
                findHiddenPair(_cellRowArr[i]);
                if (_foundHint) { return; }
                findHiddenPair(_cellColArr[i]);
                if (_foundHint) { return; }
                findHiddenPair(_cellBoxArr[i]);
                if (_foundHint) { return; }
            }
        }

        private void findHiddenPair(SudokuCell[] cellArr)
        {
            int[][] notes = getNotes(cellArr);

            var emptyCellCount = notes.Where(i => i[0] != 0).Count();
            if(emptyCellCount < 5)
            {
                return;
            }

            int[] allNotes = notes.SelectMany(i => i).Where(n => n != 0).OrderBy(n => n).ToArray();
            int[] doubles = allNotes.GroupBy(e => e).Where(e => e.Count() == 2).Select(e => e.First()).ToArray();

            if(doubles.Count() < 2)
            {
                return;
            }

            int[][] possiblePairs = genSeq(doubles, 2);

            foreach(int[] pair in possiblePairs)
            {
                for(int i = 0; i < 8; i++)
                {
                    if (notes[i].Contains(pair[0]) && notes[i].Contains(pair[1]))
                    {
                        for(int j = i + 1; j < 9; j++)
                        {
                            if(notes[j].Contains(pair[0]) && notes[j].Contains(pair[1]))
                            {
                                cellArr[i].deleteAllNotes();
                                cellArr[j].deleteAllNotes();
                                foreach(int num in pair)
                                {
                                    cellArr[i].setNote(num);
                                    cellArr[j].setNote(num);
                                }
                                if (_isOneByOne)
                                {
                                    _foundHint = true;
                                    Console.WriteLine(HIDDEN_PAIR_MESSAGE);
                                }
                                return;
                            }
                        }
                    }
                }
            }
        }

        private void setNotesNakedTriple()
        {
            for (int i = 0; i < 9; i++)
            {
                findNakedTriple(_cellRowArr[i]);
                if (_foundHint) { return; }
                findNakedTriple(_cellColArr[i]);
                if (_foundHint) { return; }
                findNakedTriple(_cellBoxArr[i]);
                if (_foundHint) { return; }
            }
        }

        private void findNakedTriple(SudokuCell[] cellArr)
        {
            int[][] notes = getNotes(cellArr);

            var emptyCellCount = notes.Where(i => i[0] != 0).Count();
            if (emptyCellCount < 6)
            {
                return;
            }

            var possibleCells = cellArr.Where(cell => ((cell.getCell() == 0) && (cell.getNotes().Count() <= 3))).ToArray();

            if(possibleCells.Count() < 3)
            {
                return;
            }

            int[][] possibleCellsNotes = getNotes(possibleCells);
            int[][] possibleTriples = genSeq(possibleCellsNotes.SelectMany(n => n).Distinct().Except(new int[] { 0 }).ToArray(), 3);

            foreach (int[] triple in possibleTriples)
            {
                int count = 0;
                SudokuCell[] cell = new SudokuCell[3];
                for(int i = 0; i < possibleCellsNotes.Count(); i++)
                {
                    bool notFound = false;
                    foreach (int note in possibleCellsNotes[i])
                    {
                        if(!triple.Contains(note))
                        {
                            notFound = true;
                            break;
                        }
                    }
                    if(!notFound)
                    {
                        cell[count] = possibleCells[i];
                        count++;
                    }
                }
                if(count == 3)
                {
                    foreach(SudokuCell c in cellArr)
                    {
                        if(!(c.Equals(cell[0]) || c.Equals(cell[1]) || c.Equals(cell[2])))
                        {
                            foreach(int num in triple)
                            {
                                if(c.getCell() == 0 && c.getNotes().Contains(num))
                                {
                                    c.deleteNote(num);
                                    _foundHint = true;
                                }
                            }
                        }
                    }
                    if (_isOneByOne && _foundHint)
                    {
                        Console.WriteLine(NAKED_TRIPLE_MESSAGE);
                        return;
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
                    if(_sudokuCells[r][c].getCell() == 0)
                    {
                        int[] notes = _sudokuCells[r][c].getNotes();
                        if ((notes.Length == 1))
                        {
                            _sudokuCells[r][c].setCell(notes[0]);
                            _hasChanged = true;
                            if (_isOneByOne)
                            {
                                Console.WriteLine("Digit {0} set in Cell[{1}, {2}]", notes[0], r + 1, c + 1);
                                return;
                            }
                        }
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
            int[][] notes = new int[cellArr.Count()][];
            for (int i = 0; i < cellArr.Count(); i++)
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

        private int[][] genSeq(int[] numArr, int desiredLength)
        {
            List<int[]> allCombi = new List<int[]>();

            for (int i = 0; i < numArr.Length - (desiredLength - 1); i++)
            {
                for(int j = i + 1; j < numArr.Length - (desiredLength - 2); j++)
                {
                    if(desiredLength == 3)
                    {
                        for(int k = j + 1; k < numArr.Length; k++)
                        {
                            int[] combi = new int[3];
                            combi[0] = numArr[i];
                            combi[1] = numArr[j];
                            combi[2] = numArr[k];
                            allCombi.Add(combi);
                        }
                    }
                    else
                    {
                        int[] combi = new int[2];
                        combi[0] = numArr[i];
                        combi[1] = numArr[j];
                        allCombi.Add(combi);
                    }
                }
            }

            return allCombi.ToArray();
        }
    }
}
