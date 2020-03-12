using System;
using System.Text.RegularExpressions;

namespace StringParser.Logic
{
	public class Parser
	{
		public void ParseToInt(string inputString, out int outputNumber)
		{
			long temp = 0;

			if (string.IsNullOrWhiteSpace(inputString))
			{
				throw new ArgumentNullException($"Value of {inputString} is null, whitespace or empty.");
			}

			if ((HaveSign(inputString) && inputString.Length > 11) ||
				(!HaveSign(inputString) && inputString.Length > 10))
			{
				throw new FormatException($"Value of {inputString} is too long for int number.");
			}

			if (IsInt(inputString))
			{
				if (HaveSign(inputString))
				{
					ProcessString(inputString.Substring(1), ref temp);

					if (inputString[0] == '-')
						temp *= -1;
				}

				else
				{
					ProcessString(inputString, ref temp);
				}

				if (temp < int.MinValue || temp > int.MaxValue)
				{
					throw new FormatException($"Value of {inputString} is long number.");
				}

				outputNumber = (int)temp;
			}

			else
			{
				throw new FormatException($"Value of {inputString} has not valid format.");
			}
		}

		private bool HaveSign(string inputString)
		{
			return inputString[0] == '-' || inputString[0] == '+';
		}

		private bool IsInt(string stringToValidate)
		{
			var result = new Regex(@"^[+|-]?\d{1,10}$");

			return result.IsMatch(stringToValidate);
		}

		private void ProcessString(string stringToParse, ref long temp)
		{
			foreach (char c in stringToParse)
			{
				temp = temp * 10 + (c - '0');
			}
		}
	}
}
