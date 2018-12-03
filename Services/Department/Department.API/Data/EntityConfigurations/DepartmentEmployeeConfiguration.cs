using Department.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Department.API.Data.EntityConfigurations
{
    public class DepartmentEmployeeConfiguration : IEntityTypeConfiguration<DepartmentEmployeeModel>
    {
        public void Configure(EntityTypeBuilder<DepartmentEmployeeModel> builder)
        {
            builder.ToTable("DepartmentEmployee");

            builder.HasKey(ci => new { ci.DepartmentId,ci.EmployeeId});

            builder.HasOne(c => c.Department)
                .WithMany()
                .IsRequired()
                .HasForeignKey(c => c.DepartmentId);
        }
    }
}
