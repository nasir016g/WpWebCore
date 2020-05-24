
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;
using Wp.Core.Domain.Common;
using Wp.Services.Localization;

namespace Wp.Services.Common
{
    public partial class CustomAttributeParser : ICustomAttributeParser
    {
        private readonly ICustomAttributeService _customAttributeService;
        private readonly ILocalizationService _localizationService;

        public ICustomAttributeService CustomAttributeService => _customAttributeService;

        public CustomAttributeParser(ICustomAttributeService customAttributeService,
            ILocalizationService localizationService)
        {
            this._customAttributeService = customAttributeService;
            this._localizationService = localizationService;
        }

        protected virtual IList<int> ParseCustomAttributeIds(string attributesXml)
        {
            var ids = new List<int>();
            if (String.IsNullOrEmpty(attributesXml))
                return ids;

            try
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(attributesXml);

                foreach (XmlNode node in xmlDoc.SelectNodes(@"//Attributes/CustomAttribute"))
                {
                    if (node.Attributes != null && node.Attributes["ID"] != null)
                    {
                        string str1 = node.Attributes["ID"].InnerText.Trim();
                        int id;
                        if (int.TryParse(str1, out id))
                        {
                            ids.Add(id);
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                Debug.Write(exc.ToString());
            }
            return ids;
        }

        public IList<CustomAttribute> ParseCustomAttributes(string attributesXml)
        {
            var result = new List<CustomAttribute>();
            var ids = ParseCustomAttributeIds(attributesXml);
            foreach (int id in ids)
            {
                var attribute = CustomAttributeService.GetCustomAttributeById(id);
                if (attribute != null)
                {
                    result.Add(attribute);
                }
            }
            return result;
        }

        public virtual IList<CustomAttributeValue> ParseCustomAttributeValues(string attributesXml)
        {
            var values = new List<CustomAttributeValue>();
            var attributes = ParseCustomAttributes(attributesXml);
            foreach (var attribute in attributes)
            {
                if (!attribute.ShouldHaveValues())
                    continue;

                var valuesStr = ParseValues(attributesXml, attribute.Id);
                foreach (string valueStr in valuesStr)
                {
                    if (!String.IsNullOrEmpty(valueStr))
                    {
                        int id;
                        if (int.TryParse(valueStr, out id))
                        {
                            var value = CustomAttributeService.GetCustomAttributeValueById(id);
                            if (value != null)
                                values.Add(value);
                        }
                    }
                }
            }
            return values;
        }

        public virtual IList<string> ParseValues(string attributesXml, int customAttributeId)
        {
            var selectedCustomAttributeValues = new List<string>();
            try
            {
                var xmlDoc = new System.Xml.XmlDocument();
                xmlDoc.LoadXml(attributesXml);

                var nodeList1 = xmlDoc.SelectNodes(@"//Attributes/CustomAttribute");
                foreach (XmlNode node1 in nodeList1)
                {
                    if (node1.Attributes != null && node1.Attributes["ID"] != null)
                    {
                        string str1 = node1.Attributes["ID"].InnerText.Trim();
                        int id;
                        if (int.TryParse(str1, out id))
                        {
                            if (id == customAttributeId)
                            {
                                var nodeList2 = node1.SelectNodes(@"CustomAttributeValue/Value");
                                foreach (XmlNode node2 in nodeList2)
                                {
                                    string value = node2.InnerText.Trim();
                                    selectedCustomAttributeValues.Add(value);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                Debug.Write(exc.ToString());
            }
            return selectedCustomAttributeValues;
        }

        public virtual string AddCustomAttribute(string attributesXml, CustomAttribute attribute, string value)
        {
            string result = string.Empty;
            try
            {
                var xmlDoc = new XmlDocument();
                if (String.IsNullOrEmpty(attributesXml))
                {
                    var element1 = xmlDoc.CreateElement("Attributes");
                    xmlDoc.AppendChild(element1);
                }
                else
                {
                    xmlDoc.LoadXml(attributesXml);
                }
                var rootElement = (XmlElement)xmlDoc.SelectSingleNode(@"//Attributes");

                XmlElement attributeElement = null;
                //find existing
                var nodeList1 = xmlDoc.SelectNodes(@"//Attributes/CustomAttribute");
                foreach (XmlNode node1 in nodeList1)
                {
                    if (node1.Attributes != null && node1.Attributes["ID"] != null)
                    {
                        string str1 = node1.Attributes["ID"].InnerText.Trim();
                        int id;
                        if (int.TryParse(str1, out id))
                        {
                            if (id == attribute.Id)
                            {
                                attributeElement = (XmlElement)node1;
                                break;
                            }
                        }
                    }
                }

                //create new one if not found
                if (attributeElement == null)
                {
                    attributeElement = xmlDoc.CreateElement("CustomAttribute");
                    attributeElement.SetAttribute("ID", attribute.Id.ToString());
                    rootElement.AppendChild(attributeElement);
                }

                var attributeValueElement = xmlDoc.CreateElement("CustomAttributeValue");
                attributeElement.AppendChild(attributeValueElement);

                var attributeValueValueElement = xmlDoc.CreateElement("Value");
                attributeValueValueElement.InnerText = value;
                attributeValueElement.AppendChild(attributeValueValueElement);

                result = xmlDoc.OuterXml;
            }
            catch (Exception exc)
            {
                Debug.Write(exc.ToString());
            }
            return result;
        }

        /// <summary>
        /// Validates custom attributes
        /// </summary>
        /// <param name="attributesXml">Attributes in XML format</param>
        /// <returns>Warnings</returns>
        public virtual IList<string> GetAttributeWarnings(string attributesXml)
        {
            var warnings = new List<string>();

            //ensure it's our attributes
            var attributes1 = ParseCustomAttributes(attributesXml);

            //validate required custom attributes (whether they're chosen/selected/entered)
            var attributes2 = CustomAttributeService.GetAllCustomAttributes();
            foreach (var a2 in attributes2)
            {
                if (a2.IsRequired)
                {
                    bool found = false;
                    //selected custom attributes
                    foreach (var a1 in attributes1)
                    {
                        if (a1.Id == a2.Id)
                        {
                            var valuesStr = ParseValues(attributesXml, a1.Id);
                            foreach (string str1 in valuesStr)
                            {
                                if (!String.IsNullOrEmpty(str1.Trim()))
                                {
                                    found = true;
                                    break;
                                }
                            }
                        }
                    }

                    //if not found
                    if (!found)
                    {
                        var notFoundWarning = string.Format("Attribute {0} not found.",a2.Name);

                        warnings.Add(notFoundWarning);
                    }
                }
            }

            return warnings;
        }

    }
}
