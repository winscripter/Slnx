namespace Slnx
{
    /// <summary>
    /// Represents a folder in the same level as the solution.
    /// </summary>
    public class Folder
    {
        /// <summary>
        /// Gets an instance of <see cref="Folder" /> with no content and name "(Folder Name)".
        /// </summary>
        public static Folder Empty => new("(Folder Name)", Array.Empty<Project>(), Array.Empty<Folder>(), Array.Empty<string>());

        /// <summary>
        /// Name of the folder.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Nested projects in the folder. If this is <see langword="NULL"/>, then there
        /// are no descendant projects.
        /// </summary>
        public List<Project>? DescendantProjects { get; set; }

        /// <summary>
        /// Nested folders in the folder. If this is <see langword="NULL"/>, then there
        /// are no descendant folders.
        /// </summary>
        public List<Folder>? DescendantFolders { get; set; }

        /// <summary>
        /// Files in the folder. If this is <see langword="NULL"/>, then there
        /// are no descendant files.
        /// </summary>
        public List<string>? DescendantFiles { get; set; }

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
            DescendantProjects = projects.Length == 0 ? null : new List<Project>(projects);
            DescendantFolders = folders.Length == 0 ? null : new List<Folder>(folders);
            DescendantFiles = files.Length == 0 ? null : new List<string>(files);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Folder"/> class.
        /// </summary>
        /// <param name="name">The name of the folder.</param>
        /// <remarks>
        /// Properties <see cref="DescendantProjects"/>, <see cref="DescendantFiles"/>,
        /// and <see cref="DescendantFolders"/> will be <see langword="null"/>.
        /// </remarks>
        public Folder(string name) : this(name, Array.Empty<Project>(), Array.Empty<Folder>(), Array.Empty<string>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Folder"/> class.
        /// </summary>
        /// <param name="name">The name of the folder.</param>
        /// <param name="projects">All projects.</param>
        /// <remarks>
        /// Properties <see cref="DescendantFiles"/> and <see cref="DescendantFolders"/>
        /// will be <see langword="null"/>.
        /// </remarks>
        public Folder(string name, Project[] projects) : this(name, projects, Array.Empty<Folder>(), Array.Empty<string>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Folder"/> class.
        /// </summary>
        /// <param name="name">The name of the folder.</param>
        /// <param name="folders">All folders.</param>
        /// <remarks>
        /// Properties <see cref="DescendantFiles"/> and <see cref="DescendantProjects"/>
        /// will be <see langword="null"/>.
        /// </remarks>
        public Folder(string name, Folder[] folders) : this(name, Array.Empty<Project>(), folders, Array.Empty<string>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Folder"/> class.
        /// </summary>
        /// <param name="name">The name of the folder.</param>
        /// <param name="files">All files.</param>
        /// <remarks>
        /// Properties <see cref="DescendantProjects"/> and <see cref="DescendantFolders"/>
        /// will be <see langword="null"/>.
        /// </remarks>
        public Folder(string name, string[] files) : this(name, Array.Empty<Project>(), Array.Empty<Folder>(), files)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Folder"/> class.
        /// </summary>
        /// <param name="name">The name of the folder.</param>
        /// <param name="folders">All folders.</param>
        /// <param name="files">All files.</param>
        /// <remarks>
        /// Property <see cref="DescendantProjects"/> will be <see langword="null"/>.
        /// </remarks>
        public Folder(string name, Folder[] folders, string[] files) : this(name, Array.Empty<Project>(), folders, files)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Folder"/> class.
        /// </summary>
        /// <param name="name">The name of the folder.</param>
        /// <param name="folders">All folders.</param>
        /// <param name="projects">All projects.</param>
        /// <remarks>
        /// Property <see cref="DescendantFiles"/> will be <see langword="null"/>.
        /// </remarks>
        public Folder(string name, Folder[] folders, Project[] projects) : this(name, projects, folders, Array.Empty<string>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Folder"/> class.
        /// </summary>
        /// <param name="name">The name of the folder.</param>
        /// <param name="project">All projects.</param>
        /// <param name="files">All files.</param>
        /// <remarks>
        /// Property <see cref="DescendantFolders"/> will be <see langword="null"/>.
        /// </remarks>
        public Folder(string name, Project[] project, string[] files) : this(name, project, Array.Empty<Folder>(), files)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Folder"/> class.
        /// </summary>
        /// <param name="name">The name of the folder.</param>
        /// <param name="project">All projects.</param>
        /// <param name="folders">All folders.</param>
        /// <remarks>
        /// Property <see cref="DescendantFiles"/> will be <see langword="null"/>.
        /// </remarks>
        public Folder(string name, Project[] project, Folder[] folders) : this(name, project, folders, Array.Empty<string>())
        {
        }

        /// <summary>
        /// Adds a given array of projects to this instance of <see cref="Folder"/>.
        /// </summary>
        /// <param name="projects">An array of projects to add.</param>
        /// <returns>Current instance of <see cref="Folder"/> after adding the given projects.</returns>
        public Folder AddProjects(Project[] projects)
        {
            DescendantProjects ??= new List<Project>();

            foreach (Project project in projects)
            {
                DescendantProjects.Add(project);
            }

            return this;
        }

        /// <summary>
        /// Adds a single project to this instance of <see cref="Folder"/>.
        /// </summary>
        /// <param name="project">A project that will be added.</param>
        /// <returns>Current instance of <see cref="Folder"/> after adding the given project.</returns>
        public Folder AddProject(Project project)
        {
            DescendantProjects ??= new List<Project>();
            DescendantProjects.Add(project);
            return this;
        }

        /// <summary>
        /// Adds a given array of files to this instance of <see cref="Folder"/>.
        /// </summary>
        /// <param name="files">An array of files to add.</param>
        /// <returns>Current instance of <see cref="Folder"/> after adding the given files.</returns>
        public Folder AddFiles(string[] files)
        {
            DescendantFiles ??= new List<string>();
            foreach (string file in files)
            {
                DescendantFiles.Add(file);
            }
            return this;
        }

        /// <summary>
        /// Adds a single file to this instance of <see cref="Folder"/>.
        /// </summary>
        /// <param name="file">A file that will be added.</param>
        /// <returns>Current instance of <see cref="Folder"/> after adding the given file.</returns>
        public Folder AddFile(string file)
        {
            DescendantFiles ??= new List<string>();
            DescendantFiles.Add(file);
            return this;
        }

        /// <summary>
        /// Adds a given array of folders to this instance of <see cref="Folder"/>.
        /// </summary>
        /// <param name="folders">An array of folders to add.</param>
        /// <returns>Current instance of <see cref="Folder"/> after adding the given folder.</returns>
        public Folder AddFolders(Folder[] folders)
        {
            DescendantFolders ??= new List<Folder>();
            foreach (Folder folder in folders)
            {
                DescendantFolders.Add(folder);
            }
            return this;
        }

        /// <summary>
        /// Adds a single folder to this instance of <see cref="Folder"/>.
        /// </summary>
        /// <param name="folder">A folder that will be added.</param>
        /// <returns>Current instance of <see cref="Folder"/> after adding the given folder.</returns>
        public Folder AddFolder(Folder folder)
        {
            DescendantFolders ??= new List<Folder>();
            DescendantFolders.Add(folder);
            return this;
        }

        /// <summary>
        /// Adds a new project without any configuration to this instance of <see cref="Folder"/>
        /// </summary>
        /// <param name="path">The path to the project.</param>
        /// <returns>
        /// A tuple that contains two elements:
        /// <list type="bullet">
        ///   <item>
        ///     <em>Item 1</em> (<see cref="Folder"/>): Current instance of
        ///     <see cref="Folder"/> after adding the project.
        ///   </item>
        ///   <item>
        ///     <em>Item 2</em> (<see cref="Project"/>): The project that was added.
        ///   </item>
        /// </list>
        /// </returns>
        public (Folder, Project) AddProjectWithPathOnly(string path)
        {
            var project = new Project(path);
            AddProject(project);
            return (this, project);
        }
    }
}
