﻿using Nsr.RestClient.RestClients.Localization;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Nsr.Wh.Web.Domain;

namespace Nsr.Wh.Web.Services.ExportImport
{

    public class ExportManager : IExportManager
    {
        #region Fields

        private readonly ILocalizationWebApi _localizationWebApi;
        private readonly ILanguageWebApi _languageWebApi;

        #endregion

        #region Ctor

        public ExportManager(ILocalizationWebApi localizationWebApi, ILanguageWebApi languageWebApi)
        {
            this._localizationWebApi = localizationWebApi;
            this._languageWebApi = languageWebApi;
        }

        #endregion

        public async Task<string> ExportResumeToXml(Resume Resume)
        {
            var languages = await _languageWebApi.GetAll();

            var sb = new StringBuilder();
            var stringWriter = new StringWriter(sb);
            var xmlWriter = new XmlTextWriter(stringWriter);
            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("Resume");            

            xmlWriter.WriteElementString("Id", null, Resume.Id.ToString());
            xmlWriter.WriteElementString("Name", null, Resume.Name);
            xmlWriter.WriteElementString("Address", null, Resume.Address);
            xmlWriter.WriteElementString("PostalCode", null, Resume.PostalCode);
            xmlWriter.WriteElementString("Town", null, Resume.Town);
            xmlWriter.WriteElementString("Email", null, Resume.Email);
            xmlWriter.WriteElementString("Website", null, Resume.Website);
            xmlWriter.WriteElementString("LinkedIn", null, Resume.LinkedIn);            
            xmlWriter.WriteElement(languages, Resume, x => Resume.Summary);  

            #region Export Educations

            xmlWriter.WriteStartElement("Educations");            
            foreach (var edu in Resume.Educations)
            {
                xmlWriter.WriteStartElement("Education");
                xmlWriter.WriteElementString("Id", null, edu.Id.ToString());
                xmlWriter.WriteElement(languages, edu, x => edu.Name); 
                xmlWriter.WriteStartElement("Entries");
                foreach (var item in edu.EducationItems)
                {
                    xmlWriter.WriteStartElement("Entry");
                    xmlWriter.WriteElementString("Id", null, item.Id.ToString());
                    xmlWriter.WriteElement(languages, item, x => item.Name);
                    xmlWriter.WriteElement(languages, item, x => item.Place);
                    xmlWriter.WriteElement(languages, item, x => item.Period);
                    xmlWriter.WriteElement(languages, item, x => item.Description); 
                    xmlWriter.WriteEndElement();
                }

                xmlWriter.WriteEndElement(); // end Entries
                xmlWriter.WriteEndElement(); // end edu
            }
            
            xmlWriter.WriteEndElement();
            #endregion

            #region Export TechnicalSkills

            xmlWriter.WriteStartElement("TechnicalSkills");
            foreach (var sk in Resume.Skills)
            {
                xmlWriter.WriteStartElement("TechnicalSkill");
                xmlWriter.WriteElementString("Id", null, sk.Id.ToString());              
                xmlWriter.WriteElement(languages, sk, x => sk.Name);           
                xmlWriter.WriteStartElement("Entries");
                foreach(var item in sk.SkillItems)
                {
                    xmlWriter.WriteStartElement("Entry");
                    xmlWriter.WriteElementString("Id", null, item.Id.ToString());
                    xmlWriter.WriteElement(languages, item, x => item.Name);
                    //xmlWriter.WriteElement(languages, item, x => item.Level);
                    xmlWriter.WriteElementString("Level", null, item.Level.ToString());
                    xmlWriter.WriteElement(languages, item, x => item.LevelDescription);             
                    xmlWriter.WriteEndElement();
                }
                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndElement();
            }
            xmlWriter.WriteEndElement();
            #endregion

            #region Export Experiences

            xmlWriter.WriteStartElement("Experiences");
            foreach (var wx in Resume.Experiences)
            {
                xmlWriter.WriteStartElement("Experience");
                xmlWriter.WriteElementString("Id", null, wx.Id.ToString());
                xmlWriter.WriteElement(languages, wx, x => wx.Name);
                xmlWriter.WriteElement(languages, wx, x => wx.Period);
                xmlWriter.WriteElement(languages, wx, x => wx.Function);
                xmlWriter.WriteElement(languages, wx, x => wx.City);
                xmlWriter.WriteElement(languages, wx, x => wx.Tasks);
                xmlWriter.WriteElementString("DisplayOrder", null, wx.DisplayOrder.ToString()); 
                             
                xmlWriter.WriteStartElement("Projects");
                foreach (var p in wx.Projects)
                {
                    xmlWriter.WriteStartElement("Project");
                    xmlWriter.WriteElementString("Id", null, p.Id.ToString());
                    xmlWriter.WriteElement(languages, p, x => p.Name);
                    xmlWriter.WriteElement(languages, p, x => p.Description);
                    xmlWriter.WriteElement(languages, p, x => p.Technology);                   
                    xmlWriter.WriteEndElement();
                }
                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndElement();
            }
            xmlWriter.WriteEndElement();
            #endregion

            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndDocument();
            xmlWriter.Close();

            return stringWriter.ToString();
        }       

        
    }
}
