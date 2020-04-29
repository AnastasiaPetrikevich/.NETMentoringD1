using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcMusicStore.Logger.Interfaces
{
	public interface ILogger
	{
		void Trace(string message);

		void Info(string message);

		void Debug(string message);

		void Error(string message);

		void Error(string message, Exception ex);
	}
}
