using Slnx;
using System.Text.Json;

// Note: This is a .NET 8 project and uses Collection Expressions.

var factory = new SlnxFactory();

var folder = new Folder("Solution Items");
folder.AddProjectWithPathOnly("./CSharp/CSharp.csproj");
folder.AddProjectWithPathOnly("./VB.NET/VB.NET.vbproj");
folder.AddProject(new Project("./DockerCompose/DockerCompose.dcproj", typeGuid: Guid.NewGuid(), config: new(solution: "*|*", project: "*|*|Deploy")));
var moreFolders = new Folder("C++");
moreFolders.AddFiles(["util.cpp", "util.h", "data.cc", "data.h"]);
folder.AddFiles(["File1.cs", "File2.cs"]);

factory.AddFolder(folder);
factory.AddProjectWithPathOnly("Slnx/Slnx.csproj");
factory.AddProjectWithPathOnly("App/App.shproj");

string content = factory.AsModel().Store();
File.AppendAllText("OutputSlnx.txt", content);

// To provide detailed information, I'll just JSONify it with System.Text.Json.
var model = SlnxModel.Load(File.ReadAllText("OutputSlnx.txt"));
Console.WriteLine(JsonSerializer.Serialize(model));
