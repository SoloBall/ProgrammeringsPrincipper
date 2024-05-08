namespace Plukliste
{
    internal partial class CSVPluklistFactory
    {
        internal class CSVItemAdder : IItemAdder
        {
            public Pluklist AddItemToPluklist(string filePath)
            {
                StreamReader reader = new StreamReader(filePath);
                reader.ReadLine();
                Pluklist pluklist = new Pluklist();
                while (!reader.EndOfStream)
                {
                    Pluklist pluklist = new Pluklist();
                    Item item = new Item();
                    string[] values = reader.ReadLine().Split(';');
                    item.ProductID = values[0];
                    item.Type = ItemType.Fysisk;
                    item.Title = values[2];
                    item.Amount = int.Parse(values[3]);
                    pluklist.AddItem(item);
                    reader.Close();
                }
                return pluklist;
            }
        }
    }
}
