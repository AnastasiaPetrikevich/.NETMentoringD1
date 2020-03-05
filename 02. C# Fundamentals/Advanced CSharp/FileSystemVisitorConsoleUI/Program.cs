using System;
using System.Collections.Generic;
using System.IO;
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
			//Console.WriteLine("Please, enter path:");
			//string searchPath = Console.ReadLine();
			string searchPath = @"D:\.NETMentoringD1";

			#region All files and directories

			Console.WriteLine("All files and directories");
			var visitor = new FileSystemVisitor(new VisitorService());
			visitor.OnStart += (s, e) => Console.WriteLine("Search started...");
			visitor.OnFinish += (s, e) => Console.WriteLine("Search fineshed.");
			visitor.OnDirectoryFinded += (s, e) => Console.WriteLine($"Directory: {e.FileSystemInfo.FullName}");
			visitor.OnFileFinded += (s, e) => Console.WriteLine($"File: {e.FileSystemInfo.FullName}");

			Console.WriteLine("Results 1:");

			foreach (FileSystemInfo entry in visitor.Search(searchPath))
			{
			}

			#endregion


			#region Exclude files without filter

			Console.WriteLine("Exclude files without filter");
			visitor = new FileSystemVisitor(new VisitorService());
			visitor.OnStart += (s, e) => Console.WriteLine("Search started...");
			visitor.OnFinish += (s, e) => Console.WriteLine("Search fineshed.");
			visitor.OnFileFinded += (s, e) =>
			{
				if (e.FileSystemInfo.Name.Contains("csp"))
				{
					e.Status = SearchStatus.Exclude;
				}
				else
				{
					Console.WriteLine($"File: {e.FileSystemInfo.FullName}");
				}
			};

			Console.WriteLine("Results 2:");

			foreach (FileSystemInfo entry in visitor.Search(searchPath))
			{
			}

			#endregion

			#region Filtered files

			Console.WriteLine("Filtered files");
			visitor = new FileSystemVisitor(new VisitorService(), (path) => path.Name.Contains(".csproj"));
			visitor.OnStart += (s, e) => Console.WriteLine("Search started...");
			visitor.OnFinish += (s, e) => Console.WriteLine("Search fineshed.");
			visitor.OnFilteredFileFinded += (s, e) => Console.WriteLine($"Fitered file: {e.FileSystemInfo.FullName}");

			Console.WriteLine("Results 3:");

			foreach (FileSystemInfo entry in visitor.Search(searchPath))
			{
			}

			#endregion

			Console.ReadKey();
		}
	}
}