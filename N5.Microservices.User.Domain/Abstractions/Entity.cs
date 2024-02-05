using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5.Microservices.User.Domain.Abstractions;
public abstract class Entity<TKey>
{
    public TKey Id { get; set; }
}
