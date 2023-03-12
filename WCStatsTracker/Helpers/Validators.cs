using System;
using System.ComponentModel.DataAnnotations;
namespace WCStatsTracker.Helpers;

public static class Validators
{
    /// <summary>
    ///     Validator for a string to be correctly formated to convert to a datetime
    /// </summary>
    /// <param name="runLength">String of new runLength</param>
    /// <param name="context">the validation context</param>
    /// <returns>ValidationResult.Success if string is valid, otherwise a ValidationResult set with an error message</returns>
    public static ValidationResult ValidateRunLength(string runLength, ValidationContext context)
    {
        var isValid = TimeSpan.TryParseExact(runLength, @"h\:mm\:ss", null, out _);

        if (isValid)
        {
            return ValidationResult.Success!;
        }

        return new ValidationResult($"{runLength} is not a valid time, use H:MM:SS format");
    }
}
