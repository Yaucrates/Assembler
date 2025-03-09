public class Program
{
    static void Main(string[] args)
    {
        if (args.Length != 2)
        {
            Console.WriteLine("usage: dotnet run [input.asm] [output.v]");
            return;
        }

        string input_file = args[0];
        string output_file = args[1];

        try
        {
            Assembler.Assemble(input_file, output_file);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to Assemble {input_file}: {ex.Message}");
        }
    }
}