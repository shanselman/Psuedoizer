using System;
using System.Runtime.InteropServices;
using System.Windows.Threading;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Shell;

namespace PsuedoizerPackage
{
	/// <summary>
	/// This is the class that implements the package exposed by this assembly.
	///
	/// The minimum requirement for a class to be considered a valid package for Visual Studio
	/// is to implement the IVsPackage interface and register itself with the shell.
	/// This package uses the helper classes defined inside the Managed Package Framework (MPF)
	/// to do it: it derives from the Package class that provides the implementation of the 
	/// IVsPackage interface and uses the registration attributes defined in the framework to 
	/// register itself and its components with the shell.
	/// </summary>
	// This attribute tells the PkgDef creation utility (CreatePkgDef.exe) that this class is
	// a package.
	[PackageRegistration(UseManagedResourcesOnly = true)]
	// This attribute is used to register the information needed to show this package
	// in the Help/About dialog of Visual Studio.
	[InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
	[Guid(GuidList.guidPsuedoizerPackagePkgString)]
	[ProvideAutoLoad(UIContextGuids80.SolutionExists)]
	public sealed class PsuedoizerPackage : Package
	{
		internal static PsuedoizerPackage Instance { get; private set; }

		internal static DTE2 DTE { get { return _dte.Value; } }
		private static Lazy<DTE2> _dte = new Lazy<DTE2>(LoadDTE);

		private static DTE2 LoadDTE()
		{
			return ServiceProvider.GlobalProvider.GetService(typeof (DTE)) as DTE2;
		}

		/// <summary>
		/// Default constructor of the package.
		/// Inside this method you can place any initialization code that does not require 
		/// any Visual Studio service because at this point the package object is created but 
		/// not sited yet inside Visual Studio environment. The place to do all the other 
		/// initialization is the Initialize method.
		/// </summary>
		public PsuedoizerPackage()
		{
		}

		/////////////////////////////////////////////////////////////////////////////
		// Overridden Package Implementation
		#region Package Members

		/// <summary>
		/// Initialization of the package; this method is called right after the package is sited, so this is the place
		/// where you can put all the initialization code that rely on services provided by VisualStudio.
		/// </summary>
		protected override void Initialize()
		{
			Instance = this;
#if DEBUG
			Logger.Level = PsuedoizerLogLevel.Verbose;
#else
			Logger.Level = PsuedoizerLogLevel.Info;
#endif
			Logger.Log(PsuedoizerLogLevel.Verbose, "Initializing PsuedoizerPackage");

			base.Initialize();

			Dispatcher.CurrentDispatcher.BeginInvoke(new Action(() =>
			{
				Logger.Log(PsuedoizerLogLevel.Verbose, "Hooking into build event");

				_dte.Value.Events.BuildEvents.OnBuildBegin += BuildEventsOnOnBuildBegin;
			}), DispatcherPriority.ApplicationIdle, null);
		}

		private void BuildEventsOnOnBuildBegin(vsBuildScope scope, vsBuildAction action)
		{
			foreach (Project project in DTE.Solution.Projects)
			{
				var pProj = new PsuedoizerProject(project);
				pProj.ProcessProject();
			}
		}

		#endregion

	}
}
