using EventSourcing.Demo.Events.Common;

namespace EventSourcing.Demo.Events;

public record InventoryAdjusted(string Sku, int Quantity, string Reason, DateTime DateTime): IEvent;
