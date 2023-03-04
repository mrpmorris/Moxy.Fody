using System.ComponentModel.DataAnnotations;

namespace ConsoleApp88.Contracts;

[AttributeUsage(AttributeTargets.Property)]
public class PersonTitleMeta : Attribute
{
	[Required, MinLength(2), MaxLength(8)]
	public object Target { get; set; }
}
