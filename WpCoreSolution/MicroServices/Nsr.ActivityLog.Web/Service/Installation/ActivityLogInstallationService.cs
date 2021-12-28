using Nsr.ActivityLogs.Web.Data;
using Nsr.ActivityLogs.Web.Data.Repositories;
using Nsr.RestClient.Models.ActivityLogs;

namespace Nsr.ActivityLogs.Web.Service.Installation
{

    public interface IActivityLogInstallationService
    {
        void InstallData();
    }
    public class ActivityLogInstallationService : IActivityLogInstallationService
    {
        private readonly IActivityLogBaseRepository<ActivityLogType> _activityLogTypeRepository;
        private readonly IActivityLogUnitOfWork _activityLogUnitOfWork;

        public ActivityLogInstallationService(IActivityLogBaseRepository<ActivityLogType> activityLogTypeRepository, IActivityLogUnitOfWork activityLogUnitOfWork)
        {
            _activityLogTypeRepository = activityLogTypeRepository;
            _activityLogUnitOfWork = activityLogUnitOfWork;
        }

        public void InstallData()
        {
            var activityLogTypes = new List<ActivityLogType>
            {
                new ActivityLogType
                {
                    SystemKeyword = "AddNewResume",
                    Enabled = true,
                    Name = "Added a new resume"
                },
                new ActivityLogType
                {
                    SystemKeyword = "EditResume",
                    Enabled = true,
                    Name = "Edited resume"
                },
                new ActivityLogType
                {
                    SystemKeyword = "DeleteResume",
                    Enabled = true,
                    Name = "Deleted resume"
                },

                #region education
                new ActivityLogType
                {
                    SystemKeyword = "AddNewEducation",
                    Enabled = true,
                    Name = "Added a new education"
                },
                new ActivityLogType
                {
                    SystemKeyword = "EditEducation",
                    Enabled = true,
                    Name = "Edited education"
                },
                new ActivityLogType
                {
                    SystemKeyword = "DeleteEducation",
                    Enabled = true,
                    Name = "Deleted education"
                },
                new ActivityLogType
                {
                    SystemKeyword = "AddNewEducationItem",
                    Enabled = true,
                    Name = "Added a new education item"
                },
                new ActivityLogType
                {
                    SystemKeyword = "EditEducationItem",
                    Enabled = true,
                    Name = "Edited education item"
                },
                new ActivityLogType
                {
                    SystemKeyword = "DeleteEducationItem",
                    Enabled = true,
                    Name = "Deleted education item"
                },
                #endregion

                #region skill
                new ActivityLogType
                {
                    SystemKeyword = "AddNewSkill",
                    Enabled = true,
                    Name = "Added a new skill"
                },
                new ActivityLogType
                {
                    SystemKeyword = "EditSkill",
                    Enabled = true,
                    Name = "Edited skill"
                },
                new ActivityLogType
                {
                    SystemKeyword = "DeleteSkill",
                    Enabled = true,
                    Name = "Deleted skill"
                },
                new ActivityLogType
                {
                    SystemKeyword = "AddNewSkillItem",
                    Enabled = true,
                    Name = "Added a new skill item"
                },
                new ActivityLogType
                {
                    SystemKeyword = "EditSkillItem",
                    Enabled = true,
                    Name = "Edited skill item"
                },
                new ActivityLogType
                {
                    SystemKeyword = "DeleteSkillItem",
                    Enabled = true,
                    Name = "Deleted skill item"
                },
                #endregion

                #region experience
                new ActivityLogType
                {
                    SystemKeyword = "AddNewExperience",
                    Enabled = true,
                    Name = "Added a new experience"
                },
                new ActivityLogType
                {
                    SystemKeyword = "EditExperience",
                    Enabled = true,
                    Name = "Edited experience"
                },
                new ActivityLogType
                {
                    SystemKeyword = "DeleteExperience",
                    Enabled = true,
                    Name = "Deleted experience"
                },
                new ActivityLogType
                {
                    SystemKeyword = "AddNewProject",
                    Enabled = true,
                    Name = "Added a new project"
                },
                new ActivityLogType
                {
                    SystemKeyword = "EditProject",
                    Enabled = true,
                    Name = "Edited project"
                },
                new ActivityLogType
                {
                    SystemKeyword = "DeleteProject",
                    Enabled = true,
                    Name = "Deleted project"
                }
                #endregion
            };

            _activityLogTypeRepository.AddRange(activityLogTypes);
            _activityLogUnitOfWork.Complete();
        }
    }
}
