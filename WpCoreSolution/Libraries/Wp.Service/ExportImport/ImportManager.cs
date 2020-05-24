using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Wp.Core.Domain.Career;
using Wp.Core.Security;
using Wp.Service.Security;
using Wp.Services.Career;
using Wp.Services.Localization;

namespace Wp.Services.ExportImport
{

    public class ImportManager : IImportManager
    {
        private XDocument _xDoc = null;
        private readonly IResumeService _resService;
        private readonly IEducationService _educationService;
        private readonly ISkillService _skillService;
        private readonly IExperienceService _workExperienceService;
        private readonly IImportExcelService _importExpenseExcelService;
        private readonly ILanguageService _languageService;
        //private readonly IIdentityService _identityService;
        private readonly ILocalizedEntityService _localizedEntityService;

        public ImportManager(IResumeService resService,
            IEducationService educationService,
            ISkillService skillService,
            IExperienceService werkExperienceService,
            IImportExcelService importExpenseExcelService,
            ILanguageService languageService,
            //IIdentityService identityService,
            ILocalizedEntityService localizedEntityService)
        {
            _resService = resService;
            _educationService = educationService;
            _skillService = skillService;
            _workExperienceService = werkExperienceService;
            _importExpenseExcelService = importExpenseExcelService;
            _languageService = languageService;
            //_identityService = identityService;
            _localizedEntityService = localizedEntityService;
        }

        private string GetValue(XElement xElement)
        {
            return GetValue<string>(xElement);
        }

        private T GetValue<T>(XElement xElement)
        {
            T value;

            if (xElement == null)
            {
                return default(T);
            }

            if (xElement.HasElements)
                value = (T)Convert.ChangeType(xElement.Element("Standard").Value, typeof(T));
            else
                value = (T)Convert.ChangeType(xElement.Value, typeof(T));

            return value;
        }

        #region Import Educations

        #region Utilities

        protected void UpdateLocales(Education entity, XElement xElement)
        {
            var xName = xElement.Element("Name");
            foreach (var locale in xName.Descendants("Locale"))
            {
                var value = locale.Element("Value").Value;
                int languageId = _languageService.GetAll().FirstOrDefault(x => x.UniqueSeoCode == locale.Attribute("Language").Value).Id;
                _localizedEntityService.SaveLocalizedValue(entity,
                                                            x => x.Name,
                                                            value,
                                                            languageId);
            }
        }

        protected void UpdateLocales(EducationItem entity, XElement xElement)
        {
            var xName = xElement.Element("Name");
            foreach (var locale in xName.Descendants("Locale"))
            {
                var value = locale.Element("Value").Value;
                int languageId = _languageService.GetAll().FirstOrDefault(x => x.UniqueSeoCode == locale.Attribute("Language").Value).Id;
                _localizedEntityService.SaveLocalizedValue(entity,
                                                            x => x.Name,
                                                            value,
                                                            languageId);
            }

            var xPeriod = xElement.Element("Period");
            foreach (var locale in xPeriod.Descendants("Locale"))
            {
                var value = locale.Element("Value").Value;
                int languageId = _languageService.GetAll().FirstOrDefault(x => x.UniqueSeoCode == locale.Attribute("Language").Value).Id;
                _localizedEntityService.SaveLocalizedValue(entity,
                                                            x => x.Period,
                                                            value,
                                                            languageId);
            }

            var xPlace = xElement.Element("Place");
            if (xPlace != null)
            {
                foreach (var locale in xPlace.Descendants("Locale"))
                {
                    var value = locale.Element("Value").Value;
                    int languageId = _languageService.GetAll().FirstOrDefault(x => x.UniqueSeoCode == locale.Attribute("Language").Value).Id;
                    _localizedEntityService.SaveLocalizedValue(entity,
                                                                x => x.Place,
                                                                value,
                                                                languageId);
                }
            }

            var xDescription = xElement.Element("Description");
            if (xDescription != null)
            {
                foreach (var locale in xDescription.Descendants("Locale"))
                {
                    var value = locale.Element("Value").Value;
                    int languageId = _languageService.GetAll().FirstOrDefault(x => x.UniqueSeoCode == locale.Attribute("Language").Value).Id;
                    _localizedEntityService.SaveLocalizedValue(entity,
                                                                x => x.Description,
                                                                value,
                                                                languageId);
                }
            }
        }

        #endregion

