using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Slnx;

BenchmarkRunner.Run<WriteBenchmark>();

[MemoryDiagnoser]
public class WriteBenchmark
{
    [Benchmark]
    public void Execute()
    {
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

        _ = factory.AsModel().Store();
    }
}
