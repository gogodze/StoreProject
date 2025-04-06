﻿namespace Domain.Common;

public static class StringExt
{
    public static string ToSnakeCaseRename(this string str)
    {
        var newStr = str[0].ToString().ToLower();
        foreach (var c in str.Skip(1))
            if (char.IsUpper(c))
                newStr += $"_{char.ToLower(c)}";
            else
                newStr += c;

        return newStr;
    }

    public static Ulid StringToUlid(this string str)
    {
        Ulid ulid = Ulid.Parse(str);
        return ulid;
    }

    public static string GetFromEnvRequired(this string str)
    {
        return Environment.GetEnvironmentVariable(str) ??
               throw new InvalidOperationException($"variable not found {str}");
    }
}