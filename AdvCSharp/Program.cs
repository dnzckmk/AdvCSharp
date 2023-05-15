using AdvCSharp;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("To close the app type 'exit'.");
        string? text = null;
        do
        {
            Console.WriteLine("Enter root folder path: ");
            text = Console.ReadLine();

            if (text == "exit")
            {
                Environment.Exit(0);
            }
            else if (string.IsNullOrEmpty(text) || !text.StartsWith("C:"))
            {
                Console.WriteLine("Please enter valid root path.");
                continue;
            }

            // Work
            FileSystemVisitor fileSystemVisitor = new FileSystemVisitor(text);
            var content = fileSystemVisitor.Traverse();

            foreach (var c in content)
            {
                Console.WriteLine(c);
            }
        }
        while (string.IsNullOrWhiteSpace(text) || !text.StartsWith("C:"));
    }
}