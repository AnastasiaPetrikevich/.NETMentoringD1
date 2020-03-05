using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace StringParser.Logic
{
	public class StringToIntParser
	{
		public int ParseStringToInt(string inputString)
		{
			if (string.IsNullOrWhiteSpace(inputString))
			{
				throw new ArgumentNullException($"This string {inputString} is null, whitespace or empty.");
			}

			if (inputString.Length > 11 && IsHaveSign(inputString) || inputString.Length > 10 && !IsHaveSign(inputString))
			{
				throw new FormatException($"This string {inputString} is too long for int number.");
			}

			if (!IsIntSymbols(inputString))
			{
				throw new FormatException($"Invalid string {inputString}.");
			}

			long result;
			if (IsHaveSign(inputString))
			{
				result = TransformString(inputString.Substring(1));

				if (inputString.First() == '-')
					result *= -1;
			}

			else
			{
				result = TransformString(inputString);
			}

			if (result < int.MinValue || result > int.MaxValue)
			{
				throw new FormatException($"This string {inputString} is long number.");
			}

			return (int)result;
		}

		private long TransformString(string input)
		{
			long result = 0;

			foreach (char c in input)
			{
				result = result * 10 + (c - '0');
			}

			return result;
		}

		private bool IsHaveSign(string input) => input.First() == '-' || input.First() == '+';

		private bool IsIntSymbols(string input)
		{
			var intSymbolsRegex = new Regex(@"^[+|-]?\d{1,10}$");

			return intSymbolsRegex.IsMatch(input);
		}

	}
}
