using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Slnx;

BenchmarkRunner.Run<Benchie>();

[MemoryDiagnoser]
public class Benchie
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
    public void ReadBenchie()
    {
        _ = SlnxModel.Load(Input);
    }
}
