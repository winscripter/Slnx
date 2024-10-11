namespace Slnx
{
    /// <summary>
    /// Represents a single project found in the SLNX file.
    /// </summary>
    public class Project
    {
        /// <summary>
        /// Checks if the project file exists.
        /// </summary>
        public bool Exists => File.Exists(System.IO.Path.Combine(Directory.GetCurrentDirectory(), Path));

        /// <summary>
        /// Path to the project file.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Type of the project.
        /// </summary>
        public ProjectType Type { get; set; }

        /// <summary>
        /// Extension of the project file. If the extension isn't present, the
        /// value is null.
        /// </summary>
        public string? Extension { get; set; }

        /// <summary>
        /// Checks if the <see cref="Extension" /> property has an extension.
        /// </summary> 
        public bool HasExtension => Extension != null;

        /// <summary>
        /// Gets the type of the project. This can be a string (for example, a string "Classic C#"),
        /// or a GUID.
        /// </summary>
        public string? ProjectType { get; set; }

        /// <summary>
        /// Descendant configuration.
        /// </summary>
        public DescendantConfiguration? Configuration { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Project" /> class.
        /// </summary>
        /// <param name="path">Path to the project file.</param>
        /// <param name="type">The project type.</param>
        /// <param name="config">The configuration.</param>
        public Project(string path, string? type = null, DescendantConfiguration? config = null)
        {
            Path = path;

            string typeString = (path.Contains('/') ? path.Split('/').Last()
                                                    : path.Contains('\\') ? path.Split('\\').Last()
                                                                          : path).TrimStart('.');
            typeString = typeString.Contains('.') ? typeString.Split('.').Last() : typeString;

            Type = typeString.FromExtension();
            Extension = path.Contains('.') ? path.Split('.').Last() : null;
            ProjectType = type;
            Configuration = config;
        }

        /// <summary>
        /// Changes the descendant configuration of this project to <paramref name="configuration"/> and returns it.
        /// </summary>
        /// <param name="configuration">The descendant project configuration.</param>
        /// <returns>Current instance of <see cref="Project"/> with the given configuration.</returns>
        public Project WithConfiguration(DescendantConfiguration? configuration)
        {
            Configuration = configuration;
            return this;
        }
    }
}
