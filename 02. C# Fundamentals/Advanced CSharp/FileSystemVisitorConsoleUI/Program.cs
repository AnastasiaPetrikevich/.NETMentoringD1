using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileSystemVisitorLogic.Models;
using FileSystemVisitorLogic.Services;

namespace FileSystemVisitorConsoleUI
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Please, enter path:");
			var searchPath = Console.ReadLine();

			var visitor = new FileSystemVisitor(new SearchService(), (path) => path.Contains(".png"));
			visitor.OnDirectoryFinded += (s, e) => Console.WriteLine($"Directory: {e.Path}");

			visitor.OnFileFinded += (s, e) => Console.WriteLine($"File: {e.Path}");
			visitor.OnFilteredFileFinded += (s, e) => e.Status = SearchStatus.Exclude;

			Console.WriteLine("Results:");
			visitor.Search($@"{searchPath}");

			Console.ReadKey();
		}
	}
}
