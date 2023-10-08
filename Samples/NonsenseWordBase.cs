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

namespace System;

public partial record class NonsenseWordBase
{
    private string _name =>
        GetType().GetRuntimeProperties().FirstOrDefault(prop => prop.Name == "Name")?.GetValue(this)
        as string;

    public override string ToString() => $"I am a nonsense word and I say, \"{this._name}\"";
}
