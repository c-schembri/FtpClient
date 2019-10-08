using System;
using FtpLibrary;

namespace FtpConsole.FtpCommands
{
    public sealed class Delete : IFtpCommand
    {
        public void Execute(string ftpUri, string path = null, string username = null, string password = null)
        {
            Console.WriteLine("Deleting file...");
            try
            {
                using (var ftp = new FileTransferProtocol(ftpUri, path, username, password))
                {
                    ftp.DeleteFile();
                    Console.Write(ftp.RequestStatus);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine($"{ftpUri} make directory error -> {exception.Message}");
            }  
        }
    }
}