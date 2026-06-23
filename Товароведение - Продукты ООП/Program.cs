using System;
using System.Text;

public class Program
{
    static void Main(string[] args)
    {
        Console.Write("Введите наименование товара: ");
        string nameProduct = Console.ReadLine();
        while (string.IsNullOrWhiteSpace(nameProduct))
        {
            Console.Write("Наименование не может быть пустым. Повторите ввод: ");
            nameProduct = Console.ReadLine();
        }

        Console.Write("Введите производителя: ");
        string production = Console.ReadLine();
        while (string.IsNullOrWhiteSpace(production))
        {
            Console.Write("Производитель не может быть пустым. Повторите ввод: ");
            production = Console.ReadLine();
        }

        decimal price;
        Console.Write("Введите цену товара: ");
        while (!decimal.TryParse(Console.ReadLine(), out price) || price <= 0)
        {
            Console.Write("Цена товара должна быть больше 0. Повторите ввод: ");
        }

        Console.Write("Введите дату производства ДД.ММ.ГГГГ: ");
        DateTime productionDate = DateTime.Parse(Console.ReadLine());

        Console.Write("Введите дату окончания срока годности ДД.ММ.ГГГГ: ");
        DateTime expirationDate = DateTime.Parse(Console.ReadLine());

        while (expirationDate <= productionDate)
            {
            Console.Write("Дата окончания срока должна быть позже даты производства. Повторите ввод : ");
            expirationDate = DateTime.Parse(Console.ReadLine());
        }
        Product product = new Product
        {
            NameProduct = nameProduct,
            Production = production,
            Price = price,
            ExpirationDate = expirationDate,
            ProductionDate = productionDate
        };

        Console.WriteLine("\nИнформация о товаре: ");
        Console.WriteLine(product);
    }
}

public class Product
{
    public required string NameProduct { get; init; }
    public required string Production { get; init; }
    public required decimal Price { get; init; }
    public required DateTime ExpirationDate { get; init; }
    public required DateTime ProductionDate { get; init; }
    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.AppendLine($"Товар: {NameProduct}");
        sb.AppendLine($"Производитель: {Production}");
        sb.AppendLine($"Цена: {Price:F2} руб.");
        sb.AppendLine($"Срок годности до: {ExpirationDate:dd.MM.yyyy}");
        sb.AppendLine($"Дата производства: {ProductionDate:dd.MM.yyyy}");
        return sb.ToString();
        
    }
}