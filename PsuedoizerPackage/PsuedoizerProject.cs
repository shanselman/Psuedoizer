using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;
using EnvDTE;
using Microsoft.VisualStudio;

namespace PsuedoizerPackage
{
	class PsuedoizerProject
	{
		public PsuedoizerProject(Project project)
		{
			Project = project;
		}

		private Project Project { get; set; }
		private PsuedoizerConfiguration ProjectConfiguration { get; set; }

		public void ProcessProject()
		{
			if (Project == null)
				return;
			if (string.IsNullOrEmpty(Project.FullName))
				return;

			Logger.Log(PsuedoizerLogLevel.Verbose, "Checking project to see whether it should be psuedo-ized: {0}", Project.Name);

			if (!ShouldProcessProject())
			{
				Logger.Log(PsuedoizerLogLevel.Verbose, "Skipping project, reason: {0}", ProjectConfiguration.SkipReason);
				return;
			}

			Logger.Log(PsuedoizerLogLevel.Info, "Psuedoizing project \"{0}\"", Project.Name);

			//ProcessItems(Project.ProjectItems);
			ProcessItems();
		}

		private void ProcessItems()
		{
			var projectPath = Project.Properties.Item("FullPath").Value.ToString();
			projectPath = Path.GetDirectoryName(projectPath);

			var solution = PsuedoizerPackage.DTE.Solution;

			foreach (var file in Directory.GetFiles(projectPath, "*.resx", SearchOption.AllDirectories))
			{
				var item = solution.FindProjectItem(file);

				if (item != null)
				{
					ProcessItem(item);
				}
			}
		}

		private void ProcessItems(ProjectItems items)
		{
			foreach (ProjectItem item in items)
			{
				ProcessItem(item);
			}
		}

		private void ProcessItem(ProjectItem item)
		{
			if (item.ProjectItems != null && item.ProjectItems.Count > 0)
			{
				ProcessItems(item.ProjectItems);
			}

			if (IsPsuedoizableResourceFile(item))
			{
				var file = new PsuedoizerFile(item, ProjectConfiguration);
				file.Psuedoize();
			}
		}

		private bool IsPsuedoizableResourceFile(ProjectItem item)
		{
			var filename = GetPropertyValue(item, "FullPath");

			Logger.Log(PsuedoizerLogLevel.Debug, "{0}", item.Name);

			if (Path.GetExtension(filename) == ".resx")
			{
				Logger.Log(PsuedoizerLogLevel.Verbose, "Found {0}", filename);

				if (!IsEmbeddedResource(item))
				{
					Logger.Log(PsuedoizerLogLevel.Verbose, "\tbut isn't an embedded resource.");
					return false;
				}

				if (!IsBaseResXFile(item))
				{
					Logger.Log(PsuedoizerLogLevel.Verbose, "\tbut isn't the root resx file");
					return false;
				}

				return true;
			}

			return false;
		}

		private bool IsEmbeddedResource(ProjectItem item)
		{
			const string EmbeddedResource = "EmbeddedResource";

			return StringComparer.InvariantCultureIgnoreCase.Equals(GetPropertyValue(item, "ItemType"), EmbeddedResource);
		}

		private bool IsBaseResXFile(ProjectItem item)
		{
			// This should probably do some sort of filename check to see that it doesn't contain a culture
			// however, for now we could assume that base resources use the ResXFileCodeGenerator
			return UsesResXCodeGenerator(item);
		}

		private bool UsesResXCodeGenerator(ProjectItem item)
		{
			const string Generator = "ResXFileCodeGenerator";

			return StringComparer.InvariantCultureIgnoreCase.Equals(GetPropertyValue(item, "CustomTool"), Generator);
		}

		private string GetPropertyValue(ProjectItem item, string property)
		{
			var prop = item.Properties.Item(property);

			if (prop == null)
				return null;

			return prop.Value.ToString();
		}

		private bool ShouldProcessProject()
		{
			ProjectConfiguration = new PsuedoizerConfiguration(Project);
			ProjectConfiguration.Parse();

			if (!ProjectConfiguration.Enable)
				return false;

			return true;
		}
	}
}
