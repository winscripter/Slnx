namespace Slnx
{
    /// <summary>
    /// Represents a folder in the same level as the solution.
    /// </summary>
    public readonly struct Folder
    {
        /// <summary>
        /// Gets an instance of <see cref="Folder" /> with no content and name "(Folder Name)".
        /// </summary>
        public static Folder Empty => new("(Folder Name)", Array.Empty<Project>(), Array.Empty<Folder>(), Array.Empty<string>());

        /// <summary>
        /// Name of the folder.
        /// </summary>
        public readonly string Name { get; init; }

        /// <summary>
        /// Nested projects in the folder.
        /// </summary>
        public readonly Project[]? DescendantProjects { get; init; }

        /// <summary>
        /// Nested folders in the folder.
        /// </summary>
        public readonly Folder[]? DescendantFolders { get; init; }

        /// <summary>
        /// Files in the folder.
        /// </summary>
        public readonly string[]? DescendantFiles { get; init; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Folder" /> class.
        /// </summary>
        /// <param name="name">Folder name.</param>
        /// <param name="projects">All projects.</param>
        /// <param name="folders">All folders.</param>
        /// <param name="files">All files.</param>
        public Folder(string name, Project[] projects, Folder[] folders, string[] files)
        {
            Name = name;
            DescendantProjects = projects.Length == 0 ? null : projects;
            DescendantFolders = folders.Length == 0 ? null : folders;
            DescendantFiles = files.Length == 0 ? null : files;
        }
    }
}
