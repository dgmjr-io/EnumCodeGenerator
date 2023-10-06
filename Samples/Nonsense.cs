/*
 * Bool.cs
 *
 *   Created: 2023-10-05-12:34:29
 *   Modified: 2023-10-05-12:34:29
 *
 *   Author: David G. Moore, Jr. <david@dgmjr.io>
 *
 *   Copyright © 2022 - 2023 David G. Moore, Jr., All Rights Reserved
 *      License: MIT (https://opensource.org/licenses/MIT)
 */

namespace System.Enums;

[GenerateEnumerationRecordStruct("NonsenseWord", "System")]
public enum Nonsense
{
    [Display(
        Name = "FOO",
        Description = "The quintessential nonsense word for computer scientists.",
        ShortName = "foo"
    )]
    Foo = 0,

    [Display(
        Name = "BAR",
        Description = "The second quintessential nonsense word for computer scientists.",
        ShortName = "bar"
    )]
    Bar = 1
}
