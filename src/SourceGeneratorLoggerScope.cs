/*
 * SourceGeneratorLoggerScope.cs
 *
 *   Created: 2023-10-10-09:00:48
 *   Modified: 2023-10-12-08:28:51
 *
 *   Author: David G. Moore, Jr. <david@dgmjr.io>
 *
 *   Copyright © 2022 - 2023 David G. Moore, Jr., All Rights Reserved
 *      License: MIT (https://opensource.org/licenses/MIT)
 */

namespace Dgmjr.Enumerations.CodeGenerator.Logging;

using Dgmjr.Enumerations.CodeGenerator;

using Microsoft.Extensions.Logging;

public class EnumerationGeneratorLoggerScope<TScope> : IDisposable
    where TScope : notnull
{
    private readonly Action _endScope;

    public EnumerationGeneratorLoggerScope(ILogger logger, Action endScope)
    {
        _endScope = endScope;
        logger.LogNewScope(typeof(TScope).Name);
    }

    private bool disposedValue;

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                _endScope();
            }

            disposedValue = true;
        }
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
