using NUnit.Framework;
using AdvCSharp;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace AdvCSharp.Tests
{
    [TestFixture]
    public class FileSystemVisitorTests
    {
        private string rootFolderPath;

        [SetUp]
        public void SetUp()
        {
            // Create a temporary root folder for testing
            rootFolderPath = Path.Combine(Path.GetTempPath(), "FileSystemVisitorTest");
            Directory.CreateDirectory(rootFolderPath);

            // Create some test files and folders
            var folder1 = Path.Combine(rootFolderPath, "Folder1");
            var folder2 = Path.Combine(rootFolderPath, "Folder2");
            var file1 = Path.Combine(rootFolderPath, "file1.txt");
            var file2 = Path.Combine(folder1, "file2.txt");
            var file3 = Path.Combine(folder1, "file3.docx");
            var file4 = Path.Combine(folder2, "file4.txt");

            Directory.CreateDirectory(folder1);
            Directory.CreateDirectory(folder2);
            File.WriteAllText(file1, "Test file 1");
            File.WriteAllText(file2, "Test file 2");
            File.WriteAllText(file3, "Test file 3");
            File.WriteAllText(file4, "Test file 4");
        }

        [TearDown]
        public void TearDown()
        {
            // Delete the temporary root folder and its contents
            Directory.Delete(rootFolderPath, true);
        }

        [Test]
        public void Traverse_ShouldReturnAllFilesAndFolders_WhenNoFilterProvided()
        {
            // Arrange
            var fileSystemVisitor = new FileSystemVisitor(rootFolderPath);

            // Act
            var items = fileSystemVisitor.Traverse().ToList();

            // Assert
            var expectedItems = new List<string>
            {
                Path.Combine(rootFolderPath, "Folder1"),
                Path.Combine(rootFolderPath, "Folder2"),
                Path.Combine(rootFolderPath, "file1.txt"),
                Path.Combine(rootFolderPath, "Folder1", "file2.txt"),
                Path.Combine(rootFolderPath, "Folder1", "file3.docx"),
                Path.Combine(rootFolderPath, "Folder2", "file4.txt")
            };
            CollectionAssert.AreEquivalent(expectedItems, items);
        }

        [Test]
        public void Traverse_ShouldReturnFilteredFiles_WhenFilterByExtension()
        {
            // Arrange
            var filterExpression = (string f) => f.EndsWith(".txt");
            var fileSystemVisitor = new FileSystemVisitor(rootFolderPath, filterExpression);

            // Act
            var items = fileSystemVisitor.Traverse().ToList();

            // Assert
            var expectedItems = new List<string>
            {
                Path.Combine(rootFolderPath, "file1.txt"),
                Path.Combine(rootFolderPath, "Folder1", "file2.txt"),
                Path.Combine(rootFolderPath, "Folder2", "file4.txt")
            };
            CollectionAssert.AreEquivalent(expectedItems, items);
        }

        [Test]
        public void Traverse_ShouldReturnFilteredFolders_WhenFilterByName()
        {
            // Arrange
            var fileSystemVisitor = new FileSystemVisitor(rootFolderPath, f => Path.GetFileName(f).StartsWith("Folder"));

            // Act
            var items = fileSystemVisitor.Traverse().ToList();

            // Assert
            var expectedItems = new List<string>
            {
                Path.Combine(rootFolderPath, "Folder1"),
                Path.Combine(rootFolderPath, "Folder2")
            };
            CollectionAssert.AreEquivalent(expectedItems, items);
        }

        [Test]
        public void Traverse_ShouldAbort_WhenAbortFlagIsSet()
        {
            // Arrange
            var fileSystemVisitor = new FileSystemVisitor(rootFolderPath);

            // Subscribe to the FileFound event and set the Abort flag
            fileSystemVisitor.FileFound += (sender, args) =>
            {
                args.Abort = true;
            };

            // Subscribe to the DirectoryFound event and set the Abort flag
            fileSystemVisitor.DirectoryFound += (sender, args) =>
            {
                args.Abort = true;
            };

            // Act
            var items = fileSystemVisitor.Traverse().ToList();

            // Assert
            Assert.IsEmpty(items);
        }

        [Test]
        public void Traverse_ShouldExcludeFiles_WhenExcludeFlagIsSet()
        {
            // Arrange
            var fileSystemVisitor = new FileSystemVisitor(rootFolderPath);
            var excludeName = "file3.docx";

            // Subscribe to the FileFound event and set the Exclude flag
            fileSystemVisitor.FileFound += (sender, args) =>
            {
                args.Exclude = args.Path.Contains(excludeName);
            };

            // Act
            var items = fileSystemVisitor.Traverse().ToList();

            // Assert
            var expectedItems = new List<string>
            {
                Path.Combine(rootFolderPath, "file1.txt"),
                Path.Combine(rootFolderPath, "Folder1"),
                Path.Combine(rootFolderPath, "Folder1", "file2.txt"),
                Path.Combine(rootFolderPath, "Folder2"),
                Path.Combine(rootFolderPath, "Folder2", "file4.txt")
            };
            CollectionAssert.AreEquivalent(expectedItems, items);
        }
    }
}