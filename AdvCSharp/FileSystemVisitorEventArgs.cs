// <copyright file="FileSystemVisitorEventArgs.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AdvCSharp
{
    /// <summary>
    /// File system visitor event args class. Extends EventArgs.
    /// </summary>
    public class FileSystemVisitorEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileSystemVisitorEventArgs"/> class.
        /// By default abort and exclude properties are false.
        /// </summary>
        /// <param name="path">Path of folder or file.</param>
        public FileSystemVisitorEventArgs(string path)
        {
            this.Path = path;
            this.Abort = false;
            this.Exclude = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileSystemVisitorEventArgs"/> class.
        /// </summary>
        /// <param name="path">Path of file or folder.</param>
        /// <param name="abort">To abort set true.</param>
        /// <param name="exclude">To exclude set true.</param>
        public FileSystemVisitorEventArgs(string path, bool abort, bool exclude)
            : this(path)
        {
            this.Abort = abort;
            this.Exclude = exclude;
        }

        public string Path { get; set; }

        public bool Abort { get; set; }

        public bool Exclude { get; set; }
    }
}
