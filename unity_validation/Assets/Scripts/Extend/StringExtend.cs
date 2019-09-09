using UnityEngine;
using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public static class StringExtend
{
    /// <summary>
    /// カラータグ付きの文字列を返す
    /// </summary>
    /// <param name="self"></param>
    /// <param name="color"></param>
    /// <returns></returns>
    public static string WithColorTag(this string self, Color color)
    {
        string colorTag = ExchangeToByte(color);
        return string.Format("<color=#{0}>{1}</color>", colorTag, self);
    }

    /// <summary>
    /// カラータグ付きの文字列を返す
    /// </summary>
    /// <param name="self"></param>
    /// <param name="color"></param>
    /// <returns></returns>
    public static string WithColorTag(this string self, Color32 color)
    {
        string colorTag = ExchangeToString(color.r, color.g, color.b);
        return string.Format("<color=#{0}>{1}</color>", colorTag, self);
    }

    /// <summary>
    /// RGB値に変換
    /// </summary>
    /// <param name="color"></param>
    /// <returns></returns>
    private static string ExchangeToByte(Color color)
    {
        int r = (int)(color.r * 255.0f);
        int g = (int)(color.g * 255.0f);
        int b = (int)(color.b * 255.0f);

        return ExchangeToString(r, g, b);
    }

    /// <summary>
    /// カラーコードに変換
    /// </summary>
    /// <param name="r"></param>
    /// <param name="g"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    private static string ExchangeToString(int r, int g, int b)
    {
        // @memo. 16進数に変換
        string r16 = string.Format("{0}", r.ToString("x4"));
        string g16 = string.Format("{0}", g.ToString("x4"));
        string b16 = string.Format("{0}", b.ToString("x4"));

        // @memo. 先頭の余分な桁数を削除
        r16 = r16.Remove(0, 2);
        g16 = g16.Remove(0, 2);
        b16 = b16.Remove(0, 2);

        return r16 + g16 + b16;
    }

    /// <summary>
    /// 指定された文字列が後部にあった場合、それを削除した文字列を返す
    /// </summary>
    /// <param name="self"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static string RemoveAtLast(this string self, string key)
    {
        return self.Remove(self.LastIndexOf(key, System.StringComparison.CurrentCulture), key.Length);
    }

    /// <summary>
    /// 自身の文字列から引数で指定された分のランダム文字列を抽出して返す
    /// </summary>
    /// <param name="self"></param>
    /// <param name="length"></param>
    /// <returns></returns>
    public static string GeneratePassword(this string self, int length)
    {
        var sb = new System.Text.StringBuilder(length);
        var r = new System.Random();

        for (int i = 0; i < length; i++)
        {
            int index = r.Next(self.Length);
            char c = self[index];
            sb.Append(c);
        }

        return sb.ToString();
    }

    /// <summary>
    /// UnicodeをShift-JISに変換する
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static string ToShiftJISFromUnicode(this string self)
    {
        var unicode = Encoding.Unicode;
        var unicodeByte = unicode.GetBytes(self);
        var shiftJis = Encoding.GetEncoding("shift_jis");
        var shiftJisByte = Encoding.Convert(unicode, shiftJis, unicodeByte);
        var shiftJisChars = new char[shiftJis.GetCharCount(shiftJisByte, 0, shiftJisByte.Length)];
        shiftJis.GetChars(shiftJisByte, 0, shiftJisByte.Length, shiftJisChars, 0);

        return new string(shiftJisChars);
    }

    /// <summary>
    /// 自身を引数で指定した数に分割する
    /// 引数は何分割するかを指定する(文字数ではない、文字数指定は標準でSubstringがある)
    /// </summary>
    /// <param name="self"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    public static string[] SubstringAtCount(this string self, int count)
    {
        List<string> result = new List<string>();
        int length = (int)Math.Ceiling((double)self.Length / count);

        for (int i = 0; i < length; i++)
        {
            int start = count * i;
            if (self.Length <= start)
            {
                break;
            }

            if (self.Length < start + count)
            {
                result.Add(self.Substring(start));
            }
            else
            {
                result.Add(self.Substring(start, count));
            }
        }

        return result.ToArray();
    }

    /// <summary>
    /// <br>タグを\nに変換
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static string ReplaceNewLineTag(this string self)
    {
        return self.Replace("<br>", "\n");
    }

    /// <summary>
    /// 指定されたパス文字列から拡張子を削除して返す
    /// </summary>
    /// <param name="self"></param>
    /// <param name="path"></param>
    /// <returns></returns>
    public static string GetPathWithoutExtension(this string self, string path)
    {
        var extention = Path.GetExtension(path);
        if (string.IsNullOrEmpty(extention))
        {
            return path;
        }

        return path.Replace(extention, string.Empty);
    }

    /// <summary>
    /// 自身の文字列をBase64でエンコードして返す
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static string Encode(this string self)
    {
        return Convert.ToBase64String(Encoding.UTF8.GetBytes(self));
    }

    /// <summary>
    /// 自身の文字列をBase64でデコードして返す
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static string Decode(this string self)
    {
        return Encoding.UTF8.GetString(Convert.FromBase64String(self));
    }

    /// <summary>
    /// 指定された文字列を含んでいるかどうか
    /// </summary>
    /// <param name="self"></param>
    /// <param name="strList"></param>
    /// <returns></returns>
    public static bool IsInclude(this string self, params string[] strList)
    {
        return strList.Any(c => self.Contains(c));
    }

    /// <summary>
    /// 自身がメールアドレスかどうか
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static bool IsMailAddress(this string self)
    {
        if (string.IsNullOrEmpty(self))
        {
            return false;
        }

        return Regex.IsMatch(self, @"^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,4}$", RegexOptions.IgnoreCase);
    }

    /// <summary>
    /// 自身が電話番号かどうか
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static bool IsPhoneNumber(this string self)
    {
        if (string.IsNullOrEmpty(self))
        {
            return false;
        }

        return Regex.IsMatch(self, @"^0\d{1,4}-\d{1,4}-\d{4}$");
    }

    /// <summary>
    /// 自身が郵便番号かどうか
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static bool IsZipCode(this string self)
    {
        if (string.IsNullOrEmpty(self))
        {
            return false;
        }

        return Regex.IsMatch(self, @"^\d\d\d-\d\d\d\d$", RegexOptions.ECMAScript);
    }

    /// <summary>
	/// 自身に半角仮名文字が含まれているかどうか
	/// </summary>
	/// <returns></returns>
    public static bool ContainsHalfWidthKana(this string self)
    {
        if (string.IsNullOrEmpty(self))
        {
            return false;
        }

        return Regex.IsMatch(self, @"[\uFF61-\uFF9F]");
    }

    /// <summary>
    /// 自身がURLかどうかを返す
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static bool IsURL(this string self)
    {
        if (string.IsNullOrEmpty(self))
        {
            return false;
        }

        return Regex.IsMatch(self, @"^s?https?://[-_.!~*'()a-zA-Z0-9;/?:@&=+$,%#]+$");
    }

    /// <summary>
	/// 引数で指定された回数分、自信を連結して返す
	/// </summary>
	/// <param name="self"></param>
	/// <param name="repeatCount"></param>
	/// <returns></returns>
    public static string Repeat(this string self, int repeatCount)
    {
        var builder = new StringBuilder();

        for (int i = 0; i < repeatCount; i++)
        {
            builder.Append(self);
        }

        return builder.ToString();
    }

    /// <summary>
	/// 指定した文字列を削除した新たな文字列を返す
	/// </summary>
	/// <param name="self"></param>
	/// <param name="removeStr"></param>
	/// <returns></returns>
    public static string ReplaceEmpty(this string self, string removeStr)
    {
        return self.Replace(removeStr, string.Empty);
    }

    /// <summary>
	/// 自身に引数で指定された分の固定少数点を追加した文字列を返す
	/// 自身が数値を文字列化している場合に限る
	/// </summary>
	/// <param name="self"></param>
	/// <param name="digits"></param>
	/// <returns></returns>
    public static string FixedPoint(this string self, int digits)
    {
        int number = int.Parse(self);
        return number.ToString("F" + digits);
    }

    /// <summary>
    /// スネークケースをアッパーキャメル(パスカル)ケースに変換
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
	public static string SnakeToUpperCamel(this string self)
    {
        if (string.IsNullOrEmpty(self))
        {
            return self;
        }

        return self.Split(new[] { '_' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(s => char.ToUpperInvariant(s[0]) + s.Substring(1, s.Length - 1))
            .Aggregate(string.Empty, (s1, s2) => s1 + s2);
    }

    /// <summary>
    /// スネークケースをローワーキャメル(キャメル)ケースに変換
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static string SnakeToLowerCamel(this string self)
    {
        if (string.IsNullOrEmpty(self))
        {
            return self;
        }

        return self.SnakeToUpperCamel()
            .Insert(0, char.ToLowerInvariant(self[0]).ToString())
            .Remove(1, 1);
    }

    /// <summary>
    /// 自身の先頭文字を大文字に変更する
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static string ToTitleCase(this string self)
    {
        return System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(self);
    }

    /// <summary>
    /// 引数で指定された文字列で自身を区切って返す
    /// </summary>
    /// <param name="self"></param>
    /// <param name="separator"></param>
    /// <returns></returns>
    public static string[] Split(this string self, string separator)
    {
        return self.Split(new[] { separator }, StringSplitOptions.None);
    }

    /// <summary>
    /// 引数で指定された文字列で自身を区切って返す
    /// 第2引数でオプションを指定する
    /// </summary>
    /// <param name="self"></param>
    /// <param name="separator"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public static string[] Split(this string self, string separator, StringSplitOptions options)
    {
        return self.Split(new[] { separator }, options);
    }

    /// <summary>
    /// 引数で指定された文字列で自身を区切って返す
    /// 複数のタグが指定されている場合
    /// </summary>
    /// <param name="self"></param>
    /// <param name="separator"></param>
    /// <returns></returns>
    public static string[] Split(this string self, params string[] separator)
    {
        return self.Split(separator, StringSplitOptions.None);
    }

    /// <summary>
    /// 自身が指定されたいずれかの文字列と等しいかどうか
    /// </summary>
    /// <returns></returns>
    public static bool IsAny(this string self, params string[] values)
    {
        return values.Any(c => c == self);
    }

    /// <summary>
    /// 自身がnullまたは空文字であるか空文字だけで構成されているかどうか
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static bool IsNullOrWhiteSpace(this string self)
    {
        return self == null || self.Trim() == "";
    }

    /// <summary>
    /// 文字列を再構成して最後に改行ラインを追加する
    /// フォーマットは{0}などで指定する
    /// </summary>
    /// <param name="self"></param>
    /// <param name="format"></param>
    /// <param name="arg"></param>
    /// <returns></returns>
    public static StringBuilder AppendLine(this StringBuilder self, string format, object arg)
    {
        return self.AppendFormat(format, arg).AppendLine();
    }

    /// <summary>
    /// 文字列を再構成して最後に改行ラインを追加する
    /// フォーマットは{0}{1}などで指定する
    /// </summary>
    /// <returns></returns>
    public static StringBuilder AppendLine(this StringBuilder self, string format, params object[] args)
    {
        return self.AppendFormat(format, args).AppendLine();
    }

    /// <summary>
    /// 文字列を再構成して最後に改行ラインを追加する
    /// フォーマットは{0}{1}で指定する
    /// </summary>
    /// <returns></returns>
    public static StringBuilder AppendLine(this StringBuilder self, string format, object arg1, object arg2)
    {
        return self.AppendFormat(format, arg1, arg2).AppendLine();
    }

    /// <summary>
    /// 文字列を再構成して最後に改行ラインを追加する
    /// フォーマットは{0}{1}{2}で指定する
    /// </summary>
    /// <returns></returns>
    public static StringBuilder AppendLine(this StringBuilder self, string format, object arg1, object arg2, object arg3)
    {
        return self.AppendFormat(format, arg1, arg2, arg3).AppendLine();
    }
}
