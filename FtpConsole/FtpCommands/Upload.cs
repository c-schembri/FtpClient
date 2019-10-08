using System;
using System.IO;
using FtpLibrary;

namespace FtpConsole.FtpCommands
{
    public sealed class Upload : IFtpCommand
    {
        public void Execute(string ftpUri, string path = null, string username = null, string password = null)
        {
            Console.WriteLine("File upload beginning...");
            if (!File.Exists(path))
            {
                Console.WriteLine($"{ftpUri} upload error -> {path} not found.");
                return;
            }
            using (var ftp = new FileTransferProtocol(ftpUri, path, username, password))
            {
                try
                {
                    ftp.UploadFile();
                    Console.Write(ftp.RequestStatus);
                }
                catch (Exception exception)
                {
                    Console.WriteLine($"{ftpUri} upload error -> {exception.Message}");
                }
            }
        }
    }
}