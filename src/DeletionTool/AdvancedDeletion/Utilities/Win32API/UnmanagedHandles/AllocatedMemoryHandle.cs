using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace YYProject.AdvancedDeletion
{
    /// <summary>
    ///  Provides a handle type to manage the unmanaged memory pointer. 
    /// </summary>
   internal sealed class AllocatedMemoryHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        private Int32 _Size;

        /// <summary>
        /// The memory size that has been allocated.
        /// </summary>
        public Int32 Size => _Size;
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="size">The memory size to be allocated.</param>
        public AllocatedMemoryHandle(Int32 size) : base(true)
        {
            _Size = size;
            this.handle = Marshal.AllocHGlobal(size);
        }
        /// <summary>
        /// Change the memory size.
        /// </summary>
        /// <param name="newSize">The new memory size.</param>
        public void ChangeMemorySize(Int32 newSize)
        {
            if (!this.IsClosed)
            {
                Marshal.FreeHGlobal(this.handle);
            }
            this.handle = Marshal.AllocHGlobal(newSize);
            _Size = newSize;
        }

        protected override Boolean ReleaseHandle()
        {
            try
            {
                Marshal.FreeHGlobal(this.handle);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
