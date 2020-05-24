using System.Collections.Generic;
using Wp.Core.Domain.Common;

namespace Wp.Services.Common
{
    public partial interface ICustomAttributeParser
    {
        /// <summary>
        /// Gets selected attributes
        /// </summary>
        /// <param name="attributesXml">Attributes in XML format</param>
        /// <returns>Selected address attributes</returns>
        IList<CustomAttribute> ParseCustomAttributes(string attributesXml);

        /// <summary>
        /// Get custom attribute values
        /// </summary>
        /// <param name="attributesXml">Attributes in XML format</param>
        /// <returns>attribute values</returns>
        IList<CustomAttributeValue> ParseCustomAttributeValues(string attributesXml);

        /// <summary>
        /// Gets selected attribute value
        /// </summary>
        /// <param name="attributesXml">Attributes in XML format</param>
        /// <param name="customAttributeId">attribute identifier</param>
        /// <returns>attribute value</returns>
        IList<string> ParseValues(string attributesXml, int customAttributeId);

        /// <summary>
        /// Adds an attribute
        /// </summary>
        /// <param name="attributesXml">Attributes in XML format</param>
        /// <param name="attribute">attribute</param>
        /// <param name="value">Value</param>
        /// <returns>Attributes</returns>
        string AddCustomAttribute(string attributesXml, CustomAttribute attribute, string value);

        /// <summary>
        /// Validates address attributes
        /// </summary>
        /// <param name="attributesXml">Attributes in XML format</param>
        /// <returns>Warnings</returns>
        IList<string> GetAttributeWarnings(string attributesXml);
    }
}