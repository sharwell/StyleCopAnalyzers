// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Settings.ObjectModel
{
    extern alias Json6;

    using J6 = Json6::Newtonsoft.Json;
    using J7 = global::Newtonsoft.Json;

    [J6.JsonObject(J6.MemberSerialization.OptIn)]
    [J7.JsonObject(J7.MemberSerialization.OptIn)]
    internal class SettingsFile
    {
        /// <summary>
        /// This is the backing field for the <see cref="Settings"/> property.
        /// </summary>
        [J6.JsonProperty("settings", DefaultValueHandling = J6.DefaultValueHandling.Ignore)]
        [J7.JsonProperty("settings", DefaultValueHandling = J7.DefaultValueHandling.Ignore)]
        private StyleCopSettings settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsFile"/> class
        /// during JSON deserialization.
        /// </summary>
        [J6.JsonConstructor]
        [J7.JsonConstructor]
        protected SettingsFile()
        {
            this.settings = new StyleCopSettings();
        }

        public StyleCopSettings Settings
        {
            get
            {
                return this.settings;
            }
        }
    }
}
