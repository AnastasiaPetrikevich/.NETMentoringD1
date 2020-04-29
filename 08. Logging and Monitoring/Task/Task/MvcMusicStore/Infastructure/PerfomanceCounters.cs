using PerformanceCounterHelper;
using System.Diagnostics;

namespace MvcMusicStore.Infastructure
{
	[PerformanceCounterCategory("MvcMusicStorePerformanceCounters", PerformanceCounterCategoryType.SingleInstance, "Performance counters.")]
	public enum PerfomanceCounters
	{
		[PerformanceCounter("SuccessLogInCounter", "Counts a number of successful log in attempts.", PerformanceCounterType.NumberOfItems32)]
		SuccessLogInCounter = 1,

		[PerformanceCounter("SuccessLogOutCounter", "Counts a number of successful log out attempts.", PerformanceCounterType.NumberOfItems32)]
		SuccessLogOutCounter,

		[PerformanceCounter("ProcessingBrowseRequestInStoreController", "Counts a number of processing Store controller Browse request.", PerformanceCounterType.AverageTimer32)]
		ProcessingBrowseRequestInStoreController
	}
}