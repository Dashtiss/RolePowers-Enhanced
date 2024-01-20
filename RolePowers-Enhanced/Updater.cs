using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
namespace RolePowers_Enhanced
{
    public class VersionChecker
    {
        private const string RepositoryOwner = "dashtiss";
        private const string RepositoryName = "YourRepository";
        private const string CurrentVersion = "1.0.0"; // Replace with your plugin's current version

        public static async Task CheckVersion()
        {
            using (var httpClient = new HttpClient())
            {
                var releasesUrl = $"https://api.github.com/repos/{RepositoryOwner}/{RepositoryName}/releases";
                httpClient.DefaultRequestHeaders.Add("User-Agent", "YourPlugin");

                try
                {
                    var response = await httpClient.GetAsync(releasesUrl);
                    response.EnsureSuccessStatusCode();
                    var releases = await response.Content.ReadAsAsync<List<Release>>();

                    if (releases.Count > 0)
                    {
                        var latestRelease = releases[0];
                        if (new Version(latestRelease.TagName) > new Version(CurrentVersion))
                        {
                            // Alert admins about the new version
                            foreach (var player in Player.List)
                            {
                                if (player.ReferenceHub.serverRoles.RemoteAdmin)
                                {
                                    player.ShowHint($"A new version of the plugin is available: {latestRelease.TagName}", 10f);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.Error($"Failed to check for new version: {ex.Message}");
                }
            }
        }

        private class Release
        {
            public string TagName { get; set; }
        }
    }
}
