using Fody;
using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace Moxy.Fody
{
	public class ModuleWeaver : BaseModuleWeaver
	{
		public override IEnumerable<string> GetAssembliesForScanning()
		{
			yield return "netstandard";
			yield return "mscorlib";
		}

		public override void Execute()
		{
			foreach (var type in ModuleDefinition.Types)
			{
				ScanType(type);
			}
		}

		private void ScanType(TypeDefinition type)
		{
			foreach (var property in type.Properties)
			{
				ScanProperty(type, property);
			}
		}

		private void ScanProperty(TypeDefinition type, PropertyDefinition property)
		{
			var minLengthAttribute = property.CustomAttributes.FirstOrDefault(a => a.AttributeType.FullName == typeof(MinLengthAttribute).FullName);

			if (minLengthAttribute != null)
			{
				WriteMessage($"{type.FullName}.{property.Name}", MessageImportance.High);
				//// Add [Required] attribute if it doesn't already exist on the property
				//var requiredAttribute = property.CustomAttributes.FirstOrDefault(a => a.AttributeType.FullName == typeof(RequiredAttribute).FullName);

				//if (requiredAttribute == null)
				//{
				//	var ctor = ModuleDefinition.ImportReference(typeof(RequiredAttribute).GetConstructor(Type.EmptyTypes));
				//	var attribute = new CustomAttribute(ctor);
				//	property.CustomAttributes.Add(attribute);
				//}
			}
		}
	}
}