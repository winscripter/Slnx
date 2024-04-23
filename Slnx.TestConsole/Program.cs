using Slnx;

var factory = new SlnxFactory();
factory.Folders.Add(new Folder("Solution Items", new[]
{
    new Project("./CSharp/CSharp.csproj"),
    new Project("./VB.NET/VB.NET.vbproj"),
    new Project("./DockerCompose/DockerCompose.dcproj", Guid.NewGuid(), new(solution: "*|*", project: "*|*|Deploy"))
}, new[]
{
    new Folder("C++", Array.Empty<Project>(), Array.Empty<Folder>(), new[]
    {
        "util.cpp",
        "util.h",
        "data.cc",
        "data.h"
    })
}, new string[] { "File1.cs", "File2.cs" }));

factory.Projects.Add(new Project("Slnx/Slnx.csproj"));
factory.Projects.Add(new Project("App/App.shproj"));

var model = factory.AsModel();
model.Store("OutputSlnx.txt");

var model2 = SlnxModel.Load(File.ReadAllText("OutputSlnx.txt"));
foreach (var project in model2.TopLevelProjects!)
{
    Console.WriteLine(project.Path);
    Console.WriteLine(project.Type.ToString());
}
