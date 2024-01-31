﻿using Microsoft.EntityFrameworkCore;
using N5.Microservices.User.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5.Microservices.User.Infrastructure;
public class EmployeeDBContext : DbContext
{
    public virtual DbSet<Employee> Employees { get; set; }
    public virtual DbSet<Permission> Permissions { get; set; }
    public virtual DbSet<PermissionType> PermissionTypes { get; set; }

    public EmployeeDBContext(DbContextOptions<EmployeeDBContext> options) : base(options) { }
}
