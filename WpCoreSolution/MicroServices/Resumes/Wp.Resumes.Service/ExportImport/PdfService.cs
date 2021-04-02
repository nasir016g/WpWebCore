using iText.IO.Font;
using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using Nsr.Common.Services;
using System;
using System.IO;
using System.Linq;
using Wp.Resumes.Core.Domain;


namespace Wp.Resumes.Services
{
    // Maybe an alternative method for generating Pdf's? http://www.codeproject.com/Articles/260470/PDF-reporting-using-ASP-NET-MVC3
    public class PdfService : IPdfService
    {
        private  ILocalizationService _localizationService;
        private readonly ILanguageService _languageService;

        public PdfService(ILocalizationService localizationService, ILanguageService languageService)
        {
            _localizationService = localizationService;
            _languageService = languageService;
        }


        #region Properties

        private Text Newline {
            get
            {
                 return new Text("\n");
            }
        }       

        #endregion

        #region Utilities

        private string StripNewline(string text)
        {
           return text.Replace(Environment.NewLine, String.Empty).Replace("\n", String.Empty).Replace("  ", String.Empty);
        }

        private void InsertExperienceRow(Experience exp, Table table, string resourceKey, Text value, float leading = 0)
        {
           
            //var cell = new Cell();
            //cell.SetBorder(Border.NO_BORDER);

            var resourceValue = _localizationService.GetResource(resourceKey);

            Cell cell = InsertColumn(new Text(resourceValue), leading);
            table.AddCell(cell);

            Cell cellValue = InsertColumn(value, leading);
            table.AddCell(cellValue);
        }

        private Cell InsertColumn(Text value, float leading = 0)
        {
            var cell = new Cell();           

            if (leading == 0)
            {
                cell.Add(new Paragraph(value));
            }
            else
            {
                cell.Add(new Paragraph(value).SetFixedLeading(leading));
            }

            return cell;
        }



        #endregion

        //#region Private Methods

        private void InsertEducations(Resume resume, Document doc)
        {
            //doc.Add(new Paragraph(_localizationService.GetResource("Resume.Fields.Educations"), h2));
            //doc.Add(line);

            foreach (var education in resume.Educations)
            {
                doc.Add(new Paragraph(new Text(education.GetLocalized(x => x.Name)).SetFontSize(12).SetBold().SetItalic())); // ict training

                foreach (var item in education.EducationItems)
                {
                    var table = new Table(UnitValue.CreatePercentArray(new float[] { 70, 30 }));
                    table.SetWidth(UnitValue.CreatePercentValue(100f));

                    var cell = new Cell();
                    cell.SetBorder(Border.NO_BORDER);
                    cell.Add(new Paragraph(new Text(item.GetLocalized(x => x.Name)).SetBold().SetFontSize(10).SetItalic()));
                    if (!String.IsNullOrEmpty(item.GetLocalized(x => x.Place)))
                        cell.Add(new Paragraph(item.GetLocalized(x => x.Place)));

                    string description = item.GetLocalized(x => x.Description);
                    if (description != null)
                    {
                        //description = StripNewline(description);
                        //description = HtmlHelper.StripTags(description);
                        cell.Add(new Paragraph(description).SetMarginLeft(20f));
                    }

                    table.AddCell(cell);

                    cell = new Cell();
                    cell.SetBorder(Border.NO_BORDER);
                    cell.Add(new Paragraph(item.GetLocalized(x => x.Period)).SetTextAlignment(TextAlignment.RIGHT));
                    table.AddCell(cell);

                    doc.Add(table);
                }
            }
        }

        private void InsertSkills(Resume resume, Document doc)
        {
            doc.Add(new Paragraph(new Text(_localizationService.GetResource("Resume.Fields.Skills")).SetBold().SetItalic().SetFontSize(12)));

            foreach (var skill in resume.Skills)
            {               
                doc.Add(new Paragraph(new Text(skill.GetLocalized(x => x.Name)).SetBold().SetFontSize(10)));
                var p = new Paragraph();
                foreach (var item in skill.SkillItems)
                {
                    if (String.IsNullOrWhiteSpace(item.LevelDescription))
                    {
                        p.Add(item.GetLocalized(x => x.Name));
                    }
                    else
                    {
                       p.Add(String.Format("{0} ({1})", item.GetLocalized(x => x.Name), item.LevelDescription));
                    }

                    p.Add(Newline);
                }
                doc.Add(p);
            }
        }

