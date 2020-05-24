using System;
//using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Wp.Core;
using Wp.Core.Common;
using Wp.Core.Domain.Career;
using Wp.Services.Localization;
using Xceed.Document.NET;
using Xceed.Words.NET;

namespace Wp.Services.ExportImport
{
    public class ExportWordService
    {
        #region Fields

        private readonly ILocalizationService _localizationService;
        private readonly ILanguageService _languageService;
        private readonly IWorkContext _workContext;

        #endregion

        #region Ctor

        public ExportWordService(ILocalizationService localizationService, ILanguageService languageService, IWorkContext workContext)
        {
            _localizationService = localizationService;
            _languageService = languageService;
            _workContext = workContext;
        }

        #endregion

        #region Properties        

        private Font TimesRoman
        {
            get
            {
                return new Font("Times New Roman");                
            }
        }

        private Formatting H2
        {
            get
            {
                return new Formatting { FontFamily = TimesRoman, FontColor = System.Drawing.Color.Black, Bold = true, Size = 16D };                
            }
        }

        private Formatting H3
        {
            get
            {
                return new Formatting { FontFamily = TimesRoman, FontColor = System.Drawing.Color.Black, Bold = true, Size = 14D };
            }
        }

        private Formatting H4
        {
            get
            {
                return new Formatting { FontFamily = TimesRoman, FontColor = System.Drawing.Color.Black, Bold = true, Italic = true, Size = 12D };
            }
        }

        private Formatting Normal
        {
            get
            {
                return new Formatting { FontFamily = new Font("Calibri"), Size = 10D };
            }
        }

        #endregion

        #region Utilities

        private static string RemoveNewLineAtStart(string description)
        {
            if (description.StartsWith("\n")) // remove new line in the start of a string
            {
                var regex = new Regex("\n");
                description = regex.Replace(description, "", 1);
            }

            if (description.StartsWith("\r\n")) // remove new line in the start of a string
            {
                var regex = new Regex("\r\n");
                description = regex.Replace(description, "", 1);
            }

            return description;
        }

        private void InsertExperienceRow(DocX doc, Table table, int rowIndex, string resourceKey, string value, Formatting formatting = null)
        {
            if (formatting == null)
            {
                formatting = Normal;
            }

            var row1 = table.Rows[rowIndex];
            row1.Cells[0].Width = doc.PageWidth * 0.2;
            row1.Cells[0].InsertParagraph(_localizationService.GetResource(resourceKey, _workContext.Current.WorkingLanguage.Id), false, Normal);
            row1.Cells[1].InsertParagraph(value, false, formatting);
        }

        private void InsertRowWith2Columns(Table table, string resourceString, string value)
        { 
            var row = table.Rows[0];
            row.Cells[0].Width = 100.0; //doc.PageWidth * 0.5;            
            row.Cells[0].InsertParagraph(resourceString, false, Normal);


            row.Cells[1].Width = 200.0; //doc.PageWidth * 0.3;                        
            var paragraph = row.Cells[1].InsertParagraph(value, false, Normal);           
        }

        #endregion

        #region Private Methods

        private void InsertEducations(Resume c, DocX doc)
        {
            // 1  educations   
            // doc.InsertParagraph(_localizationService.GetResource("Resume.Fields.Educations", _workContext.CurrentSession.WorkingLanguage.Id), false, h2);
            //.AppendLine(line);                

            foreach (var education in c.Educations)
            {
                doc.InsertParagraph().AppendLine();
                doc.InsertParagraph(education.GetLocalized(x => x.Name), false, H3); // ict training  

                foreach (var item in education.EducationItems)
                {
                    var table = doc.AddTable(1, 2); // computer science
                    table.Design = TableDesign.TableNormal;
                    //table.AutoFit = AutoFit.Contents;


                    var row = table.Rows[0];
                    row.Cells[0].Width = 440.0; //doc.PageWidth * 0.5;
                    row.Cells[0].InsertParagraph(item.GetLocalized(x => x.Name), false, H4);

                    var place = item.GetLocalized(x => x.Place);
                    if (!String.IsNullOrEmpty(item.GetLocalized(x => x.Place)))
                        row.Cells[0].InsertParagraph(item.GetLocalized(x => x.Place), false, Normal);

                    if (!String.IsNullOrEmpty(item.GetLocalized(x => x.Description)))
                    {
                        string description = item.GetLocalized(x => x.Description);
                        description = HtmlHelper.StripTags(description);   // strip html tags
                        description = RemoveNewLineAtStart(description);   // remove new line at the begining of description 
                        description = description.Replace("\t", String.Empty);

                        if (!String.IsNullOrEmpty(description))
                        {
                            row.Cells[0].InsertParagraph(description, false, Normal);
                        }
                    }

                    row.Cells[1].Width = 180.0; //doc.PageWidth * 0.3;                        
                    var paragraph = row.Cells[1].InsertParagraph(item.GetLocalized(x => x.Period), false, Normal);
                    paragraph.Alignment = Alignment.right;

                    doc.InsertTable(table);
                }
            }
        }

