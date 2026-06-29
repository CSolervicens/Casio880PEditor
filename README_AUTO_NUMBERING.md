# Auto Line Numbering and Renumbering Features

## Overview
The Casio FX-880P Program Editor now includes powerful auto-numbering and renumbering features to help you write BASIC programs more efficiently.

## Features

### 1. Auto Line Numbering
Automatically adds line numbers when you press Enter, incrementing by 10 (default).

#### How to Use:
1. Go to **Edit → Auto Line Numbering** (or press `Ctrl+L`)
2. Type a line with a line number, e.g., `10 PRINT "HELLO"`
3. Press **Enter**
4. The editor automatically inserts the next line number: `20 `
5. Continue typing your code

#### Example:
```basic
10 PRINT "HELLO"    [Press Enter]
20 _                [Cursor ready for input]
```

#### To Disable:
- Uncheck **Edit → Auto Line Numbering** (or press `Ctrl+L` again)

---

### 2. Renumber Program
Renumbers all lines in your program in multiples of 10 and automatically updates all GOTO/GOSUB references.

#### How to Use:
1. Go to **Edit → Renumber Program** (or press `Ctrl+R`)
2. Confirm the operation
3. All lines are renumbered: 10, 20, 30, 40...
4. GOTO/GOSUB references are automatically updated

#### Before Renumbering:
```basic
5 PRINT "START"
7 GOTO 15
15 PRINT "MIDDLE"
18 GOSUB 25
20 END
25 PRINT "SUBROUTINE"
27 RETURN
```

#### After Renumbering:
```basic
10 PRINT "START"
20 GOTO 30
30 PRINT "MIDDLE"
40 GOSUB 60
50 END
60 PRINT "SUBROUTINE"
70 RETURN
```

Notice how `GOTO 15` became `GOTO 30` and `GOSUB 25` became `GOSUB 60` automatically!

---

## Smart Features

### GOTO/GOSUB Reference Tracking
The renumbering engine understands BASIC control flow:
- ✅ **GOTO** statements are updated
- ✅ **GOSUB** statements are updated
- ✅ **Case-insensitive** matching (GOTO, goto, GoTo all work)
- ✅ Preserves program logic

### Safe Renumbering
- Confirms before making changes
- Updates all references in one pass
- Maintains relative spacing between line numbers

---

## Keyboard Shortcuts

| Action | Shortcut |
|--------|----------|
| Toggle Auto Numbering | `Ctrl+L` |
| Renumber Program | `Ctrl+R` |

---

## Use Cases

### 1. Writing New Programs
Enable auto-numbering and let the editor handle line numbers automatically.

### 2. Inserting Lines
After inserting lines manually, use **Renumber** to clean up the sequence.

### 3. Reorganizing Code
Move code blocks around, then use **Renumber** to fix all line references.

### 4. Import from Calculator
Download a program from the calculator and use **Renumber** to standardize line numbering.

---

## Technical Details

### Line Number Increment
- Default increment: **10**
- Start line: **10**
- Renumbering sequence: 10, 20, 30, 40, 50...

### Supported Statements
The renumbering engine recognizes:
- `GOTO line_number`
- `GOSUB line_number`

### Pattern Matching
Uses regular expressions to find and update references:
```regex
\b(GOTO|GOSUB)\s+(\d+)
```

### Line Number Extraction
Identifies BASIC line numbers at the start of each line:
```regex
^(\d+)\s
```

---

## Tips

### 💡 Best Practices
1. **Use auto-numbering** when writing new code
2. **Renumber before uploading** to the calculator for clean, organized code
3. **Enable auto-numbering early** to avoid manual line number management
4. **Use increments of 10** to leave room for future insertions

### ⚠️ Important Notes
- Renumbering cannot be undone (use `Ctrl+Z` if needed)
- Always save your work before renumbering
- Test uploaded programs on the calculator after renumbering

---

## Implementation

### Classes
- **BasicLineNumbering.cs**: Core numbering and renumbering logic
- **MonacoEditor.cs**: JavaScript integration for auto-numbering
- **Form1.cs**: UI event handlers and menu integration

### Methods
- `GetLineNumber()`: Extract line number from text
- `GetNextLineNumber()`: Calculate next line number
- `ExtractGotoGosubReferences()`: Find all GOTO/GOSUB references
- `UpdateGotoGosubReferences()`: Update references with new line numbers
- `RenumberProgram()`: Full program renumbering with reference updates
- `SetAutoNumberingAsync()`: Enable/disable auto-numbering in editor

---

## Examples

### Example 1: Simple Loop
```basic
10 FOR I=1 TO 10
20 PRINT I
30 NEXT I
40 END
```

Press Enter at line 40, auto-numbering adds:
```basic
50 _
```

### Example 2: With GOTO
```basic
10 INPUT A
20 IF A>0 THEN GOTO 40
30 GOTO 10
40 PRINT "POSITIVE"
50 END
```

After renumbering (starting from 100):
```basic
100 INPUT A
110 IF A>0 THEN GOTO 130
120 GOTO 100
130 PRINT "POSITIVE"
140 END
```

All GOTO references automatically updated!

---

## Status Messages

| Message | Meaning |
|---------|---------|
| "Auto line numbering enabled (increment: 10)" | Auto-numbering is active |
| "Auto line numbering disabled" | Auto-numbering is off |
| "Renumbering program..." | Renumbering in progress |
| "Program renumbered successfully" | Renumbering complete |
| "Renumber failed" | Error during renumbering |

---

**Build Status**: ✅ Successful  
**Features**: ✅ Fully Implemented  
**Testing**: Ready for use!
