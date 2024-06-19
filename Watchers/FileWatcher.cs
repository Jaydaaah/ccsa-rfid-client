using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ccsa_rfid_client.Watchers
{
    internal class FileWatcher
    {
        private static readonly string profilePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        private static readonly string appDataPath = profilePath + "\\AppData";
        private FileSystemWatcher watcher;

        internal delegate void CreateFileActionHandler(string fullname);
        internal CreateFileActionHandler? CreateFileAction { private get; set; }

        public FileWatcher()
        {
            // Specify the path to the Downloads directory

            // Initialize FileSystemWatcher
            watcher = new FileSystemWatcher();
            watcher.Path = profilePath;

            // Watch for changes in LastAccess and LastWrite times, and the renaming of files or directories.
            watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
                                   | NotifyFilters.FileName | NotifyFilters.DirectoryName;

            // Include all subdirectories.
            watcher.IncludeSubdirectories = true;

            // Add event handlers
            watcher.Created += OnFileCreated;
            //watcher.Deleted += OnFileDeleted;
            //watcher.Renamed += OnFileRenamed;

            // Begin watching.
            watcher.EnableRaisingEvents = true;
        }

        private void OnFileCreated(object sender, FileSystemEventArgs e)
        {
            var test = appDataPath;
            if (!e.FullPath.StartsWith(appDataPath, StringComparison.OrdinalIgnoreCase))
            {
                CreateFileAction?.Invoke(e.FullPath);
            }
        }
    }
}