        private void ImportEducations(IEnumerable<XElement> xEductaions, Resume Resume)
        {
            foreach (var xE in xEductaions)
            {
                var education = new Education
                {
                    ResumeId = Resume.Id,
                    Resume = Resume,
                    Name = GetValue(xE.Element("Name"))
                };
                _educationService.Insert(education);
                UpdateLocales(education, xE);

                var eEntries = xE.Descendants("Entry");
                foreach (var xEe in eEntries)
                {
                    var eItem = new EducationItem
                    {
                        EducationId = education.Id,
                        Education = education,
                        Name = GetValue(xEe.Element("Name")),
                        Period = GetValue(xEe.Element("Period")),
                        Place = GetValue(xEe.Element("Place")),
                        Description = GetValue(xEe.Element("Description"))
                    };
                    _educationService.InsertEducationItem(eItem);
                    UpdateLocales(eItem, xEe);
                }
            }
        }
        #endregion

        #region Import TechicalSkills

        #region Utilities

        protected void UpdateLocales(Skill entity, XElement xElement)
        {
            var xName = xElement.Element("Name");
            foreach (var locale in xName.Descendants("Locale"))
            {
                var value = locale.Element("Value").Value;
                int languageId = _languageService.GetAll().FirstOrDefault(x => x.UniqueSeoCode == locale.Attribute("Language").Value).Id;
                _localizedEntityService.SaveLocalizedValue(entity,
                                                            x => x.Name,
                                                            value,
                                                            languageId);
            }
        }

        protected void UpdateLocales(SkillItem entity, XElement xElement)
        {
            var xName = xElement.Element("Name");
            foreach (var locale in xName.Descendants("Locale"))
            {
                var value = locale.Element("Value").Value;
                int languageId = _languageService.GetAll().FirstOrDefault(x => x.UniqueSeoCode == locale.Attribute("Language").Value).Id;
                _localizedEntityService.SaveLocalizedValue(entity,
                                                            x => x.Name,
                                                            value,
                                                            languageId);
            }

            var xLevel = xElement.Element("Level");
            foreach (var locale in xLevel.Descendants("Locale"))
            {
                var value = locale.Element("Value").Value;
                int languageId = _languageService.GetAll().FirstOrDefault(x => x.UniqueSeoCode == locale.Attribute("Language").Value).Id;
                _localizedEntityService.SaveLocalizedValue(entity,
                                                            x => x.LevelDescription,
                                                            value,
                                                            languageId);
            }
        }

        #endregion
        private void ImportTechnicalSkills(IEnumerable<XElement> skills, Resume Resume)
        {
            foreach (var xS in skills)
            {
                var skill = new Skill
                {
                    ResumeId = Resume.Id,
                    Resume = Resume,
                    Name = GetValue(xS.Element("Name")),
                };
                _skillService.Insert(skill);
                UpdateLocales(skill, xS);

                var sEntries = xS.Descendants("Entry");
                foreach (var xSe in sEntries)
                {
                    var sItem = new SkillItem
                    {
                        SkillId = skill.Id,
                        Skill = skill,
                        Name = GetValue(xSe.Element("Name")),
                        Level = Convert.ToInt32(xSe.Element("Level").Value),
                        LevelDescription = GetValue(xSe.Element("LevelDescription"))
                    };
                    _skillService.InsertSkillItem(sItem);
                    UpdateLocales(sItem, xSe);
                }
            }
        }
        #endregion 

        #region Import Experience

        #region Utilities

        protected void UpdateLocales(Experience entity, XElement xElement)
        {
            var xName = xElement.Element("Name");
            foreach (var locale in xName.Descendants("Locale"))
            {
                var value = locale.Element("Value").Value;
                int languageId = _languageService.GetAll().FirstOrDefault(x => x.UniqueSeoCode == locale.Attribute("Language").Value).Id;
                _localizedEntityService.SaveLocalizedValue(entity,
                                                            x => x.Name,
                                                            value,
                                                            languageId);
            }

            var xPeriod = xElement.Element("Period");
            foreach (var locale in xPeriod.Descendants("Locale"))
            {
                var value = locale.Element("Value").Value;
                int languageId = _languageService.GetAll().FirstOrDefault(x => x.UniqueSeoCode == locale.Attribute("Language").Value).Id;
                _localizedEntityService.SaveLocalizedValue(entity,
                                                            x => x.Period,
                                                            value,
                                                            languageId);
            }

            var xFunction = xElement.Element("Function");
            foreach (var locale in xFunction.Descendants("Locale"))
            {
                var value = locale.Element("Value").Value;
                int languageId = _languageService.GetAll().FirstOrDefault(x => x.UniqueSeoCode == locale.Attribute("Language").Value).Id;
                _localizedEntityService.SaveLocalizedValue(entity,
                                                            x => x.Function,
                                                            value,
                                                            languageId);
            }

            var xTasks = xElement.Element("Tasks");
            foreach (var locale in xTasks.Descendants("Locale"))
            {
                var value = locale.Element("Value").Value;
                int languageId = _languageService.GetAll().FirstOrDefault(x => x.UniqueSeoCode == locale.Attribute("Language").Value).Id;
                _localizedEntityService.SaveLocalizedValue(entity,
                                                            x => x.Tasks,
                                                            value,
                                                            languageId);
            }
        }

