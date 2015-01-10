using System.IO;
using System.Linq;
using VersionOne.Profile;

// TODO at least move classes to separate files
namespace VersionOne.ServiceHost.Core {
    public delegate void ProcessFileDelegate(string file);

    public delegate void ProcessFileBatchDelegate(string[] files);

    public delegate void ProcessFolderDelegate(string folder);

    public delegate void ProcessFolderBatchDelegate(string[] folders);

    public abstract class FileSystemMonitor {
        private readonly IProfile profile;
        private IProfile processedPathsProfile;

        private IProfile ProcessedPaths {
            get {
                return processedPathsProfile ?? (processedPathsProfile = profile["ProcessedFiles"]);
                // Retaining name "ProcessedFiles" for backward-compatibility
            }
        }

        protected string FilterPattern { get; set; }
        protected string WatchFolder { get; set; }

        /// <summary>
        /// Get the processed state of the given file from the profile.
        /// </summary>
        /// <param name="file">File to look up.</param>
        /// <returns>True if processed. False if not processed. Null if not in profile.</returns>
        protected bool? GetState(string file) {
            var value = ProcessedPaths[file].Value;
            
            if(value == null) {
                return null;
            }

            return bool.Parse(value);
        }

        /// <summary>
        /// Save the processing state for the given file to the profile.
        /// </summary>
        /// <param name="file">File in question.</param>
        /// <param name="done">True if processed.</param>
        protected void SaveState(string file, bool? done) {
            ProcessedPaths[file].Value = done == null ? null : done.ToString();
        }

        public FileSystemMonitor(IProfile profile, string watchFolder, string filterPattern) {
            this.profile = profile;
            WatchFolder = watchFolder;
            FilterPattern = filterPattern;

            if(string.IsNullOrEmpty(FilterPattern)) {
                FilterPattern = "*.*";
            }

            var path = Path.GetFullPath(WatchFolder);

            if(!Directory.Exists(path)) {
                Directory.CreateDirectory(path);
            }
        }

        /// <summary>
        /// Perform the basic processing pattern.
        /// </summary>
        /// <param name="path">A file or directory name, depending on the subclass implementation.</param>
        protected void ProcessPath(string path) {
            if (GetState(path) == null) {
                SaveState(path, false);
                InvokeProcessor(path);
                SaveState(path, true);
            }
        }

        protected abstract void InvokeProcessor(string path);
    }

    public class FileMonitor : FileSystemMonitor {
        private readonly ProcessFileDelegate processor;

        public FileMonitor(IProfile profile, string watchfolder, string filterpattern, ProcessFileDelegate processor) : base(profile, watchfolder, filterpattern) {
            this.processor = processor;
        }

        public void ProcessFolder(object pubobj) {
            var path = Path.GetFullPath(WatchFolder);
            var files = Directory.GetFiles(path, FilterPattern);

            foreach(var file in files) {
                ProcessPath(file);
            }
        }

        protected override void InvokeProcessor(string path) {
            processor(path);
        }
    }

    public class FolderMonitor : FileSystemMonitor {
        private readonly ProcessFolderDelegate processor;

        public FolderMonitor(IProfile profile, string watchFolder, string filterPattern, ProcessFolderDelegate processor) : base(profile, watchFolder, filterPattern) {
            this.processor = processor;
        }

        public void ProcessFolder(object pubobj) {
            var path = Path.GetFullPath(WatchFolder);
            var subFolders = Directory.GetDirectories(path, FilterPattern);

            foreach(var subFolder in subFolders) {
                ProcessPath(subFolder);
            }
        }

        protected override void InvokeProcessor(string path) {
            processor(path);
        }
    }

    public class BatchFolderMonitor : FileSystemMonitor {
        private readonly ProcessFolderBatchDelegate processor;

        public BatchFolderMonitor(IProfile profile, string watchFolder, string filterPattern, ProcessFolderBatchDelegate processor) : base(profile, watchFolder, filterPattern) {
            this.processor = processor;
        }

        public void ProcessFolder(object pubobj) {
            var path = Path.GetFullPath(WatchFolder);
            var subFolders = Directory.GetDirectories(path, FilterPattern, SearchOption.AllDirectories);

            var notProcessed = subFolders.Where(subFolder => GetState(subFolder) == null).ToList();

            if(notProcessed.Count == 0) {
                return;
            }

            foreach(var subFolder in notProcessed) {
                SaveState(subFolder, false);
            }

            processor(notProcessed.ToArray());

            foreach(var subFolder in notProcessed) {
                SaveState(subFolder, true);
            }
        }

        protected override void InvokeProcessor(string path) {
            // TODO: Fix this smell
        }
    }

    /// <summary>
    /// More thoroughly determines if a file has been processed.
    /// Compares file modified stamps for paths that have been logged.
    /// </summary>
    public class RecyclingFileMonitor {
        private readonly IProfile profile;
        private IProfile processedPathsProfile;
        private readonly ProcessFileBatchDelegate processor;

        private IProfile ProcessedPaths {
            get {
                return processedPathsProfile ?? (processedPathsProfile = profile["ProcessedFiles"]);
                // Retaining name "ProcessedFiles" for backward-compatibility
            }
        }

        protected string FilterPattern { get; set; }
        protected string WatchFolder { get; set; }

        /// <summary>
        /// Get the processed state of the given file from the profile.
        /// </summary>
        /// <param name="file">File to look up.</param>
        /// <returns>True if processed. False if not processed. Null if not in profile.</returns>
        protected bool? GetState(string file) {
            var value = ProcessedPaths[file].Value;

            if(value == null) {
                return null;
            }

            var haveProcessed = bool.Parse(value);
            
            if (haveProcessed) {
                // we've seen this path before, so look at the last write timestamp
                var stampValue = ProcessedPaths[file]["LastWrite"].Value;
                long storedLastWrite;
                
                if (long.TryParse(stampValue, out storedLastWrite)) {
                    var actualLastWrite = File.GetLastWriteTimeUtc(file).ToBinary();

                    if(actualLastWrite > storedLastWrite) {
                        return null;
                    }
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Save the processing state for the given file to the profile.
        /// </summary>
        /// <param name="file">File in question.</param>
        /// <param name="done">True if processed.</param>
        protected void SaveState(string file, bool? done) {
            ProcessedPaths[file].Value = done == null ? null : done.ToString();

            if (done.HasValue && done.Value) {
                var lastWrite = File.GetLastWriteTimeUtc(file).ToBinary();
                ProcessedPaths[file]["LastWrite"].Value = lastWrite.ToString();
            }
        }

        public RecyclingFileMonitor(IProfile profile, string watchFolder, string filterPattern, ProcessFileBatchDelegate processor) {
            this.processor = processor;
            this.profile = profile;
            WatchFolder = watchFolder;
            FilterPattern = filterPattern;

            if(string.IsNullOrEmpty(FilterPattern)) {
                FilterPattern = "*.*";
            }

            var path = Path.GetFullPath(WatchFolder);

            if(!Directory.Exists(path)) {
                Directory.CreateDirectory(path);
            }
        }


        protected void InvokeProcessor(string[] files) {
            processor(files);
        }

        public void ProcessFolder(object pubobj) {
            var path = Path.GetFullPath(WatchFolder);
            var files = Directory.GetFiles(path, FilterPattern, SearchOption.AllDirectories);

            var toProcess = files.Where(file => GetState(file) == null).ToList();

            foreach(var file in toProcess) {
                SaveState(file, false);
            }

            InvokeProcessor(toProcess.ToArray());

            foreach(var file in toProcess) {
                SaveState(file, true);
            }
        }
    }
}