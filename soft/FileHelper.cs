using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace syncsoft
{
    static class FileHelper
    {
        private static MD5 md5 = MD5.Create();

        public static String GetAbsolute(String relative)
        {
            String absolute = Config.getAbsolute(relative);
            if (absolute != null)
                return absolute;

            List<String> relPaths = Config.getAllRelative();

            String relBase = relPaths.Find(s => relative.Contains(s));

            return relative.Replace(relBase,Config.getAbsolute(relBase));
        }

        public static String GetRelative(String absolute)
        {
            String relative = Config.getRelative(absolute);
            if (relative != null)
                return relative;

            List<String> absPaths = Config.getAllAbsolute();

            String absBase = absPaths.Find(s => absolute.Contains(s));

            return absolute.Replace(absBase, Config.getAbsolute(absBase));
        }

        public static Byte[] GetMD5(String path)
        {
            using (Stream st = File.OpenRead(path))
            {
                return md5.ComputeHash(st);
            }
           
        }

        public static bool ContainsFile(this List<String> list, String file)
        {
            if (list.Count == 0)
                return false;
            if (list.Contains(file))
                return true;
            try{
                list.First(s => file.Contains(s));
                return true;
            }
            catch(Exception)
            {
            return false;
            }
        }


    }
}
