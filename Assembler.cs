using System.IO;

public partial class Assembler
{
    public static void Assemble(string input_file, string output_file)
    {
        Dictionary<string, int> EncodedLabels = EncodeLabels(input_file);
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