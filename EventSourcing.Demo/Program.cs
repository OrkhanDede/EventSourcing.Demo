using EventSourcing.Demo;
using EventSourcing.Demo.Events;

internal partial class Program
{
    static void Main(string[] args)
    {
        var warehouseProductRepository = new WarehouseProductRepository();
        var key = string.Empty;
        while (key != "X")
        {
            Console.WriteLine("R: Receive Inventory");
            Console.WriteLine("S: Ship Inventory");
            Console.WriteLine("A: Inventory Adjustment");
            Console.WriteLine("Q: Quantity On Hand");
            Console.WriteLine("E: Events");
            Console.Write("> ");
            key = Console.ReadLine()?.ToUpper();
            var sku = GetSkuFromConsole();
            var warehouseProduct = warehouseProductRepository.Get(sku);

            switch (key)
            {
                case "R":
                    var receiveInput = GetQuantity();
                    if (receiveInput.IsValid)
                    {
                        warehouseProduct.ReceiveProduct(receiveInput.Quantity);
                        Console.WriteLine($"{sku} Received:{receiveInput.Quantity}");
                    }

                    break;
                case "S":
                    var shipInput = GetQuantity();
                    if (shipInput.IsValid)
                    {
                        warehouseProduct.ShipProduct(shipInput.Quantity);
                        Console.WriteLine($"{sku} Shipped:{shipInput.Quantity}");
                    }
                    break;
                case "A":
                    var adjustmentInput = GetQuantity();
                    if (adjustmentInput.IsValid)
                    {
                        var reason = GetAdjustmentReason();
                        warehouseProduct.AdjustInventory(adjustmentInput.Quantity, reason);
                        Console.WriteLine($"{sku} Adjusted:{adjustmentInput.Quantity} {reason}");
                    }
                    break;
                case "Q":
                    var currentQuantityOnHand = warehouseProduct.GetQuantity();
                    Console.WriteLine($"{sku} Quantity On Hand : {currentQuantityOnHand}");
                    break;
                case "E":
                    Console.WriteLine($"Events :{sku}");
                    foreach (var @event in warehouseProduct.GetEvents())
                    {
                        switch (@event)
                        {
                            case ProductReceived receivedProduct:
                                Console.WriteLine($"{receivedProduct.DateTime} {sku} Received: {receivedProduct.Quantity}");
                                break;
                            case ProductShipped shipProduct:
                                Console.WriteLine($"{shipProduct.DateTime} {sku} Shipped: {shipProduct.Quantity}");
                                break;
                            case InventoryAdjusted inventoryAdjusted:
                                Console.WriteLine($"{inventoryAdjusted.DateTime} {sku} Adjusted: {inventoryAdjusted.Quantity}");
                                break;
                        }
                    }
                    break;
                default:
                    throw new InvalidOperationException("Please enter R S A Q E any character.");
            }
            warehouseProductRepository.Save(warehouseProduct);
            Console.WriteLine();
            Console.WriteLine("Please enter Enter to continue or X to exit.");
        }

      
    }
    static string GetAdjustmentReason()
    {
        Console.WriteLine("Please enter reason for adjustment: ");
        return Console.ReadLine() ?? throw new Exception("Reason cannot be empty");
    }
    record Model(bool IsValid, int Quantity);

    static Model GetQuantity()
    {
        Console.WriteLine("Please enter the quantity: ");
        var quantiy = Console.ReadLine();
        if (int.TryParse(quantiy, out int number))
        {
            return new Model(true, number);
        }

        return new Model(false, 0);
    }

    static string GetSkuFromConsole()
    {
        Console.WriteLine("Please enter a sku name: ");
        return Console.ReadLine() ?? throw new Exception("Wrong sku entry");
    }
}