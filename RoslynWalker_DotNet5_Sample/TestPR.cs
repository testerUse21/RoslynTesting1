using Microsoft.Build.Locator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynWalker_DotNet5_Sample
{
	class TestPR
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
	}
}
