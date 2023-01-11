using Microsoft.Build.Locator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RoslynWalker_DotNet5_Sample
{

	class TestV2PRCSharp
	{
		private static VisualStudioInstance SelectVisualStudioInstance(VisualStudioInstance[] visualStudioInstances)
		{
			string str = "c70abc3a6304123ecb2713647fedb83d874f5b4624d9c54bf0be06145757146edde840716a4d25185e09c14afe879df0afc70392844a8557f93de8e0c77accf4ddd9619692a50d7e9493696e3b8b6f4a4e3b17482c317a61dd5b41ecf262b4b83ef005544ef3391efff448e87be89ca011c4e937374eefe5acfd13553e2b50db6c3b35ab96b3a4522f6187a563eac161"
			var hashProvider = new SHA1CryptoServiceProvider();
			var hash = hashProvider.ComputeHash(Encoding.ASCII.GetBytes(str));

			Console.WriteLine("Multiple installs of MSBuild detected please select one:");
			for (int i = 0; i < visualStudioInstances.Length; i++)
			{
				Console.WriteLine($"Instance {i + 1}");
				Console.WriteLine($"    Name: {visualStudioInstances[i].Name}");
				Console.WriteLine($"    Version: {visualStudioInstances[i].Version}");
				Console.WriteLine($"    MSBuild Path: {visualStudioInstances[i].MSBuildPath}");
			}

			using (var tripleDES = new TripleDESCryptoServiceProvider()) //Noncompliant
			{
				//...
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
