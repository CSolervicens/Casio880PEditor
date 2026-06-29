# Casio FX-880P ST Error Fix

## Problem
When uploading programs to the Casio FX-880P calculator, it returned "ST Error" (Syntax Error).

## Root Cause
According to the Casio FX-880P manual, each line transmitted via COM port requires an **extra newline** after it. The original implementation was sending only a single carriage return (`\r`) per line.

## Solution Applied

### 1. **Double Carriage Return**
Updated `SendProgramAsync()` in `CasioSerialPort.cs` to send each line with double CR:

```csharp
// Before:
_serialPort.Write(line + "\r");

// After (according to manual):
_serialPort.Write(line + "\r\r");
```

### 2. **Program Format Validation**
Added `ValidateProgramFormat()` method that checks if the program has proper line numbers:

```csharp
public static bool ValidateProgramFormat(string programText, out string errorMessage)
```

This validates that each line starts with a line number (e.g., `10`, `20`, `30`), which is required for Casio BASIC.

### 3. **Upload Instructions**
Updated the upload dialog to show proper instructions:

```
Before uploading, execute this on your calculator:

LOAD "COM:0"

Make sure the calculator is waiting for data before clicking OK.

Note: Your BASIC program lines should have line numbers (e.g., 10, 20, 30...)
```

## How to Use

### On the Calculator:
1. Execute: `LOAD "COM:0"`
2. Wait for the calculator to display "Waiting..."

### In the Editor:
1. Write your program with line numbers:
   ```basic
   10 PRINT "HELLO"
   20 FOR I=1 TO 10
   30 PRINT I
   40 NEXT I
   50 END
   ```

2. Connect to COM port
3. Click **Upload** (or press F5)
4. Click OK on the instruction dialog

### Expected Behavior:
- Each line is sent with double CR (`\r\r`)
- 200ms delay between lines for calculator processing
- Optional EOF character (Ctrl+Z by default) at the end
- Success message when complete

## Casio FX-880P COM Port Format

According to the manual, when using `LOAD "COM:0"`:
- **Each line requires double newline** ✅ (now fixed)
- Programs must have line numbers (validated)
- Format: `[line number] [BASIC statement]\r\r`
- Baud rate: 2400 (default)
- Settings: N, 8, 1

## Testing
Build successful ✅

The ST Error should now be resolved. If you still encounter issues:
1. Try different EOF settings (COM → EOF Settings)
2. Increase delay between lines (currently 200ms)
3. Verify line numbers are sequential
4. Make sure calculator is in "Waiting" state before upload
