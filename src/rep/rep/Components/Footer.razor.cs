using System;
using System.Linq;
using System.Reflection;

namespace rep.Components
{
    /// <summary>
    /// Footer
    /// </summary>
    public partial class Footer
    {
        #region readonlys

        /// <summary>
        /// copyright
        /// </summary>
        private static readonly string copyright = $"© {DateTime.Now.ToString("yyyy")} {Assembly.GetEntryAssembly().GetCustomAttributes<AssemblyCopyrightAttribute>().FirstOrDefault().Copyright}";

        /// <summary>
        /// repositoryUrl
        /// </summary>
        private static readonly string repositoryUrl = Assembly.GetEntryAssembly().GetCustomAttributes<AssemblyMetadataAttribute>().Where(x => x.Key == "RepositoryUrl").FirstOrDefault().Value;

        /// <summary>
        /// contact
        /// </summary>
        private static readonly string contact = $"mailto:{Resources.Text.Components.TextContact}";

        /// <summary>
        /// version
        /// </summary>
        private static readonly string version = $"v{Libs.BackendUtility.GetCurrentVersion()}";

        #endregion
    }
}
