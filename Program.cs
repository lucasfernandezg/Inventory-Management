using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using CsvHelper.Configuration.Attributes;

namespace Inventory
{
class Program
{
        //List<Product> productList = new List<Product>();
    public List<Product> CheckCSV(string path)
        {
            
            if (File.Exists(path))
            {
                using (var streamReader = new StreamReader(path))
                using (var csv = new CsvReader(streamReader, CultureInfo.InvariantCulture))
                {
                    csv.Context.RegisterClassMap<Product.ProductMap>();
                    Console.WriteLine("Found Records:");
                    List<Product> b = csv.GetRecords<Product>().ToList();
                    foreach (var i in b)
                    {   
                        Console.WriteLine(i);
                    }
                    return b;
                }
            }
            else
            {
                Console.WriteLine("Records Not Found");
                List<Product> b = new List<Product>();
                return b;
            }
            
        }
    public List<Product> Welcome(List<Product> productList, int counter, string path)
        {
            Console.WriteLine("Welcome to your Inventory Managment System");
            Console.WriteLine("Please enter 'a' to add a product to your list, 'p' to print all products and 's' to search for a sepecific one.");
            string input = Console.ReadLine();
            if (input == "a")
            {
                Console.WriteLine("Please insert product data:");
                Console.Write("Description: ");
                string desc = Console.ReadLine();
                Console.Write("Quantity: ");
                int qty = Convert.ToInt32(Console.ReadLine());
                Console.Write("Price: ");
                Double pri = Convert.ToDouble(Console.ReadLine());
                Console.Write("Code: ");
                int cod = Convert.ToInt32(Console.ReadLine());
                productList.Add(new Product(desc, qty, pri, cod));

                using (var writer = new StreamWriter(path))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteRecords(productList);
                }
            }
            else if (input == "p")
            {
                //Console.WriteLine("Description | Quantity | Price | Code");
                foreach (Product i in productList)
                {   
                    Console.WriteLine($"{counter}- {i}");
                    counter = counter + 1;
                }
            }
            else if (input == "s")
            {
                Console.Write("Please insert code");
                int c = Convert.ToInt32(Console.ReadLine());
                var loc = productList.Find(e => e.Code == c);
                if (loc == null)
                {
                    Console.WriteLine("Code Not Found!");
                }
                Console.WriteLine(loc);
            }
            return productList;
        }
    static void Main(string[] args)
        {
            var appPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            var path = Path.Combine(appPath, "file.csv");
            Console.WriteLine(path);
            var a = new Program();
            int counter = 0;
            Console.WriteLine("Main - Esc or Enter+Esc to Exit Program");
            List<Product> productList = a.CheckCSV(path); //Si hay CSV lo lee y lo mete en productList. Si no hay, crea un array en blanco.
            //Console.WriteLine("Checked Records");
            do
            {
                productList = a.Welcome(productList,counter,path);
            } while (Console.ReadKey().Key != ConsoleKey.Escape);
            }
            
}
class Product
    {

        [Name("description")]
        public string Description { get; set; }

        [Name("quantity")]
        public int Quantity { get; set; }

        [Name("price")]
        public double Price { get; set; }

        [Name("code")]
        public int Code { get; set; }

        public Product()
        {

        }

        public Product(string description, int quantity, double price, int code)
        {
            Description = description;
            Quantity = quantity;
            Price = price;
            Code = code;
            Console.WriteLine($"Product {Description} was created!");
        }

        public override string ToString()
        {
            return $"{Description} | qty: {Quantity} | ${Price} | {Code}";
        }


        public sealed class ProductMap : ClassMap<Product>
        {
            public ProductMap()
            {
                Map(m => m.Description).Name("description");
                Map(m => m.Quantity).Name("quantity");
                Map(m => m.Price).Name("price");
                Map(m => m.Code).Name("code");
            }
        }

    }

}
