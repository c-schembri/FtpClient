using System;
using System.IO;
using System.Net;

namespace FtpLibrary
{
    public sealed class FileTransferProtocol : IDisposable
    {
        /// <summary>
        /// The current state of the FTP request.
        /// </summary>
        public string RequestStatus { get; private set; }
        /// <summary>
        /// The current status code of the FTP request.
        /// </summary>
        public FtpStatusCode RequestStatusCode { get; private set; }
        /// <summary>
        /// The last locally stored directory representation of the FTP server.
        /// </summary>
        public string Directory { get; private set; }
        private FtpWebRequest ftpWebRequest;
        private FtpWebResponse ftpWebResponse;
        private Stream responseStream;
        private readonly string ftpUri;
        private readonly string path;
        // IDisposable implement member.
        private bool disposed;
        
        /// <summary>
        /// Initialises a new file transfer protocol object that can be used for accessing and manipulating files on an FTP server.
        /// </summary>
        /// <param name="ftpUri">The URI of the targeted FTP server object.</param>
        /// <param name="path">The destination/source path for downloading/uploading files.</param>
        /// <param name="username">The username network credential.</param>
        /// <param name="password">The password network credential.</param>
        public FileTransferProtocol(string ftpUri, string path, string username, string password)
        {
            this.ftpUri = ftpUri;
            this.path = path;
            // Initialise a basic FTP web request; its details can be overriden by the different FTP methods if necessary.
            this.ftpWebRequest = (FtpWebRequest)WebRequest.Create(this.ftpUri);
            this.ftpWebRequest.Credentials = new NetworkCredential(username, password);
        }
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
        private void Dispose(bool disposing)
        {
            if (this.disposed)
                return;
            
            if (disposing)
            {
                this.ftpWebResponse?.Dispose();
                this.responseStream?.Dispose();
            }

            this.disposed = true;
        }
        ~FileTransferProtocol()
        {
            this.Dispose(false);
        }
        
        /// <summary>
        /// Requests file download from the FTP server.
        /// </summary>
        /// <returns>File download request result.</returns>
        public bool RequestFileDownload()
        {
            this.ftpWebRequest.Method = WebRequestMethods.Ftp.DownloadFile;
            this.ftpWebResponse = (FtpWebResponse)this.ftpWebRequest.GetResponse();
            this.responseStream = this.ftpWebResponse.GetResponseStream();
            this.UpdateRequestStatus($"{this.ftpUri} download update -> {this.ftpWebResponse.StatusDescription}", this.ftpWebResponse.StatusCode);
            if (this.responseStream != null && this.responseStream != Stream.Null) 
                return true;
            
            this.RequestStatus = $"{this.ftpUri} download error -> requested file is empty.";
            return false;
        }

        /// <summary>
        /// Downloads file from the FTP server and saves to disk.
        /// </summary>
        public void DownloadFile()
        {
            using (var fileStream = new FileStream(this.path, FileMode.Create))
            {
                this.responseStream.CopyTo(fileStream);
            }
            this.ftpWebResponse.Close();
            this.UpdateRequestStatus($"{this.ftpUri} download update -> {this.ftpWebResponse.StatusDescription}", this.ftpWebResponse.StatusCode);
        }

        /// <summary>
        /// Uploads file from disk to the FTP server.
        /// </summary>
        public void UploadFile()
        {
            this.ftpWebRequest.Method = WebRequestMethods.Ftp.UploadFile;
            this.ftpWebRequest.UseBinary = true;
            this.ftpWebResponse = (FtpWebResponse)this.ftpWebRequest.GetResponse();
            var fileInfo = new FileInfo(this.path);
            byte[] fileContents;
            using (var binaryReader = new BinaryReader(fileInfo.OpenRead()))
            {
                fileContents = binaryReader.ReadBytes((int)fileInfo.Length);
            }
            using (var requestStream = this.ftpWebRequest.GetRequestStream())
            {
                requestStream.Write(fileContents, 0, fileContents.Length);
            }
            this.ftpWebResponse.Close();
            this.UpdateRequestStatus($"{this.ftpUri} upload update -> {this.ftpWebResponse.StatusDescription}", this.ftpWebResponse.StatusCode);
        }

