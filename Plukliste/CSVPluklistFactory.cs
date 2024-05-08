using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plukliste
{
    internal partial class CSVPluklistFactory : IPluklistFactory
    {
        public static List<Pluklist> CreatePluklists(string csvPath)
        {
            List<Pluklist> pluklists = new List<Pluklist>();
            CSVDataRetriever retriever = new CSVDataRetriever();
            CSVItemAdder adder = new CSVItemAdder();
            List<string> files = retriever.GetData(csvPath);
            foreach (string filePath in files)
            {
                Pluklist pluklist = adder.AddItemToPluklist(filePath);
                foreach (char c in filePath.Substring(filePath.LastIndexOf("\\")))
                {
                    if (char.IsLetter(c))
                    {
                        pluklist.Name += c;
                    }
                }
                pluklist.Name = pluklist.Name.Replace(".CSV", "");
                pluklist.Forsendelse = "Pickup";

                pluklists.Add(pluklist);
            }
            return pluklists;
        }
    }
}
