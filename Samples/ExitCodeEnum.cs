/*
 * ExitCodeEnum.cs
 *
 *   Created: 2023-01-29-01:25:45
 *   Modified: 2023-01-29-01:25:46
 *
 *   Author: David G. Mooore, Jr. <david@dgmjr.io>
 *
 *   Copyright © 2022-2023 David G. Mooore, Jr., All Rights Reserved
 *      License: MIT (https://opensource.org/licenses/MIT)
 */

namespace System.Enums;

using System.ComponentModel.DataAnnotations;

[GenerateEnumerationRecordStruct("ExitCode", "System")]
public enum ExitCode : byte
{
    [Display(Name = "Success", Description = "The command was successful.", ShortName = "SUCCESS")]
    Success = 0,

    [Display(Name = "Failure", Description = "The command failed.", ShortName = "FAILURE")]
    Failure = 1,

    [Display(
        Name = "Misuse of shell built-in",
        Description = "Exit code 2 signifies invalid usage of some shell built-in command. Examples of built-in commands include alias, echo, and printf.",
        ShortName = "MISUSE_OF_SHELL"
    )]
    MisuseOfShellBuiltIn = 2,

    #region buit-in Mac OS X exit codes
    [Display(
        Name = "Incorrect usage",
        Description = "The command was used incorrectly, e.g., with the wrong number of arguments, a bad flag, a bad syntax in a parameter, or whatever.",
        ShortName = "EX_USAGE"
    )]
    ExUsage = 64,

    [Display(
        Name = "Data format error",
        Description = "The input data was incorrect in some way. This should only be used for user's data and not system files.",
        ShortName = "EX_DATAERR"
    )]
    ExDataErr = 65,

    [Display(
        Name = "Cannot open input",
        Description = "An input file (not a system file) did not exist or was not readable. This could also include errors like \\\"No message\\\" to a mailer (if it cared to catch it).",
        ShortName = "EX_NOINPUT"
    )]
    ExNoInput = 66,

    [Display(
        Name = "Addressee unknown",
        Description = "The user specified did not exist. This might be used for mail addresses or remote logins.",
        ShortName = "EX_NOUSER"
    )]
    ExNoUser = 67,

    [Display(
        Name = "Host name unknown",
        Description = "The host specified did not exist. This is used in mail addresses or network requests.",
        ShortName = "EX_NOHOST"
    )]
    ExNoHost = 68,

    [Display(
        Name = "Service unavailable",
        Description = "A service is unavailable. This can occur if a support program or file does not exist. This can also be used as a catchall message when something you wanted to do doesn't work, but you don't know why.",
        ShortName = "EX_UNAVAILABLE"
    )]
    ExUnavail = 69,

    [Display(
        Name = "Internal software error",
        Description = "An internal software error has been detected. This should be limited to non-operating system related errors as possible.",
        ShortName = "EX_SOFTWARE"
    )]
    ExSoftware = 70,

    [Display(
        Name = "System error (e.g., can't fork)",
        Description = "An operating system error has been detected. This is intended to be used for such things as \\\"cannot fork\\\", \\\"cannot create pipe\\\", or the like. It includes things like getuid returning a user that does not exist in the passwd file.",
        ShortName = "EX_OSERR"
    )]
    ExOsErr = 71,

    [Display(
        Name = "Critical OS file missing",
        Description = "Some system file (e.g., /etc/passwd, /var/run/utx.active, etc.) does not exist, cannot be opened, or has some sort of error (e.g., syntax error).",
        ShortName = "EX_OSFILE"
    )]
    ExOsFile = 72,

    [Display(
        Name = "Cannot create (user) output file",
        Description = "A (user specified) output file cannot be created.",
        ShortName = "EX_CANTCREAT"
    )]
    ExCantCreate = 73,

    [Display(
        Name = "Input/output error",
        Description = "An error occurred while doing I/O on some file.",
        ShortName = "EX_IOERR"
    )]
    ExIoErr = 74,

    [Display(
        Name = "Temporary failure, user is invited to retry",
        Description = "Temporary failure, indicating something that is not really an error. In sendmail, this means that a mailer (e.g.) could not create a connection, and the request should be reattempted later.",
        ShortName = "EX_TEMPFAIL"
    )]
    ExTempFail = 75,

