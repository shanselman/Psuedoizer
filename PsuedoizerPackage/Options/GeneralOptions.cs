using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Shell;

namespace JamesJohnson.PsuedoizerPackage.Options
{
	class GeneralOptions : DialogPage
	{
		[LocDisplayName("Culture to generate")]
		[Description("Specifies what culture the resource strings will be generated for")]
		[Category("Misc")]
		public CultureInfo CultureToGenerate { get; set; }


	}
}
