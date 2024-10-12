# Slnx
SLNX is a fast parser and writer for the (currently) new in-preview Visual Studio XML Solution format with a .slnx extension, introduced in Visual Studio 2022 17.10 Preview 3.

Slnx has been around since April 23, 2024 - days after SLNX file format was introduced.

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

### Writing (applies to Slnx 2.0, see below code block for Slnx 3.0)
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
#### For Slnx 3.0
Note: The constructor of the `Project` class was slightly changed in Slnx 3.0. Change this:
```cs
// ...
folder.AddProject(new Project("./DockerCompose/DockerCompose.dcproj", typeGuid: Guid.NewGuid(), config: new(solution: "*|*", project: "*|*|Deploy")));
var moreFolders = new Folder("C++");
```
to:
```cs
// ...
folder.AddProject(new Project("./DockerCompose/DockerCompose.dcproj", /*HERE -->*/type: Guid.NewGuid().ToString(), /*<-- HERE*/ config: new(solution: "*|*", project: "*|*|Deploy")));
var moreFolders = new Folder("C++");
```

Starting with Slnx 3.0, you can convert `SlnxModel` to `string` asynchronously:
```cs
// ...
factory.AddProjectWithPathOnly("App/App.shproj");

string content = await factory.AsModel().StoreAsync(); // <---
File.AppendAllText("OutputSlnx.txt", content);
```

### Writing (applies to Slnx 1.0)
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

# Benchmarking
This is the benchmark for SLNX version 2.0, using `BenchmarkDotNet`. Benchmarks are found in the `./benchmark` folder.
### Read
```

BenchmarkDotNet v0.13.12, Windows 11 (10.0.22631.2861/23H2/2023Update/SunValley3)
AMD Ryzen 7 4700U with Radeon Graphics, 1 CPU, 8 logical and 8 physical cores
.NET SDK 8.0.204
  [Host]     : .NET 8.0.4 (8.0.424.16909), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.4 (8.0.424.16909), X64 RyuJIT AVX2


```
| Method  | Mean     | Error     | StdDev    | Gen0   | Allocated |
|-------- |---------:|----------:|----------:|-------:|----------:|
| Execute | 4.469 μs | 0.0834 μs | 0.0739 μs | 6.8970 |  14.17 KB |

### Write
```

BenchmarkDotNet v0.13.12, Windows 11 (10.0.22631.2861/23H2/2023Update/SunValley3)
AMD Ryzen 7 4700U with Radeon Graphics, 1 CPU, 8 logical and 8 physical cores
.NET SDK 8.0.204
  [Host]     : .NET 8.0.4 (8.0.424.16909), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.4 (8.0.424.16909), X64 RyuJIT AVX2


```
| Method  | Mean     | Error     | StdDev    | Gen0    | Allocated |
|-------- |---------:|----------:|----------:|--------:|----------:|
| Execute | 8.763 μs | 0.1065 μs | 0.0944 μs | 17.6392 |   36.2 KB |

