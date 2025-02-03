using System.ComponentModel;
using System.Reflection;

namespace Utilities
{
    #region Extend

    public static class EnumExtensions
    {
        public static string GetDescription(this Enum @enum)
        {
            FieldInfo fi = @enum.GetType().GetField(@enum.ToString());
            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attributes != null && attributes.Length > 0)
                return attributes[0].Description;
            else
                return @enum.ToString();
        }

        public static string GetDescription<TEnum>(this object @object) where TEnum : struct
        {
            return Enum.TryParse(@object.ToString(), out TEnum @enum) ? (@enum as Enum).GetDescription() : @object.ToString();
        }
    }

    public class EnumDescriptionTypeConverter : EnumConverter
    {
        public EnumDescriptionTypeConverter(Type type)
            : base(type)
        {
        }
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                if (value != null)
                {
                    return (value as Enum).GetDescription();
                }

                return string.Empty;
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

    #endregion Extend
}
