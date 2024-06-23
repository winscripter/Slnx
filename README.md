# Slnx
SLNX is a fast parser and writer for the (currently) new in-preview Visual Studio XML Solution format with a .slnx extension, introduced in Visual Studio 2022 17.10 Preview 3.

Available on NuGet: https://nuget.org/packages/Slnx

### Reading

```cs
using Slnx;

var model = SlnxModel.Load(File.ReadAllText("TestSlnx.txt"));

foreach (Folder folder in model.TopLevelFolders!)
{
    foreach (string file in folder.DescendantFiles!)
    {
        Console.WriteLine(file);
    }
}
```

TestSlnx.txt content:
```
<Solution>
    <Folder Name="Solution Items">
        <File Path="File1.cs" />
        <File Path=".editorconfig" />
        <Project Path="File.csproj" />
        <Folder Name="Test">
            <File Path=".editorconfig" />
            <File Path="data.cs" />
        </Folder>
    </Folder>
</Solution>
```
Output:
```
File1.cs
.editorconfig
```

### Writing (for Slnx 2.0)
```cs
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
```

### Writing (for Slnx 1.0)
```cs
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
```

Both of these programs will generate `OutputSlnx.txt` with the following contents:
```
<Solution>
    <Project Path="Slnx/Slnx.csproj" />
    <Project Path="App/App.shproj" />
    <Folder Name="Solution Items">
        <File Path="File1.cs" />
        <File Path="File2.cs" />
        <Project Path="./CSharp/CSharp.csproj" />
        <Project Path="./VB.NET/VB.NET.vbproj" />
        <Project Path="./DockerCompose/DockerCompose.dcproj" Type="a9ca3494-2d8e-43aa-a418-28709ddb90fc">
            <Configuration Solution="*|*" Project="*|*|Deploy" />
        </Project>
        <Folder Name="C++">
            <File Path="util.cpp" />
            <File Path="util.h" />
            <File Path="data.cc" />
            <File Path="data.h" />
        </Folder>
    </Folder>
</Solution>
```

# Contributing
Bug reports, suggestions, questions and other feedback are welcome.

# Compatibility
Slnx uses the .NET 6.0 Runtime, but it works fine for preceding versions, including .NET 7.0, 8.0, and future releases.
