public partial class Assembler
{
    public static Dictionary<string, int> EncodeLabels(string file)
    {
        Dictionary<string, int> EncodedLabels = new Dictionary<string, int>();
        int program_counter = 0;

        StreamReader sr = new StreamReader(file);
        string? line = "";
        while ((line = sr.ReadLine()) != null)
        {
            // Cleans up lines
            line = ProcessLine(line);
            
            if (line == "")
            {
                continue;
            }

            // Handles Labels
            if (line.EndsWith(':'))
            {
                string label = line.Substring(0, line.Length - 1);
                EncodedLabels[label] = program_counter;
                continue;
            }

            // Handles Instruction
            program_counter += SizeOfInstruction(line);
        }

        return EncodedLabels;
    }

    private static int SizeOfInstruction(string instruction)
    {
        const int INSTRUCTION_SIZE = 4;

        // Handles PseudoInstructions
        if (instruction.Split(' ')[0] != "stpush")
        {
            return INSTRUCTION_SIZE;
        }

        string arg = instruction.Substring(7);
        if (arg[0] != '\"' || arg[^1] != '\"')
        {
            throw new Exception("usage: stpush \"string\"");
        }

        string str = arg
            .Substring(1, arg.Length - 2) // Removes Quotes
            .Replace("\\\\", "\\")
            .Replace("\\n", "\n")
            .Replace("\\\"", "\"");
        
        int total_instructions = (str.Length + 2) / 3; // same as Ceil(str.Length / 3)
        int size_of_instructions = total_instructions * INSTRUCTION_SIZE;

        return size_of_instructions;
    }
}