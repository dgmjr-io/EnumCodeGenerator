/*
 * LoggerProvider.cs
 *
 *   Created: 2023-10-10-09:00:48
 *   Modified: 2023-10-12-08:28:44
 *
 *   Author: David G. Moore, Jr. <david@dgmjr.io>
 *
 *   Copyright © 2022 - 2023 David G. Moore, Jr., All Rights Reserved
 *      License: MIT (https://opensource.org/licenses/MIT)
 */

namespace Dgmjr.Enumerations.CodeGenerator.Logging;

using Dgmjr.Enumerations.CodeGenerator;
using Microsoft.Extensions.Logging;

public class LoggerProvider : ILoggerProvider
{
    private bool disposedValue;
    private readonly IncrementalGeneratorInitializationContext _context;

    public LoggerProvider(IncrementalGeneratorInitializationContext context)
    {
        _context = context;
    }

    public ILogger CreateLogger(string? categoryName)
    {
        return new EnumerationsGeneratorLogger(_context, categoryName);
    }

    public ILogger<T> CreateLogger<T>()
        where T : notnull => new EnumerationsGeneratorLogger<T>(_context);

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                // noop
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

public class EnumerationsGeneratorLogger<T> : EnumerationsGeneratorLogger, ILogger<T>
    where T : notnull
{
    public EnumerationsGeneratorLogger(IncrementalGeneratorInitializationContext context)
        : base(context, typeof(T).FullName) { }
}

public class EnumerationsGeneratorLogger : ILogger, IDisposable
{
    private int _indentation = 0;
    private string _strIndentation = "";
    private bool disposedValue;

    protected string Indentation => _strIndentation;
    private readonly string? _category;
    private readonly IncrementalGeneratorInitializationContext _context;
    private readonly IList<string> _logs = new List<string>();

    public EnumerationsGeneratorLogger(
        IncrementalGeneratorInitializationContext context,
        string? category = null
    )
    {
        _context = context;
        _category = category;
        this.LogBeginLog(_category, DateTimeOffset.UtcNow);
    }

    public IDisposable? BeginScope<TState>(TState state)
        where TState : notnull
    {
        _indentation++;
        ReIndent();
        return new EnumerationGeneratorLoggerScope<TState>(this, EndScope);
    }

    private void EndScope()
    {
        _indentation--;
        ReIndent();
    }

    private void ReIndent()
    {
        var indentationBuilder = new StringBuilder();
        for (var i = 0; i < _indentation; i++)
        {
            indentationBuilder.Append("  ");
        }
        _strIndentation = indentationBuilder.ToString();
    }

    public bool IsEnabled(LogLevel logLevel) => true;

    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter
    )
    {
        _logs.Add($"/* {Indentation}{formatter(state, exception)} */");
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                _context.RegisterPostInitializationOutput(
                    ctx => ctx.AddSource("log.g.cs", GetLogs())
                );
                // new FileInfo(
                //     Path.Combine(Path.GetDirectoryName(GetType().Assembly.Location), "log.g.txt")
                // )
                //     .CreateText()
                //     .Write(GetLogs());
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

    private string GetLogs() => Join("\r\n", _logs);
}
