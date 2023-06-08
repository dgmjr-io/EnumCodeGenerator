---
authors:
  - DGMJR-IO
project: Dgmjr.Types
title: Enumeration Code Generator
description: A package that creates strongly-typed (and therefore extensible) interfaces and either classes or stucts from `enum`s
license: MIT
---

# Enumeration Code Generaor
A package that creates strongly-typed (and therefore extensible) interfaces and either classes or stucts from `enum`s

## Usage

Simply decorate any enum with one of the following attributes:
- `GenerateClass[Attribute]` - Generates a POCO `class`
- `GenerateStruct[Attribute]` - Generates a POCO `struct`
- `GenerateRecordClass[Attribute]` - Generates a `record class`
- `GenerateRecordStruct[Attribute]` - Generates a `record struct`

Each class or struct will have the following characteristics:

- Inner classes named after each of the `enum`'s members, which will implement the interface
- Each inner class will have `const` values referring to but not limited to
    - The `Name` value from the `DisplayAttribute`
    - The `ShortName` value from the `DisplayAttribute`
    - The rest of the properties from the `DisplayAttribute`
    - A string `uri` if the field was decorated with the `UriAttribute`

## Example

```csharp
namespace NonsenseWordsEnums;
[GenerateEnumerationRecordStruct("NonsenseWords", "NonsenseWords")]
public enum NonsenseWords
{
    [Display(Name = "Foo, the most comon nonsense word in computer science", Description = "A nonsense word with several suspected etymologies.  The most common nonsense word in computer science")]
    Foo,

    [Uri("√")]
    [Display(Name = "supercalafragilisticexpialidocious", Description = "A word one says when there's nothing else to say.  Origin: Mary Poppins")]
    supercalafragilisticexpialidocious
}
```

Results in the following:

```csharp
namespace NonsenseWords;
public record struct NonsenseWords
{
    <summary>A nonsense word with several suspected etymologies.  The most common nonsense word in computer science
    </summary>
    <remarks>Foo, the most comon nonsense word in computer science</remarks>
    <see href="https://ietf.org/rfc/rfc3092.txt" />
    public static class Foo
    {
        private Foo() { }
        public static readoly Instance = new Foo();
        public const Name = "Foo, the most comon nonsense word in computer science";
        public const string Description = "A nonsense word with several suspected etymologies.  The most common nonsense word in computer science";
        public const string Uri = "https://ietf.org/rfc/rfc3092.txt";
        /// other properties would go here but are omitted for brevity
    }
    // ...

    <summary>A nonsense word with several suspected etymologies.  The most common nonsense word in computer science</summary>
    <remarks>Foo, the most comon nonsense word in computer science</remarks>
    <see href="https://ietf.org/rfc/rfc3092.txt" />
    public static class supercalafragilisticexpialidocious
    {
        private supercalafragilisticexpialidocious() { }
        public static readoly Instance = new supercalafragilisticexpialidocious();
        public const Name = "Foo, the most comon nonsense word in computer science";
        public const string Description = "A nonsense word with several suspected etymologies.  The most common nonsense word in computer science";

        /// other properties would go here but are omitted for brevity
    }
}
````
