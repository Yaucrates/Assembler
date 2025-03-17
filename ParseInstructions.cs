public partial class Assembler
{
    private static List<IInstruction> ParseInstructions(string file, Dictionary<string, int> encodedLabels) {
        List<IInstruction> instructions = new List<IInstruction>();

        StreamReader sr = new StreamReader(file);
        string? line;
        while ((line = sr.ReadLine()) != null)
        {
            // Skips Non-Instructions
            line = ProcessLine(line);
            if (line == "" || line.EndsWith(':'))
            {
                continue;
            }

            List<string> args = SplitInstructions(line);
            foreach (string arg in args) {
                Console.WriteLine(arg);
            }
            Console.WriteLine();

            // Console.WriteLine(line);
        }

        return instructions;
    }

    private static List<string> SplitInstructions(string line)
    {
        List<string> args = new List<string>();

        while (line != "")
        {
            int endOfArg = line[0] == '"' ? GetEndingQuote(line) : line.IndexOf(' ');
            if (endOfArg == -1) {
                args.Add(line);
                break;
            }

            string arg = line.Substring(0, endOfArg + 1);
            args.Add(arg);

            line = ProcessLine( line.Substring(endOfArg + 1) );
        }

        return args;
    }

    private static int GetEndingQuote(string str)
    {
        for (int i = 1; i < str.Length; i++)
        {
            if (str[i] == '"' && str[i-1] != '\\')
            {
                return i;
            }
        }

        throw new Exception("String not closed.");
    }
}