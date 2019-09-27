using System;
using System.IO;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace EOMoutputfixer
{
    class Program
    {
        static int FirstResidueNumber = 233;

        static void Main(string[] args)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var files = assembly.GetManifestResourceNames();
            
            var dir = System.IO.Directory.GetCurrentDirectory();

            Console.WriteLine("Starting...");
            Console.WriteLine("Writing files to: " + dir);

            foreach (var fn in files)
            {
                Console.Write("Processing file: " + fn.Replace("EOMoutputfixer.PDBs.", ""));

                string filePath = System.IO.Path.Combine(dir, "PDBs", fn.Replace("EOMoutputfixer.PDBs.", ""));
                var lines = File.ReadAllLines(filePath);

                List<string> newlines = new List<string>();

                int index = FirstResidueNumber;

                foreach (var line in lines)
                {
                    if (!line.Contains("CA")) continue;

                    var l = line;

                    l = l.Overwrite(23, "      ");
                    l = l.Overwrite(23, index.ToString());

                    newlines.Add(l);

                    index++;
                }

                var text = "";
                foreach (var l in newlines) text += l + Environment.NewLine;

                File.WriteAllText(fn.Replace("EOMoutputfixer.PDBs.", ""), text.Trim());

                Console.WriteLine(" => done");
            }

            Console.WriteLine("Complete");
        }

        
    }

    public static class Extensions
    {
        public static string Overwrite(this string text, int position, string new_text)
        {
            return text.Substring(0, position) + new_text + text.Substring(position + new_text.Length);
        }
    }
}
