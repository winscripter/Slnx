namespace Slnx
{
    /// <summary>
    /// Represents a "&lt;Configuration&gt;" like tag.
    /// </summary>
    public readonly struct DescendantConfiguration
    {
        /// <summary>
        /// Solution where this configuration applies to.
        /// </summary>
        public string? Solution { get; init; }

        /// <summary>
        /// Project where this configuration applies to.
        /// </summary>
        public string? Project { get; init; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DescendantConfiguration" /> struct.
        /// </summary>
        /// <param name="solution">Solution where this configuration applies to.</param>
        /// <param name="project">Project where this configuration applies to.</param>
        public DescendantConfiguration(string? solution, string? project)
        {
            Solution = solution;
            Project = project;
        }
    }
}
