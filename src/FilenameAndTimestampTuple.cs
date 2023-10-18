namespace Dgmjr.Enumerations.CodeGenerator;

internal readonly record struct FilenameAndTimestampTuple(string Filename)
{
    public string Timestamp => DateTimeOffset.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.ffffzzzZ");
    public string ThisAssemblyName => Constants.ThisAssemblyName;
    public string ThisAssemblyVersion => Constants.ThisAssemblyVersion;
}
