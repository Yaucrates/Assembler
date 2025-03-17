public partial class Assembler
{
    public static void Assemble(string inputFile, string outputFile)
    {
        Dictionary<string, int> encodedLabels = EncodeLabels(inputFile);
        List<IInstruction> instructions = ParseInstructions(inputFile, encodedLabels);

        if (instructions.Count == 0) {
            throw new Exception("No instructions were parsed from the input file.");
        }

        // Pad to a multiple of 4 instructions with NOPs
        while (instructions.Count % 4 != 0) {
            instructions.Add( new Nop() );
        }

        using (BinaryWriter writer = new BinaryWriter(File.Open(outputFile, FileMode.Create)))
        {
            writer.Write(0xefbeadde); // Magic Header

            // Encode Instructions
            foreach (IInstruction instruction in instructions) {
                int binaryInstruction = instruction.Encode();
                writer.Write(binaryInstruction);
            }
        }
    }

    private static string ProcessLine(string line)
    {
        return RemoveComments(line).Trim();
    }

    private static string ProcessSpecialCharacters(string str)
    {
        return str
            .Substring(1, str.Length - 2) // Removes Quotes
            .Replace("\\\\", "\\")
            .Replace("\\n", "\n")
            .Replace("\\\"", "\"");;
    }

    private static string RemoveComments(string line)
    {
        int index = line.IndexOf('#');

        if (index != -1)
        {
            line = line.Substring(0, index);
        }

        return line;
    }
}