// This C# file contains .StoreAsync methods.

using System.Text;
using System.Xml;

namespace Slnx
{
    public partial class SlnxModel
    {
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
        public async Task StoreAsync(string outputFile, XmlWriterSettings? settings = null)
        {
            settings ??= DefaultXmlSettings;

            if (File.Exists(outputFile))
            {
                File.Delete(outputFile);
            }

            string result = await StoreAsync(settings);
            File.AppendAllText(outputFile, result.ToString());
        }

        /// <summary>
        /// Converts this instance of <see cref="SlnxModel"/> to the string representation
        /// of the SLNX file with same projects and folders.
        /// </summary>
        /// <param name="xmlWriterSettings">Settings for the style of the output XML. If the value is <see langword="null" />, default parameters are used.</param>
        /// <returns>A string that represents the SLNX file with projects, folders, and other metadata from this instance of <see cref="SlnxModel"/>.</returns>
        public async Task<string> StoreAsync(XmlWriterSettings? xmlWriterSettings = null)
        {
            if (xmlWriterSettings is null)
            {
                xmlWriterSettings ??= DefaultXmlSettings;
                xmlWriterSettings.Async = true;
            }

            var output = new StringBuilder();
            using XmlWriter xmlWriter = XmlWriter.Create(output, xmlWriterSettings);
            await xmlWriter.WriteStartElementAsync(null, "Solution", null);

            foreach (Folder folder in TopLevelFolders ?? _emptyFolderList)
            {
                await WriteFolderAndDescendantsAsync(folder);
            }

            foreach (Project project in TopLevelProjects ?? _emptyProjectList)
            {
                await WriteProjectAsync(project);
            }

            foreach (PropertyCollection propertyCollection in PropertyCollections)
            {
                await xmlWriter.WriteStartElementAsync(null, "Properties", null);

                if (propertyCollection.Name is string propertyCollectionName)
                {
                    await xmlWriter.WriteAttributeStringAsync(null, "Name", null, propertyCollectionName);
                }

                if (propertyCollection.Scope is string propertyCollectionScope)
                {
                    await xmlWriter.WriteAttributeStringAsync(null, "Scope", null, propertyCollectionScope);
                }

                foreach (Property property in propertyCollection.Properties)
                {
                    await xmlWriter.WriteStartElementAsync(null, "Property", null);

                    if (property.Name is string propertyName)
                    {
                        await xmlWriter.WriteAttributeStringAsync(null, "Name", null, propertyName);
                    }

                    if (property.Value is string propertyValue)
                    {
                        await xmlWriter.WriteAttributeStringAsync(null, "Value", null, propertyValue);
                    }

                    await xmlWriter.WriteEndElementAsync();
                }

                await xmlWriter.WriteEndElementAsync();
            }

            await xmlWriter.WriteEndElementAsync();
            await xmlWriter.FlushAsync();

            return output.ToString();

            async Task WriteFolderAndDescendantsAsync(Folder folder)
            {
                await xmlWriter.WriteStartElementAsync(null, "Folder", null);
                await xmlWriter.WriteAttributeStringAsync(null, "Name", null, folder.Name);

                foreach (string file in folder.DescendantFiles ?? _emptyStringList)
                {
                    await xmlWriter.WriteStartElementAsync(null, "File", null);
                    await xmlWriter.WriteAttributeStringAsync(null, "Path", null, file);
                    await xmlWriter.WriteEndElementAsync();
                }

                foreach (Project project in folder.DescendantProjects ?? _emptyProjectList)
                {
                    await WriteProjectAsync(project);
                }

                foreach (Folder nestedFolder in folder.DescendantFolders ?? _emptyFolderList)
                {
                    await WriteFolderAndDescendantsAsync(nestedFolder);
                }

                await xmlWriter.WriteEndElementAsync();
            }

            async Task WriteProjectAsync(Project project)
            {
                await xmlWriter.WriteStartElementAsync(null, "Project", null);

                if (project.ProjectType is string projectType)
                {
                    await xmlWriter.WriteAttributeStringAsync(null, "Type", null, projectType);
                }

                if (project.Path is string path)
                {
                    await xmlWriter.WriteAttributeStringAsync(null, "Path", null, path);
                }

                if (project.Configuration is DescendantConfiguration configuration)
                {
                    await xmlWriter.WriteStartElementAsync(null, "Configuration", null);

                    if (configuration.Project is string configurationProject)
                    {
                        await xmlWriter.WriteAttributeStringAsync(null, "Project", null, configurationProject);
                    }

                    if (configuration.Solution is string configurationSolution)
                    {
                        await xmlWriter.WriteAttributeStringAsync(null, "Solution", null, configurationSolution);
                    }

                    await xmlWriter.WriteEndElementAsync();
                }

                await xmlWriter.WriteEndElementAsync();
            }
        }
    }
}
