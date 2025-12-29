#nullable enable

using System;
using System.Collections.Generic;

using Shard = System.String;


namespace Avidity
{
    public static partial class Bases
    {
        /// <summary> An enum type with associated string representations for serde. </summary>
        /// <remarks>
        /// Derivations should implement enum variants as <c>static</c> instances, then provide a <c>SerialisedEnum? FromString(Shard shard)</c> method for deserialising a variant from JSON.
        /// </remarks>
        /// <typeparam name="ChildClass">The derived class (used for typing in the abstract base class).</typeparam>
        public abstract record SerialisedEnum<ChildClass>
            where ChildClass : SerialisedEnum<ChildClass>
        {
#pragma warning disable CS8618

            /// <summary> The unique serialised string representation of the enum variant. </summary>
            public Shard  shard { get; init; }

            /// <summary> The displayed text associated with the enum variant. </summary>
            public string text  { get; init; }

#pragma warning restore CS8618

            /// <summary> Parse a string representation into the enum variant it represents. </summary>
            /// <param name="variants">The possible expected string representations variants and the enum variants they represent.</param>
            /// <param name="shard">The string representation to parse.</param>
            /// <returns>An enum variant if one matched, otherwise `null`.</returns>
            public static ChildClass? FromVariants(Dictionary<Shard, ChildClass> variants, Shard? shard)
                => shard is null ? null
                    : variants.TryGetValue(shard, out var value)
                    ? value : null;
            
            public override string ToString()
                => this.text;
        }
    }
}
