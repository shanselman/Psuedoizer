using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using EnvDTE;

namespace PsuedoizerPackage
{
	class PsuedoizerConfiguration
	{
		private const string ConfigFilename = "psuedoizer.config";

		private readonly Project _project;

		public PsuedoizerConfiguration(Project project)
		{
			_project = project;
		}

		internal string SkipReason { get; private set; }

		public bool Enable { get; set; }
		public string Language { get; set; }
		public bool IgnoreBlankResources { get; set; }

		public void Parse()
		{
			var item = GetConfigurationItem();

			if (item == null)
			{
				SkipReason = "Configuration not found";

				// Load default values
				Enable = false;
				IgnoreBlankResources = true;
				Language = "xx";
				return;
			}

			var filename = item.FileNames[0];
			Parse(filename);
		}

		private void Parse(string filename)
		{
			var doc = XDocument.Load(filename);

			// Now that we have a config file for the project
			// only disable the extension for the project explicitly
			var root = doc.Root;

			if (root == null)
			{
				SkipReason = "Malformed configuration file";
				return;
			}

			Enable = GetBooleanValue(root.Element("enabled"), true);
			IgnoreBlankResources = GetBooleanValue(root.Element("ignoreBlankValues"), true);
			Language = GetStringValue(root.Element("language"), "xx");

			if (!Enable)
				SkipReason = "Disabled in configuration file";
		}

		private bool GetBooleanValue(XElement e, bool defaultValue)
		{
			if (e != null)
			{
				var value = defaultValue;
				if (bool.TryParse((string) e, out value))
					return value;
			}

			return defaultValue;
		}

		private string GetStringValue(XElement e, string defaultValue)
		{
			if (e == null)
				return defaultValue;
			return (string) e;
		}

		private ProjectItem GetConfigurationItem()
		{
			foreach (ProjectItem item in _project.ProjectItems)
			{
				if (StringComparer.CurrentCultureIgnoreCase.Equals(ConfigFilename, item.Name))
				{
					return item;
				}
			}

			return null;
		}
	}
}
