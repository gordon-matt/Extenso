using System;
using Extenso.Data.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Demo.Extenso.AspNetCore.Blazor.OData.Data.Entities
{
    public class Person : IEntity
    {
        public int Id { get; set; }

        public string FamilyName { get; set; }

        public string GivenNames { get; set; }

        public DateTime DateOfBirth { get; set; }

        #region IEntity Members

        public object[] KeyValues
        {
            get { return new object[] { Id }; }
        }

        #endregion IEntity Members
    }

    public class PersonMap : IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person> builder)
        {
            builder.ToTable("People");
            builder.HasKey(m => m.Id);
            builder.Property(m => m.FamilyName).IsRequired().HasMaxLength(128).IsUnicode(true);
            builder.Property(m => m.GivenNames).IsRequired().HasMaxLength(128).IsUnicode(true);
            builder.Property(m => m.DateOfBirth).IsRequired();
        }
    }
}