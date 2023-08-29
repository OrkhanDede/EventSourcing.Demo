using EventSourcing.Demo.Events.Common;

namespace EventSourcing.Demo.Events;

public record ProductReceived(string Sku, int Quantity, DateTime DateTime): IEvent;