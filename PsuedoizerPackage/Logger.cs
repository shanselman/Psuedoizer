using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;

namespace JamesJohnson.PsuedoizerPackage
{
	internal class Logger
	{
		private static Lazy<IVsOutputWindowPane> _pane = new Lazy<IVsOutputWindowPane>(GetPane);

		private static IVsOutputWindowPane GetPane()
		{
			return PsuedoizerPackage.Instance.GetOutputPane(VSConstants.OutputWindowPaneGuid.BuildOutputPane_guid, "Psuedoizer");
		}

		public static void Log(string msg)
		{
			if (_pane.Value == null)
				return;

			_pane.Value.OutputString(msg + Environment.NewLine);
		}
	}
}