        private void InsertExperiences(Resume resume, DocX doc)
        {
            // 3 experience
            doc.InsertParagraph().AppendLine();
            doc.InsertParagraph(_localizationService.GetResource("Resume.Fields.Experiences", _workContext.Current.WorkingLanguage.Id), false, H2);
            //.AppendLine(line);

            foreach (var exp in resume.Experiences.OrderBy(x => x.DisplayOrder))
            {
                // doc.InsertParagraph().AppendLine();

                var table = doc.AddTable(5, 2);
                table.Design = TableDesign.TableNormal;
                table.AutoFit = AutoFit.Contents;

                InsertExperienceRow(doc, table, 0, "Resume.Fields.Experiences.Name", exp.GetLocalized(x => x.Name), H4);
                InsertExperienceRow(doc, table, 1, "Resume.Fields.Experiences.Period", exp.GetLocalized(x => x.Period));
                InsertExperienceRow(doc, table, 2, "Resume.Fields.Experiences.Function", exp.GetLocalized(x => x.Function));
                InsertExperienceRow(doc, table, 3, "Resume.Fields.Experiences.City", exp.GetLocalized(x => x.City));
                InsertExperienceRow(doc, table, 4, "Resume.Fields.Experiences.Tasks", exp.GetLocalized(x => x.Tasks));

                doc.InsertTable(table);

                //foreach (var p in exp.Projects)
                //{
                //    doc.InsertParagraph().AppendLine();
                //    doc.InsertParagraph(p.GetLocalized(x => x.Name), false, H4);
                //    doc.InsertParagraph(p.GetLocalized(x => x.Description), false, Normal);
                //    doc.InsertParagraph(p.GetLocalized(x => x.Technology), false, Normal);
                //}
            }
        }

        private void InsertSkills(Resume resume, DocX doc)
        {
            doc.InsertParagraph();
            doc.InsertParagraph(_localizationService.GetResource("Resume.Fields.Skills", _workContext.Current.WorkingLanguage.Id), false, H2);
            //.AppendLine(line);                

            foreach (var s in resume.Skills.OrderBy(x => x.DisplayOrder))
            {
                doc.InsertParagraph().AppendLine();
                doc.InsertParagraph(s.GetLocalized(x => x.Name), false, H3);

                Paragraph paragraph = doc.InsertParagraph();

                foreach (var item in s.SkillItems.OrderByDescending(x => x.Level))
                {
                    paragraph.Append(item.GetLocalized(x => x.Name));
                    if (!String.IsNullOrEmpty(item.GetLocalized(x => x.LevelDescription)))
                    {
                        paragraph.Append(String.Format(" ({0})", item.GetLocalized(x => x.LevelDescription)));
                    }
                    paragraph.Append("\n");
                }
            }
        }       

        #endregion

        public void ExportResumeToWord(Stream stream, Resume resume)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");

            if (resume == null)
                throw new ArgumentNullException("Resume");


            //StringBuilder sb = new StringBuilder();
            //for (int i = 0; i < 80; i++)
            //    sb.Append("_");
            //string line = sb.ToString();

            using (var doc = DocX.Create(stream))
            {
                doc.InsertParagraph(resume.Name, false, H3);
                doc.InsertParagraph(resume.Address, false, Normal);
                doc.InsertParagraph(string.Format("{0} {1}", resume.PostalCode, resume.Town), false, Normal).AppendLine();


                var table = doc.AddTable(1, 2);
                
                table.Design = TableDesign.TableNormal;
               
                if (!String.IsNullOrWhiteSpace(resume.Phone))
                {
                    InsertRowWith2Columns(table, _localizationService.GetResource("Resume.Fields.TelePhone"), resume.Phone);
                } 

                if (!String.IsNullOrWhiteSpace(resume.Mobile))
                {
                    InsertRowWith2Columns(table, _localizationService.GetResource("Resume.Fields.Mobile"), resume.Mobile);
                }

                if (!String.IsNullOrWhiteSpace(resume.Email))
                {
                    InsertRowWith2Columns(table, "Email:", resume.Email);
                }

                if (!String.IsNullOrWhiteSpace(resume.Website))
                {
                    InsertRowWith2Columns(table, "Website:", resume.Website);
                }

                if (!String.IsNullOrWhiteSpace(resume.LinkedIn))
                {
                    InsertRowWith2Columns(table, "LinkedIn:", resume.LinkedIn);
                }

                doc.InsertTable(table);
                doc.InsertParagraph().AppendLine();

                doc.InsertParagraph(_localizationService.GetResource("Resume.Fields.Summary", _workContext.Current.WorkingLanguage.Id), false, H2).AppendLine();
                doc.InsertParagraph(resume.GetLocalized(x => x.Summary), false, Normal).AppendLine();

                InsertEducations(resume, doc);
                InsertSkills(resume, doc);
                InsertExperiences(resume, doc);

                doc.Save();
            }
        }       
        
    }
}
