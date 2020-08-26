using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Description).IsRequired().HasMaxLength(500);
            builder.Property(x => x.Price).HasColumnType("decimal(18,2)");
            builder.Property(x => x.PictureUrl).IsRequired();
            builder.HasOne(b => b.ProductBrand).WithMany().HasForeignKey(x => x.ProductBrandId);
            //each product has a single brand that its related too
            //each brand has many products
            //the foreign key has the ProductBrandId

            builder.HasOne(t => t.ProductType).WithMany().HasForeignKey(x => x.ProductTypeId);
            

        }
    }
}