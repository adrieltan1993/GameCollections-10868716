import puzzle_cell
import time

class KenKenPuzzle:
    def __init__(self, size):
        self.size = size
        self.puzzle = []
        self.lines = []
        self.rows = []
        self.columns = []
        for r in range(self.size):
            row = []
            for c in range(self.size):
                cell = puzzle_cell.PuzzleCell()
                if(r == 0):
                    self.columns.append([cell])
                else:
                    self.columns[c].append(cell)
                row.append(cell)
            self.rows.append(row)
            self.puzzle.append(row)

    def add_line(self, line_str):
        keywords = line_str.strip().split(',')
        operator = keywords[0]
        total = keywords[1]
        cells = []
        positions = []
        for i in range(2, len(keywords), 2):
            row = int(keywords[i])-1
            col = int(keywords[i+1])-1
            positions.append([row, col])
            cells.append(self.puzzle[row][col])
        if (len(cells) == 1):
            cells[0].set_number(int(total))
        else:
            for i in range(len(cells)):
                self.set_cell_notes(int(total), operator, cells, positions, i, i, 0, [])
        line = {'operator': operator, 'total': total, 'cells': cells}
        self.lines.append(line)
    
    def set_cell_notes(self, total, operator, cells, positions, start_idx, cell_idx, curr_total, notes):
        if(cell_idx == start_idx):
            if(curr_total > 0):
                if(curr_total == total):
                    for i in range(len(cells)):
                        cells[(start_idx+i)%len(cells)].set_note(notes[i])
                return
        cell_idx = (cell_idx+1) % len(cells)
        for note in range(1, self.size+1):
            if(note in notes):
                curr_row = positions[cell_idx][0]
                curr_col = positions[cell_idx][1]
                other_idx = (start_idx+notes.index(note))% len(cells)
                other_row = positions[other_idx][0]
                other_col = positions[other_idx][1]
                if(curr_row == other_row or curr_col == other_col):
                    continue
            # print("notes: {}, curr_total: {}, note: {}".format(notes, curr_total, note))
            new_total = curr_total + note
            new_notes = notes.copy()
            new_notes.append(note)
            self.set_cell_notes(total, operator, cells, positions, start_idx, cell_idx, new_total, new_notes)
        return
    
    def print_puzzle(self):
        for r in range(self.size):
            for c in range(self.size):
                print(self.puzzle[r][c].number, end=',')
            print()
        for line in self.lines:
            print(line)
        print()

    def solve(self):
        while (not self.is_solved()):
            self.print_puzzle()
            self.find_naked_singles()
            time.sleep(0.2)
    
    def is_solved(self):
        for r in range(self.size):
            for c in range(self.size):
                if self.puzzle[r][c].number == 0:
                    return False
        for row in self.rows:
            check_list = []
            for cell in row:
                if cell.number in check_list:
                    return False
                else:
                    check_list.append(cell.number)
        for column in self.columns:
            check_list = []
            for cell in column:
                if cell.number in check_list:
                    return False
                else:
                    check_list.append(cell.number)
        return True

    def find_naked_singles(self):
        for row in self.rows:
            self.find_naked_singles_in_line(row)
        for column in self.columns:
            self.find_naked_singles_in_line(column)

    def find_naked_singles_in_line(self, line):
        for cell in line:
            if (cell.number != 0):
                for other_cell in line:
                    other_cell.unset_note(cell.number)
        for cell in line:
            if(cell.number == 0):
                if (len(cell.notes) == 1):
                    cell.set_number(cell.notes[0])
        for cell in line:
            if (cell.number != 0):
                for other_cell in line:
                    other_cell.unset_note(cell.number)
