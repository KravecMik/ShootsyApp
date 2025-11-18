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
                new ProfessionEntity { Id = (int)ITProfessionEnums.QA, Name = ITProfessionEnums.QA.ToString(), Category = "Quality Assurance" },
                new ProfessionEntity { Id = (int)ITProfessionEnums.AQA, Name = ITProfessionEnums.AQA.ToString(), Category = "Quality Assurance" },
                new ProfessionEntity { Id = (int)ITProfessionEnums.ManualQA, Name = ITProfessionEnums.ManualQA.ToString(), Category = "Quality Assurance" },
                new ProfessionEntity { Id = (int)ITProfessionEnums.SDET, Name = ITProfessionEnums.SDET.ToString(), Category = "Quality Assurance" },
                new ProfessionEntity { Id = (int)ITProfessionEnums.QALead, Name = ITProfessionEnums.QALead.ToString(), Category = "Quality Assurance" },

                new ProfessionEntity { Id = (int)ITProfessionEnums.FrontendDeveloper, Name = ITProfessionEnums.FrontendDeveloper.ToString(), Category = "Development" },
                new ProfessionEntity { Id = (int)ITProfessionEnums.BackendDeveloper, Name = ITProfessionEnums.BackendDeveloper.ToString(), Category = "Development" },
                new ProfessionEntity { Id = (int)ITProfessionEnums.FullStackDeveloper, Name = ITProfessionEnums.FullStackDeveloper.ToString(), Category = "Development" },
                new ProfessionEntity { Id = (int)ITProfessionEnums.WebDeveloper, Name = ITProfessionEnums.WebDeveloper.ToString(), Category = "Development" },
                new ProfessionEntity { Id = (int)ITProfessionEnums.MobileDeveloper, Name = ITProfessionEnums.MobileDeveloper.ToString(), Category = "Development" },
                new ProfessionEntity { Id = (int)ITProfessionEnums.iOSDeveloper, Name = ITProfessionEnums.iOSDeveloper.ToString(), Category = "Development" },
                new ProfessionEntity { Id = (int)ITProfessionEnums.AndroidDeveloper, Name = ITProfessionEnums.AndroidDeveloper.ToString(), Category = "Development" },
                new ProfessionEntity { Id = (int)ITProfessionEnums.GameDeveloper, Name = ITProfessionEnums.GameDeveloper.ToString(), Category = "Development" },

                new ProfessionEntity { Id = (int)ITProfessionEnums.DevOps, Name = ITProfessionEnums.DevOps.ToString(), Category = "DevOps & Infrastructure" },
                new ProfessionEntity { Id = (int)ITProfessionEnums.SystemAdministrator, Name = ITProfessionEnums.SystemAdministrator.ToString(), Category = "DevOps & Infrastructure" },
                new ProfessionEntity { Id = (int)ITProfessionEnums.NetworkEngineer, Name = ITProfessionEnums.NetworkEngineer.ToString(), Category = "DevOps & Infrastructure" },
                new ProfessionEntity { Id = (int)ITProfessionEnums.SecurityEngineer, Name = ITProfessionEnums.SecurityEngineer.ToString(), Category = "DevOps & Infrastructure" },
                new ProfessionEntity { Id = (int)ITProfessionEnums.SiteReliabilityEngineer, Name = ITProfessionEnums.SiteReliabilityEngineer.ToString(), Category = "DevOps & Infrastructure" },
                new ProfessionEntity { Id = (int)ITProfessionEnums.CloudEngineer, Name = ITProfessionEnums.CloudEngineer.ToString(), Category = "DevOps & Infrastructure" },

                new ProfessionEntity { Id = (int)ITProfessionEnums.DataScientist, Name = ITProfessionEnums.DataScientist.ToString(), Category = "Data" },
                new ProfessionEntity { Id = (int)ITProfessionEnums.DataAnalyst, Name = ITProfessionEnums.DataAnalyst.ToString(), Category = "Data" },
                new ProfessionEntity { Id = (int)ITProfessionEnums.DataEngineer, Name = ITProfessionEnums.DataEngineer.ToString(), Category = "Data" },
                new ProfessionEntity { Id = (int)ITProfessionEnums.MachineLearningEngineer, Name = ITProfessionEnums.MachineLearningEngineer.ToString(), Category = "Data" },
                new ProfessionEntity { Id = (int)ITProfessionEnums.BIAnalyst, Name = ITProfessionEnums.BIAnalyst.ToString(), Category = "Data" },

                new ProfessionEntity { Id = (int)ITProfessionEnums.ProjectManager, Name = ITProfessionEnums.ProjectManager.ToString(), Category = "Management & Leadership" },
                new ProfessionEntity { Id = (int)ITProfessionEnums.ProductManager, Name = ITProfessionEnums.ProductManager.ToString(), Category = "Management & Leadership" },
                new ProfessionEntity { Id = (int)ITProfessionEnums.ProductOwner, Name = ITProfessionEnums.ProductOwner.ToString(), Category = "Management & Leadership" },
                new ProfessionEntity { Id = (int)ITProfessionEnums.ScrumMaster, Name = ITProfessionEnums.ScrumMaster.ToString(), Category = "Management & Leadership" },
                new ProfessionEntity { Id = (int)ITProfessionEnums.TeamLead, Name = ITProfessionEnums.TeamLead.ToString(), Category = "Management & Leadership" },
                new ProfessionEntity { Id = (int)ITProfessionEnums.CTO, Name = ITProfessionEnums.CTO.ToString(), Category = "Management & Leadership" },
                new ProfessionEntity { Id = (int)ITProfessionEnums.HeadOfDevelopment, Name = ITProfessionEnums.HeadOfDevelopment.ToString(), Category = "Management & Leadership" },

                new ProfessionEntity { Id = (int)ITProfessionEnums.UXUIDesigner, Name = ITProfessionEnums.UXUIDesigner.ToString(), Category = "Design" },
                new ProfessionEntity { Id = (int)ITProfessionEnums.UIDesigner, Name = ITProfessionEnums.UIDesigner.ToString(), Category = "Design" },
                new ProfessionEntity { Id = (int)ITProfessionEnums.UXDesigner, Name = ITProfessionEnums.UXDesigner.ToString(), Category = "Design" },
                new ProfessionEntity { Id = (int)ITProfessionEnums.GraphicDesigner, Name = ITProfessionEnums.GraphicDesigner.ToString(), Category = "Design" },
                new ProfessionEntity { Id = (int)ITProfessionEnums.ProductDesigner, Name = ITProfessionEnums.ProductDesigner.ToString(), Category = "Design" },

                new ProfessionEntity { Id = (int)ITProfessionEnums.HR, Name = ITProfessionEnums.HR.ToString(), Category = "HR & Recruitment" },
                new ProfessionEntity { Id = (int)ITProfessionEnums.HRBP, Name = ITProfessionEnums.HRBP.ToString(), Category = "HR & Recruitment" },
                new ProfessionEntity { Id = (int)ITProfessionEnums.ITRecruiter, Name = ITProfessionEnums.ITRecruiter.ToString(), Category = "HR & Recruitment" },
                new ProfessionEntity { Id = (int)ITProfessionEnums.TalentAcquisition, Name = ITProfessionEnums.TalentAcquisition.ToString(), Category = "HR & Recruitment" },
                new ProfessionEntity { Id = (int)ITProfessionEnums.TechnicalRecruiter, Name = ITProfessionEnums.TechnicalRecruiter.ToString(), Category = "HR & Recruitment" },

                new ProfessionEntity { Id = (int)ITProfessionEnums.BusinessAnalyst, Name = ITProfessionEnums.BusinessAnalyst.ToString(), Category = "Business & Analysis" },
                new ProfessionEntity { Id = (int)ITProfessionEnums.SystemAnalyst, Name = ITProfessionEnums.SystemAnalyst.ToString(), Category = "Business & Analysis" },
                new ProfessionEntity { Id = (int)ITProfessionEnums.ProductAnalyst, Name = ITProfessionEnums.ProductAnalyst.ToString(), Category = "Business & Analysis" },
                new ProfessionEntity { Id = (int)ITProfessionEnums.TechnicalAnalyst, Name = ITProfessionEnums.TechnicalAnalyst.ToString(), Category = "Business & Analysis" },

                new ProfessionEntity { Id = (int)ITProfessionEnums.SolutionArchitect, Name = ITProfessionEnums.SolutionArchitect.ToString(), Category = "Architecture" },
                new ProfessionEntity { Id = (int)ITProfessionEnums.SystemArchitect, Name = ITProfessionEnums.SystemArchitect.ToString(), Category = "Architecture" },
                new ProfessionEntity { Id = (int)ITProfessionEnums.EnterpriseArchitect, Name = ITProfessionEnums.EnterpriseArchitect.ToString(), Category = "Architecture" },
                new ProfessionEntity { Id = (int)ITProfessionEnums.TechnicalArchitect, Name = ITProfessionEnums.TechnicalArchitect.ToString(), Category = "Architecture" },

                new ProfessionEntity { Id = (int)ITProfessionEnums.TechSupport, Name = ITProfessionEnums.TechSupport.ToString(), Category = "Support" },
                new ProfessionEntity { Id = (int)ITProfessionEnums.ITHelpdesk, Name = ITProfessionEnums.ITHelpdesk.ToString(), Category = "Support" },
                new ProfessionEntity { Id = (int)ITProfessionEnums.SupportEngineer, Name = ITProfessionEnums.SupportEngineer.ToString(), Category = "Support" },
                new ProfessionEntity { Id = (int)ITProfessionEnums.CustomerSupport, Name = ITProfessionEnums.CustomerSupport.ToString(), Category = "Support" },

                new ProfessionEntity { Id = (int)ITProfessionEnums.DatabaseAdministrator, Name = ITProfessionEnums.DatabaseAdministrator.ToString(), Category = "Database" },
                new ProfessionEntity { Id = (int)ITProfessionEnums.DBA, Name = ITProfessionEnums.DBA.ToString(), Category = "Database" },
                new ProfessionEntity { Id = (int)ITProfessionEnums.DatabaseDeveloper, Name = ITProfessionEnums.DatabaseDeveloper.ToString(), Category = "Database" },

                new ProfessionEntity { Id = (int)ITProfessionEnums.SoftwareEngineer, Name = ITProfessionEnums.SoftwareEngineer.ToString(), Category = "Technical Roles" },
                new ProfessionEntity { Id = (int)ITProfessionEnums.ResearchEngineer, Name = ITProfessionEnums.ResearchEngineer.ToString(), Category = "Technical Roles" },
                new ProfessionEntity { Id = (int)ITProfessionEnums.PerformanceEngineer, Name = ITProfessionEnums.PerformanceEngineer.ToString(), Category = "Technical Roles" },
                new ProfessionEntity { Id = (int)ITProfessionEnums.IntegrationEngineer, Name = ITProfessionEnums.IntegrationEngineer.ToString(), Category = "Technical Roles" },

                new ProfessionEntity { Id = (int)ITProfessionEnums.TechnicalWriter, Name = ITProfessionEnums.TechnicalWriter.ToString(), Category = "Content & Marketing" },
                new ProfessionEntity { Id = (int)ITProfessionEnums.ContentManager, Name = ITProfessionEnums.ContentManager.ToString(), Category = "Content & Marketing" },
                new ProfessionEntity { Id = (int)ITProfessionEnums.SEOSpecialist, Name = ITProfessionEnums.SEOSpecialist.ToString(), Category = "Content & Marketing" },
                new ProfessionEntity { Id = (int)ITProfessionEnums.ITMarketer, Name = ITProfessionEnums.ITMarketer.ToString(), Category = "Content & Marketing" },

                new ProfessionEntity { Id = (int)ITProfessionEnums.Other, Name = ITProfessionEnums.Other.ToString(), Category = "Other" }
            );
        }
    }
}