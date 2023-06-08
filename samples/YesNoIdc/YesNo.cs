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

[Display(Name = "Yes/No", ShortName = "Yes/No", Description = "Yes/No", Prompt = "Yes or no?")]
public partial record class YesNo
{
    // public YesNo() : this((YesNoEnum)No.Id) { }
    // public static implicit operator YesNo(YesNoEnum value) => FromValue(value);
}
