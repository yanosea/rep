using rep.Libs.IO;
using System;

namespace rep.Libs
{
    internal static class Initializer
    {
        internal static void Initialize()
        {
            // start log
            Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                // create necessary directories
                IOHelper.CreateRepDirectories();

                // create necessary files
                IOHelper.CreateRepFiles();

                // remove empty error log files
                Logger.Instance.DeleteAllEmptyErrorLogFiles();

                // foldering old log files
                Logger.Instance.FolderingOldLogFiles();

                // initialize data holder
                DataHolder.Instance.Initialize();
            }
            catch (Exception ex)
            {
                // if exception occured, set error flag
                DataHolder.Instance.SetInitializedSuccessfully(false);

                // fatal log
                Logger.Instance.TraceFatal(Resources.Text.Components.TextLogTagBackendFatal, Resources.Text.LogMessages.FailureInitializing, ex);
            }
            finally
            {
                // end log
                Logger.Instance.TraceDebug(Resources.Text.Components.TextLogEnd);
            }
        }
    }
}
