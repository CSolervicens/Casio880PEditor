10 REM Casio FX-880P Test Program
20 PRINT "Hello, World!"
30 INPUT "Enter your name: ", N$
40 PRINT "Welcome, "; N$
50 FOR I = 1 TO 10
60 PRINT I; " squared is "; I * I
70 NEXT I
80 GOSUB 100
90 END
100 REM Subroutine
110 PRINT "This is a subroutine"
120 RETURN
