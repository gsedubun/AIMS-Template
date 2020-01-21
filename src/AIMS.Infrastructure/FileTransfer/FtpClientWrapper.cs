using FluentFTP;
using System;
using System.Text;

namespace AIMS.Infrastructure.FileTransfer
{
    public sealed class FtpClientWrapper
    {
        private FtpClient client;
        public FtpClientWrapper(Uri uri, string username, string password)
        {
            client = new FtpClient(uri.Host, new System.Net.NetworkCredential(username, password));


        }

        public  bool UploadFile(string sourcefilepath, string destinationfilepath, bool overwrite=false)
        {
            bool result = false;
            if(overwrite)
                result=client.UploadFile(sourcefilepath, destinationfilepath, FtpRemoteExists.Overwrite);
            else
                result=client.UploadFile(sourcefilepath, destinationfilepath, FtpRemoteExists.Append);
            return result;
        }
    }
}
