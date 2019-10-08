using System;
using FtpConsole.FtpCommands;
using System.Collections.Generic;

namespace FtpConsole
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Dictionary<string, IFtpCommand> ftpCommands = new Dictionary<string, IFtpCommand>
            {
                {FtpCommandIdentifiers.Download, new Download()},
                {FtpCommandIdentifiers.Upload, new Upload()},
                {FtpCommandIdentifiers.Delete, new Delete()},
                {FtpCommandIdentifiers.Rename, new Rename()},
                {FtpCommandIdentifiers.Get_Size, new GetSize()},
                {FtpCommandIdentifiers.List_Directory, new ListDirectory()},
                {FtpCommandIdentifiers.Make_Directory, new MakeDirectory()},
                {FtpCommandIdentifiers.Remove_Directory, new RemoveDirectory()}
            };
            Console.WriteLine("FtpConsole started.");
            Console.Write("Enter username: ");
            string username = Console.ReadLine();
            Console.Write("Enter password: ");
            string password = Console.ReadLine();
            while (true)
            {
                Console.WriteLine("[download], [upload], [delete], [rename], [getsize], [listdirectory], [makedirectory], [removedirectory] of URI resource, or [help] for more information.");
                string[] parameters = (Console.ReadLine() ?? string.Empty).Split();
                string command = parameters[0].ToUpper();
                switch (command)
                {
                    case "EXIT":
                        Console.WriteLine("FtpConsole finished.");
                        return;
                    case "HELP":
                        Console.WriteLine("    [download {ftpUri} {savePath}]");
                        Console.WriteLine("    [upload {ftpUri} {loadPath}]");
                        Console.WriteLine("    [delete {ftpUri}]");
                        Console.WriteLine("    [rename {ftpUri} {newName}}]");
                        Console.WriteLine("    [getsize {ftpUri}]");
                        Console.WriteLine("    [listdirectory {ftpUri}]");
                        Console.WriteLine("    [makedirectory {ftpUri}]");
                        Console.WriteLine("    [removedirectory {ftpUri}]");
                        continue;
                }
                if (ftpCommands.TryGetValue(command, out var ftpCommand))
                {
                    if (parameters.Length < 2)
                    {
                        Console.WriteLine("Invalid command parameters.");
                        continue;
                    }
                    ftpCommand.Execute(parameters[1], parameters.Length < 3 ? null : parameters[2], username, password);
                }
                else
                {
                    Console.WriteLine("Command not recognised.");
                }
            }
        }
    }
}