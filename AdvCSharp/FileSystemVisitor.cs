// <copyright file="FileSystemVisitor.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AdvCSharp
{
    internal class FileSystemVisitor
    {
        private readonly string rootFolder;

        /// <summary>
        /// Delegate for filtering.
        /// </summary>
        private readonly Func<string, bool>? filter;

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

        private IEnumerable<string> Traverse(string path)
        {
            // Iterates each folder in the current folder path, recursive call foreach subfolder
            foreach (var subFolder in Directory.GetDirectories(path))
            {
                if (this.CheckFilter(subFolder))
                {
                    yield return subFolder;
                }

                foreach (var item in this.Traverse(subFolder))
                {
                    yield return item;
                }
            }

            // Iterate and return each file in the current folder path
            foreach (var file in Directory.GetFiles(path))
            {
                if (this.CheckFilter(file))
                {
                    yield return file;
                }
            }
        }

        /// <summary>
        /// Check if there is any filter function provided or if data exists after filtering.
        /// </summary>
        /// <param name="item">Item to be filtered.</param>
        /// <returns>Return true if no filter provided or filter applied and data exists.</returns>
        private bool CheckFilter(string item)
        {
            return this.filter == null || this.filter(item);
        }
    }
}
