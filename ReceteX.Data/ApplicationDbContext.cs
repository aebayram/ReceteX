using Microsoft.EntityFrameworkCore;
using ReceteX.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReceteX.Data
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        {
        
        
        }

        public virtual DbSet<AppUser> Users { get; set; }

        public virtual DbSet<Customer> Customers { get; set; }

        public virtual DbSet<Description> Descriptions { get; set; }

        public virtual DbSet<DescriptionType> DescriptionTypes { get; set; }

        public virtual DbSet<Diagnosis> Diagnoses { get; set; }

        public virtual DbSet<Medicine> Medicines { get; set; }

        public virtual DbSet<MedicineUsagePeriod> MedicineUsagePeriods { get; set; }

        public virtual DbSet<Prescription> Prescriptions { get; set; }

        public virtual DbSet<PrescriptionMedicine> PrescriptionMedicines { get; set; }
        public virtual DbSet<Status> Statuses { get; set; }


    }
}