        private void InsertExperiences(Resume resume, Document doc)
        {
            doc.Add(new Paragraph(new Text(_localizationService.GetResource("Resume.Fields.Experiences")).SetFontSize(12).SetBold().SetItalic()));
            //doc.Add(line);
            doc.Add(new Paragraph());
            foreach (var experience in resume.Experiences.OrderBy(x => x.DisplayOrder))
            {
                //doc.Add(new Paragraph(exp.GetLocalized(x => x.Name), h3));// company name

              var table = new Table(UnitValue.CreatePercentArray(new float[] { 20, 80 }));
              table.SetWidth(UnitValue.CreatePercentValue(100f));


                InsertExperienceRow(experience, table, "Resume.Fields.Experiences.Name", new Text(experience.GetLocalized(x => x.Name)).SetBold(), 20);
                InsertExperienceRow(experience, table, "Resume.Fields.Experiences.Period", new Text(experience.GetLocalized(x => x.Period)));
                InsertExperienceRow(experience, table, "Resume.Fields.Experiences.Function", new Text(experience.GetLocalized(x => x.Function)));
                InsertExperienceRow(experience, table, "Resume.Fields.Experiences.City", new Text(experience.GetLocalized(x => x.City)));
                InsertExperienceRow(experience, table, "Resume.Fields.Experiences.Tasks", new Text(experience.GetLocalized(x => x.Tasks)));

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

        //#endregion

        #region Ctor

        //public PdfService(ILocalizationService localizationService, IWorkContext workContext)
        //{
        //    _localizationService = localizationService;
        //    _workContext = workContext;
        //}

        #endregion

        public void PrintResume(Stream stream, Resume resume)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");

            if (resume == null)
                throw new ArgumentNullException("Resume");

            var pageSize = iText.Kernel.Geom.PageSize.A4;

            var line = new LineSeparator(new SolidLine());
            var writer = new PdfWriter(stream);
            var pdf = new PdfDocument(writer);
            PdfFont font = PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN);

            var doc = new Document(pdf);
            doc.SetFont(font).SetFontSize(8);
            

            doc.Add(new Paragraph()
                .Add(new Text(resume.Name).SetItalic().SetBold().SetFontSize(12)).Add(Newline)
                .Add(new Text(resume.Address)).Add(Newline)
                .Add(new Text(string.Format("{0} {1}", resume.PostalCode, resume.Town))));
            
            //doc.Add(new Paragraph(10, "\u00a0"));
            var table = new Table(UnitValue.CreatePercentArray(new float[] { 10, 80 }));
            table.SetWidth(UnitValue.CreatePercentValue(100f));

            if (!String.IsNullOrWhiteSpace(resume.Phone))
            {
                table.AddCell(InsertColumn(new Text(string.Format("{0}", _localizationService.GetResource("Resume.Fields.TelePhone")))));
                table.AddCell(InsertColumn(new Text(resume.Phone)));
            }

            if (!String.IsNullOrWhiteSpace(resume.Mobile))
            {
                table.AddCell(InsertColumn(new Text(string.Format("{0}", _localizationService.GetResource("Resume.Fields.Mobile")))));
                table.AddCell(InsertColumn(new Text(resume.Mobile)));
            }

            if (!String.IsNullOrWhiteSpace(resume.Email))
            {
                table.AddCell(InsertColumn(new Text("Email:")));
                table.AddCell(InsertColumn(new Text(resume.Email)));
            }

            if (!String.IsNullOrWhiteSpace(resume.Website))
            {
                table.AddCell(InsertColumn(new Text("Website:")));
                table.AddCell(InsertColumn(new Text(resume.Website)));
            }

            if (!String.IsNullOrWhiteSpace(resume.LinkedIn))
            {
                table.AddCell(InsertColumn(new Text("LinkedIn:")));
                table.AddCell(InsertColumn(new Text(resume.LinkedIn)));
            }

            doc.Add(table);

            doc.Add(new Paragraph().Add(new Text(_localizationService.GetResource("Resume.Fields.Summary")).SetItalic().SetBold().SetFontSize(12)));
            doc.Add(new Paragraph().Add(new Text(resume.GetLocalized(x => x.Summary))).SetMarginLeft(20f));
            

            InsertEducations(resume, doc);
            InsertSkills(resume, doc);
            InsertExperiences(resume, doc);

            doc.Close();
        }


    }
}
