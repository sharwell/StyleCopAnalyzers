// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Settings.ObjectModel
{
    extern alias Json6;

    using J6 = Json6::Newtonsoft.Json;
    using J7 = global::Newtonsoft.Json;

    [J6.JsonObject(J6.MemberSerialization.OptIn)]
    [J7.JsonObject(J7.MemberSerialization.OptIn)]
    internal class StyleCopSettings
    {
        /// <summary>
        /// This is the backing field for the <see cref="SpacingRules"/> property.
        /// </summary>
        [J6.JsonProperty("spacingRules", DefaultValueHandling = J6.DefaultValueHandling.Ignore)]
        [J7.JsonProperty("spacingRules", DefaultValueHandling = J7.DefaultValueHandling.Ignore)]
        private SpacingSettings spacingRules;

        /// <summary>
        /// This is the backing field for the <see cref="ReadabilityRules"/> property.
        /// </summary>
        [J6.JsonProperty("readabilityRules", DefaultValueHandling = J6.DefaultValueHandling.Ignore)]
        [J7.JsonProperty("readabilityRules", DefaultValueHandling = J7.DefaultValueHandling.Ignore)]
        private ReadabilitySettings readabilityRules;

        /// <summary>
        /// This is the backing field for the <see cref="OrderingRules"/> property.
        /// </summary>
        [J6.JsonProperty("orderingRules", DefaultValueHandling = J6.DefaultValueHandling.Ignore)]
        [J7.JsonProperty("orderingRules", DefaultValueHandling = J7.DefaultValueHandling.Ignore)]
        private OrderingSettings orderingRules;

        /// <summary>
        /// This is the backing field for the <see cref="NamingRules"/> property.
        /// </summary>
        [J6.JsonProperty("namingRules", DefaultValueHandling = J6.DefaultValueHandling.Ignore)]
        [J7.JsonProperty("namingRules", DefaultValueHandling = J7.DefaultValueHandling.Ignore)]
        private NamingSettings namingRules;

        /// <summary>
        /// This is the backing field for the <see cref="MaintainabilityRules"/> property.
        /// </summary>
        [J6.JsonProperty("maintainabilityRules", DefaultValueHandling = J6.DefaultValueHandling.Ignore)]
        [J7.JsonProperty("maintainabilityRules", DefaultValueHandling = J7.DefaultValueHandling.Ignore)]
        private MaintainabilitySettings maintainabilityRules;

        /// <summary>
        /// This is the backing field for the <see cref="DocumentationRules"/> property.
        /// </summary>
        [J6.JsonProperty("documentationRules", DefaultValueHandling = J6.DefaultValueHandling.Ignore)]
        [J7.JsonProperty("documentationRules", DefaultValueHandling = J7.DefaultValueHandling.Ignore)]
        private DocumentationSettings documentationRules;

        /// <summary>
        /// Initializes a new instance of the <see cref="StyleCopSettings"/> class during JSON deserialization.
        /// </summary>
        [J6.JsonConstructor]
        [J7.JsonConstructor]
        protected internal StyleCopSettings()
        {
            this.spacingRules = new SpacingSettings();
            this.readabilityRules = new ReadabilitySettings();
            this.orderingRules = new OrderingSettings();
            this.namingRules = new NamingSettings();
            this.maintainabilityRules = new MaintainabilitySettings();
            this.documentationRules = new DocumentationSettings();
        }

        public SpacingSettings SpacingRules =>
            this.spacingRules;

        public ReadabilitySettings ReadabilityRules =>
            this.readabilityRules;

        public OrderingSettings OrderingRules =>
            this.orderingRules;

        public NamingSettings NamingRules =>
            this.namingRules;

        public MaintainabilitySettings MaintainabilityRules =>
            this.maintainabilityRules;

        public DocumentationSettings DocumentationRules =>
            this.documentationRules;
    }
}
