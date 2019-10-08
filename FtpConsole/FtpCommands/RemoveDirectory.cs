using System;
using FtpLibrary;

namespace FtpConsole.FtpCommands
{
    public sealed class RemoveDirectory : IFtpCommand
    {
        public void Execute(string ftpUri, string path = null, string username = null, string password = null)
        {
            Console.WriteLine("Removing directory...");
            try
            {
                using (var ftp = new FileTransferProtocol(ftpUri, path, username, password))
                {
                    ftp.RemoveDirectory();
                    Console.Write(ftp.RequestStatus);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine($"{ftpUri} remove directory error -> {exception.Message}");
            }  
        }
    }
}