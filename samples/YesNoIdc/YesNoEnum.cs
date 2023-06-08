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

[GenerateEnumerationRecordClass]
public enum YesNoEnum
{
    /// <summary>No, negative, nope, false, etc.</summary>
    [Display(Name = "No", ShortName = "No", Description = "Negative", Prompt = "Yes or no?")]
    No = 0,

    /// <summary>Yes, affirmative, yep, true, etc.</summary>
    [Display(Name = "Yes", ShortName = "Yes", Description = "Affirmative", Prompt = "Yes or no?")]
    Yes = 1,
}
