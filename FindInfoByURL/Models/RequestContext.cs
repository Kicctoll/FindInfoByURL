using System;
using Microsoft.EntityFrameworkCore;

namespace BrightTest.Models
{
    public class RequestContext : DbContext
    {
        public RequestContext(DbContextOptions<RequestContext> options)
            : base(options) { }

        public DbSet<Request> Requests { get; set; }
    }
}
