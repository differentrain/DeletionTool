using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YYProject.AdvancedDeletion
{
    /// <summary>
    /// Restart Manager helper.
    /// <para>See https://msdn.microsoft.com/en-us/library/windows/desktop/cc948910. </para>
    /// </summary>
    internal abstract class RmHelper
    {
        private static RmHelper _Shared = new DefaultRmHelper();
        private Boolean _IsClosed = true;
        protected UInt32 _SessionHandle;
        protected String _SessionKey;

  
        /// <summary>
        /// Gets a shared instance of <see cref="RmHelper"/>.
        /// </summary>
        public static RmHelper SharedInstance => _Shared;

        /// <summary>
        /// Gets whether the session is finished.
        /// </summary>
        public Boolean IsClosed => _IsClosed;

        /// <summary>
        /// Start the session.
        /// </summary>
        public void StartSession()
        {
            if (_IsClosed == true)
            {
                NativeAPI.TryRmStartSession(out _SessionHandle, out  _SessionKey);
                _IsClosed = false;
            }

        }
        /// <summary>
        /// Close the session.
        /// </summary>
        public void EndSession()
        {
            if (_IsClosed == false)
            {
                NativeAPI.TryRmEndSession(_SessionHandle);
                _IsClosed = true;
            }

        }
        ~RmHelper()
        {
            EndSession();
        }

        /// <summary>
        ///  Gets a <see cref="RmProcessInfo"/>[] contains all process that are using object files.
        /// </summary>
        /// <param name="fileNames">An file path list.</param>
        /// <param name="suggestion">Result.</param>
        public abstract Boolean TryGetHolderListOfFiles(String[] fileNames, out RmProcessInfo[] result);


    }
}
