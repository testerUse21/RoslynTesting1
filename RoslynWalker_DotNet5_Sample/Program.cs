using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;
using System;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RoslynWalker_DotNet5_Sample
{
	class Program
	{
		private static VisualStudioInstance SelectVisualStudioInstance(VisualStudioInstance[] visualStudioInstances)
		{
			Console.WriteLine("Multiple installs of MSBuild detected please select one:");
			for (int i = 0; i < visualStudioInstances.Length; i++)
			{
				Console.WriteLine($"Instance {i + 1}");
				Console.WriteLine($"    Name: {visualStudioInstances[i].Name}");
				Console.WriteLine($"    Version: {visualStudioInstances[i].Version}");
				Console.WriteLine($"    MSBuild Path: {visualStudioInstances[i].MSBuildPath}");
			}

			while (true)
			{
				var userResponse = Console.ReadLine();
				if (int.TryParse(userResponse, out int instanceNumber) &&
					instanceNumber > 0 &&
					instanceNumber <= visualStudioInstances.Length)
				{
					return visualStudioInstances[instanceNumber - 1];
				}
				Console.WriteLine("Input not accepted, try again.");
			}
		}

		private class ConsoleProgressReporter : IProgress<ProjectLoadProgress>
		{
			public void Report(ProjectLoadProgress loadProgress)
			{
				var projectDisplay = Path.GetFileName(loadProgress.FilePath);
				if (loadProgress.TargetFramework != null)
				{
					projectDisplay += $" ({loadProgress.TargetFramework})";
				}

				Console.WriteLine($"{loadProgress.Operation,-15} {loadProgress.ElapsedTime,-15:m\\:ss\\.fffffff} {projectDisplay}");
			}
		}

		static async Task Main(string[] args)
		{

			//var visualStudioInstances = MSBuildLocator.QueryVisualStudioInstances().ToArray();
			
			//var instance = visualStudioInstances.Length == 1
			//// If there is only one instance of MSBuild on this machine, set that as the one to use.
			//? visualStudioInstances[0]
			//// Handle selecting the version of MSBuild you want to use.
			//: SelectVisualStudioInstance(visualStudioInstances);

			//Console.WriteLine($"Using MSBuild at '{instance.MSBuildPath}' to load projects.");
			//MSBuildLocator.RegisterInstance(instance);


			try
			{
				// Attempt to set the version of MSBuild.
				var msBuildLocator = MSBuildLocator.QueryVisualStudioInstances();

				if (msBuildLocator != null)
				{
					var msBuildInstance = msBuildLocator.FirstOrDefault();

					if (msBuildInstance != null)
					{
						var logMessage = "Using MSBuild at " + msBuildInstance.MSBuildPath + " to load projects.";
						Console.WriteLine(logMessage);

						LooseVersionAssemblyLoader.Register(msBuildInstance.MSBuildPath);
						MSBuildLocator.RegisterInstance(msBuildInstance);
					}
					else
					{
						Console.WriteLine("msBuildInstance is null");
						MSBuildLocator.RegisterDefaults();
					}
				}
				else
				{
					Console.WriteLine("msBuildInstance is null 2");
					MSBuildLocator.RegisterDefaults();
				}

				string[] projectsToParse =
				Directory.GetFiles(args[0], "*.csproj", SearchOption.AllDirectories);

				Console.WriteLine("Projects to Parse: " + projectsToParse);
				foreach (var currentFile in projectsToParse)
				{
					Console.WriteLine("Project: " + currentFile);
					BrowseProjectsToParse(currentFile);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.StackTrace);
			}
		}

		private static void BrowseProjectsToParse(string projectPath)
		{
			try
			{
				using (var workspace = MSBuildWorkspace.Create())
				{
					Project currentProject = workspace.OpenProjectAsync(projectPath).Result;
					workspace.LoadMetadataForReferencedProjects = true;

					if (currentProject != null && currentProject.Documents.Count() <= 0)
					{
						ImmutableList<WorkspaceDiagnostic> diagnostics = workspace.Diagnostics;
						foreach (var diagnostic in diagnostics)
						{
							Console.WriteLine("Project Issue Diagnostic Issues: " + currentProject.Name + "::::" + diagnostic.Message);
						}
					}

					if (currentProject.Documents.Any())
						Console.WriteLine("Project " + currentProject.Name + " has .cs documents");
					else
						Console.WriteLine("Project " + currentProject.Name + " does not have any .cs documents");
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("BrowseProjectsToParse: " + ex.StackTrace);
			}
		}
	}
}
