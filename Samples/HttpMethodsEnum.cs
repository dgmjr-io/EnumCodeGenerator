namespace System.Net.Http.Enums;

[GenerateEnumerationRecordStructAttribute("HttpMethods", "System.Net.Http"), Flags]
public enum HttpMethod : sbyte
{
    None = 0,
    Get = 1,
    Post = 2,
    Put = 4,
    Head = 8,
    Options = 16,
    Trace = 32,
    All = Get | Post | Put | Head | Options | Trace
}
