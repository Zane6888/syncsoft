using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace syncsoft
{
    public static class Config
    {
        private static Dictionary<String, String> paths;//key: relative path, value: absolute path
        private static List<String> exclude;// absolute paths

        private static string appName = "syncsoft";
        private static string basePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), appName);

        private static string dirConfig = Path.Combine(basePath, "dir.cfg"); // one kvp per line, relative:absolute
        private static string excludeConfig = Path.Combine(basePath, "exclude.cfg"); //one excluded absolute path per line


        public static void init()
        {
            //TODO add general config
            paths = new Dictionary<string, string>();
            exclude = new List<string>();

            if (File.Exists(dirConfig))
            {
                using (StreamReader s = File.OpenText(dirConfig))
                {
                    String line;
                    while ((line = s.ReadLine()) != null)
                    {
                        String[] split = s.ReadLine().Split(':');
                        paths.Add(split[0], split[1]);
                    }
                }
            }

            if (File.Exists(excludeConfig))
            {
                using (StreamReader s = File.OpenText(dirConfig))
                {
                    String line;
                    while ((line = s.ReadLine()) != null)
                    {
                        exclude.Add(line);
                    }
                }
            }

        }

        public static bool isLinux
        {
            get
            {
                int p = (int)Environment.OSVersion.Platform;
                return (p == 4) || (p == 6) || (p == 128);
            }
        }

        /// <summary>
        /// Adds a path to track and writes it to the config file
        /// </summary>
        /// <param name="absolute">The local path to add</param>
        /// <param name="relative">The location on the syncbox</param>
        public static void addPath(String relative,String absolute)
        {
            paths.Add(relative, absolute);
            using(StreamWriter s = new StreamWriter(dirConfig,true))
            {
                s.WriteLine(relative + ":" + absolute);
            }
        }

        /// <summary>
        /// Adds a path to track and writes it to the config file. Uses the standard location on the Syncbox.
        /// </summary>
        /// <param name="absolute">Local path to add</param>
        public static void addPath(String absolute)
        {
            String[] split = absolute.Split(new Char[]{'/','\\'});
            String relative = "/" + split[split.Length - 1];
            addPath(relative, absolute);
        }

        /// <summary>
        /// Removes a path from tracking and writes the changes to the config.
        /// </summary>
        /// <param name="relative">Path on the Syncbox</param>
        public static void removePath(String relative)
        {
            if (!paths.ContainsKey(relative))
                throw new ArgumentException("can not remove non tracked path");
            paths.Remove(relative);

            using (StreamWriter s = new StreamWriter(dirConfig, false))
            {
                foreach(KeyValuePair<String,String> kvp in paths)
                    s.WriteLine(kvp.Key + ":" + kvp.Value);
            }
        }

        /// <summary>
        /// Adds a path to always exclude from syncing and writes the changes to the config.
        /// </summary>
        /// <param name="excludePath">Path to exclude</param>
        public static void addExclude(String excludePath)
        {
            exclude.Add(excludePath);
            using (StreamWriter s = new StreamWriter(excludeConfig, true))
            {
                s.WriteLine(excludePath);
            }
        }

        /// <summary>
        /// Removes a path from exclusion and writes the changes to the config.
        /// </summary>
        /// <param name="excludePath">Path to stop excluding</param>
        public static void removeExcludePath(String excludePath)
        {
            if (!exclude.Contains(excludePath))
                throw new ArgumentException("can not unexclude non excluded path");

            exclude.Remove(excludePath);
            using (StreamWriter s = new StreamWriter(dirConfig, false))
            {
                foreach (String path in exclude)
                    s.WriteLine(path);
            }
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
