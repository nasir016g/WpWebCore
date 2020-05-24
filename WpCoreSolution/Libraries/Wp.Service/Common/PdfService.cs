using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;
using System;
using System.IO;
using Wp.Core;
using Wp.Core.Domain.Career;
using Wp.Services.Localization;
using System.Linq;

namespace Wp.Services.Common
{  
    // Maybe an alternative method for generating Pdf's? http://www.codeproject.com/Articles/260470/PDF-reporting-using-ASP-NET-MVC3
    public class PdfService : IPdfService
    {
        #region Fields

        private readonly IWebHelper _webHelper;
        private readonly ILocalizationService _localizationService;
        private readonly ILanguageService _languageService;
        private readonly IWorkContext _workContext;

        #endregion

        #region Properties

        private BaseFont TimesRoman
        {
            get
            {
                return BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
            }
        }

        private Font H2
        {
            get
            {
                return new Font(TimesRoman, 14, Font.BOLD, BaseColor.BLACK);
            }
        }

        private Font H3
        {
            get
            {
                return new Font(TimesRoman, 12, Font.BOLDITALIC, BaseColor.BLACK);
            }
        }

        private Font H4
        {
            get
            {
                return new Font(TimesRoman, 8, Font.BOLD, BaseColor.BLACK);
            }
        }

        private Font Normal
        {
            get
            {
                return new Font(TimesRoman, 8, Font.NORMAL, BaseColor.BLACK);
            }
        }

        #endregion

        #region Utilities

        private string StripNewline(string text)
        {
           return text.Replace(Environment.NewLine, String.Empty).Replace("\n", String.Empty).Replace("  ", String.Empty);
        }

        private void InsertExperienceRow(Experience exp, PdfPTable table, string resourceKey, string value, Font formatting = null, float leading = 0)
        {
            if (formatting == null) formatting = Normal;

           var resourceValue = _localizationService.GetResource(resourceKey, _workContext.Current.WorkingLanguage.Id);

           PdfPCell cell = InsertColumn(resourceValue, Normal, leading);
           table.AddCell(cell);

           PdfPCell cellValue = InsertColumn(value, formatting, leading);
           table.AddCell(cellValue);           
        }

        private PdfPCell InsertColumn(string value, Font formatting, float leading = 0)
        {
            var cell = new PdfPCell();
            cell.Border = Rectangle.NO_BORDER;
            cell.Padding = 0;

            if (leading == 0)
            {
                cell.AddElement(new Paragraph(value, formatting));
            }
            else
            {
                cell.AddElement(new Paragraph(leading, value, formatting));
            }

            return cell;
        }

        #endregion

        #region Private Methods

        private void InsertEducations(Resume resume, Document doc)
        {
            //doc.Add(new Paragraph(_localizationService.GetResource("Resume.Fields.Educations"), h2));
            //doc.Add(line);

            foreach (var education in resume.Educations)
            {
                doc.Add(new Paragraph(30, education.GetLocalized(x => x.Name), H3)); // ict training

                foreach (var item in education.EducationItems)
                {
                    var table = new PdfPTable(2); // computer science
                    table.WidthPercentage = 100f;
                    table.SetWidths(new[] { 70, 30 });

                    var cell = new PdfPCell();
                    cell.Border = Rectangle.NO_BORDER;
                    cell.AddElement(new Paragraph(20, item.GetLocalized(x => x.Name), H4));
                    if (!String.IsNullOrEmpty(item.GetLocalized(x => x.Place)))
                        cell.AddElement(new Paragraph(item.GetLocalized(x => x.Place), Normal));

                    string description = item.GetLocalized(x => x.Description);
                    if (description != null)
                    {
                        //description = StripNewline(description);
                        //description = HtmlHelper.StripTags(description);
                        cell.AddElement(new Paragraph(description, Normal));
                    }

                    table.AddCell(cell);

                    var paraPeriod = new Paragraph(item.GetLocalized(x => x.Period), Normal);
                    paraPeriod.Alignment = Element.ALIGN_RIGHT;

                    cell = new PdfPCell();
                    cell.Border = Rectangle.NO_BORDER;                   
                    cell.AddElement(paraPeriod);
                    table.AddCell(cell);

                    doc.Add(table);
                }
            }
        }

        private void InsertSkills(Resume resume, Document doc)
        {
            doc.Add(new Paragraph(30, _localizationService.GetResource("Resume.Fields.Skills"), H3));

            foreach (var skill in resume.Skills)
            {

                doc.Add(new Paragraph(20, skill.GetLocalized(x => x.Name), H3));
                foreach (var item in skill.SkillItems)
                {
                    if (String.IsNullOrWhiteSpace(item.LevelDescription))
                    {
                        doc.Add(new Paragraph(item.GetLocalized(x => x.Name), Normal));
                    }
                    else
                    {
                        doc.Add(new Paragraph(String.Format("{0} ({1})", item.GetLocalized(x => x.Name), item.LevelDescription), Normal));
                    }
                }
            }
        }

