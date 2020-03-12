using NUnit.Framework;
using StringParser.Logic;
using System;

namespace StringParser.Test
{
	public class StringToIntParserTests
	{
		private StringToIntParser stringToIntParser;
		private int result;

		[SetUp]
		public void Init()
		{
			stringToIntParser = new StringToIntParser();
		}

		[Test]
		[TestCase("0")]
		[TestCase("765")]
		[TestCase("-1")]
		[TestCase("+2568")]
		[TestCase("2147483647")]
		[TestCase("-2147483648")]
		public void ParseStringToInt_ValidStrings_SuccessfulParsing(string inputString)
		{
			result = stringToIntParser.ParseStringToInt(inputString);
			Assert.AreEqual(int.Parse(inputString), result);
		}

		[Test]
		[TestCase(null)]
		[TestCase("")]
		[TestCase("\n")]
		public void ParseStringToInt_NullOrWhitespaceOrEmpty_ThrowArgumentNullException(string inputString)
		{
			Assert.Throws<ArgumentNullException>(() => result = stringToIntParser.ParseStringToInt(inputString));
		}

		[Test]
		[TestCase("tst12356")]
		[TestCase("-{2334454]")]
		[TestCase("-2356lt")]
		[TestCase("7854.22")]
		public void ParseStringToInt_InvalidStrings_ThrowFormatException(string inputString)
		{
			Assert.Throws<FormatException>(() => result = stringToIntParser.ParseStringToInt(inputString));
		}

		[Test]
		[TestCase("2147483648")]
		[TestCase("-2147483649")]
		[TestCase("21474836493333")]
		public void ParseStringToInt_LongType_ThrowFormatException(string inputString)
		{
			Assert.Throws<FormatException>(() => result = stringToIntParser.ParseStringToInt(inputString));
		}
	}
}