namespace AdvCSharp
{
    internal class FileSystemVisitor
    {
        private readonly string rootFolder;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileSystemVisitor"/> class.
        /// </summary>
        /// <param name="rootFolder">Root folder path.</param>
        public FileSystemVisitor(string rootFolder)
        {
            this.rootFolder = rootFolder ?? throw new ArgumentNullException(nameof(rootFolder));
        }

        /// <summary>
        /// Traverse through folders and files of root folder.
        /// </summary>
        /// <returns>IEnumerable string of folders and files in linear sequence.</returns>
        public IEnumerable<string> Traverse()
        {
            return this.Traverse(this.rootFolder);
        }

        private IEnumerable<string> Traverse(string path)
        {
            // Return current folder path
            yield return path;

            // Iterate and return each file in the current folder path
            foreach (var file in Directory.GetFiles(path))
            {
                yield return file;
            }

            // Iterates each folder in the current folder path, recursive call foreach subfolder
            foreach (var subFolder in Directory.GetDirectories(path))
            {
                foreach (var item in this.Traverse(subFolder))
                {
                    yield return item;
                }
            }
        }
    }
}
