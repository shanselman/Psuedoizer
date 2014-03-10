using System.Diagnostics;
using System.IO;
using EnvDTE;
using Pseudo.Globalization;

namespace PsuedoizerPackage
{
	class PsuedoizerFile
	{
		private static string GetFullPath(ProjectItem item)
		{
			return item.Properties.Item("FullPath").Value.ToString();
		}

		private readonly string _filePath;
		private readonly ProjectItem _parentItem;
		private readonly PsuedoizerConfiguration _config;
		
		public PsuedoizerFile(ProjectItem parentItem, PsuedoizerConfiguration config) : this(GetFullPath(parentItem), config)
		{
			_parentItem = parentItem;
		}

		public PsuedoizerFile(string file, PsuedoizerConfiguration config)
		{
			_parentItem = null;
			_filePath = file;
			_config = config;
		}

		public void Psuedoize()
		{
			var outputFilename = GenerateOutputFilename();

			Logger.Log(PsuedoizerLogLevel.Verbose, "\t{0} -> {1}", _filePath, Path.GetFileName(outputFilename));

			Psuedoizer.Psuedoize(_filePath, outputFilename, !_config.IgnoreBlankResources);

			if (_parentItem != null)
			{
				AddFileToProject(outputFilename);
			}
		}

		private string GenerateOutputFilename()
		{
			var filename = Path.GetFileNameWithoutExtension(_filePath);
			var path = Path.GetDirectoryName(_filePath);

			filename = string.Format("{0}.{1}.resx", filename, _config.Language);

			return Path.Combine(path, filename);
		}

		private void AddFileToProject(string outputFile)
		{
			Debug.Assert(_parentItem != null);

			var newItem = PsuedoizerPackage.DTE.Solution.FindProjectItem(outputFile);

			if (newItem != null)
			{
				Logger.Log(PsuedoizerLogLevel.Verbose, "\t\tNot adding project item for file because it already exists");
				return;
			}

			if (_parentItem.ProjectItems != null)
				newItem = _parentItem.ProjectItems.AddFromFile(outputFile);
			else
				newItem = _parentItem.ContainingProject.ProjectItems.AddFromFile(outputFile);
		}
	}
}
