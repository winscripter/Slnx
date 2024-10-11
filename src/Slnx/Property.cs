namespace Slnx
{
    /// <summary>
    /// Represents a &lt;Property&gt; element.
    /// </summary>
    public sealed class Property
    {
        /// <summary>
        /// Name of the property.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Property value.
        /// </summary>
        public string? Value { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Property"/> class.
        /// </summary>
        /// <param name="name">Name of the property.</param>
        /// <param name="value">Property value.</param>
        public Property(string? name, string? value)
        {
            Name = name;
            Value = value;
        }

        /// <summary>
        /// Checks if the name of the property signifies an MSBuild Shared Items import.
        /// </summary>
        /// <remarks>
        /// Example:
        /// <code>
        ///   Path\To\ProjItems\File.projitems*{AnyGuidHere}*SharedItemsImports
        /// </code>
        /// </remarks>
        public bool IsMSBuildSharedItemsImports
        {
            get
            {
                // Example:
                // Path\To\ProjItems\File.projitems*{AnyGuidHere}*SharedItemsImports

                if (Name?.Contains('*') != true)
                {
                    return false;
                }

                string[] splitted = Name.Split('*');
                bool isGuid = Guid.TryParse(splitted[1], out _);

                if (!isGuid)
                {
                    return false;
                }

                if (!splitted[2].Equals("SharedItemsImports", StringComparison.CurrentCultureIgnoreCase))
                {
                    return false;
                }

                return true;
            }
        }
    }
}
