﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.IO;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Microsoft.Build.Shared
{
    /// <summary>
    /// This class contains utility methods for file IO.
    /// Separate from FileUtilities because some assemblies may only need the patterns.
    /// PERF\COVERAGE NOTE: Try to keep classes in 'shared' as granular as possible. All the methods in 
    /// each class get pulled into the resulting assembly.
    /// </summary>
    internal static class FileUtilitiesRegex
    {
        // regular expression used to match file-specs comprising exactly "<drive letter>:" (with no trailing characters)
        internal static readonly Regex DrivePattern = new Regex(@"^[A-Za-z]:$", RegexOptions.Compiled);

        /// <summary>
        /// Checks for drive pattern without regex. Using this function over regex results in
        /// improved memory performance.
        /// </summary>
        /// <param name="drivePattern"></param>
        /// <returns>Whether or not the input follows the drive pattern "<drive letter>:" with no trailing characters.</drive></returns>
        internal static bool IsDrivePattern(string s)
        {
            // Format must be two characters long. Ex: "C:"
            return s.Length == 2 &&
                DoesStartWithDrivePattern(s); 
        }

        // regular expression used to match file-specs beginning with "<drive letter>:"
        internal static readonly Regex StartWithDrivePattern = new Regex(@"^[A-Za-z]:", RegexOptions.Compiled);

        internal static bool DoesStartWithDrivePattern(string input)
        {
            // Format dictates a length of at least 2
            // First character must be a letter
            // Second character must be a ":"
            return input.Length >= 2 &&
                (input[0] >= 'A' && input[0] <= 'Z') || (input[0] >= 'a' && input[0] <= 'z') &&
                input[1] == ':';
        }

        private static readonly string s_baseUncPattern = string.Format(
            CultureInfo.InvariantCulture,
            @"^[\{0}\{1}][\{0}\{1}][^\{0}\{1}]+[\{0}\{1}][^\{0}\{1}]+",
            Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

        // regular expression used to match UNC paths beginning with "\\<server>\<share>"
        internal static readonly Regex StartsWithUncPattern = new Regex(s_baseUncPattern, RegexOptions.Compiled);

        // regular expression used to match UNC paths comprising exactly "\\<server>\<share>"
        internal static readonly Regex UncPattern =
            new Regex(
                string.Format(CultureInfo.InvariantCulture, @"{0}$", s_baseUncPattern),
                RegexOptions.Compiled);
    }
}
