using DIContainer.Attributes;

namespace DIContainer.Models
{
	[Export]
	public class Logger
	{
		public string LoggerMessage = "Logger";
	}

	[Export]
	public class Logger2
	{
		public string LoggerMessage = "Logger2";
	}
}
