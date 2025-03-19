using System.Security.Cryptography.X509Certificates;

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
        { "GOTO", (args, labels, pc) =>
            {
                if (args.Count != 1) {
                    throw new Exception("Improper arguments passed to GOTO.");
                }

                string label = args[0];
                int offset = labels[label] - pc;

                return new Goto(offset);
            }
        },
        { "PUSH", (args, labels, _) =>
            {
                if (args.Count == 0) {
                    return new Push();
                }
                else if (args.Count != 1) {
                    throw new Exception("Improper arguments passed to PUSH.");
                }

                string input = args[0];
                int val = StringToDecimal(input) ?? StringToHex(input) ?? labels[input];

                return new Push(val);
            }
        },
        { "STPRINT", (args, _, _) =>
            {
                if (args.Count == 0) {
                    return new Stprint();
                }
                else if (args.Count != 1) {
                    throw new Exception("Improper arguments passed to STPRINT.");
                }

                string input = args[0];
                int? val = StringToDecimal(input) ?? StringToHex(input);

                return new Stprint(val ?? throw new Exception("Improper arguments passed to STPRINT."));
            }
        },
        { "POP", (args, _, _) =>
            {
                if (args.Count == 0) {
                    return new Pop();
                }
                else if (args.Count != 1) {
                    throw new Exception("Improper arguments passed to POP.");
                }

                string input = args[0];
                int? val = StringToDecimal(input) ?? StringToHex(input);

                return new Pop(val ?? throw new Exception("Improper arguments passed to POP."));
            }
        },
        { "INPUT", (args, _, _) =>
            {
                if (args.Count != 0) {
                    throw new Exception("Improper arguments passed to INPUT.");
                }

                return new Input();
            }
        },
        { "UNARYIF", (args, labels, pc) =>
            {
                if (args.Count != 2) {
                    throw new Exception("Improper arguments passed to UNARYIF.");
                }

                int? condition = StringToDecimal(args[0]);
                string offsetStr = args[1];
                int offset = StringToDecimal(offsetStr) ?? StringToHex(offsetStr) ?? labels[offsetStr];

                return new UnaryIf(condition ?? throw new Exception("Bad condition passed to UNARYIF."), offset);
            }
        },
        { "IFEZ", (args, labels, pc) =>
            {
                if (args.Count != 1) {
                    throw new Exception("Improper arguments passed to IFEZ.");
                }

                args.Insert(0, "00");

                var assemblyFunction = INSTRUCTIONS != null ? INSTRUCTIONS["UNARYIF"] : throw new Exception("Assembly instruction UNARYIF does not exist.");
                var assemblyCode = assemblyFunction(args, labels, pc);
                
                return assemblyCode;
            }
        },
        { "IFNZ", (args, labels, pc) =>
            {
                if (args.Count != 1) {
                    throw new Exception("Improper arguments passed to IFNZ.");
                }

                args.Insert(0, "01");

                var assemblyFunction = INSTRUCTIONS != null ? INSTRUCTIONS["UNARYIF"] : throw new Exception("Assembly instruction UNARYIF does not exist.");
                var assemblyCode = assemblyFunction(args, labels, pc);
                
                return assemblyCode;
            }
        },
        { "IFMI", (args, labels, pc) =>
            {
                if (args.Count != 1) {
                    throw new Exception("Improper arguments passed to IFMI.");
                }

                args.Insert(0, "10");

                var assemblyFunction = INSTRUCTIONS != null ? INSTRUCTIONS["UNARYIF"] : throw new Exception("Assembly instruction UNARYIF does not exist.");
                var assemblyCode = assemblyFunction(args, labels, pc);
                
                return assemblyCode;
            }
        },
        { "IFPL", (args, labels, pc) =>
            {
                if (args.Count != 1) {
                    throw new Exception("Improper arguments passed to IFPL.");
                }

                args.Insert(0, "11");

                var assemblyFunction = INSTRUCTIONS != null ? INSTRUCTIONS["UNARYIF"] : throw new Exception("Assembly instruction UNARYIF does not exist.");
                var assemblyCode = assemblyFunction(args, labels, pc);
                
                return assemblyCode;
            }
        },
        { "NEG", (args, labels, pc) =>
            {
                if (args.Count != 0) {
                    throw new Exception("Improper arguments passed to NEG.");
                }

                return new Neg();
            }
        },
        // { "COMMAND", (args, labels, pc) =>
        //     {
        //         if (args.Count == 0) {
        //             return new COMMAND();
        //         }
        //         else if (args.Count != 1) {
        //             throw new Exception("Improper arguments passed to COMMAND.");
        //         }

        //         return new COMMAND;
        //     }
        // },
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
            if (instruction == "STPUSH") {
                var instructionSize = SizeOfInstruction(line);
                var instructionCount = instructionSize / 4;
                var str = args[1];

                int length = str.Length;
                char continueChar = (char) 0x01;
                for (int i = 0; i < instructionCount; i++) {
                    int index = i * 3;

                    char byte1 = (index < length) ? str[index] : continueChar;
                    char byte2 = (index+1 < length) ? str[index+1] : continueChar;
                    char byte3 = (index+2 < length) ? str[index+2] : continueChar;
                    char term = (index+3 < length) ? continueChar : '\0';

                    int val = (term << 24) | (byte3 << 16) | (byte2 << 8) | byte1;

                    instructions.Add(new Push(val));
                }

                programCounter += instructionSize;
                continue;
            }
            else if (!INSTRUCTIONS.ContainsKey(instruction)) {
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
            bool isString = line[0] == '"';
            int startOfArg = isString ? 1 : 0;
            int endOfArg = isString ? GetEndingQuote(line) : line.IndexOf(' ');
            if (endOfArg == -1) {
                args.Add(line);
                break;
            }

            string arg = line.Substring(startOfArg, isString ? endOfArg - 1 : endOfArg);
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

    public static int? StringToDecimal(string input)
    {
        if (decimal.TryParse(input, out decimal result))
        {
            return (int)result;
        }
        return null;
    }

    public static int? StringToHex(string input)
    {
        if (input.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
        {
            input = input.Substring(2); // Remove "0x" prefix
        }

        if (int.TryParse(input, System.Globalization.NumberStyles.HexNumber, null, out int result))
        {
            return result;
        }

        return null;
    }
}