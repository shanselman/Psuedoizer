using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;

namespace PsuedoizerPackage
{
	internal class Logger
	{
		private static readonly Lazy<IVsOutputWindowPane> _pane = new Lazy<IVsOutputWindowPane>(GetPane);

		private static IVsOutputWindowPane GetPane()
		{
			return PsuedoizerPackage.Instance.GetOutputPane(VSConstants.OutputWindowPaneGuid.BuildOutputPane_guid, "Psuedoizer");
		}

		public static PsuedoizerLogLevel Level { get; set; }

		public static void Log(PsuedoizerLogLevel level, string msg)
		{
			if (_pane.Value == null)
				return;

			if ((int) Level <= (int) level)
				_pane.Value.OutputString(string.Format("Psuedoizer {1} - {0}: {2}{3}", level, DateTime.Now, msg, Environment.NewLine));
		}

		public static void Log(PsuedoizerLogLevel level, string format, params object[] args)
		{
			Log(level, String.Format(format, args));
		}
	}

	public enum PsuedoizerLogLevel
	{
		Verbose,
		Debug,
		Info,
	}
}
