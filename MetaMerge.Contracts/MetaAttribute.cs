using System;

namespace MetaMerge.Contracts
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
	public sealed class MetaAttribute : Attribute
	{
		public Type MetaType { get; set; }

		public MetaAttribute(Type metaType)
		{
			if (metaType is null)
				throw new ArgumentNullException(nameof(metaType));

			MetaType = metaType;
		}
	}

}
