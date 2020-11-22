using Mono.Addins;
using Mono.Addins.Setup;
using Pinta.Core;
using Mono.Unix;

namespace Pinta
{
	public class AddinSetupService: SetupService
	{
		internal AddinSetupService (AddinRegistry r): base (r)
		{
		}
		
		public bool AreRepositoriesRegistered ()
		{
			string url = GetPlatformRepositoryUrl ();
			return Repositories.ContainsRepository (url);
		}
		
		public void RegisterRepositories (bool enable)
		{
			RegisterRepository (GetPlatformRepositoryUrl (),
			                    Catalog.GetString ("Pinta Community Addins - Platform-Specific"),
			                    enable);

			RegisterRepository (GetAllRepositoryUrl (),
			                    Catalog.GetString ("Pinta Community Addins - Cross-Platform"),
			                    enable);
		}

		private void RegisterRepository(string url, string name, bool enable)
		{
			if (!Repositories.ContainsRepository (url)) {
				var rep = Repositories.RegisterRepository (null, url, false);
				rep.Name = name;
				// Although repositories are enabled by default, we should always call this method to ensure
				// that the repository name from the previous line ends up being saved to disk.
				Repositories.SetRepositoryEnabled (url, enable);
			}
		}
		
		private static string GetPlatformRepositoryUrl ()
		{
			string platform;
			if (SystemManager.GetOperatingSystem () == OS.Windows)
				platform = "Windows";
			else
				if (SystemManager.GetOperatingSystem () == OS.Mac)
					platform = "Mac";
				else
					platform = "Linux";

            return "https://www.youtube.com/channel/UCCKUFbgkch_MYvBaPgwlq2w" + platform + "/main.mrep";
		}

		private static string GetAllRepositoryUrl ()
		{
            return "https://www.youtube.com/channel/UCCKUFbgkch_MYvBaPgwlq2w";
		}
	}
}
