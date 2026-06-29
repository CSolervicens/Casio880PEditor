using System.Text.RegularExpressions;

namespace Casio880PEditor
{
    /// <summary>
    /// Helper class for BASIC line numbering and renumbering operations
    /// </summary>
    public static class BasicLineNumbering
    {
        public const int DEFAULT_INCREMENT = 10;
        public const int DEFAULT_START_LINE = 10;

        /// <summary>
        /// Parse line number from a BASIC line
        /// </summary>
        public static int? GetLineNumber(string line)
        {
            if (string.IsNullOrWhiteSpace(line))
                return null;

            var match = Regex.Match(line.Trim(), @"^(\d+)\s");
            if (match.Success && int.TryParse(match.Groups[1].Value, out int lineNum))
                return lineNum;

            return null;
        }

        /// <summary>
        /// Get the next line number based on current line and increment
        /// </summary>
        public static int GetNextLineNumber(string currentLine, int increment = DEFAULT_INCREMENT)
        {
            var currentNum = GetLineNumber(currentLine);
            if (currentNum.HasValue)
                return currentNum.Value + increment;

            return DEFAULT_START_LINE;
        }

        /// <summary>
        /// Check if a line number can be inserted between two line numbers
        /// </summary>
        public static bool CanInsertBetween(int previousLine, int nextLine, out int suggestedNumber)
        {
            suggestedNumber = (previousLine + nextLine) / 2;
            return suggestedNumber > previousLine && suggestedNumber < nextLine;
        }

        /// <summary>
        /// Extract all GOTO/GOSUB/ELSE line number references from a line
        /// </summary>
        public static List<int> ExtractGotoGosubReferences(string line)
        {
            var references = new List<int>();

            // Match GOTO, GOSUB, and ELSE followed by a line number
            var matches = Regex.Matches(line, @"\b(GOTO|GOSUB|ELSE)\s+(\d+)", RegexOptions.IgnoreCase);

            foreach (Match match in matches)
            {
                if (int.TryParse(match.Groups[2].Value, out int lineNum))
                {
                    references.Add(lineNum);
                }
            }

            // Also match THEN followed by a line number (e.g., IF A>0 THEN 100)
            var thenMatches = Regex.Matches(line, @"\bTHEN\s+(\d+)", RegexOptions.IgnoreCase);
            foreach (Match match in thenMatches)
            {
                if (int.TryParse(match.Groups[1].Value, out int lineNum))
                {
                    references.Add(lineNum);
                }
            }

            return references;
        }

        /// <summary>
        /// Update GOTO/GOSUB/ELSE/THEN references in a line based on line number mapping
        /// </summary>
        public static string UpdateGotoGosubReferences(string line, Dictionary<int, int> lineNumberMap)
        {
            // Update GOTO, GOSUB, and ELSE references
            line = Regex.Replace(line, @"\b(GOTO|GOSUB|ELSE)\s+(\d+)", match =>
            {
                string command = match.Groups[1].Value;
                if (int.TryParse(match.Groups[2].Value, out int oldLineNum))
                {
                    if (lineNumberMap.TryGetValue(oldLineNum, out int newLineNum))
                    {
                        return $"{command} {newLineNum}";
                    }
                }
                return match.Value;
            }, RegexOptions.IgnoreCase);

            // Update THEN references (e.g., IF A>0 THEN 100)
            line = Regex.Replace(line, @"\bTHEN\s+(\d+)", match =>
            {
                if (int.TryParse(match.Groups[1].Value, out int oldLineNum))
                {
                    if (lineNumberMap.TryGetValue(oldLineNum, out int newLineNum))
                    {
                        return $"THEN {newLineNum}";
                    }
                }
                return match.Value;
            }, RegexOptions.IgnoreCase);

            return line;
        }

        /// <summary>
        /// Renumber all lines in a program starting from startLine with given increment
        /// </summary>
        public static string RenumberProgram(string programText, int startLine = DEFAULT_START_LINE, int increment = DEFAULT_INCREMENT)
        {
            var lines = programText.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            var lineNumberMap = new Dictionary<int, int>();
            var result = new List<string>();
            int currentNumber = startLine;

            // First pass: build line number mapping
            foreach (var line in lines)
            {
                var lineNum = GetLineNumber(line);
                if (lineNum.HasValue)
                {
                    lineNumberMap[lineNum.Value] = currentNumber;
                    currentNumber += increment;
                }
            }

            // Second pass: renumber lines and update GOTO/GOSUB references
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    result.Add(line);
                    continue;
                }

                var lineNum = GetLineNumber(line);
                if (lineNum.HasValue)
                {
                    // Remove old line number
                    var contentWithoutNumber = Regex.Replace(line.Trim(), @"^\d+\s*", "");

                    // Update GOTO/GOSUB references
                    contentWithoutNumber = UpdateGotoGosubReferences(contentWithoutNumber, lineNumberMap);

                    // Add new line number
                    var newLineNum = lineNumberMap[lineNum.Value];
                    result.Add($"{newLineNum} {contentWithoutNumber}");
                }
                else
                {
                    result.Add(line);
                }
            }

            return string.Join(Environment.NewLine, result);
        }

        /// <summary>
        /// Smart renumber: only renumber if needed to make space
        /// </summary>
        public static string SmartRenumber(string programText, int insertAfterLine, int increment = DEFAULT_INCREMENT)
        {
            var lines = programText.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).ToList();
            var lineNumbers = new List<(int index, int lineNumber)>();

            // Find all line numbers
            for (int i = 0; i < lines.Count; i++)
            {
                var lineNum = GetLineNumber(lines[i]);
                if (lineNum.HasValue)
                {
                    lineNumbers.Add((i, lineNum.Value));
                }
            }

            // Find the position where we need to insert
            int insertIndex = -1;
            int? previousLine = null;
            int? nextLine = null;

            for (int i = 0; i < lineNumbers.Count; i++)
            {
                if (lineNumbers[i].lineNumber == insertAfterLine)
                {
                    previousLine = lineNumbers[i].lineNumber;
                    if (i + 1 < lineNumbers.Count)
                    {
                        nextLine = lineNumbers[i + 1].lineNumber;
                        insertIndex = i + 1;
                    }
                    break;
                }
            }

            // Check if we can insert without renumbering
            if (previousLine.HasValue && nextLine.HasValue)
            {
                if (CanInsertBetween(previousLine.Value, nextLine.Value, out _))
                {
                    // No need to renumber, there's space
                    return programText;
                }

                // Need to renumber from this point
                return RenumberProgram(programText, DEFAULT_START_LINE, increment);
            }

            return programText;
        }

        /// <summary>
        /// Get the last line number in the program
        /// </summary>
        public static int? GetLastLineNumber(string programText)
        {
            var lines = programText.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

            for (int i = lines.Length - 1; i >= 0; i--)
            {
                var lineNum = GetLineNumber(lines[i]);
                if (lineNum.HasValue)
                    return lineNum.Value;
            }

            return null;
        }

        /// <summary>
        /// Format a line with proper line number
        /// </summary>
        public static string FormatLineWithNumber(int lineNumber, string content)
        {
            // Remove any existing line number
            content = Regex.Replace(content.Trim(), @"^\d+\s*", "");
            return $"{lineNumber} {content}";
        }
    }
}
