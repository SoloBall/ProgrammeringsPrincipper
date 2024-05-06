//Eksempel på funktionel kodning hvor der kun bliver brugt et model lag
namespace PickList;

class PickListProgram { 

    static void Main()
    {
        //Arrange
        char readKey = ' ';

        List<string> files;
        var index = -1;
        var standardColor = Console.ForegroundColor;
        Directory.CreateDirectory("import");

        if (!Directory.Exists("export"))
        {
            Console.WriteLine("Directory \"export\" not found");
            Console.ReadLine();
            return;
        }
        files = Directory.EnumerateFiles("export").ToList();

        //ACT
        while (readKey != 'Q')
        {
            if (files.Count == 0)
            {
                Console.WriteLine("No files found.");
            }
            else
            {
                index = 0;
                Console.WriteLine($"PickList {index + 1} of {files.Count}");
                Console.WriteLine($"\nfile: {files[index]}");

                //read file
                FileStream file = File.OpenRead(files[index]);
                System.Xml.Serialization.XmlSerializer xmlSerializer =
                    new System.Xml.Serialization.XmlSerializer(typeof(PickList));
                var pickList = (PickList?)xmlSerializer.Deserialize(file);

                //print pickList
                if (pickList != null && pickList.Lines != null)
                {
                    Console.WriteLine("\n{0, -13}{1}", "\nName:", pickList.Name);
                    Console.WriteLine("{0, -13}{1}", "Shipment:", pickList.Shipment);
                    //TODO: Add address to screen print

                    Console.WriteLine("\n{0,-7}{1,-9}{2,-20}{3}", "Amount", "Type", "ProductID.", "Title");
                    foreach (var item in pickList.Lines)
                    {
                        Console.WriteLine("{0,-7}{1,-9}{2,-20}{3}", item.Amount, item.Type, item.ProductID, item.Title);
                    }
                }
                file.Close();
            }

            //Print options
            Console.WriteLine("\n\nOptions:");
            ColorFirstChar(ConsoleColor.Green, "Q", "uit");
            if (index >= 0)
            {
                ColorFirstChar(ConsoleColor.Green, "F", "inish pick slip");
            }
            if (index > 0)
            {
                ColorFirstChar(ConsoleColor.Green, "P", "revious pick slip");
            }
            if (index < files.Count - 1)
            {
                ColorFirstChar(ConsoleColor.Green, "N", "ext pick slip");
            }
            ColorFirstChar(ConsoleColor.Green, "R", "efresh pick slips");

            readKey = Console.ReadKey().KeyChar;
            if (char.IsAsciiLetter(readKey)) 
            { 
                readKey = char.ToUpper(readKey);
            }
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Red; //status in red
            switch (readKey)
            {
                case 'F':
                    //Move files to import directory
                    var filewithoutPath = files[index].Substring(files[index].LastIndexOf('\\'));
                    File.Move(files[index], string.Format(@"import\\{0}", filewithoutPath));
                    Console.WriteLine($"Plukseddel {files[index]} afsluttet.");
                    files.Remove(files[index]);
                    if (index == files.Count) index--;
                    break;

                case 'P':
                    if (index > 0)
                    {
                        index--;
                    }
                    break;

                case 'N':
                    if (index < files.Count - 1)
                    {
                        index++;
                    }
                    break;

                case 'R':
                    files = Directory.EnumerateFiles("export").ToList();
                    index = -1;
                    Console.WriteLine("Pick slips Refreshed");
                    break;
            }
            Console.ForegroundColor = standardColor; //reset charColor

        }
    }
    public static void ColorFirstChar(ConsoleColor charColor, string @char, string line)
    {
        Console.ForegroundColor = charColor;
        Console.WriteLine(@char);
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.WriteLine(line);
    }
}
