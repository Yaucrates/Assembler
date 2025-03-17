public partial class Assembler
{
    private static readonly Dictionary<string, Func<List<string>, Dictionary<string, int>, int, IInstruction>> INSTRUCTIONS =
    new Dictionary<string, Func<List<string>, Dictionary<string, int>, int, IInstruction>>()
    {
        { "NOP", (args, _, _) =>
            {
                if (args.Count != 0) {
                    throw new Exception("Improper arguments passed to NOP.");
                }

                return new Nop();
            }
        },
        { "GOTO", (args, dict, pc) =>
            {
                if (args.Count != 1) {
                    throw new Exception("Improper arguments passed to GOTO.");
                }

                string label = args[0];
                int offset = dict[label] - pc;

                return new Goto(offset);
            }
        },
    };


    private static List<IInstruction> ParseInstructions(string file, Dictionary<string, int> encodedLabels) {
        List<IInstruction> instructions = new List<IInstruction>();
        int programCounter = 0;

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
            var assemblyCode = assemblyFunction(args, encodedLabels, programCounter);
            instructions.Add(assemblyCode);

            programCounter += SizeOfInstruction(line);
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