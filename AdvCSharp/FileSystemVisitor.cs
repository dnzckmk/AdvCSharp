// <copyright file="FileSystemVisitor.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AdvCSharp
{
    public class FileSystemVisitor
    {
        /// <summary>
        /// Root folder path.
        /// </summary>
        private readonly string rootFolder;

        /// <summary>
        /// Delegate for filtering.
        /// </summary>
        private readonly Func<string, bool>? filter;

        /// <summary>
        /// If search aborted by the user it will be true.
        /// </summary>
        private bool isSearchAborted;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileSystemVisitor"/> class.
        /// </summary>
        /// <param name="rootFolder">Root folder path.</param>
        public FileSystemVisitor(string rootFolder)
        {
            this.rootFolder = rootFolder ?? throw new ArgumentNullException(nameof(rootFolder));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileSystemVisitor"/> class with filter.
        /// </summary>
        /// <param name="rootFolder">Root folder path.</param>
        /// <param name="filter">Lambda expression for filtering.</param>
        public FileSystemVisitor(string rootFolder, Func<string, bool> filter)
            : this(rootFolder)
        {
            this.filter = filter;
        }

        /// <summary>
        /// Start event.
        /// </summary>
        public event EventHandler? Start;

        /// <summary>
        /// Finish event.
        /// </summary>
        public event EventHandler? Finish;

        /// <summary>
        /// File found event.
        /// </summary>
        public event EventHandler<FileSystemVisitorEventArgs>? FileFound;

        /// <summary>
        /// Directory found event.
        /// </summary>
        public event EventHandler<FileSystemVisitorEventArgs>? DirectoryFound;

        /// <summary>
        /// Filtered file found event.
        /// </summary>
        public event EventHandler<FileSystemVisitorEventArgs>? FilteredFileFound;

        /// <summary>
        /// Filtered directory found event.
        /// </summary>
        public event EventHandler<FileSystemVisitorEventArgs>? FilteredDirectoryFound;

        /// <summary>
        /// Traverse through folders and files of the root folder.
        /// Triggers Start and Finish events.
        /// </summary>
        /// <returns>IEnumerable string of folders and files in linear sequence.</returns>
        public IEnumerable<string> Traverse()
        {
            this.OnStart();

            foreach (var item in this.Traverse(this.rootFolder))
            {
                yield return item;
            }

            this.OnFinish();
        }

        /// <summary>
        /// Trigger Start event.
        /// </summary>
        protected virtual void OnStart()
        {
            this.Start?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Trigger Finish event.
        /// </summary>
        protected virtual void OnFinish()
        {
            this.Finish?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Trigger file found event.
        /// </summary>
        /// <param name="args">Event args.</param>
        protected virtual void OnFileFound(FileSystemVisitorEventArgs args)
        {
            this.FileFound?.Invoke(this, args);
        }

        /// <summary>
        /// Trigger directory found event.
        /// </summary>
        /// <param name="args">Event args.</param>
        protected virtual void OnDirectoryFound(FileSystemVisitorEventArgs args)
        {
            this.DirectoryFound?.Invoke(this, args);
        }

        /// <summary>
        /// Trigger filtered file found event.
        /// </summary>
        /// <param name="args">Event args.</param>
        protected virtual void OnFilteredFileFound(FileSystemVisitorEventArgs args)
        {
            this.FilteredFileFound?.Invoke(this, args);
        }

        /// <summary>
        /// Trigger filtered directory found event.
        /// </summary>
        /// <param name="args">Event args.</param>
        protected virtual void OnFilteredDirectoryFound(FileSystemVisitorEventArgs args)
        {
            this.FilteredDirectoryFound?.Invoke(this, args);
        }

        /// <summary>
        /// Traverse through files and folders in the given path. If any filter provided it applies.
        /// </summary>
        /// <param name="path">Path of the file or folder.</param>
        /// <returns>IEnumerable file and folder paths.</returns>
        private IEnumerable<string> Traverse(string path)
        {
            // Iterate and return each file in the current folder path
            FileSystemVisitorEventArgs args;
            foreach (var file in Directory.GetFiles(path))
            {
                if (this.isSearchAborted)
                {
                    yield break;
                }

                Thread.Sleep(1000);
                args = new FileSystemVisitorEventArgs(file);

                // Trigger FileFound delegate
                this.OnFileFound(args);

                if (args.Abort)
                {
                    this.isSearchAborted = true;
                    Console.WriteLine("1");
                    yield break;
                }

                if (!args.Exclude)
                {
                    if (this.filter != null && this.filter(file))
                    {
                        this.OnFilteredFileFound(args);
                        yield return file;
                    }
                    else if (this.filter == null)
                    {
                        yield return file;
                    }
                }
            }

            // Iterates each folder in the current folder path, recursive call foreach subfolder
            foreach (var subFolder in Directory.GetDirectories(path))
            {
                if (this.isSearchAborted)
                {
                    yield break;
                }

                Thread.Sleep(1000);
                args = new FileSystemVisitorEventArgs(subFolder);

                // Trigger DirecoryFound delegate
                this.OnDirectoryFound(args);
                if (args.Abort)
                {
                    this.isSearchAborted = true;
                    Console.WriteLine("2");
                    yield break;
                }

                if (!args.Exclude)
                {
                    if (this.filter != null && this.filter(subFolder))
                    {
                        this.OnFilteredDirectoryFound(args);
                        yield return subFolder;
                    }
                    else if (this.filter == null)
                    {
                        yield return subFolder;
                    }

                    foreach (var item in this.Traverse(subFolder))
                    {
                        yield return item;
                    }
                }
            }
        }
    }
}
