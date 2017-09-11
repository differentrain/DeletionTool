using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace YYProject.AdvancedDeletion
{



    /// <summary>
    /// Provides a static class for file/directory deletion.
    /// <para> Notice that this class is not complete, and very dependent on front-end. So it's unsuitable as an independent libary, unless there are enough judgement and error handling before(when) using it.</para>
    /// </summary>
    public static class AdvancedDeletion
    {
        private const String _LongPathPrefix = @"\\?\"; //Long path prefix

        private static List<String> _OccupiedFiles = StringListPool.Rent();
        private static List<String> _OccupiedDirectories = StringListPool.Rent();

        /// <summary>
        /// Delete a file or directory.
        /// </summary>
        /// <param name="path">The path of a file or a directory.</param>
        public static void Delete(String path)
        {
            InnerDeleteObject(path);

        }

        /// <summary>
        /// Determines whether the specified path exists.
        /// </summary>
        /// <param name="path">Object path.</param>
        /// <param name="mayHasDot">Determines whether the specified path may end with some dots.</param>
        /// <returns></returns>
        public static Boolean IsExists(String path, out Boolean mayHasDot)
        {
            mayHasDot = false;
            if (NativeAPI.IsFileExist(@"\\?\" + path, out var error))
            {
                return true;
            }
            if (error == NativeAPI.ERROR_FILE_NOT_FOUND)
            {
                mayHasDot = true;
            }
            return false;
        }


        private static void InnerDeleteObject(String path)
        {
            _OccupiedFiles.Clear();
            _OccupiedDirectories.Clear();
            path = @"\\?\" + path; //UNC Path
            var objType = GetPathType(path);
            if (!objType.HasValue)
            {
                return;
            }
            if (objType.Value.dwFileAttributes.HasFlag(FileAttributes.Directory))
            {
                InnerDeleteDirectory(path);
            }
            else
            {
                InnerDeleteFile(path, objType.Value.dwFileAttributes);
                DeleteOccupiedFiles();
            }
        }
        // File or directory or invalid path?
        private static Win32FileAttributeData? GetPathType(String path)
        {
            if (NativeAPI.IsFileExist(path, out var error))
            {
                if (NativeAPI.TryGetFileAttributes(path, out var fileAttributes))
                {
                    return fileAttributes;
                }
            }
            return null;
        }

        private static void InnerDeleteFile(String path, FileAttributes fileAttributes)
        {
            if (fileAttributes.HasFlag(FileAttributes.ReadOnly))
            {
                --fileAttributes; //deselect Read-Only
                NativeAPI.SetFileAttributes(path, fileAttributes);
            }
            if (!NativeAPI.DeleteFile(path, out var occupied) && occupied)
            {
                _OccupiedFiles.Add(path);
            }
        }

        //Notice that win api FindFirstFile does not support the path end with "\".
        private static void InnerDeleteDirectory(String path)
        {
            var dirList = TraverseDirectory(path);  // Traverse directory and delete files. 
            DeleteOccupiedFiles(); //Delete occupied files
            foreach (var item in dirList) //Delete directories.
            {
               var b= NativeAPI.RemoveDirectory(item,out var occupied);
                if (occupied)
                {
                    _OccupiedDirectories.Add(item);
                }
            }
            DeleteOccupiedDirectories(dirList); //Delete occupied directories. 
        }

        private static Stack<String> TraverseDirectory(String path)
        {
            var paths = StringListPool.Rent();
            var result = new Stack<String>();
            paths.Add(path);
            TraverseDirectoryPower(paths, result);
            return result;
        }

        private static void TraverseDirectoryPower(List<String> paths, Stack<String> allPaths)
        {
            if (paths == null)
            {
                return;
            }
            Parallel.ForEach<String>(paths, item =>
            {
                allPaths.Push(item);
                TraverseDirectoryPower(GetDirectories(item), allPaths);
            });
            StringListPool.Return(paths);
        }

        private static List<String> GetDirectories(String path)
        {
            List<String> paths;
            String findPath;
            if (NativeAPI.TryFindFirstFile(path + @"\*", out var findHandle, out var foo)) // in
            {
                paths = StringListPool.Rent();
                while (true)
                {
                    if (!NativeAPI.TryFindNextFile(findHandle, out var findInfo))
                    {
                        break;  //error or end
                    }
                    findPath = path + @"\" + findInfo.cFileName;

                    if (!findInfo.FileAttributes.dwFileAttributes.HasFlag(FileAttributes.Directory))
                    {
                        InnerDeleteFile(findPath, findInfo.FileAttributes.dwFileAttributes);
                    }
                    else if (findInfo.cFileName != ".." && findInfo.cFileName != ".") //ignores ".." or "." .
                    {
                        paths.Add(findPath);

                    }
                }
            }
            else
            {
                paths = null;
            }
            if (findHandle != null)
            {
                findHandle.Close();
            }
            return paths.ToList();
        }

        private static void DeleteOccupiedFiles()
        {
            if (_OccupiedFiles.Count > 0)
            {
                Unlocker.ReleaseObjects(_OccupiedFiles.ToArray());
               
                foreach (var item in _OccupiedFiles)
                {
                    NativeAPI.DeleteFile(item, out var kd);
                
                }
            }
        
        }

        private static void DeleteOccupiedDirectories(Stack<String> paths)
        {
            if (_OccupiedDirectories.Count > 0)
            {
                Unlocker.ReleaseObjects(_OccupiedDirectories.ToArray(),false);

                foreach (var item in paths) //all in list
                {
                  NativeAPI.RemoveDirectory(item,out var foo);
                }

            }
        }


    }
}
