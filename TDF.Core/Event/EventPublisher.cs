using System;
using System.Linq;
using System.Threading.Tasks;
using TDF.Core.Log;

namespace TDF.Core.Event
{
    public class EventPublisher : IEventPublisher
    {
        public virtual void Publish<T>(T eventEntity) where T : class, IEvent
        {
            var consumers = Ioc.Ioc.ResolveAll<IConsumer<T>>().ToList();
            foreach (var consumer in consumers)
            {
                PublishToConsumer(consumer, eventEntity);
            }
        }

        public virtual void PublishToConsumer<T>(IConsumer<T> consumer, T eventEntity) where T : IEvent
        {
            try
            {
                if (eventEntity.Async)
                {
                    Task.Factory.StartNew(() => consumer.Handler(eventEntity));
                }
                else
                {
                    consumer.Handler(eventEntity);
                }
            }
            catch (Exception ex)
            {
                LogFactory.GetLogger(GetType()).Error("事件消费者异常=>" + ex.Message, ex);
            }

        }
    }
}
