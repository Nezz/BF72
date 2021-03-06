﻿using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using BF72.Shared;

namespace BF72.Arecibo
{
    class Arecibo
    {
        static void Main(string[] args)
        {
            args = new[]
            {
                "C:\\Users\\Adam\\Downloads\\bf72\\bf72-010.html",
                "C:\\Users\\Adam\\Downloads\\bf72\\bf72-011.html",
                //"C:\\Users\\Adam\\Downloads\\bf72\\bf72-020.html",
            };
            
            foreach (var path in args)
            {
                var atoms = Atom.ParseFromFile(path);

                var pixelCount = atoms.Max(atom => atom.Value);
                var rows = atoms.Count(atom => atom.Value == pixelCount);
                var columns = pixelCount / rows;

                using (var bitmap = new Bitmap(columns, rows))
                {
                    foreach (var atom in atoms.Where(atom => atom.Value < pixelCount).OrderBy(atom => atom.Value))
                    {
                        bitmap.SetPixel(atom.Value % columns, atom.Value / columns, Color.Purple);
                    }

                    bitmap.Save(Path.ChangeExtension(path, "png"), ImageFormat.Png);
                }
            }
        }
    }
}
