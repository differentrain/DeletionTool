using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace YYProject.AdvancedDeletion
{
    /// <summary>
    /// Provides a handle type to manage the normal unsafe handle. 
    /// </summary>
    [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
    internal sealed class NativeNormalHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        private NativeNormalHandle() : base(true) { }

        protected override Boolean ReleaseHandle()
        {
            var q = NativeAPI.TryCloseHandle(this.handle);
            return q;
        }
    }
}
