using Fody;
using Mono.Cecil;
using Mono.Cecil.Rocks;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MetaMerge.Fody
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
				ScanType(type);

			var contractsReference = ModuleDefinition.AssemblyReferences.FirstOrDefault(x => x.Name == "MetaMerge.Contacts");
			if (contractsReference is null)
				return;

			ModuleDefinition.AssemblyReferences.Remove(contractsReference);
		}

		private void ScanType(TypeDefinition type)
		{
			foreach (var property in type.Properties)
				ScanProperty(property);
		}

		private void ScanProperty(PropertyDefinition property)
		{
			var metaAttribute = property.CustomAttributes.FirstOrDefault(a => a.AttributeType.FullName == "MetaMerge.Contracts.MetaAttribute");
			if (metaAttribute is null)
				return;
			property.CustomAttributes.Remove(metaAttribute);

			CustomAttributeArgument metaType = metaAttribute.ConstructorArguments.First();
			TypeReference metaTypeReference = (TypeReference)metaType.Value;
			TypeDefinition metaTypeDefinition = metaTypeReference.Resolve();

			PropertyDefinition metaTypeTargetProperty = metaTypeDefinition.Properties.FirstOrDefault(x => x.Name == "Target");
			if (metaTypeTargetProperty is null)
			{
				WriteError($"{metaTypeDefinition.FullName} does not have a property \"public object Target {{ get; set; }}\".");
				return;
			}

			var targetAssembly = ModuleDefinition.Assembly;
			var sourceAttributes = metaTypeTargetProperty.CustomAttributes;
			foreach (var currentSourceAttribute in sourceAttributes)
			{
				string currentSourceAttributeAssemblyName = currentSourceAttribute.AttributeType.Module.Assembly.FullName;
				var targetAssemblyReference = ModuleDefinition.AssemblyReferences.FirstOrDefault(x => x.FullName == currentSourceAttributeAssemblyName);
				if (targetAssemblyReference is null)
					WriteError($"\"{ModuleDefinition.Assembly.FullName}\" does not reference \"{currentSourceAttributeAssemblyName}\"");
				if (!ModuleDefinition.TryGetTypeReference(currentSourceAttribute.AttributeType.FullName, out var typeReference))
					WriteError($"Could not find type \"{currentSourceAttribute.AttributeType.FullName}\" in target assembly \"{ModuleDefinition.Assembly.FullName}\"");
				//property.CustomAttributes.Add(currentAttribute.);
			}
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