        protected void UpdateLocales(Project entity, XElement xElement)
        {
            var xName = xElement.Element("Name");
            foreach (var locale in xName.Descendants("Locale"))
            {
                var value = locale.Element("Value").Value;
                int languageId = _languageService.GetAll().FirstOrDefault(x => x.UniqueSeoCode == locale.Attribute("Language").Value).Id;
                _localizedEntityService.SaveLocalizedValue(entity,
                                                            x => x.Name,
                                                            value,
                                                            languageId);
            }

            var xDescription = xElement.Element("Description");
            foreach (var locale in xDescription.Descendants("Locale"))
            {
                var value = locale.Element("Value").Value;
                int languageId = _languageService.GetAll().FirstOrDefault(x => x.UniqueSeoCode == locale.Attribute("Language").Value).Id;
                _localizedEntityService.SaveLocalizedValue(entity,
                                                            x => x.Description,
                                                            value,
                                                            languageId);
            }

            var xTechnology = xElement.Element("Technology");
            foreach (var locale in xTechnology.Descendants("Locale"))
            {
                var value = locale.Element("Value").Value;
                int languageId = _languageService.GetAll().FirstOrDefault(x => x.UniqueSeoCode == locale.Attribute("Language").Value).Id;
                _localizedEntityService.SaveLocalizedValue(entity,
                                                            x => x.Technology,
                                                            value,
                                                            languageId);
            }
        }

        #endregion
        private void ImportExperience(IEnumerable<XElement> experiences, Resume Resume)
        {
            foreach (var xExp in experiences)
            {
                var experience = new Experience
                {
                    ResumeId = Resume.Id,
                    Resume = Resume,
                    Name = GetValue(xExp.Element("Name")),
                    Function = GetValue(xExp.Element("Function")),
                    Period = GetValue(xExp.Element("Period")),
                    City = GetValue(xExp.Element("Town")),
                    Tasks = GetValue(xExp.Element("Tasks")),
                    DisplayOrder = GetValue<int>(xExp.Element("DisplayOrder"))
                };
                _workExperienceService.Insert(experience);
                UpdateLocales(experience, xExp);

                var projects = xExp.Descendants("Project");

                foreach (var xP in projects)
                {
                    var project = new Project
                    {
                        ExperienceId = experience.Id,
                        Experience = experience,
                        Name = GetValue(xP.Element("Name")),
                        Technology = GetValue(xP.Element("Technology")),
                        Description = GetValue(xP.Element("Description"))
                    };
                    _workExperienceService.InsertProject(project);
                    UpdateLocales(project, xP);
                }
            }
        }

        #endregion

        #region Utilities

        protected void UpdateLocales(Resume entity, XElement xElement)
        {
            var xName = xElement.Element("Summary");
            foreach (var locale in xName.Descendants("Locale"))
            {
                var value = locale.Element("Value").Value;
                int languageId = _languageService.GetAll().FirstOrDefault(x => x.UniqueSeoCode == locale.Attribute("Language").Value).Id;
                _localizedEntityService.SaveLocalizedValue(entity,
                                                            x => x.Summary,
                                                            value,
                                                            languageId);
            }
        }

        #endregion

        public Resume ImportWorkFromXml(string content, ApplicationUser user)
        {
            this._xDoc = XDocument.Parse(content, LoadOptions.SetLineInfo);
            var r = (from c in _xDoc.Descendants("Resume")
                     select c).FirstOrDefault();

            var resume = new Resume
            {
                Name = r.Element("Name").Value,
                Address = r.Element("Address").Value,
                PostalCode = r.Element("PostalCode").Value,
                Town = r.Element("Town").Value,
                ApplicationUserName = user.UserName,
                Summary = GetValue(r.Element("Summary"))
            };
            _resService.Insert(resume);
            UpdateLocales(resume, r);

            ImportEducations(_xDoc.Descendants("Education"), resume);
            ImportTechnicalSkills(_xDoc.Descendants("TechnicalSkill"), resume);
            ImportExperience(_xDoc.Descendants("Experience"), resume);

            return resume;
        }

        public void ImportExpensesFromXlsx(Stream stream)
        {
            _importExpenseExcelService.ImportExpensesFromXlsx(stream);
        }
    }
}
