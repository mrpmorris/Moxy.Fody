using ConsoleApp88.Contracts;
using MetaMerge.Contracts;

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
		[Meta(typeof(PersonTitleMeta))]
		public string Name { get; set; }

		public string FirstName { get; set; }

		public string Surname { get; set; }
	}

	public class CreatePersonCommand
	{
		public string Name { get; set; }
		public string FirstName { get; set; }
		public string Surname { get; set; }
	}

	public class UpdatePersonCommand
	{
		public string Name { get; set; }
		public string FirstName { get; set; }
		public string Surname { get; set; }
	}

}