namespace Foo;

public partial record class Fuck : Baz
{
    public Fuck(Baz value) : base()
    {

    }
}

public partial record class Baz
{

}

/// <summary>This is an enumeration demonstration</summary>
/// <remarks>This should show up as an XML comment for the enumeration class too!</remarks>
[GenerateEnumerationRecordClass("Fuck", baseType: typeof(Baz))]
public enum FuckEnum
{
    [Display(Name = "Baz", Description = "This is the first value")]
    IInheritFromBaz
}
