using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace CompGeo2
{
    class SVGReader
    {
        private List<Country> countries;
        private Vec cursor;
        private Polygon p;
        private Country c;
        private string filePath;

        public SVGReader(string path)
        {
            this.filePath = path;
            cursor = new Vec(0, 0);
            countries = new List<Country>();
        }

        public List<Country> parse()
        {
            XmlNodeList svgPaths = getCountriesFromSVG();

            foreach (XmlNode path in svgPaths)
            {
                c = new Country(path.Attributes["id"].Value);

                string coords = path.Attributes["d"].Value;
                foreach (var raw_line in coords.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
                {
                    
                    string line = raw_line.Trim();
                    if (line.Length > 0)
                    {

                        string pattern = @"(?<1>[MmLlZz])((?<2>-?\d+(\.\d+)?),(?<3>-?\d+(\.\d+)?)((?<4>[HhVv])(?<5>-?\d+(\.\d+)?))?)?";
                        var match = Regex.Match(line, pattern, RegexOptions.ExplicitCapture);

                        string command = match.Groups[1].Value;
                        float x = 0;
                        float y = 0;
                        float hvValue = 0;


                        float.TryParse(match.Groups[2].Value, NumberStyles.Float, CultureInfo.InvariantCulture, out x);
                        float.TryParse(match.Groups[3].Value, NumberStyles.Float, CultureInfo.InvariantCulture, out y);
                        string hv = match.Groups[4].Value;
                        float.TryParse(match.Groups[5].Value, NumberStyles.Float, CultureInfo.InvariantCulture, out hvValue);

                        //string hvValueString = match.Groups[5].Value;
                        //float hvValue = hvValueString.Length > 0 ? float.Parse(hvValueString, CultureInfo.InvariantCulture) : 0;
                        //var command = line.Substring(0, 1);
                        //var remaining = line.Substring(1);
                        //if (remaining.Contains("H"))
                        //{
                        //    remaining = remaining.Split('H')[0];
                        //}

                        //if (remaining.Length > 0)
                        //{
                        //    x = float.Parse(remaining.Split(',')[0], CultureInfo.InvariantCulture);
                        //    y = float.Parse(remaining.Split(',')[1], CultureInfo.InvariantCulture);
                        //}

                        handleSVGExpression(command, x, y, hv, hvValue);

                    }
                }
                countries.Add(c);
            }
            return countries;
        }

        private void handleSVGExpression(string command, float x, float y, string hv, float hvValue)
        {
            switch (command)
            {
                case "M":
                    cursor.x = x;
                    cursor.y = y;
                    p = new Polygon();
                    p.points.Add(new Vec(cursor.x, cursor.y));
                    break;

                case "L":
                    cursor.x = x;
                    cursor.y = y;
                    p.points.Add(new Vec(cursor.x, cursor.y));
                    break;

                case "l":
                    cursor.x += x;
                    cursor.y += y;
                    p.points.Add(new Vec(cursor.x, cursor.y));
                    break;

                case "z":
                case "Z":
                    c.parts.Add(p);
                    break;
            }

            switch (hv)
            {
                case "V":
                case "v":
                    cursor.y += hvValue;
                    p.points.Add(new Vec(cursor.x, cursor.y));
                    break;

                case "H":
                case "h":
                    cursor.x += hvValue;
                    p.points.Add(new Vec(cursor.x, cursor.y));
                    break;
            }
        }

       private XmlNodeList getCountriesFromSVG()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(filePath);
            return xmlDoc["svg"]["g"].GetElementsByTagName("path");
        }
    }
}
