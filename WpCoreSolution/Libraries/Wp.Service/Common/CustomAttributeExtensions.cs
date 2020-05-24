using Wp.Core.Domain.Common;

namespace Wp.Services.Common
{
    public static class CustomAttributeExtensions
    {
        /// <summary>
        /// A value indicating whether this custom attribute should have values
        /// </summary>
        /// <param name="customAttribute">custom attribute</param>
        /// <returns>Result</returns>
        public static bool ShouldHaveValues(this CustomAttribute customAttribute)
        {
            if (customAttribute == null)
                return false;

            if (customAttribute.AttributeControlType == AttributeControlType.TextBox ||
                customAttribute.AttributeControlType == AttributeControlType.MultilineTextbox ||
                customAttribute.AttributeControlType == AttributeControlType.Datepicker ||
                customAttribute.AttributeControlType == AttributeControlType.FileUpload)
                return false;

            //other attribute controle types support values
            return true;
        }
    }
}
