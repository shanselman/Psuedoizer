using System;
using System.Resources;
using System.IO;
using System.Collections;
using System.Text;

namespace Pseudo.Globalization
{
	///Takes an English resource file (resx) and creates an artificial");/
	///but still readable Euro-like language to exercise your i18n code");
	///without a formal translation.");
	class Psuedoizer
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		public static bool Psuedoize(string filename, string outputFilename, bool includeBlankResources)
		{
			// Open the input file.
			var reader = new ResXResourceReader(filename);

			try
			{
				// Get the enumerator.  If this throws an ArguementException
				// it means the file is not a .RESX file.
				IDictionaryEnumerator enumerator = reader.GetEnumerator();
			}
			catch (ArgumentException)
			{
				return false;
			}

			// Allocate the list for this instance.
			var textResourcesList = new SortedList();

			// Run through the file looking for only true text related
			// properties and only those with values set.
			foreach (DictionaryEntry dic in reader)
			{
				// Only consider this entry if the value is something.
				if (null != dic.Value)
				{
					// Is this a System.String.
					if ("System.String" == dic.Value.GetType().ToString())
					{
						String KeyString = dic.Key.ToString();

						// Make sure the key name does not start with the
						// "$" or ">>" meta characters and is not an empty
						// string (or we're explicitly including empty strings).
						if ((false == KeyString.StartsWith(">>")) &&
							(false == KeyString.StartsWith("$")) &&
							(includeBlankResources || "" != dic.Value.ToString()))
						{
							// We've got a winner.
							textResourcesList.Add(dic.Key, dic.Value);
						}

						// Special case the Windows Form "$this.Text" or
						// I don't get the form titles.
						if (0 == String.Compare(KeyString, "$this.Text"))
						{
							textResourcesList.Add(dic.Key, dic.Value);
						}

					}
				}
			}

			// It's entirely possible that there are no text strings in the
			// .ResX file.
			if (textResourcesList.Count > 0)
			{
				if (null != outputFilename)
				{
					// Create the new file.
					ResXResourceWriter writer =
						new ResXResourceWriter(outputFilename);

					foreach (DictionaryEntry textdic in textResourcesList)
					{
						writer.AddResource(textdic.Key.ToString(), Psuedoizer.ConvertToFakeInternationalized(textdic.Value.ToString()));
					}

					writer.Generate();
					writer.Close();
				}
			}

			return true;
		}

		/// <summary>
		/// Converts a string to a pseudo internationized string.
		/// </summary>
		/// <remarks>
		/// Primarily for latin based languages.  This will need updating to
		/// work with Eastern languages.
		/// </remarks>
		/// <param name="inputString">
		/// The string to use as a base.
		/// </param>
		/// <returns>
		/// A longer and twiddled string.
		/// </returns>
		public static string ConvertToFakeInternationalized(string inputString)
		{
			// Calculate the extra space necessary for pseudo
			// internationalization.  The rules, according to "Developing
			// International Software" is that < 10  characters you should grow
			// by 400% while >= 10 characters should grow by 30%.

			var origLen = inputString.Length;
			var pseudoLen = 0;

			if (origLen < 10)
			{
				pseudoLen = (origLen * 4) + origLen;
			}
			else
			{
				pseudoLen = ((int)(origLen * 0.3)) + origLen;
			}

			var sb = new StringBuilder(pseudoLen);

			// The pseudo string will always start with a "[" and end
			// with a "]" so you can tell if strings are not built
			// correctly in the UI.
			sb.Append("[");

			bool waitingForEndBrace = false;
			bool waitingForGreaterThan = false;
			foreach (var currChar in inputString)
			{
				switch (currChar)
				{
					case '{':
						waitingForEndBrace = true;
						break;
					case '}':
						waitingForEndBrace = false;
						break;
					case '<':
						waitingForGreaterThan = true;
						break;
					case '>':
						waitingForGreaterThan = false;
						break;
				}
				if (waitingForEndBrace || waitingForGreaterThan)
				{
					sb.Append(currChar);
					continue;
				}
				switch (currChar)
				{
					case 'A':
						sb.Append('Å');
						break;
					case 'B':
						sb.Append('ß');
						break;
					case 'C':
						sb.Append('C');
						break;
					case 'D':
						sb.Append('Đ');
						break;
					case 'E':
						sb.Append('Ē');
						break;
					case 'F':
						sb.Append('F');
						break;
					case 'G':
						sb.Append('Ğ');
						break;
					case 'H':
						sb.Append('Ħ');
						break;
					case 'I':
						sb.Append('Ĩ');
						break;
					case 'J':
						sb.Append('Ĵ');
						break;
					case 'K':
						sb.Append('Ķ');
						break;
					case 'L':
						sb.Append('Ŀ');
						break;
					case 'M':
						sb.Append('M');
						break;
					case 'N':
						sb.Append('Ń');
						break;
					case 'O':
						sb.Append('Ø');
						break;
					case 'P':
						sb.Append('P');
						break;
					case 'Q':
						sb.Append('Q');
						break;
					case 'R':
						sb.Append('Ŗ');
						break;
					case 'S':
						sb.Append('Ŝ');
						break;
					case 'T':
						sb.Append('Ŧ');
						break;
					case 'U':
						sb.Append('Ů');
						break;
					case 'V':
						sb.Append('V');
						break;
					case 'W':
						sb.Append('Ŵ');
						break;
					case 'X':
						sb.Append('X');
						break;
					case 'Y':
						sb.Append('Ÿ');
						break;
					case 'Z':
						sb.Append('Ż');
						break;


					case 'a':
						sb.Append('ä');
						break;
					case 'b':
						sb.Append('þ');
						break;
					case 'c':
						sb.Append('č');
						break;
					case 'd':
						sb.Append('đ');
						break;
					case 'e':
						sb.Append('ę');
						break;
					case 'f':
						sb.Append('ƒ');
						break;
					case 'g':
						sb.Append('ģ');
						break;
					case 'h':
						sb.Append('ĥ');
						break;
					case 'i':
						sb.Append('į');
						break;
					case 'j':
						sb.Append('ĵ');
						break;
					case 'k':
						sb.Append('ĸ');
						break;
					case 'l':
						sb.Append('ľ');
						break;
					case 'm':
						sb.Append('m');
						break;
					case 'n':
						sb.Append('ŉ');
						break;
					case 'o':
						sb.Append('ő');
						break;
					case 'p':
						sb.Append('p');
						break;
					case 'q':
						sb.Append('q');
						break;
					case 'r':
						sb.Append('ř');
						break;
					case 's':
						sb.Append('ş');
						break;
					case 't':
						sb.Append('ŧ');
						break;
					case 'u':
						sb.Append('ū');
						break;
					case 'v':
						sb.Append('v');
						break;
					case 'w':
						sb.Append('ŵ');
						break;
					case 'x':
						sb.Append('χ');
						break;
					case 'y':
						sb.Append('y');
						break;
					case 'z':
						sb.Append('ž');
						break;
					default:
						sb.Append(currChar);
						break;
				}
			}

			// Poke on extra text to fill out the string.
			const String PadStr = " !!!";
			int padCount = (pseudoLen - origLen - 2) / PadStr.Length;
			if (padCount < 2)
			{
				padCount = 2;
			}

			for (int x = 0; x < padCount; x++)
			{
				sb.Append(PadStr);
			}

			// Pop on the trailing "]"
			sb.Append("]");

			return (sb.ToString());
		}
	}

}