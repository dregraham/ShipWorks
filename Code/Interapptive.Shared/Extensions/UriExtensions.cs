using System;

namespace Interapptive.Shared.Extensions
{
    /// <summary>
    /// Uri extension methods
    /// </summary>
    public static class UriExtensions
    {
        /// <summary>
        /// Add to a Uri's path without worrying about slashes
        /// </summary>
        public static Uri AddToPath(this Uri uri, string path)
        {
            UriBuilder builder = new UriBuilder(uri);

            string trimmedPath = path.Trim('/');

            builder.Path = $"{builder.Path.TrimEnd('/')}/{trimmedPath}";

            return builder.Uri;
        }
    }
}
