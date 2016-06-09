using System;
using System.Collections.Generic;
using System.Linq;

namespace TestUsbI2cInterface
{
    public static class EnumExtensions
    {
        //
        // http://stackoverflow.com/questions/23794691/extension-method-to-get-the-values-of-any-enum
        //
        public static IEnumerable<TEnum> Values<TEnum>()
            where TEnum : struct, IComparable, IFormattable, IConvertible
        {
            var enumType = typeof(TEnum);

            // Optional runtime check for completeness    
            if (!enumType.IsEnum)
            {
                throw new ArgumentException();
            }

            return Enum.GetValues(enumType).Cast<TEnum>();
        }
    }
}