        private void InsertExperiences(Resume resume, Document doc)
        {
            doc.Add(new Paragraph(30, _localizationService.GetResource("Resume.Fields.Experiences"), H2));
            //doc.Add(line);
            doc.Add(new Paragraph());
            foreach (var experience in resume.Experiences.OrderBy(x => x.DisplayOrder))
            {
                //doc.Add(new Paragraph(exp.GetLocalized(x => x.Name), h3));// company name

                var table = new PdfPTable(2);
                table.WidthPercentage = 100f;
                table.SetWidths(new[] { 20, 80 });

                InsertExperienceRow(experience, table, "Resume.Fields.Experiences.Name", experience.GetLocalized(x => x.Name), H4, 20);
                InsertExperienceRow(experience, table, "Resume.Fields.Experiences.Period", experience.GetLocalized(x => x.Period));
                InsertExperienceRow(experience, table, "Resume.Fields.Experiences.Function", experience.GetLocalized(x => x.Function));
                InsertExperienceRow(experience, table, "Resume.Fields.Experiences.City", experience.GetLocalized(x => x.City));
                InsertExperienceRow(experience, table, "Resume.Fields.Experiences.Tasks", experience.GetLocalized(x => x.Tasks));

                doc.Add(table);


                //foreach (var p in exp.Projects)
                //{
                //    string description = p.GetLocalized(x => x.Description);
                //    description = HtmlHelper.StripTags(StripNewline(description));

                //    doc.Add(new Paragraph(p.GetLocalized(x => x.Name), h4));                   
                //    doc.Add(new Paragraph(description, normal));
                //    doc.Add(new Paragraph(p.GetLocalized(x => x.Technology), normal));
                //    doc.Add(new Paragraph("\n"));
                //}
            }
        }

        #endregion

        #region Ctor

        public PdfService(IWebHelper webHelper, ILocalizationService localizationService, ILanguageService languageService, IWorkContext workContext)
        {
            this._webHelper = webHelper;
            this._localizationService = localizationService;
            this._languageService = languageService;
            this._workContext = workContext;
        }

        #endregion

        public void PrintResume(Stream stream, Resume resume)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");

            if (resume == null)
                throw new ArgumentNullException("Resume");

            var pageSize = PageSize.A4;
            var line = new LineSeparator(1, 100, BaseColor.BLACK, Element.ALIGN_CENTER, -6);

            var doc = new Document(pageSize);
            PdfWriter.GetInstance(doc, stream);
            doc.Open();          

            doc.Add(new Paragraph(resume.Name, H3));
            doc.Add(new Paragraph(resume.Address, Normal));
            doc.Add(new Paragraph(string.Format("{0} {1}", resume.PostalCode, resume.Town), Normal));

            doc.Add(new Paragraph(10, "\u00a0"));
            var table = new PdfPTable(2);
            table.WidthPercentage = 100f;
            table.SetWidths(new[] { 10, 80 });

            if(!String.IsNullOrWhiteSpace(resume.Phone))
            {
                table.AddCell(InsertColumn(string.Format("{0}", _localizationService.GetResource("Resume.Fields.TelePhone")), Normal));
                table.AddCell(InsertColumn(resume.Phone, Normal)); 
            }

            if (!String.IsNullOrWhiteSpace(resume.Mobile))
            {
                table.AddCell(InsertColumn(string.Format("{0}", _localizationService.GetResource("Resume.Fields.Mobile")), Normal));
                table.AddCell(InsertColumn(resume.Mobile, Normal)); 
            }

            if (!String.IsNullOrWhiteSpace(resume.Email))
            {
                table.AddCell(InsertColumn("Email:", Normal));
                table.AddCell(InsertColumn(resume.Email, Normal));
            }

            if (!String.IsNullOrWhiteSpace(resume.Website))
            {
                table.AddCell(InsertColumn("Website:", Normal));
                table.AddCell(InsertColumn(resume.Website, Normal)); 
            }

            if (!String.IsNullOrWhiteSpace(resume.LinkedIn))
            {
                table.AddCell(InsertColumn("LinkedIn:", Normal));
                table.AddCell(InsertColumn(resume.LinkedIn, Normal));
            }

            doc.Add(table);
            doc.Add(new Paragraph(10, "\u00a0"));

            doc.Add(new Paragraph(20, _localizationService.GetResource("Resume.Fields.Summary"), H3));
            doc.Add(new Paragraph(10, "\u00a0"));
            doc.Add(new Paragraph(resume.GetLocalized(x => x.Summary), Normal));  
           
            InsertEducations(resume, doc); 
            InsertSkills(resume, doc);
            InsertExperiences(resume, doc);

            doc.Close();
        }

        //public void PrintResume(Stream stream, Resume c)
        //{
        //    Document document = new Document();
        //    try
        //    {                
        //        //PdfWriter.GetInstance(document, new FileStream("c:\\my.pdf", FileMode.Create));
        //        var writer = PdfWriter.GetInstance(document, stream);
        //        document.Open();
        //        WebClient wc = new WebClient();
        //        string htmlText = wc.DownloadString("http://localhost/wp.web/resume/details");
        //        StringReader html = new StringReader(htmlText);
               
        //        //List<IElement> htmlarraylist = HTMLWorker.ParseToList(new StringReader(htmlText), null);
        //        XMLWorkerHelper.GetInstance().ParseXHtml(writer, document, html); //(writer, document html);

        //        //for (int k = 0; k < htmlarraylist.Count; k++)
        //        //{
        //        //    document.Add((IElement)htmlarraylist[k]);
        //        //}

        //        document.Close();
        //    }
        //    catch
        //    {
        //    }
        //}
    }
}
