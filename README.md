# Slnx
SLNX is a fast parser and writer for the (currently) new in-preview Visual Studio XML Solution format with a .slnx extension, introduced in Visual Studio 2022 17.10 Preview 4.

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

### Writing
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

Contents of `OutputSlnx.txt`:
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
