using System;
using System.IO;
using System.Net;
using FtpLibrary;
using NUnit.Framework;

namespace FtpUnitTests
{
    [TestFixture]
    public class BasicFileTests
    {
        /// <summary>
        /// Downloads file from an FTP server: fails if unable to open a connection with the server, unable to download file, or an invalid stream is returned.
        /// </summary>
        /// <remarks>Assertion also fails if a WebException is thrown.</remarks>
        [Test, Order(1)]
        public void CheckDownloadFile()
        {
            const string ftp_uri = "ftp://_serveraddress_/zip_file.zip";
            const string path = "downloaded_zip_file.zip";
            try
            {
                using (var ftp = new FileTransferProtocol(ftp_uri, path, "username", "password"))
                {
                    if (ftp.RequestFileDownload())
                    {
                        Assert.That(ftp.RequestStatusCode == FtpStatusCode.OpeningData || ftp.RequestStatusCode == FtpStatusCode.DataAlreadyOpen);
                        ftp.DownloadFile();
                        Assert.That(ftp.RequestStatusCode == FtpStatusCode.ClosingData);
                        Assert.That(File.Exists(path));
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
        /// Uploads file to an FTP server: fails if file is unable to be uploaded.
        /// </summary>
        /// <remarks>Assertion also fails if a WebException is thrown.</remarks>
        [Test, Order(2)]
        public void CheckUploadFile()
        {
            const string ftp_uri = "ftp://_serveraddress_/uploaded_zip_file.zip";
            const string path = "downloaded_zip_file.zip";
            try
            {
                using (var ftp = new FileTransferProtocol(ftp_uri, path, "username", "password"))
                {
                    Assert.That(File.Exists(path));
                    ftp.UploadFile();
                    Assert.That(ftp.RequestStatusCode == FtpStatusCode.ClosingData);
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
        /// Renames a file on an FTP server: fails if the file is unable to be renamed.
        /// </summary>
        /// <remarks>Assertion also fails if a WebException is thrown.</remarks>
        [Test, Order(3)]
        public void CheckRenameFile()
        {
            const string ftp_uri = "ftp://_serveraddress_/uploaded_zip_file.zip";
            const string path = "renamed_zip_file.zip";
            try
            {
                using (var ftp = new FileTransferProtocol(ftp_uri, path, "username", "password"))
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
        /// Gets the size of a file on an FTP server: fails if the file size was unable to be retrieved.
        /// </summary>
        /// <remarks>Assertion also fails if a WebException is thrown.</remarks>
        [Test, Order(4)]
        public void CheckGetSize()
        {
            const string ftp_uri = "ftp://_serveraddress_/renamed_zip_file.zip";
            try
            {
                using (var ftp = new FileTransferProtocol(ftp_uri, null, "username", "password"))
                {
                    ftp.GetSize();
                    Assert.That(ftp.RequestStatusCode == FtpStatusCode.FileStatus);
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
        /// Deletes a file from an FTP server: fails if the file was unable to be deleted.
        /// </summary>
        /// <remarks>Assertion also fails if a WebException is thrown.</remarks>
        [Test, Order(5)]
        public void CheckDeleteFile()
        {
            const string ftp_uri = "ftp://_serveraddress_/renamed_zip_file.zip";
            try
            {
                using (var ftp = new FileTransferProtocol(ftp_uri, null, "username", "password"))
                {
                    ftp.DeleteFile();
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