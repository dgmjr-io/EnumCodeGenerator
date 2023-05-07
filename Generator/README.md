---
authors:
  - dgmjr
project: Dgmjr.Types
title: Enumeration Code Generator
description: A package that can create strongly-typed (and therefore extensible) classes or stucts from enums
---

# Enumeration Code Generaor
A package that can create strongly-typed (and therefore extensible) classes or stucts from enums

## Usage

Simply decorate any enum with one of the following attributes:
- `GenerateClass[Attribute]` - Generates a POCO class
- `GenerateStruct[Attribute]` - Generates a POCO struct
- `GenerateRecordClass[Attribute]` - Generates a `record class`
- `GenerateRecordStruct[Attribute]` - Generates a `record struct`

Each class or struct will have the following characteristics:

- Inner classes named after each of the `enum`'s members
- Each inner class will have `const` values referring to but not limited to
    - The `Name` value from the `Display` attribute
    - The `ShortName` value from the `Display` attribute
    - The rest of the values from the `Display` attribute
    - A string `uri` if the field was decorated with the `Uri` attribute

## Example

```csharp
namespace NonsenseWordsEnums;
[GenerateEnumerationRecordStruct("NonsenseWords", "NonsenseWords")]
public enum NonsenseWords
{
    [Display(Name = "Foo, the most comon nonsense word in computer science", Description = "A nonsense word with several suspected etymologies.  The most common nonsense word in computer science")]
    [Uri("https://www.ietf.org/rfc/rfc3092.txt")]
    [Display(Name = "supercalafragilisticexpialidocious", Description = "A word one says when there's nothing else to say.  Origin: Mary Poppins")]
}
```

Results in the following:

```csharp
namespace NonsenseWords;
public record struct NonsenseWords
{
    public static class Foo
    {
        private Foo() { }
        public static readoly Instance = new Foo();
        public const Name = "Foo, the most comon nonsense word in computer science";
        public const string Description = "A nonsense word with several suspected etymologies.  The most common nonsense word in computer science";
        public const string Uri = "https://www.ietf.org/rfc/rfc3092.txt";
        /// other properties would go here but are omitted for brevity
    }
}
````
