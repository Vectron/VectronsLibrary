using System;
using Microsoft.Extensions.Logging;

namespace VectronsLibrary.DI;

/// <content>
/// The logging methods.
/// </content>
public partial class AssemblyResolver
{
    [LoggerMessage(EventId = 0, Level = LogLevel.Error, Message = "Requested assembly is null or name is empty")]
    private partial void LogInvalidAssemblyName();

    [LoggerMessage(EventId = 5, Level = LogLevel.Debug, Message = "{AssemblyName}: Assembly path does not exist: '{Path}', continuing")]
    private partial void LogInvalidPath(string assemblyName, string? path);

    [LoggerMessage(EventId = 9, Level = LogLevel.Error, Message = "{AssemblyName}: Failed to load assembly")]
    private partial void LogLoadFailed(Exception exception, string assemblyName);

    [LoggerMessage(EventId = 10, Level = LogLevel.Debug, Message = "{AssemblyName}: Failed to load assembly, continuing")]
    private partial void LogLoadFailedContinue(Exception exception, string assemblyName);

    [LoggerMessage(EventId = 7, Level = LogLevel.Debug, Message = "{AssemblyName}: Loading assembly '{Path}'")]
    private partial void LogLoading(string assemblyName, string? path);

    [LoggerMessage(EventId = 11, Level = LogLevel.Error, Message = "{AssemblyName}: Not found in any location")]
    private partial void LogNotFound(string assemblyName);

    [LoggerMessage(EventId = 8, Level = LogLevel.Debug, Message = "{AssemblyName}: Resolved assembly: {AssemblyName}, from path: {AssemblyPath}")]
    private partial void LogResolved(string assemblyName, string? AssemblyPath);

    [LoggerMessage(EventId = 2, Level = LogLevel.Trace, Message = "{AssemblyName}: Resolved from cache")]
    private partial void LogResolvedCache(string assemblyName);

    [LoggerMessage(EventId = 1, Level = LogLevel.Debug, Message = "{AssemblyName} Resolving Assembly")]
    private partial void LogResolvingAssembly(string assemblyName);

    [LoggerMessage(EventId = 4, Level = LogLevel.Trace, Message = "{AssemblyName}: searching in {Directory}")]
    private partial void LogSearchDirectory(string assemblyName, string? directory);

    [LoggerMessage(EventId = 3, Level = LogLevel.Trace, Message = "{AssemblyName}: Skipped search")]
    private partial void LogSkipped(string assemblyName);

    [LoggerMessage(EventId = 6, Level = LogLevel.Debug, Message = "{AssemblyName}: File exists but version/public key is wrong, continuing")]
    private partial void LogWrongVersion(string assemblyName);
}
