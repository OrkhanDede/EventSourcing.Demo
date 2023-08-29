using EventSourcing.Demo.Events.Common;

namespace EventSourcing.Demo.Events;

public record ProductShipped(string Sku, int Quantity, DateTime DateTime):IEvent;