using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace YYProject.AdvancedDeletion
{
    internal static class NativeAPI
    {
        #region Win32APIs

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern Boolean GetFileAttributesExW(String lpFileName, GetFileExInfoLevels fInfoLevelId, out Win32FileAttributeData lpFileInformation);
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern Boolean FindClose(IntPtr hFindFile);
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern NativeFindFileHandle FindFirstFileExW(String lpFileName, FindExInfoLevels fInfoLevelId, out Win32FindData lpFindFileData, FindExSearchOps fSearchOp, IntPtr lpSearchFilter, AdditionalFlags dwAdditionalFlags);
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern Boolean FindNextFileW(IntPtr hFindFile, out Win32FindData lpFindFileData);
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern Boolean DeleteFileW(String lpFileName);
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern Boolean RemoveDirectoryW(String lpPathName);
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern Boolean SetFileAttributesW(String lpFileName, FileAttributes dwFileAttributes);
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern Boolean QueryDosDevice(String lpDeviceName, StringBuilder lpTargetPath, Int32 ucchMax);
        [DllImport("ntdll.dll")]
        private static extern NtStatus NtQuerySystemInformation(SystemInfomationClass InfoClass, [Out] IntPtr Info, Int32 Size, out Int32 Length);
        [DllImport("kernel32.dll", EntryPoint = "CloseHandle", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern Boolean NativeCloseHandle(IntPtr hObject);
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern NativeNormalHandle OpenProcess(ProcessAccessFlags processAccess, Boolean bInheritHandle, Int32 processId);
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern Boolean DuplicateHandle(IntPtr hSourceProcessHandle, IntPtr hSourceHandle, IntPtr hTargetProcessHandle, out NativeNormalHandle lpTargetHandle, UInt32 dwDesiredAccess, Boolean bInheritHandle, DuplicateOptions dwOptions);
        [DllImport("ntdll.dll")]
        private static extern NtStatus NtQueryObject(IntPtr objectHandle, ObjectInfoamtionClass informationClass, [Out] IntPtr informationPtr, Int32 informationLength, out Int32 Length);
        [DllImport("Rstrtmgr.dll", CharSet = CharSet.Unicode)]
        private static extern Int32 RmRegisterResources(UInt32 dwSessionHandle, UInt32 nFiles, String[] rgsFilenames, UInt32 nApplications, RmUniqueProcess[] rgApplications, UInt32 nServices, String[] rgsServiceNames);
        [DllImport("Rstrtmgr.dll", CharSet = CharSet.Unicode)]
        private static extern Int32 RmGetList(UInt32 dwSessionHandle, out UInt32 pnProcInfoNeeded, ref UInt32 pnProcInfo, [Out] RmProcessInfo[] rgAffectedApps, out RmRebootReason lpdwRebootReasons);
        [DllImport("rstrtmgr.dll", EntryPoint = "RmEndSession")]
        private static extern Int32 NativeRmEndSession(UInt32 pSessionHandle);
        [DllImport("Rstrtmgr.dll")]
        private static extern Int32 RmStartSession(out UInt32 pSessionHandle, UInt32 dwSessionFlags, StringBuilder strSessionKey);
        [DllImport("kernel32.dll")]
        public static extern IntPtr GetCurrentProcess();

        #endregion

        #region Win32ErrorCodes
        private const Int32 ERROR_SHARING_VIOLATION = 0x20;
        private const Int32 NO_MORE_FILES = 0x12; //It's not an error. 
        public const Int32 ERROR_FILE_NOT_FOUND = 0x2;
        public const Int32 ERROR_PATH_NOT_FOUND = 0x3;
        private const Int32 ERROR_MORE_DATA = 234;
        #endregion

        #region The fields below is thread-safe in this project.
        private static StringBuilder _SessionKey = new StringBuilder(Marshal.SizeOf(Guid.NewGuid()) * 2 + 1);
        private static StringBuilder _DosDeviceName = new StringBuilder(260);
        #endregion

        public static Boolean TryRmStartSession(out UInt32 sessionHandle, out String sessionKey)
        {
            _SessionKey.Clear();
            var error = RmStartSession(out  sessionHandle, 0, _SessionKey);
            sessionKey = _SessionKey.ToString();
            return error == 0 ? true : false;
        }

        public static Boolean TryRmEndSession(UInt32 pSessionHandle)
        {
            return NativeRmEndSession(pSessionHandle) == 0 ? true : false;
        }

        public static Boolean TryRmGetList(UInt32 sessionHandle, out RmRebootReason rebootReasons, out RmProcessInfo[] affectedApps)
        {
            affectedApps =null;
            var nCount = 0U;
            var error = RmGetList(sessionHandle, out var nlength, ref nCount, affectedApps, out rebootReasons);
            while (error == ERROR_MORE_DATA)
            {
                affectedApps = new RmProcessInfo[nlength];
                nCount = (UInt32)affectedApps.Length;
                error = RmGetList(sessionHandle, out nlength, ref nCount, affectedApps, out rebootReasons);
            }
            return error == 0 ? true : false;
        }

        public static Boolean TryRmRegisterResources(UInt32 dwSessionHandle, String[] rgsFilenames, RmUniqueProcess[] rgApplications, String[] rgsServiceNames)
        {

            var error = RmRegisterResources(dwSessionHandle,
              rgsFilenames == null ? 0 : (UInt32)rgsFilenames.Length, rgsFilenames,
              rgApplications == null ? 0 : (UInt32)rgApplications.Length, rgApplications,
              rgsServiceNames == null ? 0 : (UInt32)rgsServiceNames.Length, rgsServiceNames);
            return error == 0 ? true : false;
        }

        public static Boolean TryNtQueryObject(IntPtr objectHandle, ObjectInfoamtionClass informationClass, out AllocatedMemoryHandle informationPtr, Int32 startSize = 0)
        {
            informationPtr = new AllocatedMemoryHandle(startSize);
            var info = NtQueryObject(objectHandle, informationClass, informationPtr.DangerousGetHandle(), informationPtr.Size, out var actualSize);
            if (info == NtStatus.InvalidHandle)
            {
                if (informationPtr.IsInvalid) informationPtr.Close();
                informationPtr = null;
                return false;
            }
            while (info == NtStatus.InfoLengthMismatch)
            {
                informationPtr.ChangeMemorySize(actualSize);
                info = NtQueryObject(objectHandle, informationClass, informationPtr.DangerousGetHandle(), informationPtr.Size, out actualSize);
            }
            if (info != NtStatus.Success)
            {
                if (informationPtr.IsInvalid) informationPtr.Close();
                informationPtr = null;
                return false;
            }
            return true;
        }

        public static Boolean TryDuplicateHandle(NativeNormalHandle sourceProcessHandle, IntPtr sourceHandle, IntPtr targetProcessHandle, out NativeNormalHandle targetHandle, Boolean removeSourceHandle = false)
        {
            if (!DuplicateHandle(sourceProcessHandle.DangerousGetHandle(), sourceHandle, targetProcessHandle, out targetHandle, 0, false, DuplicateOptions.DuplicateSameAccess | (removeSourceHandle ? DuplicateOptions.DuplicateCloseSource : DuplicateOptions.None)))
            {
                if (targetHandle.IsInvalid)
                {
                    targetHandle.Close();
                }
                targetHandle = null;
                return false;
            }
            return true;
        }

        public static Boolean TryOpenProcess(Int32 pID, out NativeNormalHandle handle, Boolean inheritHandle = false)
        {
            handle = OpenProcess(ProcessAccessFlags.All, inheritHandle, pID);
            if (handle.IsInvalid)
            {
                handle = null;
                return false;
            }
            return true;
        }

        public static Boolean TryCloseHandle(IntPtr handle)
        {
            return NativeCloseHandle(handle);
        }

        public static Boolean TryQuerySystemInformation(SystemInfomationClass infoClass, out AllocatedMemoryHandle infoHandle, Int32 startSize = 0)
        {
            infoHandle = new AllocatedMemoryHandle(startSize);
            var info = NtQuerySystemInformation(infoClass, infoHandle.DangerousGetHandle(), infoHandle.Size, out var actualSize);
            while (info == NtStatus.InfoLengthMismatch)
            {
                infoHandle.ChangeMemorySize(actualSize);
                info = NativeAPI.NtQuerySystemInformation(infoClass, infoHandle.DangerousGetHandle(), infoHandle.Size, out actualSize);
            }
            if (info != NtStatus.Success)
            {
                if (infoHandle.IsInvalid)
                {
                    infoHandle.Close();
                }
                infoHandle = null;
                return false;
            }
            return true;

        }

        public static Boolean TryQueryDosDevice(String ntDeviceName, out String result)
        {
            result = null;
            _DosDeviceName.Clear();
            if (!QueryDosDevice(ntDeviceName, _DosDeviceName, _DosDeviceName.Capacity))
            {
                return false;
            }
            result = _DosDeviceName.ToString();
            return true;
        }

        public static Boolean SetFileAttributes(String path, FileAttributes fileAttributes)
        {
            return SetFileAttributesW(path, fileAttributes);
        }

        public static Boolean TryGetFileAttributes(String path, out Win32FileAttributeData fileAttribute)
        {
            return GetFileAttributesExW(path, GetFileExInfoLevels.GetFileExInfoStandard, out fileAttribute);
        }

        public static Boolean TryCloseFindHandle(IntPtr findHandle)
        {
            return FindClose(findHandle);
        }

        public static Boolean TryFindFirstFile(String path, out NativeFindFileHandle findHandle, out Win32FindData findData)
        {
            findHandle = FindFirstFileExW(path, FindExInfoLevels.FindExInfoBasic, out findData, FindExSearchOps.FindExSearchNameMatch, IntPtr.Zero, AdditionalFlags.FindFirstEXLargeFetch);
            if (findHandle.IsInvalid)
            {
                findHandle = null;
                return false;
            }
            return true;
        }

        public static Boolean IsFileExist(String path, out Int32 error)
        {
            error = 0;
            using (var handle = FindFirstFileExW(path, FindExInfoLevels.FindExInfoBasic, out var findData, FindExSearchOps.FindExSearchNameMatch, IntPtr.Zero, AdditionalFlags.FindFirstEXLargeFetch))
            {
                if (handle.IsInvalid)
                {
                    error = Marshal.GetLastWin32Error();
                    return false;
                }
                return true;
            }
        }

        public static Boolean TryFindNextFile(NativeFindFileHandle findHandle, out Win32FindData findData)
        {
            if (!FindNextFileW(findHandle.DangerousGetHandle(), out findData))
            {
                var error = Marshal.GetLastWin32Error();
                findData.NoMoreFiles = error == NO_MORE_FILES;
                return false;
            }
            else
            {
                findData.NoMoreFiles = false;
            }
            return true;
        }

        public static Boolean DeleteFile(String path)
        {
            return DeleteFileW(path);
        }

        public static Boolean DeleteFile(String path, out Boolean isOccupied)
        {
            if (!DeleteFileW(path))
            {
                var error = Marshal.GetLastWin32Error();
                if (error == ERROR_SHARING_VIOLATION)
                {
                    isOccupied = true;
                }
                else
                {
                    isOccupied = false;
                }
                return false;
            }
            isOccupied = false;
            return true;
        }

  
        public static Boolean RemoveDirectory(String path, out Boolean isOccupied)
        {
            if (!RemoveDirectoryW(path))
            {
                var error = Marshal.GetLastWin32Error();
                if (error == ERROR_SHARING_VIOLATION)
                {
                    isOccupied = true;
                }
                else
                {
                    isOccupied = false;
                }
                return false;

            }
            isOccupied = false;
            return true;
        }
    }
}
