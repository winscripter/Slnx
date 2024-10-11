namespace Slnx
{
    /// <summary>
    /// Extension methods for <see cref="ProjectType" />.
    /// </summary>
    public static class ProjectTypeExtensions
    {
        /// <summary>
        /// Returns the extension for the project type.
        /// </summary>
        /// <param name="projectType">Type of the project.</param>
        /// <returns>A file extension for that project type, excluding the period.</returns>
        public static string GetExtension(this ProjectType projectType)
            => (byte)projectType switch
            {
                0 => "csproj",
                1 => "vcxproj",
                2 => "vbproj",
                3 => "fsproj",
                4 => "vdproj",
                5 => "shproj",
                6 => "dbproj",
                7 => "dcproj",
                8 => "sqlproj",
                9 => "esproj",
                10 => "xproj",
                11 => "msbuildproj",
                12 => "proj",
                _ => string.Empty
            };

        /// <summary>
        /// Returns the project type for the project extension.
        /// </summary>
        /// <param name="extension">Extension of the project, excluding the period.</param>
        /// <returns>A type of the project from the extension.</returns>
        public static ProjectType FromExtension(this string extension)
            => (ProjectType)(byte)(extension.ToLowerInvariant() switch
            {
                "csproj" => 0,
                "vcxproj" => 1,
                "vbproj" => 2,
                "fsproj" => 3,
                "vdproj" => 4,
                "shproj" => 5,
                "dbproj" => 6,
                "dcproj" => 7,
                "sqlproj" => 8,
                "esproj" => 9,
                "xproj" => 10,
                "msbuildproj" => 11,
                "proj" => 12,
                _ => 0xFF
            });
    }
}
