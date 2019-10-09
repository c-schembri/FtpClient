# FtpClient
A modular file transfer protocol client solution that enables the complete manipulation of an FTP server's contents, completely written in C#.

## What the repository contains
The repository is composed of three major components: the library, unit tests, and an example console project.
These three components are more than sufficient in understanding the source code, as well as verifying its success.

## Further information & resources
The approach I used in developing this FTP client heavily involved the notion of modularity. That is, I find it more beneficial to use the file transfer protocol implementation by virtue of *using* statements, hence the IDisposable implement, thereby allowing exceptional scalability and ease of use. 

The supplied (successful) unit tests cover only the basic possible operations that can be performed via the library function; it does not, for example, handle multiple FTP methods in succession nor any data confirmation (e.g., ensuring a zip files contents are properly transferred). These more advanced tests, however, are on the to-do list.

## A final note
Remember to input the proper server address and authentication details (i.e., username & password) in the unit test method strings. I have left these strings with mock details for obvious reasons.

