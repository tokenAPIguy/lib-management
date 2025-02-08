namespace Logging;

public class FileManager {
        public static bool DriveExists(char driveLetter) {
            string drive = $@"{driveLetter.ToString()}:\";
            bool eExists = false;
            DriveInfo[] allDrives = DriveInfo.GetDrives();

            foreach (DriveInfo d in allDrives) {
                if (d.Name == drive && d.DriveType == DriveType.Fixed) {
                    eExists = true;
                }
            }
            
            return eExists;
        }

        public static void DeleteDirectoryContents(string path, int days = -1) {
            File.SetAttributes(path, FileAttributes.Normal);
            
            List<string> initialFiles = Directory.GetFiles(path).ToList();
            List<string> files = [];

            if (days > -1) {
                foreach (string file in files) {
                    if (new FileInfo(file).LastWriteTime < DateTime.Now.AddDays(days * -1)) {
                        files.Add(file);
                    }
                }
            } else {
              files.AddRange(initialFiles);  
            }

            foreach (string file in files) {
                File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
            };
            
            List<string> dirs = Directory.GetDirectories(path).ToList();
            foreach (string dir in dirs) {
                DeleteDirectoryContents(dir, days);
            }
            
            Directory.Delete(path, false);
        }

        public static void CopyDirectoryContents(string sourcePath, string targetPath) {
            try {
                string source = Path.GetDirectoryName(sourcePath);
                if (string.IsNullOrEmpty(source)) {
                    return;
                }
                
                string target = Path.GetDirectoryName(targetPath);

                try {
                    foreach (string path in Directory.GetDirectories(source, "*", SearchOption.AllDirectories)) {
                        Directory.CreateDirectory(path.Replace(source, target));
                    }
                }
                catch (Exception ex) {
                    Logger.Default.Error($"Failed to create Directories {ex.ToString()}");                    
                }

                try {
                    foreach (string path in Directory.GetFiles(source, "*.*", SearchOption.AllDirectories)) { 
                        File.Copy(path, path.Replace(source, target), true);   
                    }
                }
                catch (Exception ex) {
                    Logger.Default.Error($"Failed to copy Files{ex.ToString()}");
                }
            }
            catch (Exception) { }
        }
    }