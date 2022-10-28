

using Newtonsoft.Json;
using ReadData.Models;
using System.Data;
using System.Xml;
using System.Xml.Serialization;

class Program
{
    static void Main(string[] args)
    {
        var categories = LoadCategoryCSV();
        var products = LoadProductCSV();

        categories = categories.Select(c => new Category
        {
            Name = c.Name,
            Description = c.Description,
            Id = c.Id,
            Products = products.Where(p => p.CategoryId == c.Id).ToList(),
        }).ToList();

        #region File read and write
        var jsonString = JsonConvert.SerializeObject(categories);

        string outputFolder = $@"{Directory.GetCurrentDirectory()}\Output\";
        if (!Directory.Exists(outputFolder))
            Directory.CreateDirectory(outputFolder);

        string jsonFilePath = $"{outputFolder}catlog.json";
        File.WriteAllText(jsonFilePath, jsonString);
        #endregion

        Console.ReadLine();
    }

    /// <summary>
    /// Serialize categories object to XML
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="categories"></param>
    /// <returns></returns>
    public static string SerializeToXml<T>(List<Category> categories)
    {
        if (categories == null)
        {
            return string.Empty;
        }

        var xmlserializer = new XmlSerializer(typeof(T));
        var stringWriter = new StringWriter();
        using (var writer = XmlWriter.Create(stringWriter))
        {
            xmlserializer.Serialize(writer, categories);
            return stringWriter.ToString();
        }
    }


    /// <summary>
    /// Serialize categories object to Json
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="categories"></param>
    /// <returns></returns>
    public static string SerializeToJson<T>(List<Category> categories)
    {
        if (categories == null)
        {
            return string.Empty;
        }

        return JsonConvert.SerializeObject(categories);
    }

    /// <summary>
    /// Read data from Categories file and return list Category object
    /// </summary>
    /// <returns></returns>
    static List<Category> LoadCategoryCSV()
    {
        string filePath = $@"{Directory.GetCurrentDirectory()}\Input\Categories.csv";

        var categoryFileContent = System.IO.File.ReadLines(filePath);

        List<Category> categorys = new List<Category>();

        var header = categoryFileContent.FirstOrDefault().Split(";");

        foreach (var line in categoryFileContent)
        {
            Category category = new Category();
            var rowData = line.Split(";");

            for (int c = 0; c < rowData.Length; c++)
            {
                switch (header[c].ToLower())
                {
                    case "id":
                        category.Id = rowData[c];
                        break;
                    case "name":
                        category.Name = rowData[c];
                        break;
                    case "description":
                        category.Description = rowData[c];
                        break;
                }
            }
            categorys.Add(category);
        }

        return categorys;
    }

    /// <summary>
    /// Read data from product file and return list product object
    /// </summary>
    /// <returns></returns>
    static List<Product> LoadProductCSV()
    {
        string filePath = $@"{Directory.GetCurrentDirectory()}\\Input\\Products.csv";

        var productsFileContent = System.IO.File.ReadLines(filePath);

        List<Product> products = new List<Product>();

        var header = productsFileContent.FirstOrDefault().Split(";");

        foreach (var line in productsFileContent)
        {
            Product product = new Product();
            var rowData = line.Split(";");

            for (int c = 0; c < rowData.Length; c++)
            {
                switch (header[c].ToLower())
                {
                    case "id":
                        product.Id = rowData[c];
                        break;
                    case "categoryid":
                        product.CategoryId = rowData[c];
                        break;
                    case "name":
                        product.Name = rowData[c];
                        break;
                    case "price":
                        product.Price = rowData[c];
                        break;
                }
            }
            products.Add(product);
        }

        return products;
    }
}