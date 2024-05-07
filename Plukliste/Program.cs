//Eksempel på funktionel kodning hvor der kun bliver brugt et model lag
using System.Text;

namespace PlukListe;

class PickListProgram {
    static readonly string exportPath = "export";
    static readonly string importPath = "import";
    static readonly string printPath = "print";
    static readonly string templatePath = "templates";
    static readonly System.Xml.Serialization.XmlSerializer xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(PlukList));
    static void Main()
    {
        //Arrange
        char readKey = ' ';

        List<string> files;
        var currentFile = 0;
        Directory.CreateDirectory(importPath);

        if (!Directory.Exists(exportPath))
        {
            Console.WriteLine($"Directory \"{exportPath}\" not found");
            Console.ReadLine();
            return;
        }
        files = Directory.EnumerateFiles(exportPath).ToList();

        PlukList? plukliste = null;
        //ACT
        while (readKey != 'Q')
        {
            if (files.Count == 0)
            {
                Console.WriteLine("No files found.");
            }
            else
            {
                Console.WriteLine($"PlukList {currentFile + 1} of {files.Count}");
                Console.WriteLine($"\nfile: {files[currentFile]}");

                //read file
                FileStream file = File.OpenRead(files[currentFile]);
                plukliste = (PlukList?)xmlSerializer.Deserialize(file);

                //print plukliste
                if (plukliste != null && plukliste.Lines != null)
                {
                    Console.WriteLine("\n{0, -13}{1}", "\nName:", plukliste.Name);
                    Console.WriteLine("{0, -13}{1}", "Forsendelse:", plukliste.Forsendelse);

                    Console.WriteLine("\n{0,-7}{1,-9}{2,-20}{3}", "Amount", "Type", "ProductID.", "Title");
                    foreach (var item in plukliste.Lines)
                    {
                        Console.WriteLine("{0,-7}{1,-9}{2,-20}{3}", item.Amount, item.Type, item.ProductID, item.Title);
                    }
                }
                file.Close();
            }

            //Print options
            Console.WriteLine("\n\nOptions:");
            ColorFirstChar("Quit");
            if (currentFile >= 0)
            {
                ColorFirstChar("Complete pick slip");
            }
            if (currentFile > 0)
            {
                ColorFirstChar("Former pick slip");
            }
            if (currentFile < files.Count - 1)
            {
                ColorFirstChar("Next pick slip");
            }
            ColorFirstChar("Refresh pick slips");

            readKey = Console.ReadKey().KeyChar;
            if (char.IsAsciiLetter(readKey)) 
            { 
                readKey = char.ToUpper(readKey);
            }
            Console.Clear();

            ConsoleColor standardColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red; //status in red
            switch (readKey)
            {
                case 'C':
                    //Move files to importPath directory
                    var filewithoutPath = files[currentFile].Substring(files[currentFile].LastIndexOf('\\'));
                    File.Move(files[currentFile], string.Format(importPath + @"\\{0}", filewithoutPath));
                    Console.WriteLine($"Plukseddel {files[currentFile]} afsluttet.");
                    files.Remove(files[currentFile]);
                    if (currentFile == files.Count) currentFile--;
                    break;

                case 'F':
                    if (currentFile > 0)
                    {
                        currentFile--;
                    }
                    break;

                case 'N':
                    if (currentFile < files.Count - 1)
                    {
                        currentFile++;
                    }
                    break;

                case 'R':
                    files = Directory.EnumerateFiles(exportPath).ToList();
                    currentFile = -1;
                    Console.WriteLine("Pick slips Refreshed");
                    break;

                case 'P':
                    StreamReader reader = new StreamReader(templatePath);
                    List<Item> items = plukliste.Lines.Where(x => x.Type == ItemType.Print).ToList();
                    string pluklisteReplacement = "";
                    foreach (Item item in items)
                    {
                        pluklisteReplacement += $"<li>{item.Title} - {item.Amount}</li> <br />";
                    }
                    PrintTemplate(reader, plukliste, pluklisteReplacement);
                    break;
            }
            Console.ForegroundColor = standardColor; //reset charColor

        }
    }
    public static void ColorFirstChar(string line, ConsoleColor charColor = ConsoleColor.Green)
    {
        Console.ForegroundColor = charColor;
        Console.WriteLine(line[0]);
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.WriteLine(line[1..]);
    }
    public static void PrintTemplate(StreamReader reader, PlukList? plukliste, string pluklisteReplacement)
    {
        string template = reader.ReadToEnd();
        template = template.Replace("[Adresse]", plukliste.Adresse);
        template = template.Replace("[Name]", plukliste.Name);
        template = template.Replace("[Plukliste]", pluklisteReplacement);

        StreamWriter writer = new StreamWriter(printPath + ".html");
    }
}
