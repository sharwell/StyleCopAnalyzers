// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Settings.ObjectModel
{
    extern alias Json6;

    using System.Collections.Immutable;
    using J6 = Json6::Newtonsoft.Json;
    using J7 = global::Newtonsoft.Json;

    [J6.JsonObject(J6.MemberSerialization.OptIn)]
    [J7.JsonObject(J7.MemberSerialization.OptIn)]
    internal class NamingSettings
    {
        /// <summary>
        /// This is the backing field for the <see cref="AllowCommonHungarianPrefixes"/> property.
        /// </summary>
        [J6.JsonProperty("allowCommonHungarianPrefixes", DefaultValueHandling = J6.DefaultValueHandling.Include)]
        [J7.JsonProperty("allowCommonHungarianPrefixes", DefaultValueHandling = J7.DefaultValueHandling.Include)]
        private bool allowCommonHungarianPrefixes;

        /// <summary>
        /// This is the backing field for the <see cref="AllowedHungarianPrefixes"/> property.
        /// </summary>
        [J6.JsonProperty("allowedHungarianPrefixes", DefaultValueHandling = J6.DefaultValueHandling.Ignore)]
        [J7.JsonProperty("allowedHungarianPrefixes", DefaultValueHandling = J7.DefaultValueHandling.Ignore)]
        private ImmutableArray<string>.Builder allowedHungarianPrefixes;

        /// <summary>
        /// Initializes a new instance of the <see cref="NamingSettings"/> class during JSON deserialization.
        /// </summary>
        [J6.JsonConstructor]
        [J7.JsonConstructor]
        protected internal NamingSettings()
        {
            this.allowCommonHungarianPrefixes = true;
            this.allowedHungarianPrefixes = ImmutableArray<string>.Empty.ToBuilder();
        }

        public bool AllowCommonHungarianPrefixes =>
            this.allowCommonHungarianPrefixes;

        public ImmutableArray<string> AllowedHungarianPrefixes
        {
            get
            {
                return this.allowedHungarianPrefixes.ToImmutable();
            }
        }
    }
}
