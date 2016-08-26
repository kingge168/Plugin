using System;
using System.IO;
using System.Security.AccessControl;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spider.Util
{
    public static class FileHelper
    {
        public static void MoveFiles(DirectoryInfo source, DirectoryInfo target, string searchPattern)
        {
            ArgumentValidator.Validate("source", source, arg => arg == null || !arg.Exists);
            ArgumentValidator.Validate("target", target, arg => arg == null);

            if (target.Exists)
            {
                target.Delete(true);
            }
            target.Create();

            FileInfo[] files;
            if (string.IsNullOrEmpty(searchPattern))
            {
                files = source.GetFiles();
            }
            else
            {
                files = source.GetFiles(searchPattern, SearchOption.AllDirectories);
            }

            foreach (FileInfo file in files)
            {
                string relativePath=file.FullName.Substring(source.FullName.Length+1);
                string fullFilePath = Path.Combine(target.FullName, relativePath);
                DirectoryInfo fullPathDir = new DirectoryInfo(Path.GetDirectoryName(fullFilePath));
                if (!fullPathDir.Exists)
                {
                    fullPathDir.Create();
                }
                file.CopyTo(fullFilePath,true);
            }
        }
    }
}
