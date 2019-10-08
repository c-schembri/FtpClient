using System;
using FtpLibrary;

namespace FtpConsole.FtpCommands
{
    public sealed class Rename : IFtpCommand
    {
        public void Execute(string ftpUri, string path = null, string username = null, string password = null)
        {
            Console.WriteLine("Renaming...");
            try
            {
                using (var ftp = new FileTransferProtocol(ftpUri, path, username, password))
                {
                    ftp.Rename();
                    Console.Write(ftp.RequestStatus);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine($"{ftpUri} rename error -> {exception.Message}");
            }  
        }
    }
}