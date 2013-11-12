using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;

public static class Extensions
{
    public static int ToIntegerOrDefault(this String input, int defaultValue)
    {
        int returnValue = (!String.IsNullOrEmpty(input)) ? Int32.Parse(input) : defaultValue;

        return returnValue;
    }

    public static IEnumerable<T> MyDistinct<T, TSelector>(this IEnumerable<T> dataObjs, Func<TSelector> propSelector)
    {
        HashSet<TSelector> previous = new HashSet<TSelector>();

        foreach (var dataObj in dataObjs)
        {
            PropertyInfo[] infos = dataObj.GetType().GetProperties();
            foreach (var info in infos)
            {
                if (propSelector.Invoke().GetType().Name == info.GetType().Name)
                {
                    previous.Add((TSelector)info.GetValue(dataObj, null));
                    yield return dataObj;
                }
            }
        }
    }

    public static int[] IndexesOf(this String input, char value)
    {
        List<int> indexList = new List<int>();

        for (int c = 0; c < input.Length; c++)
        {
            if (input[c] == value)
            {
                indexList.Add(c);
            }
        }

        return indexList.ToArray();
    }

    public static string SubtringByIndex(this String input, int startIndex, int endIndex)
    {
        int subStrLength = endIndex - startIndex;
        return input.Substring(startIndex, subStrLength);
    }

    public static string AsEmptyIfNull(this String input)
    {
        if (input == null)
        {
            return String.Empty;
        }
        else
        {
            return input;
        }

    }

    /// <summary>
    /// Attempts to convert current Object into a string. If object is NULL or EMPTY then default value passed will be returned.
    /// </summary>
    /// <param name="input">Current object.</param>
    /// <param name="defaultVal">Default value to pass back if input is NULL or Empty.</param>
    /// <returns>String representation of object IF SUCCESSFUL. ELSE, passed default value.</returns>
    public static string ToStringOrDefault(this object input, string defaultVal)
    {
        string returnVal = defaultVal;
        if (input != null && input.ToString() != String.Empty)
        {
            returnVal = input.ToString();
        }

        return returnVal;
    }

    public static bool ToBooleanOrDefault(this object input, bool defaultVal)
    {
        bool returnVal = defaultVal;
        if (input != null)
        {
            if (!Boolean.TryParse(input.ToString(), out returnVal))
            {
                returnVal = defaultVal;
            }
        }
        return returnVal;
    }

    public static int IndexOf(this string[] input, string valToFind)
    {
        int returnVal = 0;
        for (int i = 0; i < input.Length; i++)
        {
            if (input[i] == valToFind)
            {
                returnVal = i;
                break;
            }
        }

        return returnVal;
    }

    public static string ToSimplePlural(this string input)
    {
        string returnVal = input;
        bool formAlreadyEndsInS = (input.ToLower().EndsWith("s", StringComparison.OrdinalIgnoreCase));
        if (!formAlreadyEndsInS)
        {
            returnVal = input + "s";
        }

        return returnVal;
    }

    public static string Truncate(this string input, int length)
    {
        string safeInput = (input != null) ? input : "";
        if (safeInput.Length > length)
        {
            return safeInput.Substring(0, length);
        }

        return safeInput;
    }

    public static string RemoveHTML(this string input)
    {
        string noHtml = string.Empty;
        noHtml = System.Text.RegularExpressions.Regex.Replace(input, @"<[^>]*>", " ");
        noHtml = System.Text.RegularExpressions.Regex.Replace(noHtml, @"[\s\r\n]+", " ");

        return noHtml.Trim();
    }

    
}