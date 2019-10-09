using System;
using System.Net;
using FtpLibrary;
using NUnit.Framework;

namespace FtpUnitTests
{
    [TestFixture]
    public class BasicDirectoryTests
    {
        /// <summary>
        /// Gets the directory list from an FTP server: fails if directory list is unable to be returned.
        /// </summary>
        /// <remarks>Assertion also fails if a WebException is thrown.</remarks>
        [Test]
        public void CheckListDirectory()
        {
            string ftp_uri = $"{Config.Server_Address}/";
            try
            {
                using (var ftp = new FileTransferProtocol(ftp_uri, null, Config.Username, Config.Password))
                {
                    if (ftp.UpdateDirectoryList())
                    {
                        Assert.That(ftp.RequestStatusCode == FtpStatusCode.OpeningData || ftp.RequestStatusCode == FtpStatusCode.DataAlreadyOpen);
                    }
                    else
                    {
                        throw new ArgumentException("Retrieved stream is invalid.");
                    }
                }
            }
            catch (Exception exception)
            {
                if (exception is WebException)
                {
                    Assert.Fail(exception.Message);
                }
            }
        }
        
        /// <summary>
        /// Downloads file from an FTP server: fails if unable to open a connection with the server, unable to download file, or an invalid stream is returned.
        /// </summary>
        /// <remarks>Assertion also fails if a WebException is thrown.</remarks>
        [Test, Order(1)]
        public void CheckMakeDirectory()
        {
            string ftp_uri = $"{Config.Server_Address}/new_directory";
            try
            {
                using (var ftp = new FileTransferProtocol(ftp_uri, null, Config.Username, Config.Password))
                {
                    ftp.MakeDirectory();
                    Assert.That(ftp.RequestStatusCode == FtpStatusCode.PathnameCreated);
                }
            }
            catch (Exception exception)
            {
                if (exception is WebException)
                {
                    Assert.Fail(exception.Message);
                }
            }
        }

        /// <summary>
        /// Renames a directory on an FTP server: fails if the directory is unable to be renamed.
        /// </summary>
        /// <remarks>Assertion also fails if a WebException is thrown.</remarks>
        [Test, Order(2)]
        public void CheckRenameDirectory()
        {
            string ftp_uri = $"{Config.Server_Address}/new_directory";
            try
            {
                using (var ftp = new FileTransferProtocol(ftp_uri, "new_directory_renamed", Config.Username, Config.Password))
                {
                    ftp.Rename();
                    Assert.That(ftp.RequestStatusCode == FtpStatusCode.FileActionOK);
                }
            }
            catch (Exception exception)
            {
                if (exception is WebException)
                {
                    Assert.Fail(exception.Message);
                }
            }
        }
        
        /// <summary>
        /// Removes a directory from an FTP server: fails if the directory is unable to be removed.
        /// </summary>
        /// <remarks>Assertion also fails if a WebException is thrown.</remarks>
        [Test, Order(3)]
        public void CheckRemoveDirectory()
        {
            string ftp_uri = $"{Config.Server_Address}/new_directory_renamed";
            try
            {
                using (var ftp = new FileTransferProtocol(ftp_uri, null, Config.Username, Config.Password))
                {
                    ftp.RemoveDirectory();
                    Assert.That(ftp.RequestStatusCode == FtpStatusCode.FileActionOK);
                }
            }
            catch (Exception exception)
            {
                if (exception is WebException)
                {
                    Assert.Fail(exception.Message);
                }
            }
        }
    }
}