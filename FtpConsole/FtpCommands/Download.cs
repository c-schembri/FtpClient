using System;
using System.IO;
using FtpLibrary;

namespace FtpConsole.FtpCommands
{
    public sealed class Download : IFtpCommand
    {
        public void Execute(string ftpUri, string path = null, string username = null, string password = null)
        {
            Console.WriteLine("File download beginning...");
            using (var ftp = new FileTransferProtocol(ftpUri, path, username, password))
            {
                try
                {
                    if (!ftp.RequestFileDownload())
                    {
                        Console.Write(ftp.RequestStatus);
                        return;
                    }
                    Console.Write(ftp.RequestStatus);
                    if (File.Exists(path))
                    {
                        Console.WriteLine("A file with the same name already exists. Overwrite file? [y/n]");
                        string userResponse = Console.ReadLine() ?? string.Empty;
                        if (userResponse.ToUpper() != "Y")
                        {
                            Console.WriteLine("File download aborted.");
                            return;
                        }
                    }
                    ftp.DownloadFile();
                    Console.Write(ftp.RequestStatus);
                }
                catch (Exception exception)
                {
                    Console.WriteLine($"{ftpUri} download error -> {exception.Message}");
                }
            }
        }
    }
}
    