using EventSourcing.Demo.Entities;
using EventSourcing.Demo.Events.Common;

namespace EventSourcing.Demo;

public class WarehouseProductRepository
{
    private readonly Dictionary<string, IList<IEvent>> _inMemoryStreams = new();

    public WarehouseProduct Get(string sku)
    {
        var warehouseProduct = new WarehouseProduct(sku);
        if (_inMemoryStreams.TryGetValue(sku, out var stream))
        {
            foreach (var @event in stream)
            {
                warehouseProduct.AddEvent(@event);
            }
        }
        return warehouseProduct;
    }

    public void Save(WarehouseProduct warehouseProduct)
    {
        _inMemoryStreams[warehouseProduct.Sku] = warehouseProduct.GetEvents();
    }
}