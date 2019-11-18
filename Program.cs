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
            Console.Write("Broken EOM pdb file folder: ");
            string pdbpathstring = Console.ReadLine().Trim();

            while (!Path.IsPathFullyQualified(pdbpathstring))
            {
                Console.Write(">Path for PDBs: ");
                pdbpathstring = Console.ReadLine().Trim();
            }

            pdbpathstring = pdbpathstring.Replace(@"\", "");

            Console.Write("Output folder: ");
            var outputfolderstring = Console.ReadLine().Trim();

            while (!Path.IsPathFullyQualified(outputfolderstring))
            {
                Console.Write(">Output folder: ");
                outputfolderstring = Console.ReadLine().Trim();
            }

            outputfolderstring = outputfolderstring.Replace(@"\", "");

            Console.Write("First residue number: ");
            var res = Console.ReadLine().Trim();

            while (!int.TryParse(res, out FirstResidueNumber))
            {
                Console.Write(">First residue number: ");
                res = Console.ReadLine().Trim();
            }

            var files = Directory.GetFiles(pdbpathstring);

            Console.WriteLine("Found " + files.Length.ToString() + " files");
            Console.WriteLine("Starting...");
            Console.WriteLine("Writing files to: " + outputfolderstring);
            Console.WriteLine();

            foreach (var fn in files)
            {
                Console.Write("Processing file: " + Path.GetFileName(fn));

                if (Path.GetExtension(fn).ToLower() != ".pdb") { Console.WriteLine(" => not pdb file"); continue; }

                string filePath = System.IO.Path.Combine(pdbpathstring, Path.GetFileName(fn));
                var lines = File.ReadAllLines(fn);

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

                File.WriteAllText(Path.Combine(outputfolderstring, Path.GetFileName(fn)), text.Trim());

                Console.WriteLine(" => done");
            }

            Console.WriteLine();
            Console.WriteLine("Completed");
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
