using Confluent.Kafka;

namespace product_category.api.Workers.InventoryUploadTrackingWorkers;

public interface IScopedInventoryUploadTrackingService
{
	Task TrackAndProcessAsync(IConsumer<string, string> consumer, CancellationToken stoppingToken);
}
