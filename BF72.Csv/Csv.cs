using System.Collections.Generic;
using System.IO;
using System.Linq;
using BF72.Shared;

namespace BF72.Csv
{
    class Csv
    {
        static void Main(string[] args)
        {
            args = new[]
            {
                "C:\\Users\\Adam\\Downloads\\bf72\\bf72-001.html",
                "C:\\Users\\Adam\\Downloads\\bf72\\bf72-002.html",
                "C:\\Users\\Adam\\Downloads\\bf72\\bf72-003.html",
                "C:\\Users\\Adam\\Downloads\\bf72\\bf72-010.html",
                "C:\\Users\\Adam\\Downloads\\bf72\\bf72-011.html",
                "C:\\Users\\Adam\\Downloads\\bf72\\bf72-020.html",
            };
            foreach (var path in args)
            {
                var atoms = Atom.ParseFromFile(path);
                Normalize(atoms);
                WriteFile(atoms, Path.ChangeExtension(path, "csv"));
            }
        }

        /// <summary>
        /// Append zeros to the end of atoms to make sure all have the same size
        /// </summary>
        private static void Normalize(List<Atom> atoms)
        {
            var maxSize = atoms.Max(a => a.Count);

            foreach (var atom in atoms)
            {
                var itemsToAdd = maxSize - atom.Count;
                for (int i = 0; i < itemsToAdd; i++)
                    atom.Add(0);
            }
        }

        private static void WriteFile(List<Atom> atoms, string path)
        {
            var separator = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ListSeparator;
            var maxSize = atoms.Max(a => a.Count);

            using (var sw = new StreamWriter(path))
            {
                sw.Write(string.Join(separator, Enumerable.Range(1, maxSize).Select(a => "Shell " + a)));
                sw.WriteLine(separator + "Value");

                foreach (var atom in atoms)
                {
                    sw.Write(string.Join(separator, atom));
                    sw.WriteLine(separator + atom.Value);
                }
            }
        }
    }
}
