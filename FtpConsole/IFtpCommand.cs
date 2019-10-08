namespace FtpConsole
{
    public interface IFtpCommand
    {
        /// <summary>
        /// Executes the FTP command by initiating a request with the FTP server.
        /// </summary>
        void Execute(string ftpUri, string path = null, string username = null, string password = null);
    }
}