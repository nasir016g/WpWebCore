using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace Wp.Services.ExportImport.Help
{

    /// <summary>
    /// Class for working with PropertyByName object list
    /// </summary>
    /// <typeparam name="T">Object type</typeparam>
    public class PropertyManager<T>
    {
        /// <summary>
        /// All properties
        /// </summary>
        private readonly Dictionary<string, PropertyByName<T>> _properties;
       
        public PropertyManager(IEnumerable<PropertyByName<T>> properties)
        {
            _properties = new Dictionary<string, PropertyByName<T>>();

            var poz = 1;
            foreach (var propertyByName in properties.Where(p => !p.Ignore))
            {
                propertyByName.PropertyOrderPosition = poz;
                poz++;
                _properties.Add(propertyByName.PropertyName, propertyByName);
            }
        }
        
        public void AddProperty(PropertyByName<T> property)
        {
            if (_properties.ContainsKey(property.PropertyName))
                return;

            _properties.Add(property.PropertyName, property);
        }      
        
        public T CurrentObject { get; set; }

        /// <summary>
        /// Return property index
        /// </summary>       
        public int GetIndex(string propertyName)
        {
            if (!_properties.ContainsKey(propertyName))
                return -1;

            return _properties[propertyName].PropertyOrderPosition;
        }

        /// <summary>
        /// Access object by property name
        /// </summary>
        /// <param name="propertyName">Property name</param>
        /// <returns>Property value</returns>
        public object this[string propertyName] => _properties.ContainsKey(propertyName) && CurrentObject != null
            ? _properties[propertyName].GetProperty(CurrentObject)
            : null;

        /// <summary>
        /// Remove object by property name
        /// </summary>
        /// <param name="propertyName">Property name</param>
        public void Remove(string propertyName)
        {
            _properties.Remove(propertyName);
        }
                              
        public int Count => _properties.Count;
               
        public PropertyByName<T> GetProperty(string propertyName)
        {
            return _properties.ContainsKey(propertyName) ? _properties[propertyName] : null;
        }
        
        public PropertyByName<T>[] GetProperties => _properties.Values.ToArray();
        
        public bool IsCaption
        {
            get { return _properties.Values.All(p => p.IsCaption); }
        }

        public virtual void ReadFromXlsx(ExcelWorksheet worksheet, int row, int cellOffset = 0)
        {
            if (worksheet?.Cells == null)
                return;

            foreach (var prop in _properties.Values)
            {
                prop.PropertyValue = worksheet.Cells[row, prop.PropertyOrderPosition + cellOffset].Value;
            }
        }

        public virtual void WriteToXlsx(ExcelWorksheet worksheet, int row, int cellOffset = 0, ExcelWorksheet fWorksheet = null)
        {
            if (CurrentObject == null)
                return;

            foreach (var prop in _properties.Values)
            {
                var cell = worksheet.Cells[row, prop.PropertyOrderPosition + cellOffset];
                cell.Value = prop.GetProperty(CurrentObject);
            }
        }

        public virtual byte[] ExportToXlsx(IEnumerable<T> itemsToExport)
        {
            using (var stream = new MemoryStream())
            {
                // ok, we can run the real code of the sample now
                using (var xlPackage = new ExcelPackage(stream))
                {
                    // uncomment this line if you want the XML written out to the outputDir
                    //xlPackage.DebugMode = true; 

                    // get handles to the worksheets
                    var worksheet = xlPackage.Workbook.Worksheets.Add(typeof(T).Name);
                    var fWorksheet = xlPackage.Workbook.Worksheets.Add("DataForFilters");
                    fWorksheet.Hidden = eWorkSheetHidden.VeryHidden;

                    //create Headers and format them 
                    WriteCaption(worksheet);

                    var row = 2;
                    foreach (var items in itemsToExport)
                    {
                        CurrentObject = items;
                        WriteToXlsx(worksheet, row++, fWorksheet: fWorksheet);
                    }

                    xlPackage.Save();
                }

                CurrentObject = default(T);
                return stream.ToArray();
            }
        }

        public virtual void WriteCaption(ExcelWorksheet worksheet, int row = 1, int cellOffset = 0)
        {
            foreach (var caption in _properties.Values)
            {
                var cell = worksheet.Cells[row, caption.PropertyOrderPosition + cellOffset];
                cell.Value = caption;

                SetCaptionStyle(cell);
                cell.Style.Hidden = false;
            }
        }
        public void SetCaptionStyle(ExcelRange cell)
        {
            cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
            cell.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(184, 204, 228));
            cell.Style.Font.Bold = true;
        }
    }
}
