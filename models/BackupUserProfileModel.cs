using System;
using System.IO;
using System.Threading.Tasks;
using Qml.Net;

namespace Admin_Assistant_CS
{
    public class BackupUserProfileModel
    {
        public async Task<string> runScript(string UserProfile)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(500));

            return "Done";
        }
    }
}