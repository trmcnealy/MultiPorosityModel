using System;
using System.Collections.Generic;
using System.IO;

using MultiPorosity.Presentation.Models;

namespace MultiPorosity.Presentation.Services
{
    public static class ProjectService
    {
        public static Dictionary<string, ProjectFileMetaData>? ProjectFilesInRepository(string repositoryPath)
        {
            if(!Directory.Exists(repositoryPath))
            {
                return null;
            }

            string[] files = Directory.GetFiles(repositoryPath, "*.mpm", SearchOption.TopDirectoryOnly);

            Dictionary<string, ProjectFileMetaData> projectFiles = new(files.Length);

            string   fileName;
            FileInfo fi;
            DateTime created;
            DateTime lastmodified;

            foreach(string file in files)
            {
                fileName     = Path.GetFileNameWithoutExtension(file);
                fi           = new FileInfo(file);
                created      = fi.CreationTime;
                lastmodified = fi.LastWriteTime;

                projectFiles.Add(fileName, new ProjectFileMetaData(fileName, file, created, lastmodified));
            }

            return projectFiles;
        }
    }
}

//public static void Main()
//{
//    Run();
//}

//private static void Run()
//{
//    string[] args = Environment.GetCommandLineArgs();

//    // If a directory is not specified, exit program.
//    if(args.Length != 2)
//    {
//        // Display the proper way to call the program.
//        Console.WriteLine("Usage: Watcher.exe (directory)");

//        return;
//    }

//    // Create a new FileSystemWatcher and set its properties.
//    using(FileSystemWatcher watcher = new FileSystemWatcher())
//    {
//        watcher.Path = args[1];

//        // Watch for changes in LastAccess and LastWrite times, and
//        // the renaming of files or directories.
//        watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;

//        // Only watch text files.
//        watcher.Filter = "*.txt";

//        // Add event handlers.
//        watcher.Changed += OnChanged;
//        watcher.Created += OnChanged;
//        watcher.Deleted += OnChanged;
//        watcher.Renamed += OnRenamed;

//        // Begin watching.
//        watcher.EnableRaisingEvents = true;

//        // Wait for the user to quit the program.
//        Console.WriteLine("Press 'q' to quit the sample.");

//        while(Console.Read() != 'q')
//        {
//        }

//        ;
//    }
//}

//private static void OnChanged(object              source,
//                              FileSystemEventArgs e)
//{
//    Console.WriteLine($"File: {e.FullPath} {e.ChangeType}");
//}

//private static void OnRenamed(object           source,
//                              RenamedEventArgs e)
//{
//    Console.WriteLine($"File: {e.OldFullPath} renamed to {e.FullPath}");
//}
