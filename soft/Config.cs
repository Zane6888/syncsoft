using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace syncsoft
{
    public static class Config
    {
        private static Dictionary<String, String> paths;//key: relative path, value: absolute path
        private static List<String> exclude;// absolute paths

        public static void init()
        {
            //TODO read from config file
            paths = new Dictionary<string, string>();
            exclude = new List<string>();
        }

        public static String getAbsolute(String relative)
        {
            String s;
            paths.TryGetValue(relative,out s);

            return s != String.Empty ? s : null;
        }

        public static String getRelative(String absolute)
        {
            if (!paths.ContainsValue(absolute))
                return null;

            return paths.First(x => x.Value == absolute).Key;
        }

        public static List<String> getAllAbsolute()
        {
            return paths.Values.ToList();
        }

        public static List<String> getAllRelative()
        {
            return paths.Keys.ToList();
        }

        public static List<String> getAllExclusions()
        {
            return new List<string>(exclude);
        }
    }
}