        /// <summary>
        /// Deletes file from the FTP server.
        /// </summary>
        public void DeleteFile()
        {
            this.ftpWebRequest.Method = WebRequestMethods.Ftp.DeleteFile;
            this.ftpWebResponse = (FtpWebResponse)this.ftpWebRequest.GetResponse();
            this.ftpWebResponse.Close();
            this.UpdateRequestStatus($"{this.ftpUri} delete file update -> {this.ftpWebResponse.StatusDescription}", this.ftpWebResponse.StatusCode);
        }
        
        /// <summary>
        /// Renames a file or directory that currently exists on the FTP server.
        /// </summary>
        public void Rename()
        {
            this.ftpWebRequest.Method = WebRequestMethods.Ftp.Rename;
            this.ftpWebRequest.RenameTo = this.path;
            this.ftpWebResponse = (FtpWebResponse)this.ftpWebRequest.GetResponse();
            this.ftpWebResponse.Close();
            this.UpdateRequestStatus($"{this.ftpUri} rename update -> {this.ftpWebResponse.StatusDescription}", this.ftpWebResponse.StatusCode);
        }

        /// <summary>
        /// Gets the size of a file that currently exists on the FTP server.
        /// </summary>
        public void GetSize()
        {
            this.ftpWebRequest.Method = WebRequestMethods.Ftp.GetFileSize;
            this.ftpWebResponse = (FtpWebResponse)this.ftpWebRequest.GetResponse();
            this.ftpWebResponse.Close();
            this.UpdateRequestStatus($"{this.ftpUri} get size update -> {this.ftpWebResponse.StatusDescription}", this.ftpWebResponse.StatusCode);
        }
        
        /// <summary>
        /// Requests the directory list of the FTP server and updates this object's internal copy of it. 
        /// </summary>
        /// <returns>Update directory list result.</returns>
        public bool UpdateDirectoryList()
        {
            this.ftpWebRequest.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
            this.ftpWebResponse = (FtpWebResponse)this.ftpWebRequest.GetResponse();
            this.responseStream = this.ftpWebResponse.GetResponseStream();
            this.UpdateRequestStatus($"{this.ftpUri} list directory update -> {this.ftpWebResponse.StatusDescription}", this.ftpWebResponse.StatusCode);
            if (this.responseStream == null || this.responseStream == Stream.Null) 
                return false;
            
            using (var streamReader = new StreamReader(this.responseStream))
            {
                this.Directory = streamReader.ReadToEnd();
            }
            this.ftpWebResponse.Close();
            return true;
        }

        /// <summary>
        /// Makes a new directory on the FTP server.
        /// </summary>
        public void MakeDirectory()
        {
            this.ftpWebRequest.Method = WebRequestMethods.Ftp.MakeDirectory;
            this.ftpWebResponse = (FtpWebResponse)this.ftpWebRequest.GetResponse();
            this.ftpWebResponse.Close();
            this.UpdateRequestStatus($"{this.ftpUri} make directory update -> {this.ftpWebResponse.StatusDescription}", this.ftpWebResponse.StatusCode);
        }

        /// <summary>
        /// Removes an empty directory from the FTP server.
        /// </summary>
        public void RemoveDirectory()
        {
            this.ftpWebRequest.Method = WebRequestMethods.Ftp.RemoveDirectory;
            this.ftpWebResponse = (FtpWebResponse)this.ftpWebRequest.GetResponse();
            this.ftpWebResponse.Close();
            this.UpdateRequestStatus($"{this.ftpUri} remove directory update -> {this.ftpWebResponse.StatusDescription}", this.ftpWebResponse.StatusCode);
        }

        /// <summary>
        /// Updates the internal status and status code.
        /// </summary>
        /// <param name="newStatus">The new status to update to.</param>
        /// <param name="newStatusCode">The new status code to update to.</param>
        private void UpdateRequestStatus(string newStatus, FtpStatusCode newStatusCode)
        {
            this.RequestStatus = newStatus;
            this.RequestStatusCode = newStatusCode;
        }
    }
}