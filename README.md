<!--
> [!IMPORTANT]
> This project is only a temporary solution to work with SLNX files before MSBuild supports SLNX files.
> 
> Currently, MSBuild does not support reading and manipulating SLNX files. However, as soon as it does, this
> library may not be suitable anymore.
>
> However, MSBuild is actively getting new commits and PRs that put effort into supporting SLNX files. This is a good sign,
> as we can finally begin building SLNX files in apps that support them. This, however, also means that the Slnx library will not
> have much use when MSBuild begins to support SLNX files. Furthermore, the SLNX file format is not officially documented. Because of this,
> the Slnx library may not be able to parse every single SLNX file, because the format is not officially documented.
>
-->

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

### Writing (applies to Slnx 3.0)
```cs
using Slnx;
using System.Text.Json;

// Note: This is a .NET 8 project and uses Collection Expressions.

var factory = new SlnxFactory();

var folder = new Folder("Solution Items");
folder.AddProjectWithPathOnly("./CSharp/CSharp.csproj");
folder.AddProjectWithPathOnly("./VB.NET/VB.NET.vbproj");
folder.AddProject(new Project("./DockerCompose/DockerCompose.dcproj", typ: Guid.NewGuid().ToString(), config: new(solution: "*|*", project: "*|*|Deploy")));
var moreFolders = new Folder("C++");
moreFolders.AddFiles(["util.cpp", "util.h", "data.cc", "data.h"]);
folder.AddFiles(["File1.cs", "File2.cs"]);
folder.AddFolder(moreFolders);

factory.AddFolder(folder);
factory.AddProjectWithPathOnly("Slnx/Slnx.csproj");
factory.AddProjectWithPathOnly("App/App.shproj");

string content = factory.AsModel().Store();
File.AppendAllText("OutputSlnx.txt", content);

// To provide detailed information, I'll just JSONify it with System.Text.Json.
var model = SlnxModel.Load(File.ReadAllText("OutputSlnx.txt"));
Console.WriteLine(JsonSerializer.Serialize(model));
```
The program will generate a file named `OutputSlnx.txt` with these contents:

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
Bug reports, feature suggestions, questions, and other feedback are welcome.

# Compatibility
Slnx uses the .NET 6.0 Runtime, but it works fine for preceding versions, including .NET 7.0, 8.0, and future releases.

# Benchmarking

## Read Benchmark
Input:
```xml
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

Benchmark result:
```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.22631.2861/23H2/2023Update/SunValley3)
AMD Ryzen 7 4700U with Radeon Graphics, 1 CPU, 8 logical and 8 physical cores
.NET SDK 8.0.303
  [Host]     : .NET 8.0.7 (8.0.724.31311), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.7 (8.0.724.31311), X64 RyuJIT AVX2


```
| Method      | Mean     | Error     | StdDev    | Gen0   | Allocated |
|------------ |---------:|----------:|----------:|-------:|----------:|
| ReadBenchie | 4.486 μs | 0.0301 μs | 0.0267 μs | 7.1106 |  14.55 KB |

## Write Benchmark
Code:
```cs
var factory = new SlnxFactory();

var folder = new Folder("Solution Items");
folder.AddProjectWithPathOnly("./CSharp/CSharp.csproj");
folder.AddProjectWithPathOnly("./VB.NET/VB.NET.vbproj");
folder.AddProject(new Project("./DockerCompose/DockerCompose.dcproj", type: null, config: new(solution: "*|*", project: "*|*|Deploy")));
var moreFolders = new Folder("C++");
moreFolders.AddFiles(["util.cpp", "util.h", "data.cc", "data.h"]);
folder.AddFiles(["File1.cs", "File2.cs"]);
folder.AddFolder(moreFolders);

factory.AddFolder(folder);
factory.AddProjectWithPathOnly("Slnx/Slnx.csproj");
factory.AddProjectWithPathOnly("App/App.shproj");

_ = factory.AsModel().Store();
```
To provide the accurate benchmark, we're not saving or logging the result anywhere like console or file - we're
just focusing on how fast Slnx can export the SlnxFactory instance as a string.

Benchmark result:
```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.22631.2861/23H2/2023Update/SunValley3)
AMD Ryzen 7 4700U with Radeon Graphics, 1 CPU, 8 logical and 8 physical cores
.NET SDK 8.0.303
  [Host]     : .NET 8.0.7 (8.0.724.31311), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.7 (8.0.724.31311), X64 RyuJIT AVX2


```
| Method       | Mean     | Error     | StdDev    | Gen0   | Allocated |
|------------- |---------:|----------:|----------:|-------:|----------:|
| WriteBenchie | 4.841 μs | 0.0371 μs | 0.0310 μs | 9.4299 |  19.33 KB |
