using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml;
using Wp.Core;
using Wp.Core.Domain.Localization;
using Wp.Services.Localization;

namespace Wp.Services.ExportImport
{
    public static class XmlWriterExtensions
    {
        public static void WriteElement<T, R>(this XmlTextWriter writer, IList<Language> languages, T item, Expression<Func<T, R>> keySelector)
         where T : IEntity, ILocalizedEntity
        {
            var member = keySelector.Body as MemberExpression;
            if (member == null)
            {
                throw new ArgumentException(string.Format(
                    "Expression '{0}' refers to a method, not a property.",
                    keySelector));
            }
            
            var propInfo = member.Member as PropertyInfo;
            if (propInfo == null)
            {
                throw new ArgumentException(string.Format(
                       "Expression '{0}' refers to a field, not a property.",
                       keySelector));
            }

            var exp = (MemberExpression)member.Expression;
            var constant = (ConstantExpression)exp.Expression;
            var fieldInfoValue = ((FieldInfo)exp.Member).GetValue(constant.Value);
            var value = ((PropertyInfo)member.Member).GetValue(fieldInfoValue, null);
            var item2 = ((T)fieldInfoValue);

            if(value != null)
            {
                writer.WriteStartElement(propInfo.Name); 
                //writer.WriteElementString("Standard", null, propInfo.GetValue(item).ToString());
                writer.WriteElementString("Standard", null, value.ToString());
                writer.WriteStartElement("Locales");

                foreach (var l in languages)
                {
                    //var result = item.GetLocalized(keySelector, l.Id, false);
                    var result = item2.GetLocalized(keySelector, l.Id, false);

                    if (result != null)
                        WriteLocale(writer, l.UniqueSeoCode, result.ToString());
                    else
                        WriteLocale(writer, l.UniqueSeoCode, string.Empty);
                }

                writer.WriteEndElement(); // end locals
                writer.WriteEndElement(); // end name
            }
        }

        public static void WriteLocale(this XmlTextWriter writer, string languaeCode, string localizedValue)
        {
            writer.WriteStartElement("Locale");
            writer.WriteAttributeString("Language", languaeCode);
            writer.WriteElementString("Value", null, localizedValue);
            writer.WriteEndElement();
        }
    }
}
