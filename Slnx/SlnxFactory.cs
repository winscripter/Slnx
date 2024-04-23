namespace Slnx
{
    /// <summary>
    /// Builds a SLNX file.
    /// </summary>
    public class SlnxFactory
    {
        /// <summary>
        /// All folders.
        /// </summary>
        public List<Folder> Folders = new();

        /// <summary>
        /// All projects.
        /// </summary>
        public List<Project> Projects = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="SlnxFactory" /> class.
        /// </summary>
        public SlnxFactory()
        {
        }
        
        /// <summary>
        /// Converts this instance of <see cref="SlnxFactory" /> into <see cref="SlnxModel" />.
        /// </summary>
        /// <returns>An instance of <see cref="SlnxModel" />.</returns>
        public SlnxModel AsModel() => new(Projects.ToArray(), Folders.ToArray());
    }
}
