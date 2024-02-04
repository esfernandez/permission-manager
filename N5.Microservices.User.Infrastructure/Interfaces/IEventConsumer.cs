using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5.Microservices.User.Infrastructure.Interfaces;
public interface IEventConsumer<TValue>
{
    void Subscribe(string key);

    void StopConsume();
    Task Consume(CancellationToken cancellationToken, Func<TValue, CancellationToken, Task> action);
}
