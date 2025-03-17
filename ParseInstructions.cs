public partial class Assembler
{
    private static List<IInstruction> ParseInstructions(string input_file, Dictionary<string, int> encodedLabels) {
        List<IInstruction> instructions = new List<IInstruction>();

        instructions.Add( new Exit() );

        return instructions;
    }
}