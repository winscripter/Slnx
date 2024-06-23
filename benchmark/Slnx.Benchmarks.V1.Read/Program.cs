using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Slnx;

BenchmarkRunner.Run<ReadBenchmark>();

[MemoryDiagnoser]
public class ReadBenchmark
{
    private const string Input = """
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
""";

    [Benchmark]
    public void Execute()
    {
        _ = SlnxModel.Load(Input);
    }
}

