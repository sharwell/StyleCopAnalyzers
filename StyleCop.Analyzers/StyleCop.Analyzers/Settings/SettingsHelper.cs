// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers
{
    extern alias Json6;

    using System.Collections.Immutable;
    using System.IO;
    using System.Threading;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Diagnostics;
    using StyleCop.Analyzers.Settings.ObjectModel;
    using J6 = Json6::Newtonsoft.Json;
    using J7 = global::Newtonsoft.Json;

    /// <summary>
    /// Class that manages the settings files for StyleCopAnalyzers.
    /// </summary>
    internal static class SettingsHelper
    {
        private const string SettingsFileName = "stylecop.json";

        private static int useVersion6;

        /// <summary>
        /// Gets the StyleCop settings.
        /// </summary>
        /// <param name="context">The context that will be used to determine the StyleCop settings.</param>
        /// <returns>A <see cref="StyleCopSettings"/> instance that represents the StyleCop settings for the given context.</returns>
        internal static StyleCopSettings GetStyleCopSettings(this SyntaxTreeAnalysisContext context)
        {
            return context.Options.GetStyleCopSettings();
        }

        /// <summary>
        /// Gets the StyleCop settings.
        /// </summary>
        /// <param name="options">The analyzer options that will be used to determine the StyleCop settings.</param>
        /// <returns>A <see cref="StyleCopSettings"/> instance that represents the StyleCop settings for the given context.</returns>
        internal static StyleCopSettings GetStyleCopSettings(this AnalyzerOptions options)
        {
            bool version6 = (Interlocked.Increment(ref useVersion6) % 2) != 0;
            if (version6)
            {
                return GetStyleCopSettings6(options != null ? options.AdditionalFiles : ImmutableArray.Create<AdditionalText>());
            }
            else
            {
                return GetStyleCopSettings7(options != null ? options.AdditionalFiles : ImmutableArray.Create<AdditionalText>());
            }
        }

        private static StyleCopSettings GetStyleCopSettings6(ImmutableArray<AdditionalText> additionalFiles)
        {
            try
            {
                foreach (var additionalFile in additionalFiles)
                {
                    if (Path.GetFileName(additionalFile.Path).ToLowerInvariant() == SettingsFileName)
                    {
                        var root = J6.JsonConvert.DeserializeObject<SettingsFile>(additionalFile.GetText().ToString());
                        return root.Settings;
                    }
                }
            }
            catch (J6.JsonException)
            {
                // The settings file is invalid -> return the default settings.
            }

            return new StyleCopSettings();
        }

        private static StyleCopSettings GetStyleCopSettings7(ImmutableArray<AdditionalText> additionalFiles)
        {
            try
            {
                foreach (var additionalFile in additionalFiles)
                {
                    if (Path.GetFileName(additionalFile.Path).ToLowerInvariant() == SettingsFileName)
                    {
                        var root = J7.JsonConvert.DeserializeObject<SettingsFile>(additionalFile.GetText().ToString());
                        return root.Settings;
                    }
                }
            }
            catch (J7.JsonException)
            {
                // The settings file is invalid -> return the default settings.
            }

            return new StyleCopSettings();
        }
    }
}
