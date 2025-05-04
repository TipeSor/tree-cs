using System;
using System.IO;
using System.Linq;

namespace tree
{
    public static class Program
    {
        public static readonly string[] things = ["├", "└", "│  ", "   ", "──"];
        public static string[] Blacklist { get; set; } = [];

        public static void Main(string[] args)
        {
            Blacklist = args.Length == 0 ? Blacklist : args;

            string path = Environment.CurrentDirectory;
            Console.Clear();

            string name = Path.GetFileName(path).ToString();
            Console.WriteLine(name);
            PrintTree(path);
        }

        public static void PrintTree(string path, string prefix = "")
        {
            string[] dirs = [.. Directory.GetDirectories(path).OrderBy(static d => Path.GetFileName(d))];
            string[] files = [.. Directory.GetFiles(path).OrderBy(static f => Path.GetFileName(f))];
            string[] items = [.. dirs, .. files];

            items = [.. items.Where(static i => !Blacklist.Contains(Path.GetFileName(i)))];

            for (int i = 0; i < items.Length; i++)
            {
                bool isLast = i == items.Length - 1;
                string itemPath = items[i];
                string itemName = Path.GetRelativePath(path, itemPath);

                if (Directory.Exists(itemPath))
                {
                    string dirPath = itemPath;
                    string dirName = itemName;
                    string dirSymbol = isLast ? things[1] : things[0];
                    Console.WriteLine($"{prefix}{dirSymbol} {dirName}");

                    string newPrefix = $"{prefix}{(isLast ? things[3] : things[2])}";
                    PrintTree(dirPath, newPrefix);
                    continue;
                }
                string fileName = itemName;
                string fileSymbol = isLast ? things[1] : things[0];
                Console.WriteLine($"{prefix}{fileSymbol}{things[4]} {fileName}");
            }
        }
    }
}
