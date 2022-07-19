/*
 * Created by Alexander Sosnovskiy. May 3, 2016
 */
using System;
using System.Security.Cryptography;
using System.Text;
using System.Globalization;
using System.Collections.Generic;

public static class StringExtensions
{
    public static int ParseInt(this string str)
    {
        if (string.IsNullOrEmpty(str))
            return 0;
        return int.Parse(str, CultureInfo.InvariantCulture);
    }

    public static float ParseFloat(this string str)
    {
        if (string.IsNullOrEmpty(str))
            return 0f;
        return float.Parse(str, CultureInfo.InvariantCulture);
    }

    public static bool ParseBool(this string str)
    {
        if (string.IsNullOrEmpty(str))
            return false;

        return str.Equals("1") || str.ToLower().Equals("true");
    }

    public static DateTime ParseDateTime(this string str)
    {
        if (string.IsNullOrEmpty(str))
            return DateTime.Now;

        return new DateTime(long.Parse(str));
    }

    public static string GetMd5Hash(this string str)
    {
		// step 1, calculate MD5 hash from input
		using (var md5 = MD5.Create())
		{
			byte[] inputBytes = Encoding.UTF8.GetBytes(str);
			byte[] hash = md5.ComputeHash(inputBytes);

			// step 2, convert byte array to hex string
			var sb = new StringBuilder();
			for (int i = 0; i < hash.Length; i++)
			{
				sb.Append(hash[i].ToString("X2"));
			}

			var res = sb.ToString().ToLower();

			return res;
		}
    }

	public static string Reverse(this string text)
	{
		char[] array = text.ToCharArray();
		Array.Reverse(array);
		return (new string(array));
	}
}