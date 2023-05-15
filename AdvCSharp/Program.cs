// <copyright file="Program.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

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
                Console.WriteLine("Please enter valid root path (Should start with 'C:' ).");
                continue;
            }

            // Work
            var filterExpression = (string f) => f.EndsWith(".docx");
            FileSystemVisitor fileSystemVisitor = new FileSystemVisitor(text, filterExpression);

            // Assign Notify method to the Start and Finish events
            fileSystemVisitor.Start += (sender, e) => Notify(sender, e, "Process Started.");
            fileSystemVisitor.Finish += (sender, e) => Notify(sender, e, "Process Finished.");

            foreach (var item in fileSystemVisitor.Traverse())
            {
                Console.WriteLine(item);
            }
        }
        while (text != "exit");
    }

    private static void Notify(object? sender, EventArgs e, string message)
    {
        Console.WriteLine($"{sender}, {message}");
    }
}