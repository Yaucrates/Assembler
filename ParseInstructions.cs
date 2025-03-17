public partial class Assembler
{
    private static readonly Dictionary<string, Func<List<string>, IInstruction>> INSTRUCTIONS = new Dictionary<string, Func<List<string>, IInstruction>>()
    {
        { "NOP", args =>
            {
                return new Nop();
            }
        }
    };

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

            // Create each instruction
            List<string> args = SplitInstructions(line);
            string instruction = args[0].ToUpper();
            if (!INSTRUCTIONS.ContainsKey(instruction)) {
                throw new Exception($"Assembly instruction \"{args[0]}\" does not exist.");
            }
            args.RemoveAt(0);

            var assemblyFunction = INSTRUCTIONS[instruction];
            var assemblyCode = assemblyFunction(args);
            instructions.Add(assemblyCode);
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

            string arg = line.Substring(0, endOfArg);
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