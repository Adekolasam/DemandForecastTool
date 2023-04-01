using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DemandForecastTool.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }

        public DbSet<Environment> Environments { get; set; }
        public DbSet<DataCentre> DataCentres { get; set; }
        public DbSet<ResourceType> ResourceTypes { get; set; }
        public DbSet<ResourceRequest> ResourceRequests { get; set; }
    }

    //Environment - prod (cpu and mem) and non prod (server)
    public class Environment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(255)]
        [Required]
        public string Name { get; set; }

        [Required]
        public ICollection<ResourceRequest> ResourceRequests { get; set; }
    }

    public class DataCentre
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(255)]
        [Required]
        public string Name { get; set; }

        [Required]
        public ICollection<ResourceRequest> ResourceRequests { get; set; }
    }

    public class ResourceType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(255)]
        [Required]
        public string Name { get; set; }

        [Required]
        public ICollection<ResourceRequest> ResourceRequests { get; set; }
    }

    // Host
    public class ResourceRequest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int EnvironmentId { get; set; }

        [ForeignKey("EnvironmentId")]
        public Environment Environment { get; set; }

        [Required]
        public int DataCentreId { get; set; }

        [ForeignKey("DataCentreId")]
        public DataCentre DataCentre { get; set; }

        [Required]
        public int ResourceTypeId { get; set; }

        [ForeignKey("ResourceTypeId")]
        public ResourceType ResourceType { get; set; }

        public DateTime RequestedDate { get; set; }
        public int CTS { get; set; }
        public int EIS { get; set; }
        public int GMB { get; set; }
        public int GMIT { get; set; }
        public int RISK { get; set; }
    }

}
