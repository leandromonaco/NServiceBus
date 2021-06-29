using System.Threading.Tasks;

namespace NServiceBus.API.Queue
{
    public interface IPublisher
    {
        Task PublishAsync(OrderSubmitted orderSubmitted);
    }
}
