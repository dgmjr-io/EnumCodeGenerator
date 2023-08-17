namespace Dgmjr.Enumerations.CodeGenerator.Samples.Enums;

[GenerateEnumerationStructAttribute("HttpMethod", "System.Net.Http")]
public enum HttpMethod
{
    Get,
    Post,
    Put,
    Head,
    Options,
    Trace
}