    [Display(
        Name = "Remote error in protocol",
        Description = "The remote system returned something that was \\\"not possible\\\" during a protocol exchange",
        ShortName = "EX_PROTOCOL"
    )]
    ExProtocol = 76,

    [Display(
        Name = "Permission denied",
        Description = "You did not have sufficient permission to perform the operation. This is not intended for file system problems, which should use ExNoInput or ExCantCreate, but rather for higher level permissions.",
        ShortName = "EX_NOPERM"
    )]
    ExNoPerm = 77,

    [Display(
        Name = "Configuration error",
        Description = "Something was found in an unconfigured or misconfigured state.",
        ShortName = "EX_CONFIG"
    )]
    ExConfig = 78,

    [Display(
        Name = "Invalid argument",
        Description = "An invalid argument was supplied to a function or program.",
        ShortName = "EX_ARG"
    )]
    #endregion

    #region Addtional exit codes

    InvalidArguments = 2,

    [Display(
        Name = "Invalid project file",
        Description = "The project file is invalid.",
        ShortName = "EX_INVALID_PROJECT_FILE"
    )]
    InvalidProjectFile = 3,

    [Display(
        Name = "Invalid target",
        Description = "The target is invalid.",
        ShortName = "EX_INVALID_TARGET"
    )]
    InvalidTarget = 4,

    [Display(
        Name = "Invalid configuration",
        Description = "The configuration is invalid.",
        ShortName = "EX_INVALID_CONFIGURATION"
    )]
    InvalidConfiguration = 5,

    [Display(
        Name = "Invalid verbosity",
        Description = "The verbosity is invalid.",
        ShortName = "EX_INVALID_VERBOSITY"
    )]
    InvalidVerbosity = 6,

    [Display(
        Name = "Invalid version",
        Description = "The version is invalid.",
        ShortName = "EX_INVALID_VERSION"
    )]
    InvalidVersion = 7,

    [Display(
        Name = "Invalid assembly version",
        Description = "The assembly version is invalid.",
        ShortName = "EX_INVALID_ASSEMBLY_VERSION"
    )]
    InvalidAssemblyVersion = 8,

    [Display(
        Name = "Invalid property",
        Description = "The property is invalid.",
        ShortName = "EX_INVALID_PROPERTY"
    )]
    InvalidProperty = 9,

    [Display(
        Name = "Invalid binary logger",
        Description = "The binary logger is invalid.",
        ShortName = "EX_INVALID_BINARY_LOGGER"
    )]
    InvalidBinaryLogger = 10,

    [Display(
        Name = "Invalid binary log file",
        Description = "The binary log file is invalid.",
        ShortName = "EX_INVALID_BINARY_LOG_FILE"
    )]
    InvalidBinaryLogFile = 11,

    [Display(
        Name = "Invalid help",
        Description = "The help is invalid.",
        ShortName = "EX_INVALID_HELP"
    )]
    InvalidHelp = 12,

    [Display(
        Name = "Invalid command",
        Description = "The command is invalid.",
        ShortName = "EX_INVALID_COMMAND"
    )]
    InvalidCommand = 13,

    [Display(
        Name = "Invalid project path",
        Description = "The project path is invalid.",
        ShortName = "EX_INVALID_PROJECT_PATH"
    )]
    InvalidProjectPath = 14,

    [Display(
        Name = "Invalid target path",
        Description = "The target path is invalid.",
        ShortName = "EX_INVALID_TARGET_PATH"
    )]
    InvalidTargetPath = 15,

    [Display(
        Name = "Invalid target framework",
        Description = "The target framework is invalid.",
        ShortName = "EX_INVALID_TARGET_FRAMEWORK"
    )]
    InvalidTargetFramework = 16,

    [Display(
        Name = "Invalid output path",
        Description = "The output path is invalid.",
        ShortName = "EX_INVALID_OUTPUT_PATH"
    )]
    InvalidOutputPath = 17,

    [Display(
        Name = "Invalid output type",
        Description = "The output type is invalid.",
        ShortName = "EX_INVALID_OUTPUT_TYPE"
    )]
    InvalidOutputType = 18,

