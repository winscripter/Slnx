namespace Slnx
{
    /// <summary>
    /// Represents the type of a project file.
    /// </summary>
    public enum ProjectType
    {
        /// <summary>
        /// C# Project (*.csproj)
        /// </summary>
        CSharp = 0,

        /// <summary>
        /// Visual C++ Project (*.vcxproj)
        /// </summary>
        VC = 1,

        /// <summary>
        /// VB.NET Project (*.vbproj)
        /// </summary>
        VisualBasic = 2,

        /// <summary>
        /// F# Project (*.fsproj)
        /// </summary>
        FSharp = 3,

        /// <summary>
        /// Microsoft Installer Project (*.vdproj)
        /// </summary>
        Installer = 4,

        /// <summary>
        /// Shared Project (*.shproj)
        /// </summary>
        Shared = 5,

        /// <summary>
        /// Database Project (*.dbproj)
        /// </summary>
        Database = 6,

        /// <summary>
        /// Docker Compose Project (*.dcproj)
        /// </summary>
        DockerCompose = 7,

        /// <summary>
        /// SQL Project (*.sqlproj)
        /// </summary>
        Sql = 8,

        /// <summary>
        /// JavaScript Application/ECMAScript Project (*.esproj)
        /// </summary>
        JavaScript = 9,

        /// <summary>
        /// .NET Core 2015 Project (*.xproj)
        /// </summary>
        NetCore2015 = 10,

        /// <summary>
        /// Common MSBuild Project (*.msbuildproj)
        /// </summary>
        Common = 11,

        /// <summary>
        /// Project (*.proj)
        /// </summary>
        Project = 12,

        /// <summary>
        /// Any project type which is not defined.
        /// </summary>
        Unknown = 0xFF
    }
}
