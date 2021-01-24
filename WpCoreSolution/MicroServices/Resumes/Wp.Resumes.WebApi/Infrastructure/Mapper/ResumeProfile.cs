using AutoMapper;
using Wp.Resumes.Core.Domain;
using Wp.Resumes.WebApi.Models;

namespace Wp.Resumes.WebApi.Infrastructure
{
    public class ResumeProfile : Profile
    {
        public ResumeProfile()
        {

            CreateMap<ResumeModel, Resume>()
                .ForMember(dest => dest.Experiences, mo => mo.Ignore())
                .ForMember(dest => dest.Educations, mo => mo.Ignore())
                .ForMember(dest => dest.Skills, mo => mo.Ignore())
                .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                .ForMember(dest => dest.UpdatedOn, mo => mo.Ignore());
            CreateMap<Resume, ResumeModel>()
                .ForMember(dest => dest.Locales, mo => mo.Ignore());

            CreateMap<EducationModel, Education>()
                .ForMember(dest => dest.Resume, mo => mo.Ignore())
                .ForMember(dest => dest.EducationItems, mo => mo.Ignore())
                .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                .ForMember(dest => dest.UpdatedOn, mo => mo.Ignore());
            CreateMap<Education, EducationModel>()
                .ForMember(dest => dest.Locales, mo => mo.Ignore());
            CreateMap<EducationItemModel, EducationItem>()
                .ForMember(dest => dest.Education, mo => mo.Ignore())
                .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                .ForMember(dest => dest.UpdatedOn, mo => mo.Ignore());
            CreateMap<EducationItem, EducationItemModel>()
                .ForMember(dest => dest.Locales, mo => mo.Ignore());

            CreateMap<SkillModel, Skill>()
                .ForMember(dest => dest.Resume, mo => mo.Ignore())
                .ForMember(dest => dest.SkillItems, mo => mo.Ignore())
                .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                .ForMember(dest => dest.UpdatedOn, mo => mo.Ignore());
            CreateMap<Skill, SkillModel>()
                .ForMember(dest => dest.Locales, mo => mo.Ignore());
            CreateMap<SkillItemModel, SkillItem>()
                .ForMember(dest => dest.Skill, mo => mo.Ignore())
                .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                .ForMember(dest => dest.UpdatedOn, mo => mo.Ignore());
            CreateMap<SkillItem, SkillItemModel>()
                .ForMember(dest => dest.Locales, mo => mo.Ignore());

            CreateMap<ExperienceModel, Experience>()
                .ForMember(dest => dest.Resume, mo => mo.Ignore())
                .ForMember(dest => dest.Projects, mo => mo.Ignore())
                .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                .ForMember(dest => dest.UpdatedOn, mo => mo.Ignore());
            CreateMap<Experience, ExperienceModel>()
                .ForMember(dest => dest.Locales, mo => mo.Ignore());
            CreateMap<ProjectModel, Project>()
                .ForMember(dest => dest.Experience, mo => mo.Ignore())
                .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                .ForMember(dest => dest.UpdatedOn, mo => mo.Ignore());
            CreateMap<Project, ProjectModel>()
                .ForMember(dest => dest.Locales, mo => mo.Ignore());

        }

    }
}
