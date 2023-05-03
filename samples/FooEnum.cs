namespace Foo;

/// <summary>This is an enumeration demonstration</summary>
/// <remarks>This should show up as an XML comment for the enumeration class too!</remarks>
[GenerateEnumerationRecordStruct("Bar", "BarNamespace")]
public enum BarEnum
{
    [Display(Name = "Baz", Description = "This is the first value")]
    Baz = 1,
    Qux = 2,
    Quux = 3,
    Corge = 4,
    Grault = 5,
    Garply = 6,
    Waldo = 7,
    Fred = 8,
    Plugh = 9,
    Xyzzy = 10,
    Thud = 11,
}
