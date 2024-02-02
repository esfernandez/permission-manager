using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5.Microservices.User.Infrastructure.Interfaces;
public interface IEventProducer
{
    Task SendEvent(string topic, object msg);
}
