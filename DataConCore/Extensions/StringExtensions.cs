using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace MarketingSuite360.Core;

public static class StringExtensions
{
    public static bool IsNullOrEmpty(this string s)
    {
        return string.IsNullOrEmpty(s);
    }

    public static bool IsNotNullOrEmpty(this string s)
    {
        return !string.IsNullOrEmpty(s);
    }

    public static string FormatWith(this string s, params object[] args)
    {
        return string.Format(s, args);
    }

    public static long ToInt64(this object thisValue, int defaultValue = 0)
    {
        long result = 0L;
        if (thisValue != null && thisValue != DBNull.Value && long.TryParse(thisValue.ToString(), out result))
        {
            return result;
        }

        return defaultValue;
    }

    public static T ToObject<T>(this string value)
    {
        if (string.IsNullOrEmpty(value)) return default;
        return JsonConvert.DeserializeObject<T>(value);
    }

    public static bool IsNumber(this string _value)
    {
        return QuickValidate("^[1-9]*[0-9]*$", _value);
    }

    public static bool QuickValidate(string _express, string _value)
    {
        if (_value == null)
        {
            return false;
        }

        Regex regex = new Regex(_express);
        if (_value.Length == 0)
        {
            return false;
        }

        return regex.IsMatch(_value);
    }
}
