using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Shootsy.Database.Entities;
using Shootsy.Enums;

namespace Shootsy.Database.Configurations
{
    public class ProfessionEntityConfiguration : IEntityTypeConfiguration<ProfessionEntity>
    {
        public void Configure(EntityTypeBuilder<ProfessionEntity> builder)
        {
            builder.ToTable("it_professions", "public");
            builder.HasKey(x => x.Id);

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(p => p.Category)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasIndex(p => p.Name).IsUnique();
            builder.HasIndex(p => p.Category);
            builder.HasIndex(p => new { p.Category, p.Name });

            builder.HasMany(p => p.Users)
                .WithOne(u => u.ProfessionEntity)
                .HasForeignKey(u => u.ProfessionId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasData(
                new ProfessionEntity { Id = (int)ProfessionEnums.QA, Name = ProfessionEnums.QA.ToString(), Category = "Quality Assurance" },
                new ProfessionEntity { Id = (int)ProfessionEnums.AQA, Name = ProfessionEnums.AQA.ToString(), Category = "Quality Assurance" },
                new ProfessionEntity { Id = (int)ProfessionEnums.ManualQA, Name = ProfessionEnums.ManualQA.ToString(), Category = "Quality Assurance" },
                new ProfessionEntity { Id = (int)ProfessionEnums.SDET, Name = ProfessionEnums.SDET.ToString(), Category = "Quality Assurance" },
                new ProfessionEntity { Id = (int)ProfessionEnums.QALead, Name = ProfessionEnums.QALead.ToString(), Category = "Quality Assurance" },

                new ProfessionEntity { Id = (int)ProfessionEnums.FrontendDeveloper, Name = ProfessionEnums.FrontendDeveloper.ToString(), Category = "Development" },
                new ProfessionEntity { Id = (int)ProfessionEnums.BackendDeveloper, Name = ProfessionEnums.BackendDeveloper.ToString(), Category = "Development" },
                new ProfessionEntity { Id = (int)ProfessionEnums.FullStackDeveloper, Name = ProfessionEnums.FullStackDeveloper.ToString(), Category = "Development" },
                new ProfessionEntity { Id = (int)ProfessionEnums.WebDeveloper, Name = ProfessionEnums.WebDeveloper.ToString(), Category = "Development" },
                new ProfessionEntity { Id = (int)ProfessionEnums.MobileDeveloper, Name = ProfessionEnums.MobileDeveloper.ToString(), Category = "Development" },
                new ProfessionEntity { Id = (int)ProfessionEnums.iOSDeveloper, Name = ProfessionEnums.iOSDeveloper.ToString(), Category = "Development" },
                new ProfessionEntity { Id = (int)ProfessionEnums.AndroidDeveloper, Name = ProfessionEnums.AndroidDeveloper.ToString(), Category = "Development" },
                new ProfessionEntity { Id = (int)ProfessionEnums.GameDeveloper, Name = ProfessionEnums.GameDeveloper.ToString(), Category = "Development" },

                new ProfessionEntity { Id = (int)ProfessionEnums.DevOps, Name = ProfessionEnums.DevOps.ToString(), Category = "DevOps & Infrastructure" },
                new ProfessionEntity { Id = (int)ProfessionEnums.SystemAdministrator, Name = ProfessionEnums.SystemAdministrator.ToString(), Category = "DevOps & Infrastructure" },
                new ProfessionEntity { Id = (int)ProfessionEnums.NetworkEngineer, Name = ProfessionEnums.NetworkEngineer.ToString(), Category = "DevOps & Infrastructure" },
                new ProfessionEntity { Id = (int)ProfessionEnums.SecurityEngineer, Name = ProfessionEnums.SecurityEngineer.ToString(), Category = "DevOps & Infrastructure" },
                new ProfessionEntity { Id = (int)ProfessionEnums.SiteReliabilityEngineer, Name = ProfessionEnums.SiteReliabilityEngineer.ToString(), Category = "DevOps & Infrastructure" },
                new ProfessionEntity { Id = (int)ProfessionEnums.CloudEngineer, Name = ProfessionEnums.CloudEngineer.ToString(), Category = "DevOps & Infrastructure" },

                new ProfessionEntity { Id = (int)ProfessionEnums.DataScientist, Name = ProfessionEnums.DataScientist.ToString(), Category = "Data" },
                new ProfessionEntity { Id = (int)ProfessionEnums.DataAnalyst, Name = ProfessionEnums.DataAnalyst.ToString(), Category = "Data" },
                new ProfessionEntity { Id = (int)ProfessionEnums.DataEngineer, Name = ProfessionEnums.DataEngineer.ToString(), Category = "Data" },
                new ProfessionEntity { Id = (int)ProfessionEnums.MachineLearningEngineer, Name = ProfessionEnums.MachineLearningEngineer.ToString(), Category = "Data" },
                new ProfessionEntity { Id = (int)ProfessionEnums.BIAnalyst, Name = ProfessionEnums.BIAnalyst.ToString(), Category = "Data" },

                new ProfessionEntity { Id = (int)ProfessionEnums.ProjectManager, Name = ProfessionEnums.ProjectManager.ToString(), Category = "Management & Leadership" },
                new ProfessionEntity { Id = (int)ProfessionEnums.ProductManager, Name = ProfessionEnums.ProductManager.ToString(), Category = "Management & Leadership" },
                new ProfessionEntity { Id = (int)ProfessionEnums.ProductOwner, Name = ProfessionEnums.ProductOwner.ToString(), Category = "Management & Leadership" },
                new ProfessionEntity { Id = (int)ProfessionEnums.ScrumMaster, Name = ProfessionEnums.ScrumMaster.ToString(), Category = "Management & Leadership" },
                new ProfessionEntity { Id = (int)ProfessionEnums.TeamLead, Name = ProfessionEnums.TeamLead.ToString(), Category = "Management & Leadership" },
                new ProfessionEntity { Id = (int)ProfessionEnums.CTO, Name = ProfessionEnums.CTO.ToString(), Category = "Management & Leadership" },
                new ProfessionEntity { Id = (int)ProfessionEnums.HeadOfDevelopment, Name = ProfessionEnums.HeadOfDevelopment.ToString(), Category = "Management & Leadership" },

                new ProfessionEntity { Id = (int)ProfessionEnums.UXUIDesigner, Name = ProfessionEnums.UXUIDesigner.ToString(), Category = "Design" },
                new ProfessionEntity { Id = (int)ProfessionEnums.UIDesigner, Name = ProfessionEnums.UIDesigner.ToString(), Category = "Design" },
                new ProfessionEntity { Id = (int)ProfessionEnums.UXDesigner, Name = ProfessionEnums.UXDesigner.ToString(), Category = "Design" },
                new ProfessionEntity { Id = (int)ProfessionEnums.GraphicDesigner, Name = ProfessionEnums.GraphicDesigner.ToString(), Category = "Design" },
                new ProfessionEntity { Id = (int)ProfessionEnums.ProductDesigner, Name = ProfessionEnums.ProductDesigner.ToString(), Category = "Design" },

                new ProfessionEntity { Id = (int)ProfessionEnums.HR, Name = ProfessionEnums.HR.ToString(), Category = "HR & Recruitment" },
                new ProfessionEntity { Id = (int)ProfessionEnums.HRBP, Name = ProfessionEnums.HRBP.ToString(), Category = "HR & Recruitment" },
                new ProfessionEntity { Id = (int)ProfessionEnums.ITRecruiter, Name = ProfessionEnums.ITRecruiter.ToString(), Category = "HR & Recruitment" },
                new ProfessionEntity { Id = (int)ProfessionEnums.TalentAcquisition, Name = ProfessionEnums.TalentAcquisition.ToString(), Category = "HR & Recruitment" },
                new ProfessionEntity { Id = (int)ProfessionEnums.TechnicalRecruiter, Name = ProfessionEnums.TechnicalRecruiter.ToString(), Category = "HR & Recruitment" },

                new ProfessionEntity { Id = (int)ProfessionEnums.BusinessAnalyst, Name = ProfessionEnums.BusinessAnalyst.ToString(), Category = "Business & Analysis" },
                new ProfessionEntity { Id = (int)ProfessionEnums.SystemAnalyst, Name = ProfessionEnums.SystemAnalyst.ToString(), Category = "Business & Analysis" },
                new ProfessionEntity { Id = (int)ProfessionEnums.ProductAnalyst, Name = ProfessionEnums.ProductAnalyst.ToString(), Category = "Business & Analysis" },
                new ProfessionEntity { Id = (int)ProfessionEnums.TechnicalAnalyst, Name = ProfessionEnums.TechnicalAnalyst.ToString(), Category = "Business & Analysis" },

                new ProfessionEntity { Id = (int)ProfessionEnums.SolutionArchitect, Name = ProfessionEnums.SolutionArchitect.ToString(), Category = "Architecture" },
                new ProfessionEntity { Id = (int)ProfessionEnums.SystemArchitect, Name = ProfessionEnums.SystemArchitect.ToString(), Category = "Architecture" },
                new ProfessionEntity { Id = (int)ProfessionEnums.EnterpriseArchitect, Name = ProfessionEnums.EnterpriseArchitect.ToString(), Category = "Architecture" },
                new ProfessionEntity { Id = (int)ProfessionEnums.TechnicalArchitect, Name = ProfessionEnums.TechnicalArchitect.ToString(), Category = "Architecture" },

                new ProfessionEntity { Id = (int)ProfessionEnums.TechSupport, Name = ProfessionEnums.TechSupport.ToString(), Category = "Support" },
                new ProfessionEntity { Id = (int)ProfessionEnums.ITHelpdesk, Name = ProfessionEnums.ITHelpdesk.ToString(), Category = "Support" },
                new ProfessionEntity { Id = (int)ProfessionEnums.SupportEngineer, Name = ProfessionEnums.SupportEngineer.ToString(), Category = "Support" },
                new ProfessionEntity { Id = (int)ProfessionEnums.CustomerSupport, Name = ProfessionEnums.CustomerSupport.ToString(), Category = "Support" },

                new ProfessionEntity { Id = (int)ProfessionEnums.DatabaseAdministrator, Name = ProfessionEnums.DatabaseAdministrator.ToString(), Category = "Database" },
                new ProfessionEntity { Id = (int)ProfessionEnums.DBA, Name = ProfessionEnums.DBA.ToString(), Category = "Database" },
                new ProfessionEntity { Id = (int)ProfessionEnums.DatabaseDeveloper, Name = ProfessionEnums.DatabaseDeveloper.ToString(), Category = "Database" },

                new ProfessionEntity { Id = (int)ProfessionEnums.SoftwareEngineer, Name = ProfessionEnums.SoftwareEngineer.ToString(), Category = "Technical Roles" },
                new ProfessionEntity { Id = (int)ProfessionEnums.ResearchEngineer, Name = ProfessionEnums.ResearchEngineer.ToString(), Category = "Technical Roles" },
                new ProfessionEntity { Id = (int)ProfessionEnums.PerformanceEngineer, Name = ProfessionEnums.PerformanceEngineer.ToString(), Category = "Technical Roles" },
                new ProfessionEntity { Id = (int)ProfessionEnums.IntegrationEngineer, Name = ProfessionEnums.IntegrationEngineer.ToString(), Category = "Technical Roles" },

                new ProfessionEntity { Id = (int)ProfessionEnums.TechnicalWriter, Name = ProfessionEnums.TechnicalWriter.ToString(), Category = "Content & Marketing" },
                new ProfessionEntity { Id = (int)ProfessionEnums.ContentManager, Name = ProfessionEnums.ContentManager.ToString(), Category = "Content & Marketing" },
                new ProfessionEntity { Id = (int)ProfessionEnums.SEOSpecialist, Name = ProfessionEnums.SEOSpecialist.ToString(), Category = "Content & Marketing" },
                new ProfessionEntity { Id = (int)ProfessionEnums.ITMarketer, Name = ProfessionEnums.ITMarketer.ToString(), Category = "Content & Marketing" },

                new ProfessionEntity { Id = (int)ProfessionEnums.Other, Name = ProfessionEnums.Other.ToString(), Category = "Other" }
            );
        }
    }
}