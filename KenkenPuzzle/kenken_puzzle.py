import puzzle_cell

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
        for i in range(2, len(keywords), 2):
            row = int(keywords[i])-1
            col = int(keywords[i+1])-1
            cells.append(self.puzzle[row][col])
        line = {'operator': operator, 'total': total, 'cells': cells}
        self.lines.append(line)
    
    def print_puzzle(self):
        for r in range(self.size):
            for c in range(self.size):
                print(self.puzzle[r][c].number, end=',')
            print()
        for line in self.lines:
            print(line)
        print()