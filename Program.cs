using System;
using System.Collections.Generic;

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Welcome to Ordering System!");

        // Initialize menu
        var menu = new Dictionary<int, MenuItem>
        {
            { 1, new MenuItem { Name = "Ham and Cheese Toastie", FoodType = "Food", Price = 6.23m } },
            { 2, new MenuItem { Name = "Pizza", FoodType = "Food", Price = 5.78m } },
            { 3, new MenuItem { Name = "Chocolate Brownie", FoodType = "Food", Price = 3.50m } },
            { 4, new MenuItem { Name = "Tea", FoodType = "Drink", Price = 3.65m } },
            { 5, new MenuItem { Name = "Coffee", FoodType = "Drink", Price = 4.64m } },
            { 6, new MenuItem { Name = "Water", FoodType = "Drink", Price = 0.00m } }
        };

        // Initialize view
        var view = new ConsoleView();

        // Initialize and run controller
        var controller = new OrderingController(menu, view);
        controller.Run();
    }
}

public class OrderingController
{
    private readonly Dictionary<int, MenuItem> _menu;
    private readonly ConsoleView _view;
    private readonly Order _order;

    public OrderingController(Dictionary<int, MenuItem> menu, ConsoleView view)
    {
        _menu = menu;
        _view = view;
        _order = new Order();
    }

    public void Run()
    {
        _view.DisplayMenu(_menu);

        string input;
        do
        {
            input = _view.GetInput("\nEnter the number of the item you'd like to order (or type 'done' to finish):");

            if (int.TryParse(input, out int itemNumber) && _menu.ContainsKey(itemNumber))
            {
                var selectedItem = _menu[itemNumber];
                string quantityInput = _view.GetInput($"How many {selectedItem.Name}(s) would you like to order?");
                if (int.TryParse(quantityInput, out int quantity) && quantity > 0)
                {
                    _order.Items.Add((selectedItem, quantity));
                    Console.WriteLine($"Added {quantity} {selectedItem.Name}(s) to your order.");
                }
                else
                {
                    Console.WriteLine("Invalid quantity. Please enter a positive number.");
                }
            }
            else if (input?.ToLower() != "done")
            {
                Console.WriteLine("Invalid input. Please enter a valid menu item number.");
            }

        } while (input?.ToLower() != "done");

        decimal total = _order.CalculateTotal(out decimal discount);
        _view.DisplayOrderSummary(_order, total, discount);
    }
}
public class ConsoleView
{
    public void DisplayMenu(Dictionary<int, MenuItem> menu)
    {
        Console.WriteLine("Here is our menu:");
        foreach (var item in menu)
        {
            Console.WriteLine($" {item.Key}|{item.Value.Name}| {item.Value.FoodType}| ${item.Value.Price:F2}");
        }
    }

    public void DisplayOrderSummary(Order order, decimal total, decimal discount)
    {
        Console.WriteLine("\nYour order summary:");
        foreach (var (item, quantity) in order.Items)
        {
            decimal itemTotal = item.Price * quantity;
            Console.WriteLine($"- {item.Name} x{quantity} - ${itemTotal:F2}");
        }

        if (discount > 0)
        {
            Console.WriteLine($"A discount of ${discount:F2} has been applied.");
        }

        Console.WriteLine($"Total: ${total:F2}");
        Console.WriteLine("Thank you for your order!");
    }

    public string GetInput(string prompt)
    {
        Console.WriteLine(prompt);
        return Console.ReadLine()?.Trim();
    }
}
public class MenuItem
{
    public string Name { get; set; }
    public string FoodType { get; set; }
    public decimal Price { get; set; }
}

public class Order
{
    public List<(MenuItem Item, int Quantity)> Items { get; set; } = new();

    public decimal CalculateTotal(out decimal discount)
    {
        decimal total = 0;
        bool hasFood = false;
        bool hasValidDrink = false;

        foreach (var (item, quantity) in Items)
        {
            total += item.Price * quantity;

            if (item.FoodType == "Food") hasFood = true;
            if (item.FoodType == "Drink" && item.Name != "Water") hasValidDrink = true;
        }

        discount = 0;

        if (hasFood && hasValidDrink)
        {
            discount = total * 0.10m;
        }

        if (total >= 20.00m)
        {
            discount = total * 0.20m;
        }

        if (discount > 6.00m)
        {
            discount = 6.00m;
        }

        return total - discount;
    }
}


