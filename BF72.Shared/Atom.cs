using System.Collections.Generic;
using System.Xml.Linq;

namespace BF72.Shared
{
    public class Atom : List<int>
    {
        private int? value;

        public int Value
        {
            get
            {
                if (!value.HasValue)
                    value = this.CalculateValue();

                return value.Value;
            }
        }

        /// <summary>
        /// https://en.wikipedia.org/wiki/Factorial_number_system
        /// </summary>
        private int CalculateValue()
        {
            int value = 0;

            int localValue = 1;

            for (int i = 0; i < this.Count; i++)
            {
                localValue *= i + 1;
                value += localValue * this[i];
            }

            return value;
        }

        public static List<Atom> ParseFromFile(string path)
        {
            var xdocument = XDocument.Load(path);
            var body = xdocument.Root.Element("body");

            var atoms = new List<Atom>();
            foreach (var svg in body.Elements("svg"))
            {
                var atom = new Atom();
                atoms.Add(atom);

                int counter = 0;

                foreach (var circle in svg.Elements("circle"))
                {
                    var isShell = circle.Attribute("fill").Value == "none";

                    if (isShell)
                    {
                        atom.Add(counter);
                        counter = 0;
                    }
                    else
                    {
                        counter++;
                    }
                }

                atom.RemoveRange(0, 2); // Inner shell cannot contain dots, it is discarded along with one more shell due to my parsing algorithm
                atom.Add(counter);
            }

            return atoms;
        }
    }
}
