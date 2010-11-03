using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WordWrapSnippet 
{
	[TestClass]
	public class WordWrapTests
	{
		public static List<string> WordWrap( string text, int maxLineLength )
		{
			var list = new List<string>();

			int currentIndex;
			var lastWrap = 0;
			var whitespace = new[] { ' ', '\r', '\n', '\t' };
			do
			{
				currentIndex = lastWrap + maxLineLength > text.Length ? text.Length : (text.LastIndexOfAny( new[] { ' ', ',', '.', '?', '!', ':', ';', '-', '\n', '\r', '\t' }, Math.Min( text.Length - 1, lastWrap + maxLineLength)  ) + 1);
				if( currentIndex <= lastWrap )
					currentIndex = Math.Min( lastWrap + maxLineLength, text.Length );
				list.Add( text.Substring( lastWrap, currentIndex - lastWrap ).Trim( whitespace ) );
				lastWrap = currentIndex;
			} while( currentIndex < text.Length );

			return list;
		}

		[TestMethod]
		public void WordWrap_SplitsTextIntoAllowableLengthsAndAvoidsBreakingWords()
		{
			var rows = WordWrap( "12345 abcd 1234", 7 );
			Assert.AreEqual(3, rows.Count);
			Assert.AreEqual("12345", rows[0] );
			Assert.AreEqual("abcd", rows[1] );
			Assert.AreEqual("1234", rows[2] );
		}

		[TestMethod]
		public void WordWrap_BreaksWordExceedingLineLimit()
		{
			var rows = WordWrap( "foobarhello", 7 );
			Assert.AreEqual(2, rows.Count );
			Assert.AreEqual("foobarh", rows[0] );
			Assert.AreEqual("ello", rows[1] );
		}

		[TestMethod]
		public void WordWrap_BreaksWordExceedingLimitOfSeveralLines()
		{
			var rows = WordWrap( "foobar_line_foobar", 6 );
			Assert.AreEqual(3, rows.Count );
			Assert.AreEqual( "foobar", rows[0] );
			Assert.AreEqual( "_line_", rows[1] );
			Assert.AreEqual( "foobar", rows[2] );
		}

		[TestMethod]
		public void WordWrap_BreaksWordsExceedingLineLimitAndBreaksBetweenWordThatFitsWithinLimit()
		{
			var rows = WordWrap( "lorem ipsum testing 12345 abc", 12 );
			Assert.AreEqual( 3, rows.Count );
			Assert.AreEqual( "lorem ipsum", rows[0] );
			Assert.AreEqual( "testing", rows[1]);
			Assert.AreEqual( "12345 abc", rows[2] );
		}
	}
}