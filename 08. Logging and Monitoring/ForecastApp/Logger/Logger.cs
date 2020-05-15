using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml;
using log4net;
using log4net.Config;
using log4net.Repository;

namespace ForecastApp.Logger
{
	public class Logger : ILogger
	{
		public ILog Log { get; private set; }

		public Logger()
		{
			InitLogger();
		}

		public void Fatal(string message)
		{
			Log.Fatal(message);
		}

		public void Info(string message)
		{
			Log.Info(message);
		}

		public void Debug(string message)
		{
			Log.Debug(message);
		}

		public void Error(string message)
		{
			Log.Error(message);
		}

		private void InitLogger()
		{
			Log = LogManager.GetLogger(typeof(Logger));
			var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
			XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
		}
	}
}
