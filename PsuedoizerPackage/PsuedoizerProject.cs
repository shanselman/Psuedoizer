using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnvDTE;

namespace JamesJohnson.PsuedoizerPackage
{
	class PsuedoizerProject
	{
		public PsuedoizerProject(Project project)
		{
			Project = project;
		}

		protected Project Project { get; private set; }
		protected PsuedoizerConfiguration Configuration { get; private set; }

		public void ProcessProject()
		{
			if (Project == null)
				return;
			if (string.IsNullOrEmpty(Project.FullName))
				return;

			if (!ShouldProcessProject())
				return;
		}

		private bool ShouldProcessProject()
		{
			Configuration = new PsuedoizerConfiguration(Project);
			Configuration.Parse();

			if (!Configuration.Enable)
				return false;

			return true;
		}
	}
}
