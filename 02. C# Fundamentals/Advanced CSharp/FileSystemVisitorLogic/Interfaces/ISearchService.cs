using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSystemVisitorLogic.Models
{
	public interface ISearchService
	{
		IEnumerable<string> SearchFiles(string path);
		IEnumerable<string> SearchDirectories(string path);
		bool IsDirectoryExists(string path);
	}
}
