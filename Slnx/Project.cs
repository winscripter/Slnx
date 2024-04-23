using System.Xml;

namespace Slnx
{
    /// <summary>
    /// Represents a single project found in the SLNX file.
    /// </summary>
    public readonly struct Project
    {
        /// <summary>
        /// Checks if the project file exists.
        /// </summary>
        public readonly bool Exists => File.Exists(System.IO.Path.Combine(Directory.GetCurrentDirectory(), Path));

        /// <summary>
        /// Path to the project file.
        /// </summary>
        public readonly string Path { get; init; }

        /// <summary>
        /// Type of the project.
        /// </summary>
        public readonly ProjectType Type { get; init; }

        /// <summary>
        /// Extension of the project file. If the extension isn't present, the
        /// value is null.
        /// </summary>
        public readonly string? Extension { get; init; }

        /// <summary>
        /// Checks if the <see cref="Extension" /> property has an extension.
        /// </summary> 
        public readonly bool HasExtension => Extension != null;

        /// <summary>
        /// Gets the type of the project from the GUID. The value is always null
        /// unless the XML attribute "Type" exists.
        /// </summary>
        public Guid? TypeGuid { get; init; }

        /// <summary>
        /// Descendant configuration.
        /// </summary>
        public DescendantConfiguration? Configuration { get; init; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Project" /> struct.
        /// </summary>
        /// <param name="path">Path to the project file.</param>
        /// <param name="typeGuid">The GUID indicating its type.</param>
        /// <param name="config">The configuration.</param>
        public Project(string path, Guid? typeGuid = null, DescendantConfiguration? config = null)
        {
            Path = path;

            string typeString = (path.Contains('/') ? path.Split('/').Last()
                                                    : path.Contains('\\') ? path.Split('\\').Last()
                                                                          : path).TrimStart('.');
            typeString = typeString.Contains('.') ? typeString.Split('.').Last() : typeString;

            Type = typeString.FromExtension();
            Extension = path.Contains('.') ? path.Split('.').Last() : null;
            TypeGuid = typeGuid;
            Configuration = config;
        }
    }
}
