using NUnit.Framework;
using StringParser.Logic;
using System;

namespace StringParser.Test
{
	public class ParserTests
	{
		private Parser parser;
		private int outputNumber;

		[SetUp]
		public void Init()
		{
			parser = new Parser();
		}

		[Test]
		[TestCase("0")]
		[TestCase("134")]
		[TestCase("-153")]
		[TestCase("+24")]
		[TestCase("2147483647")]
		[TestCase("-2147483648")]
		public void ParseToInt_ValidInput_SuccessfulParsing(string inputString)
		{
			parser.ParseToInt(inputString, out outputNumber);
			Assert.AreEqual(int.Parse(inputString), outputNumber);
		}

		[Test]
		[TestCase(null)]
		[TestCase("")]
		[TestCase("\n")]
		public void ParseToInt_NullOrWhitespaceOrEmpty_ThrowArgumentNullException(string inputString)
		{
			Assert.Throws<ArgumentNullException>(() => parser.ParseToInt(inputString, out outputNumber));
		}

		[Test]
		[TestCase("12356test")]
		[TestCase("-{26532}")]
		[TestCase("1234.34")]
		public void ParseToInt_NotValidInput_ThrowFormatException(string inputString)
		{
			Assert.Throws<FormatException>(() => parser.ParseToInt(inputString, out outputNumber));
		}

		[Test]
		[TestCase("2147483648")]
		[TestCase("-2147483649")]
		[TestCase("21474836493333")]
		public void ParseToInt_LongType_ThrowFormatException(string inputString)
		{
			Assert.Throws<FormatException>(() => parser.ParseToInt(inputString, out outputNumber));
		}
	}
}