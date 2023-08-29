using EventSourcing.Demo.Events;
using EventSourcing.Demo.Events.Common;

namespace EventSourcing.Demo.Entities;

public class WarehouseProduct
{
    public string Sku { get; }
    private int _quantity { get; set; }
    private readonly IList<IEvent> _events = new List<IEvent>();


    public WarehouseProduct(string sku)
    {
        Sku = sku;
    }

    public void ShipProduct(int quantity)
    {
        if (quantity > _quantity)
        {
            throw new Exception("We don't have enough product to ship");
        }
        AddEvent(new ProductShipped(Sku, quantity, DateTime.UtcNow));
    }

    public void ReceiveProduct(int quantity)
    {
        AddEvent(new ProductReceived(Sku, quantity, DateTime.UtcNow));
    }

    public void AdjustInventory(int quantity, string reason)
    {
        if (_quantity + quantity < 0)
        {
            throw new Exception("Cannot adjust to new product");
        }

        AddEvent(new InventoryAdjusted(Sku, quantity, reason, DateTime.UtcNow));
    }
    public void AddEvent(IEvent @event)
    {
        switch (@event)
        {
            case ProductShipped data:
                Apply(data);
                break;
            case InventoryAdjusted data:
                Apply(data);
                break;
            case ProductReceived data:
                Apply(data);
                break;
        }
        _events.Add(@event);
    }
    private void Apply(ProductShipped @event)
    {
        _quantity -= @event.Quantity;
    }

    private void Apply(ProductReceived @event)
    {
        _quantity += @event.Quantity;
    }

    private void Apply(InventoryAdjusted @event)
    {
        _quantity += @event.Quantity;
    }


    public IList<IEvent> GetEvents()
    {
        return _events;
    }
    public int GetQuantity()
    {
        return _quantity;
    }
}