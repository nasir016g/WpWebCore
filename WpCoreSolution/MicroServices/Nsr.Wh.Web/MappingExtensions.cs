using Nsr.RestClient.Models.WorkHistories;
using System.Collections.Generic;
using Nsr.Wh.Web.Domain;
using Nsr.Wh.Web.Infrastructure.Mapper;

namespace Nsr.Wh.Web
{
    public static class MappingExtensions
    {
        public static TDestination MapTo<TSource, TDestination>(this TSource source)
        {
            return AutoMapperConfiguration.Mapper.Map<TSource, TDestination>(source);
        }

        public static TDestination MapTo<TSource, TDestination>(this TSource source, TDestination destination)
        {
            return AutoMapperConfiguration.Mapper.Map(source, destination);
        }

        #region Work history
        public static ResumeModel ToModel(this Resume entity)
        {
            return entity.MapTo<Resume, ResumeModel>();
        }

        public static Resume ToEntity(this ResumeModel model)
        {
            return model.MapTo<ResumeModel, Resume>();
        }

        public static Resume ToEntity(this ResumeModel model, Resume destination)
        {
            return model.MapTo(destination);
        }

        public static IEnumerable<ResumeModel> ToModels(this IEnumerable<Resume> entities)
        {
            return entities.MapTo<IEnumerable<Resume>, IEnumerable<ResumeModel>>();
        }

        public static EducationModel ToModel(this Education entity)
        {
            return entity.MapTo<Education, EducationModel>();
        }

        public static Education ToEntity(this EducationModel model)
        {
            return model.MapTo<EducationModel, Education>();
        }

        public static Education ToEntity(this EducationModel model, Education destination)
        {
            return model.MapTo(destination);
        }

        public static IEnumerable<EducationModel> ToModels(this IEnumerable<Education> entities)
        {
            return entities.MapTo<IEnumerable<Education>, IEnumerable<EducationModel>>();
        }

        public static EducationItemModel ToModel(this EducationItem entity)
        {
            return entity.MapTo<EducationItem, EducationItemModel>();
        }

        public static EducationItem ToEntity(this EducationItemModel model)
        {
            return model.MapTo<EducationItemModel, EducationItem>();
        }

        public static EducationItem ToEntity(this EducationItemModel model, EducationItem destination)
        {
            return model.MapTo(destination);
        }

        public static IEnumerable<EducationItemModel> ToModels(this IEnumerable<EducationItem> entities)
        {
            return entities.MapTo<IEnumerable<EducationItem>, IEnumerable<EducationItemModel>>();
        }

        public static SkillModel ToModel(this Skill entity)
        {
            return entity.MapTo<Skill, SkillModel>();
        }

        public static Skill ToEntity(this SkillModel model)
        {
            return model.MapTo<SkillModel, Skill>();
        }

        public static Skill ToEntity(this SkillModel model, Skill destination)
        {
            return model.MapTo(destination);
        }

        public static IEnumerable<SkillModel> ToModels(this IEnumerable<Skill> entities)
        {
            return entities.MapTo<IEnumerable<Skill>, IEnumerable<SkillModel>>();
        }

        public static SkillItemModel ToModel(this SkillItem entity)
        {
            return entity.MapTo<SkillItem, SkillItemModel>();
        }

        public static SkillItem ToEntity(this SkillItemModel model)
        {
            return model.MapTo<SkillItemModel, SkillItem>();
        }

        public static SkillItem ToEntity(this SkillItemModel model, SkillItem destination)
        {
            return model.MapTo(destination);
        }

        public static IEnumerable<SkillItemModel> ToModels(this IEnumerable<SkillItem> entities)
        {
            return entities.MapTo<IEnumerable<SkillItem>, IEnumerable<SkillItemModel>>();
        }

        public static ExperienceModel ToModel(this Experience entity)
        {
            return entity.MapTo<Experience, ExperienceModel>();
        }

        public static Experience ToEntity(this ExperienceModel model)
        {
            return model.MapTo<ExperienceModel, Experience>();
        }

        public static Experience ToEntity(this ExperienceModel model, Experience destination)
        {
            return model.MapTo(destination);
        }

        public static IEnumerable<ExperienceModel> ToModels(this IEnumerable<Experience> entities)
        {
            return entities.MapTo<IEnumerable<Experience>, IEnumerable<ExperienceModel>>();
        }

        public static ProjectModel ToModel(this Project entity)
        {
            return entity.MapTo<Project, ProjectModel>();
        }

        public static Project ToEntity(this ProjectModel model)
        {
            return model.MapTo<ProjectModel, Project>();
        }

        public static Project ToEntity(this ProjectModel model, Project destination)
        {
            return model.MapTo(destination);
        }

        public static IEnumerable<ProjectModel> ToModels(this IEnumerable<Project> entities)
        {
            return entities.MapTo<IEnumerable<Project>, IEnumerable<ProjectModel>>();
        }
        #endregion
    }
}
