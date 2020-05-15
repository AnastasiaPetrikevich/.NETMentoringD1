using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForecastApp.Logger
{
	public interface ILogger
	{
		void Fatal(string message);

		void Info(string message);

		void Debug(string message);

		void Error(string message);
	}
}
