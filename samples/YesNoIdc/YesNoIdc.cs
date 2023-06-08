/*
 * YesNoIdc.cs
 *
 *   Created: 2022-12-17-12:47:12
 *   Modified: 2022-12-17-12:47:12
 *
 *   Author: David G. Mooore, Jr. <david@dgmjr.io>
 *
 *   Copyright © 2022 David G. Mooore, Jr., All Rights Reserved
 *      License: MIT (https://opensource.org/licenses/MIT)
 */
namespace System;

[Display(
    Name = "Yes/No/IDC",
    ShortName = "Yes/No/IDC",
    Description = "Yes/No/IDC",
    Prompt = "Yes/No/IDC?"
)]
public partial record class YesNoIdc : YesNo
{
    // public YesNoIdc() : base() { }
}
