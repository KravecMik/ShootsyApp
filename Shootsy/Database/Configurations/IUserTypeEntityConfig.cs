using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shootsy.Database.Entities;

namespace Shootsy.Database.Configurations
{
    public interface IUserTypeEntityConfig
    {
        void Configure(EntityTypeBuilder<UserTypeEntity> entity);
    }
}