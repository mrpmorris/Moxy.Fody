using System.ComponentModel.DataAnnotations;

namespace ConsoleApp88
{
	internal class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Hello, World!");
		}
	}

	public class Person
	{
		[MinLength(32)]
		public string Name { get; set; }
	}
}