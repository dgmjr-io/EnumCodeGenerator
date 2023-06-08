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
public enum YesNoIdcEnum
{
    /// <summary>I  don't care</summary>
    [Display(
        Name = "I don't care",
        ShortName = "IDC",
        Description = "I don't care",
        Prompt = "Yes/No/IDC?"
    )]
    Idc = -1,

    /// <summary>I don't give a fuck</summary>
    [Display(
        Name = "I don't give a fuck",
        ShortName = "IDGAF",
        Description = "I don't give a fuck",
        Prompt = "Yes/No/IDC?"
    )]
    Idgaf = Idc,
}
