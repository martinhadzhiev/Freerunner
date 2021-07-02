public static class Base64Helper
{
    public static string Base64Encode(this string text)
    {
        var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(text);
        return System.Convert.ToBase64String(plainTextBytes);
    }

    public static string Base64Decode(this string text)
    {
        var base64EncodedBytes = System.Convert.FromBase64String(text);
        return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
    }
}
