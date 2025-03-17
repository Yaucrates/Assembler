public partial class Assembler
{
    public static void Assemble(string inputFile, string outputFile)
    {
        Dictionary<string, int> encodedLabels = EncodeLabels(inputFile);
        List<IInstruction> instructions = ParseInstructions(inputFile, encodedLabels);

        if (instructions.Count == 0) {
            throw new Exception("No instructions were parsed from the input file.");
        }

        using (BinaryWriter writer = new BinaryWriter(File.Open(outputFile, FileMode.Create)))
        {
            writer.Write(0xefbeadde); // Magic Header

            // Encode Instructions
            foreach (IInstruction instruction in instructions) {
                int binaryInstruction = instruction.Encode();
                writer.Write(binaryInstruction);
            }

            // Pad with NOPs
        }
    }

    private static string ProcessLine(string line)
    {
        return RemoveComments(line).Trim();
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