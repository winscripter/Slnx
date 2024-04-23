using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Slnx
{
    /// <summary>
    /// Represents information about a SLNX file.
    /// </summary>
    public readonly struct SlnxModel
    {
        /// <summary>
        /// Gets all projects in the same level as the SLNX.
        /// </summary>
        public readonly Project[]? TopLevelProjects { get; init; }

        /// <summary>
        /// Gets all folders in the same level as the SLNX.
        /// </summary>
        public readonly Folder[]? TopLevelFolders { get; init; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SlnxModel" /> class.
        /// </summary>
        /// <param name="projects">A list of projects.</param>
        /// <param name="folders">A list of folders.</param>
        public SlnxModel(Project[] projects, Folder[] folders)
        {
            TopLevelProjects = projects.Length == 0 ? null : projects;
            TopLevelFolders = folders.Length == 0 ? null : folders;
        }

        /// <summary>
        /// Loads and parses a SLNX file.
        /// </summary>
        /// <param name="slnxContent">The string contents of the SLNX file. Use <see cref="File.ReadAllText(string)" />.</param>
        /// <returns>Information about that SLNX file.</returns>
        public static SlnxModel Load(string slnxContent) => SlnxParser.ParseSlnx(slnxContent);

        /// <summary>
        /// Stores the data of this instance of <see cref="SlnxModel" /> into a SLNX file.
        /// </summary>
        /// <param name="outputFile">
        /// Output file. Can be anything but it is recommended for the
        /// extension to end with .slnx.
        /// </param>
        /// <param name="settings">Settings for the style of the output XML. If the value is <see langword="null" />, default parameters are used.</param>
        /// <remarks>
        /// The output file will be attempted to be deleted if it exists.
        /// </remarks>
        public void Store(string outputFile, XmlWriterSettings? settings = null)
        {
            settings ??= XmlPrivate.DefaultSettings;

            IndentedAppender innerContent = new(new StringBuilder());

            innerContent.WriteLine("<Solution>");
            innerContent.IndentUp();

            if (TopLevelProjects != null)
            {
                foreach (Project project in TopLevelProjects)
                {
                    EmitProject(project);
                }
            }

            if (TopLevelFolders != null)
            {
                foreach (Folder folder in TopLevelFolders)
                {
                    EmitFolder(folder);
                }
            }

            innerContent.IndentDown();
            innerContent.WriteLine("</Solution>");

            // Format the result
            var value = innerContent.GetBuilder().ToString();
            innerContent.SetContent(XDocument.Parse(value).FormatIndented(settings!));

            if (File.Exists(outputFile))
            {
                File.Delete(outputFile);
            }

            File.AppendAllText(outputFile, innerContent.GetBuilder().ToString());

            void EmitProject(Project project)
            {
                innerContent.WriteIndented($"<Project Path=\"{project.Path}\"");
                if (project.TypeGuid is not null)
                {
                    innerContent.WriteLine($" Type=\"{project.TypeGuid?.ToString()}\"");
                }

                if (project.Configuration is null)
                {
                    innerContent.WriteLine(" />");
                }
                else
                {
                    innerContent.Write(">");
                    innerContent.IndentUp();

                    innerContent.WriteIndented($"<Configuration ");

                    if (project.Configuration.Value.Solution is not null)
                    {
                        innerContent.Write($"Solution=\"{project.Configuration.Value.Solution}\" ");
                    }

                    if (project.Configuration.Value.Project is not null)
                    {
                        innerContent.Write($"Project=\"{project.Configuration.Value.Project}\" ");
                    }

                    innerContent.WriteLine(" />");

                    innerContent.WriteLineIndented("</Project>");
                    innerContent.IndentDown();
                }

                innerContent.IndentDown();
            }

            void EmitFolder(Folder folder)
            {
                innerContent.WriteLineIndented($"<Folder Name=\"{folder.Name}\">");
                innerContent.IndentUp();

                foreach (string descendantFile in folder.DescendantFiles ?? Array.Empty<string>())
                {
                    innerContent.WriteLineIndented($"<File Path=\"{descendantFile}\" />");
                }

                foreach (Project project in folder.DescendantProjects ?? Array.Empty<Project>())
                {
                    EmitProject(project);
                }

                if (folder.DescendantFolders != null)
                {
                    if (folder.DescendantFolders.Length > 0)
                    {
                        foreach (Folder fldr in folder.DescendantFolders)
                        {
                            EmitFolder(fldr);
                        }
                    }
                }

                innerContent.IndentDown();
                innerContent.WriteLineIndented("</Folder>");
                innerContent.IndentUp();
            }
        }
    }
}
