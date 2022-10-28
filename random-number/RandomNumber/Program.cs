class Program
{
    static void Main(string[] args)
    {
        string outputFolder = $@"{Directory.GetCurrentDirectory()}\Output\";
        if (!Directory.Exists(outputFolder))
            Directory.CreateDirectory(outputFolder);

        string filePath = $"{outputFolder}random-numbers.txt";

        try
        {
            List<int> numbers = new List<int>();
            var data = "";

            Random number = new Random();
            for (int i = 1; i <= 100000; i++)
            {
                while (true)
                {
                    int genNumber = (int)number.NextInt64(1, 9999999);
                    if (!numbers.Contains(genNumber))
                    {
                        numbers.Add(genNumber);
                        data += $"{genNumber}\n";
                        break;
                    }
                }
            }

            File.AppendAllText(filePath, data);
        }
        catch (Exception Ex)
        {
            Console.WriteLine(Ex.ToString());
        }
    }
}

      