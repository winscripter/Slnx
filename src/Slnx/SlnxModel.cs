using System.Text;
using System.Xml;

namespace Slnx
{
    /// <summary>
    /// Represents information about an SLNX file.
    /// </summary>
    public partial class SlnxModel
    {
        // Can't use collection expressions because this is C# 10 at least
        private static readonly List<Folder> _emptyFolderList = new();
        private static readonly List<string> _emptyStringList = new();
        private static readonly List<Project> _emptyProjectList = new();

        /// <summary>
        /// If you pass <see langword="null"/> to <see cref="Store(XmlWriterSettings?)"/>, this is
        /// the <see cref="XmlWriterSettings"/> instance that <see cref="Slnx"/> will use.
        /// </summary>
        public static readonly XmlWriterSettings DefaultXmlSettings = new()
        {
            Indent = true,
            IndentChars = "    ", // 4 spaces
            NewLineChars = "\r\n",
            NewLineHandling = NewLineHandling.Replace,
            Encoding = new UTF8Encoding(false),
            OmitXmlDeclaration = true
        };

        /// <summary>
        /// Gets all projects in the same level as the SLNX.
        /// </summary>
        public List<Project>? TopLevelProjects { get; set; }

        /// <summary>
        /// Gets all folders in the same level as the SLNX.
        /// </summary>
        public List<Folder>? TopLevelFolders { get; set; }

        /// <summary>
        /// Gets all property collections in the same level as the SLNX. These can only appear
        /// in the same level as the SLNX - they cannot appear inside folder or project elements.
        /// </summary>
        public List<PropertyCollection> PropertyCollections { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SlnxModel" /> class.
        /// </summary>
        /// <param name="projects">A list of projects.</param>
        /// <param name="folders">A list of folders.</param>
        /// <param name="propertyCollections">A list of property collections.</param>
        public SlnxModel(List<Project> projects, List<Folder> folders, List<PropertyCollection> propertyCollections)
        {
            TopLevelProjects = projects.Count == 0 ? null : projects;
            TopLevelFolders = folders.Count == 0 ? null : folders;
            PropertyCollections = propertyCollections;
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
            settings ??= DefaultXmlSettings;
            
            if (File.Exists(outputFile))
            {
                File.Delete(outputFile);
            }

            string result = Store(settings);
            File.AppendAllText(outputFile, result.ToString());
        }

        /// <summary>
        /// Converts this instance of <see cref="SlnxModel"/> to the string representation
        /// of the SLNX file with same projects and folders.
        /// </summary>
        /// <param name="xmlWriterSettings">Settings for the style of the output XML. If the value is <see langword="null" />, default parameters are used.</param>
        /// <returns>A string that represents the SLNX file with projects, folders, and other metadata from this instance of <see cref="SlnxModel"/>.</returns>
        public string Store(XmlWriterSettings? xmlWriterSettings = null)
        {
            xmlWriterSettings ??= DefaultXmlSettings;

            var output = new StringBuilder();
            using XmlWriter xmlWriter = XmlWriter.Create(output, xmlWriterSettings);
            xmlWriter.WriteStartElement("Solution");

            foreach (Folder folder in TopLevelFolders ?? _emptyFolderList)
            {
                WriteFolderAndDescendants(folder);
            }

            foreach (Project project in TopLevelProjects ?? _emptyProjectList)
            {
                WriteProject(project);
            }

            foreach (PropertyCollection propertyCollection in PropertyCollections)
            {
                xmlWriter.WriteStartElement("Properties");

                if (propertyCollection.Name is string propertyCollectionName)
                {
                    xmlWriter.WriteAttributeString("Name", propertyCollectionName);
                }

                if (propertyCollection.Scope is string propertyCollectionScope)
                {
                    xmlWriter.WriteAttributeString("Scope", propertyCollectionScope);
                }

                foreach (Property property in propertyCollection.Properties)
                {
                    xmlWriter.WriteStartElement("Property");

                    if (property.Name is string propertyName)
                    {
                        xmlWriter.WriteAttributeString("Name", propertyName);
                    }

                    if (property.Value is string propertyValue)
                    {
                        xmlWriter.WriteAttributeString("Value", propertyValue);
                    }

                    xmlWriter.WriteEndElement();
                }

                xmlWriter.WriteEndElement();
            }

            xmlWriter.WriteEndElement();
            xmlWriter.Flush();

            return output.ToString();

            void WriteFolderAndDescendants(Folder folder)
            {
                xmlWriter.WriteStartElement("Folder");
                xmlWriter.WriteAttributeString("Name", folder.Name);

                foreach (string file in folder.DescendantFiles ?? _emptyStringList)
                {
                    xmlWriter.WriteStartElement("File");
                    xmlWriter.WriteAttributeString("Path", file);
                    xmlWriter.WriteEndElement();
                }

                foreach (Project project in folder.DescendantProjects ?? _emptyProjectList)
                {
                    WriteProject(project);
                }

                foreach (Folder nestedFolder in folder.DescendantFolders ?? _emptyFolderList)
                {
                    WriteFolderAndDescendants(nestedFolder);
                }

                xmlWriter.WriteEndElement();
            }

            void WriteProject(Project project)
            {
                xmlWriter.WriteStartElement("Project");

                if (project.ProjectType is string projectType)
                {
                    xmlWriter.WriteAttributeString("Type", projectType);
                }

                if (project.Path is string path)
                {
                    xmlWriter.WriteAttributeString("Path", path);
                }

                if (project.Configuration is DescendantConfiguration configuration)
                {
                    xmlWriter.WriteStartElement("Configuration");

                    if (configuration.Project is string configurationProject)
                    {
                        xmlWriter.WriteAttributeString("Project", configurationProject);
                    }

                    if (configuration.Solution is string configurationSolution)
                    {
                        xmlWriter.WriteAttributeString("Solution", configurationSolution);
                    }

                    xmlWriter.WriteEndElement();
                }

                xmlWriter.WriteEndElement();
            }
        }
    }
}
