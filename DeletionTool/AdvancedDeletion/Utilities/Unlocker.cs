using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace YYProject.AdvancedDeletion
{
    internal static class Unlocker
    {
        private static readonly Dictionary<String, String> _DeviceNameMap;

        //determines whether it's a file/diretory handle.
        private static readonly Int32 _HandleTypeStringOffset = 0x58 + IntPtr.Size * 2;
        //Gets the file name of the file/diretory handle.
        private static readonly Int32 _FileNameStringOffset = IntPtr.Size * 2;
        private const String _DevicePre = @"\Device\HarddiskVolume"; //Hard disk only. 
        private static readonly Int32 _HandleEntrySize = 8 + IntPtr.Size * 2;
        private static readonly Int32 _HandleInfoSize = _HandleEntrySize + IntPtr.Size;

        static Unlocker()
        {
            var drives = Environment.GetLogicalDrives();
            _DeviceNameMap = new Dictionary<String, String>(drives.Length);
            foreach (var item in drives)
            {
                string deviceName = item.Substring(0, 2);
                if (!NativeAPI.TryQueryDosDevice(deviceName, out var result))
                {
                    continue;
                }
                _DeviceNameMap.Add(result, deviceName);
            }
        }

        public static RmProcessInfo[] HolderList;

        public static void ReleaseObjects(String[] objectNames, Boolean IsFiles = true)
        {
            if (IsFiles)
            {
                HolderList = GetHolderList(objectNames);
            }
            if (HolderList == null)
            {
                return;
            }

            /*There's no need to define these structs, becouse we only need a little part of them.
             *http://www.geoffchappell.com/studies/windows/km/ntoskrnl/api/ex/sysinfo/handle.htm
             *http://www.geoffchappell.com/studies/windows/km/ntoskrnl/api/ex/sysinfo/handle_table_entry.htm?ts=0,67 */

            if (!NativeAPI.TryQuerySystemInformation(SystemInfomationClass.SystemHandleInformation, out var info, _HandleInfoSize))
            {
                return;
            }

            #region X86 or X64
            Int32 count = IntPtr.Size == 4 ? Marshal.ReadInt32(info.DangerousGetHandle()) : (Int32)Marshal.ReadInt64(info.DangerousGetHandle());
            var longHandles = info.DangerousGetHandle() + IntPtr.Size;
            #endregion

            Parallel.For(0, count, i =>
            {
                var pid = Marshal.ReadInt32(longHandles, i * _HandleEntrySize);
                var handle = Marshal.ReadInt16(longHandles, i * _HandleEntrySize + 6);
                foreach (var item in HolderList)
                {
                    if (pid == item.Process.ProcessId && IsObject(pid, (IntPtr)handle, objectNames))
                    {
                        TryReleaseHandle(pid, (IntPtr)handle);
                    }
                }
                //Parallel.ForEach<RmProcessInfo>(HolderList, item =>
                //{
                //    if (pid == item.Process.ProcessId && IsObject(pid, (IntPtr)handle, objectNames))
                //    {
                //            TryReleaseHandle(pid, (IntPtr)handle);
                //    }
                //});
            });
            if (!IsFiles)
            {
                HolderList = null;
            }
            info.Close();
        }


        private static RmProcessInfo[] GetHolderList(String[] fileNames)
        {
            try
            {
                RmHelper.SharedInstance.StartSession();
                if (RmHelper.SharedInstance.TryGetHolderListOfFiles(fileNames, out var holderList))
                {
                    return holderList;
                };
                return null;
            }
            finally
            {

                RmHelper.SharedInstance.EndSession();
            }
        }

        //determines whether it's object.
        private static Boolean IsObject(Int32 pID, IntPtr remoteHandle, IEnumerable<String> fileNames)
        {
            NativeNormalHandle handle = null;
            try
            {
                handle = HandleCopy(pID, remoteHandle);
                if (handle != null && IsFileOrDirectoryHandle(handle.DangerousGetHandle()))
                {
                    var fileName = GetFileNameFromHandle(handle.DangerousGetHandle());
                    foreach (var item in fileNames)
                    {
                        if (item.Equals(fileName))
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
            finally
            {
                if (handle != null)
                {
                    handle.Close();
                }
            }

        }

        private static Boolean TryReleaseHandle(Int32 pID, IntPtr remoteHandle)
        {
            NativeNormalHandle handle = null;
            try
            {
                handle = HandleCopy(pID, remoteHandle, true);
                if (handle != null)
                {

                    return true;
                }
                return false;
            }
            finally
            {
                if (handle != null)
                {
                    handle.Close();
                }

            }

        }

        private static NativeNormalHandle HandleCopy(Int32 pID, IntPtr remoteHandle, Boolean removeSourceHandle = false)
        {
            NativeNormalHandle pHandle = null;
            try
            {
                if (NativeAPI.TryOpenProcess(pID, out pHandle) && NativeAPI.TryDuplicateHandle(pHandle, remoteHandle, NativeAPI.GetCurrentProcess(), out var lHandle, removeSourceHandle))
                {
                    return lHandle;
                }
                return null;
            }

            finally
            {
                if (pHandle != null)
                {
                    pHandle.Close();
                }
            }
        }

        private static Boolean IsFileOrDirectoryHandle(IntPtr localHandle)
        {
            AllocatedMemoryHandle handleType = null;
            try
            {

                if (NativeAPI.TryNtQueryObject(localHandle, ObjectInfoamtionClass.ObjectTypeInformation, out handleType))
                {
                    //https://msdn.microsoft.com/en-us/library/bb432383(
                    var length = Marshal.ReadInt16(handleType.DangerousGetHandle()) >> 1;
                    var result = Marshal.PtrToStringUni(handleType.DangerousGetHandle() + _HandleTypeStringOffset, length);
                    return result.Equals("File") || result.Equals("Directory");
                }
                return false;
            }
            finally
            {
                if (handleType != null)
                {
                    handleType.Close();
                }
            }

        }

        private static String GetFileNameFromHandle(IntPtr localHandle)
        {
            AllocatedMemoryHandle handleType = null;
            try
            {
                if (NativeAPI.TryNtQueryObject(localHandle, ObjectInfoamtionClass.ObjectNameInformation, out handleType))
                {
                    //https://msdn.microsoft.com/en-us/library/windows/hardware/ff550990
                    var length = Marshal.ReadInt16(handleType.DangerousGetHandle()) >> 1;
                    var result = Marshal.PtrToStringUni(handleType.DangerousGetHandle() + _FileNameStringOffset, length);
                    if (result.LastIndexOf(_DevicePre) != 0)
                    {
                        return null;

                    }
                    var dosName = result.Substring(0, result.IndexOf(@"\", _DevicePre.Length));
                    var ret = _DeviceNameMap.TryGetValue(dosName, out var ntName);
                    if (!ret || !dosName.StartsWith(_DevicePre))
                    {
                        return null;
                    }
                    return @"\\?\" + result.Replace(dosName, ntName);
                }
                return null;
            }
            catch
            {
                return null;
            }
            finally
            {
                if (handleType != null)
                {
                    handleType.Close();
                }
            }

        }
    }
}
