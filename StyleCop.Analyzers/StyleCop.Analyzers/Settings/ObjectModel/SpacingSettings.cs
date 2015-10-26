// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Settings.ObjectModel
{
    extern alias Json6;

    using J6 = Json6::Newtonsoft.Json;
    using J7 = global::Newtonsoft.Json;

    [J6.JsonObject(J6.MemberSerialization.OptIn)]
    [J7.JsonObject(J7.MemberSerialization.OptIn)]
    internal class SpacingSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SpacingSettings"/> class during JSON deserialization.
        /// </summary>
        [J6.JsonConstructor]
        [J7.JsonConstructor]
        protected internal SpacingSettings()
        {
        }
    }
}
