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
            FileSystemVisitor fileSystemVisitor = new FileSystemVisitor(text, f => f.EndsWith(".docx"));
            var items = fileSystemVisitor.Traverse();

            if (!items.Any())
            {
                Console.WriteLine("No file or folder found.");
                continue;
            }

            foreach (var item in items)
            {
                Console.WriteLine(item);
            }
        }
        while (text != "exit");
    }
}