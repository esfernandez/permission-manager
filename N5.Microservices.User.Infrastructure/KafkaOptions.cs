using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5.Microservices.User.Infrastructure;
public class KafkaOptions
{
    public string Url { get; set; } = string.Empty;
    public string Topic { get; set; } = string.Empty;
}
