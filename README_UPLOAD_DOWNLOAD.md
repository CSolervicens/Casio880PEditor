# Casio FX-880P Program Editor - Upload/Download Guide

## Upload Termination Fix

The calculator was waiting indefinitely after upload because it didn't receive an end-of-transmission signal. This has been fixed!

### What Changed

1. **EOF Character Support**: The editor now sends an End-of-File (EOF) character after uploading programs to signal completion.

2. **Configurable EOF Settings**: You can choose the termination character via **COM → EOF Settings**:
   - **Ctrl+Z (0x1A)** - Default, standard DOS EOF
   - **Ctrl+D (0x04)** - Unix/Linux EOF alternative
   - **None** - No EOF character (if your calculator doesn't need it)

### How to Upload Programs

1. **Connect** to the calculator via COM port
2. **Prepare** your Casio FX-880P:
   ```
   On calculator: LOAD "COM:0"
   ```
3. Click **Upload** (or press F5) in the editor
4. The editor will:
   - Send your program line by line
   - Send the EOF character (Ctrl+Z by default)
   - Display "Successfully sent" when complete

### How to Download Programs

1. **Connect** to the calculator via COM port
2. Click **Download** (or press F6)
3. A message will remind you to execute on your calculator:
   ```
   SAVE "COM:0,5,N,8,1"
   ```
4. Execute the SAVE command on your calculator
5. The program will be received in the editor

### Troubleshooting Upload Issues

If the calculator still waits after upload:

1. **Try Ctrl+D instead**:
   - Go to **COM → EOF Settings → Ctrl+D (0x04)**
   - Upload again

2. **Try None**:
   - Go to **COM → EOF Settings → None**
   - The calculator may expect no EOF character

3. **Manual termination**:
   - If using "None", you may need to press Ctrl+C or ESC on the calculator after upload

### Communication Settings

- **Baud Rate**: 2400
- **Parity**: None
- **Data Bits**: 8
- **Stop Bits**: 1
- **Descriptor**: 5,N,8,1,N,N,N,N

### Features

- ✅ Extra newlines are automatically removed from downloaded programs
- ✅ Empty lines at end of file are skipped during upload
- ✅ Async operations with progress reporting
- ✅ Monaco Editor with Casio BASIC syntax highlighting

---

**Note**: The exact protocol may vary depending on your Casio FX-880P firmware version. If you experience issues, try different EOF settings.
