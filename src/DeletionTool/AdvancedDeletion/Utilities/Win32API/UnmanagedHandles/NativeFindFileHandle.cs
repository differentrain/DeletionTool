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
    /// Provides a handle type to manage the find-file-handle, which was openned by FindFirstFile API. 
    /// </summary>
    [SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
    internal sealed class NativeFindFileHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        private NativeFindFileHandle() : base(true) { }

        protected override Boolean ReleaseHandle()
        {
            return NativeAPI.TryCloseFindHandle(this.handle);
        }
    }
}
