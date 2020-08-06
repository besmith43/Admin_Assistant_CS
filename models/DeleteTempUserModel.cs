using System;
using System.IO;
using System.Threading.Tasks;
using Qml.Net;

namespace Admin_Assistant_CS
{
    public class DeleteTempUserModel
    {
        public async Task<string> RunScript(string tempUserAccount)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(500));

            return "Done";
        }
    }
}