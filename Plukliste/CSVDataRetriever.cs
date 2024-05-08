namespace Plukliste
{
    internal partial class CSVPluklistFactory
    {
        private class CSVDataRetriever : IDataRetriever
        {
            public List<string> GetData(string csvPath)
            {
                return Directory.GetFiles(csvPath).ToList().Where(x => x.EndsWith(".CSV")).ToList();
            }
        }
    }
}
