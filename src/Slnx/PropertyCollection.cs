namespace Slnx
{
    /// <summary>
    /// Represents the &lt;Properties&gt; element.
    /// </summary>
    public sealed class PropertyCollection
    {
        /// <summary>
        /// Name of the property collection (if present). Example: <c>SharedMSBuildProjectFiles</c>.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Scope of the property collection (if present). Example: <c>PreLoad</c>.
        /// </summary>
        public string? Scope { get; set; }

        /// <summary>
        /// All descendant properties.
        /// </summary>
        public List<Property> Properties { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyCollection"/>.
        /// </summary>
        /// <param name="name">Name of the property collection (if present).</param>
        /// <param name="scope">Scope of the property collection (if present).</param>
        /// <param name="properties">All descendant properties.</param>
        public PropertyCollection(string? name, string? scope, List<Property> properties)
        {
            Name = name;
            Scope = scope;
            Properties = properties;
        }
    }
}
