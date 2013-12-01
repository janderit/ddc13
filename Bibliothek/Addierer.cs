using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bibliothek
{
    public class Addierer
    {
        public int Addiere(int a, int b)
        {
            return a + b;
        }

        public string AddiereJson(int a, int b)
        {
            fastJSON.JSON.Instance.Parameters.UseExtensions = false;
            return fastJSON.JSON.Instance.ToJSON(new Summe(Addiere(a, b)));
        }
    }

    public struct Summe
    {
        public readonly int Ergebnis;

        public Summe(int ergebnis)
        {
            Ergebnis = ergebnis;
        }
    }

}