    [Display(
        Name = "Invalid no logo",
        Description = "The no logo is invalid.",
        ShortName = "EX_INVALID_NO_LOGO"
    )]
    InvalidNoLogo = 19,

    [Display(
        Name = "Invalid no restore",
        Description = "The no restore is invalid.",
        ShortName = "EX_INVALID_NO_RESTORE"
    )]
    InvalidNoRestore = 20,

    [Display(
        Name = "Invalid no build",
        Description = "The no build is invalid.",
        ShortName = "EX_INVALID_NO_BUILD"
    )]
    InvalidNoBuild = 21,

    [Display(
        Name = "Invalid no dependencies",
        Description = "The no dependencies is invalid.",
        ShortName = "EX_INVALID_NO_DEPENDENCIES"
    )]
    InvalidNoDependencies = 22,

    [Display(
        Name = "Invalid no incremental",
        Description = "The no incremental is invalid.",
        ShortName = "EX_INVALID_NO_INCREMENTAL"
    )]
    InvalidNoIncremental = 23,

    [Display(
        Name = "Invalid no interactive",
        Description = "The no interactive is invalid.",
        ShortName = "EX_INVALID_NO_INTERACTIVE"
    )]
    InvalidNoInteractive = 24,

    [Display(
        Name = "Invalid interactive",
        Description = "The interactive is invalid.",
        ShortName = "EX_INVALID_INTERACTIVE"
    )]
    InvalidInteractive = 25,

    [Display(
        Name = "Invalid no warn",
        Description = "The no warn is invalid.",
        ShortName = "EX_INVALID_NO_WARN"
    )]
    InvalidNoWarn = 26,

    [Display(
        Name = "Invalid warn as error",
        Description = "The warn as error is invalid.",
        ShortName = "EX_INVALID_WARN_AS_ERROR"
    )]
    InvalidWarnAsError = 27,

    [Display(
        Name = "Invalid force",
        Description = "The force is invalid.",
        ShortName = "EX_INVALID_FORCE"
    )]
    InvalidForce = 28,

    [Display(
        Name = "Invalid no cache",
        Description = "The no cache is invalid.",
        ShortName = "EX_INVALID_NO_CACHE"
    )]
    InvalidNoCache = 29,

    [Display(
        Name = "Invalid no version",
        Description = "The no version is invalid.",
        ShortName = "EX_INVALID_NO_VERSION"
    )]
    InvalidNoVersion = 30,

    [Display(
        Name = "Invalid no package analysis",
        Description = "The no package analysis is invalid.",
        ShortName = "EX_INVALID_NO_PACKAGE_ANALYSIS"
    )]
    InvalidNoPackageAnalysis = 31,

    [Display(
        Name = "Invalid no dependencies analysis",
        Description = "The no dependencies analysis is invalid.",
        ShortName = "EX_INVALID_NO_DEPENDENCIES_ANALYSIS"
    )]
    CommandFoundNotExecutable = 126,

    [Display(
        Name = "Command not found",
        Description = "The command was not found.",
        ShortName = "EX_COMMAND_NOT_FOUND"
    )]
    CommandNotFound = 127,

    [Display(
        Name = "Command encountered error",
        Description = "The command encountered an error.",
        ShortName = "EX_COMMAND_ENCOUNTERED_ERROR"
    )]
    CommandEncounteredError = 128,

    [Display(
        Name = "Command terminated by signal 2",
        Description = "The command was terminated by signal 2.",
        ShortName = "EX_COMMAND_TERMINATED_BY_SIGNAL_2"
    )]
    CommandTerminatedBySignal2 = 130,

    [Display(
        Name = "Command terminated by signal 15",
        Description = "The command was terminated by signal 15.",
        ShortName = "EX_COMMAND_TERMINATED_BY_SIGNAL_15"
    )]
    CommandTerminatedBySignal15 = 143,
    #endregion

    [Display(
        Name = "Status out of range",
        Description = "The status is out of range.",
        ShortName = "EX_STATUS_OUT_OF_RANGE"
    )]
    ExStatusOutOfRange = 255
}
