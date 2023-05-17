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
            FileSystemVisitor fileSystemVisitor;

            // Filter by file extension or folder name
            Console.Write("Enter file or folder name or file extension to filter (To skip hit enter.):");
            var filter = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(filter))
            {
                var filterExpression = (string f) => f.Contains(filter);
                fileSystemVisitor = new FileSystemVisitor(text, filterExpression);
            }
            else
            {
                fileSystemVisitor = new FileSystemVisitor(text);
            }

            // Register Notify method to the Start and Finish events
            fileSystemVisitor.Start += (sender, args) => Notify(sender, args, "Process Started.");
            fileSystemVisitor.Finish += (sender, args) => Notify(sender, args, "Process Finished.");

            // Abort search and Exclude files or folders
            Console.WriteLine("Exclude files or folders if their name contains (To skip hit enter.): ");
            var excludeName = Console.ReadLine();

            Console.WriteLine("Press ESC to abort search.");
            fileSystemVisitor.FileFound += (sender, eventArgs) =>
            {
                // Abort search when a file found.
                eventArgs.Abort = Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape;
                if (eventArgs.Abort)
                {
                    Console.WriteLine("Search aborted.1");
                    return;
                }

                // Exclude files which name contains given name in general searches.
                if (!string.IsNullOrWhiteSpace(excludeName))
                {
                    eventArgs.Exclude = eventArgs.Path.Contains(excludeName);
                }
            };

            fileSystemVisitor.DirectoryFound += (sender, eventArgs) =>
            {
                // Abort search when a folder found.
                eventArgs.Abort = Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape;
                if (eventArgs.Abort)
                {
                    Console.WriteLine("Search aborted.2");
                    return;
                }

                // Exclude folders which name contains given name in general searches.
                if (!string.IsNullOrWhiteSpace(excludeName))
                {
                    eventArgs.Exclude = eventArgs.Path.Contains(excludeName);
                }
            };

            fileSystemVisitor.FilteredFileFound += (sender, eventArgs) =>
            {
                // Abort search when filtered file found.
                eventArgs.Abort = Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape;
                if (eventArgs.Abort)
                {
                    Console.WriteLine("Search aborted.3");
                    return;
                }

                // Exclude files which name contains given name in general searches.
                if (!string.IsNullOrWhiteSpace(excludeName))
                {
                    eventArgs.Exclude = eventArgs.Path.Contains(excludeName);
                }
            };

            fileSystemVisitor.FilteredDirectoryFound += (sender, eventArgs) =>
            {
                // Abort search when filtered folder found.
                eventArgs.Abort = Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape;
                if (eventArgs.Abort)
                {
                    Console.WriteLine("Search aborted.4");
                    return;
                }

                // Exclude files which name contains given name in general searches.
                if (!string.IsNullOrWhiteSpace(excludeName))
                {
                    eventArgs.Exclude = eventArgs.Path.Contains(excludeName);
                }
            };

            try
            {
                foreach (var item in fileSystemVisitor.Traverse())
                {
                    Console.WriteLine(item);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
            }
        }
        while (text != "exit");
    }

    private static void Notify(object? sender, EventArgs args, string message)
    {
        Console.WriteLine($"Notification: {message}");
    }
}