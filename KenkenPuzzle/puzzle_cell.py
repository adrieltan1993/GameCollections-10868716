class PuzzleCell:
    def __init__ (self):
        self.number = 0
        self.notes = []

    def get_notes(self):
        return self.notes

    def set_note(self, note):
        if(note not in self.notes):
            self.notes.append(note)

    def unset_note(self, note):
        if(note in self.notes):
            self.notes.remove(note)
    
    def clear_notes(self):
        self.notes = []
    
    def set_number(self, num):
        self.number = num

    def clear_number(self):
        self.number = 0