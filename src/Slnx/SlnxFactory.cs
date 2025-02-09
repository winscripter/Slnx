﻿namespace Slnx
{
    /// <summary>
    /// Builds an SLNX file.
    /// </summary>
    public class SlnxFactory
    {
        /// <summary>
        /// All folders.
        /// </summary>
        public List<Folder> Folders { get; init; }

        /// <summary>
        /// All projects.
        /// </summary>
        public List<Project> Projects { get; init; }

        /// <summary>
        /// All &lt;Properties&gt; elements.
        /// </summary>
        public List<PropertyCollection> Properties { get; init; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SlnxFactory" /> class.
        /// </summary>
        public SlnxFactory()
        {
            Folders = new List<Folder>();
            Projects = new List<Project>();
            Properties = new List<PropertyCollection>();
        }
        
        /// <summary>
        /// Converts this instance of <see cref="SlnxFactory" /> into <see cref="SlnxModel" />.
        /// </summary>
        /// <returns>An instance of <see cref="SlnxModel" />.</returns>
        public SlnxModel AsModel() => new(Projects, Folders, Properties);

        /// <summary>
        /// Adds a new folder to the SLNX file.
        /// </summary>
        /// <param name="folder">The folder to add.</param>
        /// <returns>Current instance of <see cref="SlnxFactory"/> after adding a folder.</returns>
        /// <remarks>
        ///   This will also return a copy of the current instance, which could introduce additional
        ///   memory allocation.
        /// </remarks>
        public SlnxFactory AddFolder(Folder folder)
        {
            Folders.Add(folder);
            return this;
        }

        /// <summary>
        /// Adds a new project to the SLNX file.
        /// </summary>
        /// <param name="path">The project to add.</param>
        /// <returns>Current instance of <see cref="SlnxFactory"/> after adding a project.</returns>
        /// <remarks>
        ///   This will also return a copy of the current instance, which could introduce additional
        ///   memory allocation.
        /// </remarks>
        public SlnxFactory AddProjectWithPathOnly(string path)
        {
            Projects.Add(new Project(path));
            return this;
        }
    }
}
