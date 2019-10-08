using System;
using FtpLibrary;

namespace FtpConsole.FtpCommands
{
    public sealed class ListDirectory : IFtpCommand
    {
        public void Execute(string ftpUri, string path = null, string username = null, string password = null)
        {
            Console.WriteLine("Listing directory...");
            try
            {
                using (var ftp = new FileTransferProtocol(ftpUri, path, username, password))
                {
                    if (ftp.UpdateDirectoryList())
                    {
                        Console.WriteLine(ftp.Directory);
                    }
                    else
                    {
                        Console.Write(ftp.RequestStatus);
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine($"{ftpUri} list directory error -> {exception.Message}");
            }  
        }
    }
}