using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ProjetPOO.Utilities.EntriesValidation
{
    public class ValidUtils
    {

        public static bool CheckIfPositiveNumber(int value)
        {
            return value > 0;
        }

        public static bool CheckIfPositiveNumber(double value)
        {
            return value > 0;
        }

        public static bool CheckIfNonNegativeNumber(int value)
        {
            return value >= 0;
        }

        public static bool CheckIfNonNegativeNumber(double value)
        {
            return value >= 0;
        }

        public static bool CheckEntryName(string value, int minLength = 3, int maxLength = 100)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            value = value.Trim();

            if (value.Length < minLength || value.Length > maxLength)
            {
                return false;
            }

            // Autorise :
            // - lettres (avec accents)
            // - chiffres
            // - espaces
            // - ponctuation simple : . , ' ! ? - :
            if (!Regex.IsMatch(value, @"^[\p{L}0-9 .,'!?:-]+$"))
            {
                return false;
            }

            // Interdit les doubles espaces
            if (value.Contains("  "))
            {
                return false;
            }

            return true;
        }

        public static bool CheckEntryDescription(string description, int lenght)
        {
            if (string.IsNullOrWhiteSpace(description))
                return false;

            description = description.Trim();

            if (description.Length < lenght)
            {

                return false;
            }

            return true;
        }

        public static bool CheckEntryPictureName(string value)
        {
            if (!Regex.IsMatch(value, @"^[a-zA-Z0-9_-]+\.(jpg|png)$"))
            {
                return false;
            }

            return true;
        }

        public static bool CheckFileFormat(string fileName, string[] AuthorizedFormats)
        {
            foreach (string format in AuthorizedFormats)
            {
                if (fileName.EndsWith("." + format, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }


        public static bool IsInRange(int value, int min, int max)
        {
            return value >= min && value <= max;
        }
        /// <summary>
        /// Check if value is in range defined by a low limit included
        /// </summary>
        /// <param name="value"></param>
        /// <param name="min"></param>
        /// <returns></returns>
        public static bool IsInRange(int value, int min)
        {
            return value >= min;
        }

        public static bool CheckIfNotNullOrEmpty(string value)
        {
            return !string.IsNullOrWhiteSpace(value);
        }

        public static bool CheckStringLength(string value, int minLength, int maxLength)
        {
            if (value == null)
            {
                return false;
            }
            value = value.Trim();
            return value.Length >= minLength && value.Length <= maxLength;
        }

        public static bool CheckIfNotNull(object value)
        {
            return value != null;
        }

        public static bool CheckIfNotNullAndPositive(int? value)
        {
            return value.HasValue && value.Value >= 0;
        }
    }
}
