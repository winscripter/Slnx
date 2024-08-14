using System.Text;
using System.Xml.Linq;

namespace Slnx
{
    internal static class SlnxParser
    {
        public static SlnxModel ParseSlnx(string slnxContent)
        {
            var doc = XDocument.Parse(slnxContent);
            var root = doc.Root ?? throw new SolutionException("Root tag is missing.");

            var folders = new List<Folder>();
            var projects = new List<Project>();

            foreach (var childTag in root.Elements())
            {
                switch (childTag.Name.LocalName)
                {
                    case "Folder":
                        folders.Add(ParseFolder(childTag));
                        break;
                    case "Project":
                        projects.Add(ParseProject(childTag));
                        break;
                    case "File":
                        throw new SolutionException("A File element cannot be at the root level. Please add it into the Solution Items folder first.");
                    default:
                        throw new SolutionException($"Unrecognized tag \"{childTag.Name.LocalName}\"");
                }
            }

            return new SlnxModel(projects.ToArray(), folders.ToArray());
        }

        private static Folder ParseFolder(XElement folderElement)
        {
            var name = folderElement.Attribute("Name")?.Value ?? throw new SolutionException("Missing attribute \"Name\"");
            var projects = folderElement.Elements("Project").Select(ParseProject).ToArray();
            var folders = folderElement.Elements("Folder").Select(ParseFolder).ToArray();
            var files = folderElement.Elements("File").Select(ParseFile).ToArray();

            return new Folder(name, projects, folders, files);
        }

        private static string ParseFile(XElement fileElement)
        {
            var path = fileElement.Attribute("Path")?.Value ?? throw new SolutionException("Missing attribute \"Path\"");

            return path;
        }

        private static Project ParseProject(XElement projectElement)
        {
            var path = projectElement.Attribute("Path")?.Value ?? throw new SolutionException("Missing attribute \"Path\"");

            var typeGuid = projectElement.Attribute("TypeGuid") != null
                           ? Guid.Parse(projectElement.Attribute("TypeGuid")!.Value)
                           : (Guid?)null;
            var configElement = projectElement.Element("Configuration");
            var config = configElement != null ? ParseConfiguration(configElement) : (DescendantConfiguration?)null;

            return new Project(path, typeGuid, config);
        }

        private static DescendantConfiguration ParseConfiguration(XElement configElement)
        {
            var solution = configElement.Attribute("Solution")?.Value;
            var project = configElement.Attribute("Project")?.Value;

            return new DescendantConfiguration(solution, project);
        }
    }
}
