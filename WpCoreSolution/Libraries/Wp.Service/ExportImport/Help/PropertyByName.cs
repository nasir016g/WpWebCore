using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Wp.Services.ExportImport.Help
{
        
    public class PropertyByName<T>
    {
        private object _propertyValue;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="propertyName">Property name</param>
        /// <param name="func">Feature property access</param>
        /// <param name="ignore">Specifies whether the property should be exported</param>
        public PropertyByName(string propertyName, Func<T, object> func = null, bool ignore = false)
        {
            PropertyName = propertyName;
            GetProperty = func;
            PropertyOrderPosition = 1;
            Ignore = ignore;
        }
        
        public int PropertyOrderPosition { get; set; }
        
        public Func<T, object> GetProperty { get; }
        
        public string PropertyName { get; }
        
        public object PropertyValue
        {
            get => _propertyValue;
            set => _propertyValue = value;
        }
        
        public int IntValue
        {
            get
            {
                if (PropertyValue == null || !int.TryParse(PropertyValue.ToString(), out var rez))
                    return default(int);
                return rez;
            }
        }
        
        public bool BooleanValue
        {
            get
            {
                if (PropertyValue == null || !bool.TryParse(PropertyValue.ToString(), out var rez))
                    return default(bool);
                return rez;
            }
        }
       
        public string StringValue => PropertyValue == null ? string.Empty : Convert.ToString(PropertyValue);
       
        public decimal DecimalValue
        {
            get
            {
                if (PropertyValue == null || !decimal.TryParse(PropertyValue.ToString(), out var rez))
                    return default(decimal);
                return rez;
            }
        }

        public decimal DecimalValueCommaDecimalSeparator
        {
            get
            {
                if (PropertyValue == null || !decimal.TryParse(PropertyValue.ToString(), NumberStyles.Currency, new NumberFormatInfo() {NumberDecimalSeparator = "," }, out var rez))
                    return default(decimal);
                return rez;
            }
        }

        public decimal? DecimalValueNullable
        {
            get
            {
                if (PropertyValue == null || !decimal.TryParse(PropertyValue.ToString(), out var rez))
                    return null;
                return rez;
            }
        }
        
        public double DoubleValue
        {
            get
            {
                if (PropertyValue == null || !double.TryParse(PropertyValue.ToString(), out var rez))
                    return default(double);
                return rez;
            }
        }

        public DateTime DateTime 
        {
            get 
            {
                DateTime.TryParseExact(StringValue, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out var rez);
                return rez;
            }
        }


        public DateTime? DateTimeNullable => PropertyValue == null ? null : DateTime.FromOADate(DoubleValue) as DateTime?;
        public override string ToString()
        {
            return PropertyName;
        }

        /// <summary>
        /// Specifies whether the property should be exported
        /// </summary>
        public bool Ignore { get; set; }
               
        public bool IsCaption => PropertyName == StringValue || PropertyName == _propertyValue.ToString();
    }
}

