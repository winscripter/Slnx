namespace Slnx
{
    /// <summary>
    /// Represents the type of a project file.
    /// </summary>
    public enum ProjectType : byte
    {
        /// <summary>
        /// C# Project (*.csproj)
        /// </summary>
        CSharp,

        /// <summary>
        /// Visual C++ Project (*.vcxproj)
        /// </summary>
        VC,

        /// <summary>
        /// VB.NET Project (*.vbproj)
        /// </summary>
        VisualBasic,

        /// <summary>
        /// F# Project (*.fsproj)
        /// </summary>
        FSharp,

        /// <summary>
        /// Microsoft Installer Project (*.vdproj)
        /// </summary>
        Installer,

        /// <summary>
        /// Shared Project (*.shproj)
        /// </summary>
        Shared,

        /// <summary>
        /// Database Project (*.dbproj)
        /// </summary>
        Database,

        /// <summary>
        /// Docker Compose Project (*.dcproj)
        /// </summary>
        DockerCompose,

        /// <summary>
        /// SQL Project (*.sqlproj)
        /// </summary>
        Sql,

        /// <summary>
        /// JavaScript Application/ECMAScript Project (*.esproj)
        /// </summary>
        JavaScript,

        /// <summary>
        /// .NET Core 2015 Project (*.xproj)
        /// </summary>
        NetCore2015,

        /// <summary>
        /// Common MSBuild Project (*.msbuildproj)
        /// </summary>
        Common,

        /// <summary>
        /// Project (*.proj)
        /// </summary>
        Project,

        /// <summary>
        /// Any project type which is not defined here.
        /// </summary>
        Unknown = 0xFF
    }
}
