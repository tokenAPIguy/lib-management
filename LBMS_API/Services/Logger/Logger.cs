using System.Runtime.InteropServices;
using System.Security;

/*
    This is a custom Logging that is being used as a drop in replacement for the default logging provider in ASP.NET
    https://learn.microsoft.com/en-us/aspnet/core/fundamentals/logging/?view=aspnetcore-9.0
*/

namespace Logging {
    public class Logger(bool b) { 
        private static readonly Lazy<Logger> _Default = new Lazy<Logger>(() => new Logger(false));
        public static Logger Default => _Default.Value;

        private const int _MaxFileSize = 2048;
        private const string _DEFAULT_LOG_FOLDER = "Logs";
        private bool _RotatingLog; 
        private readonly Lock _Lock = new();
        
        private static string LogsDir { get; } = $@"{Directory.GetCurrentDirectory()}\{_DEFAULT_LOG_FOLDER}\";
        public static string LogFileBaseName { get; set; } = "access";
        private string LogFile { get; set;  } = $@"{LogsDir}\{LogFileBaseName}.log";
        public LogLevel Level { get; set; } = LogLevel.Information;

        [DllImport("Kernel32.dll"), SuppressUnmanagedCodeSecurity]
        private static extern int GetCurrentProcessorNumber();
        public bool SwitchToVerboseOnError { get; set; } = true;
        public bool KeepLogsOnRotation { get; set; }
        public bool DebugMode {
            get => Level == LogLevel.Verbose;
            set => Level = value ? LogLevel.Verbose : LogLevel.Information;
        }
        
        /// <summary>
        /// Logging Methods
        /// </summary>
        public void Critical(string message) => WriteToFile(LogLevel.Critical, message);
        public void Critical(string message, string filename) => WriteToFile(LogLevel.Critical, message, filename);
        
        public void Error(string message) => WriteToFile(LogLevel.Error, message);
        public void Error(string message, string filename) => WriteToFile(LogLevel.Error, message, filename);

        public void Warning(string message) => WriteToFile(LogLevel.Warning, message);
        public void Warning(string message, string filename) => WriteToFile(LogLevel.Warning, message, filename);

        public void Information(string message) => WriteToFile(LogLevel.Information, message);
        public void Information(string message, string filename) => WriteToFile(LogLevel.Information, message, filename);

        public void Verbose(string message) => WriteToFile(LogLevel.Verbose, message);
        public void Verbose(string message, string filename) => WriteToFile(LogLevel.Verbose, message, filename);

        public void Debug(string message) => WriteToFile(LogLevel.Debug, message);
        public void Debug(string message, string filename) => WriteToFile(LogLevel.Debug, message, filename);

        
        public void Write(LogLevel level, string myMessage) => WriteToFile(level, myMessage, LogFileBaseName);

        // Internal Methods
        private void WriteToFile(LogLevel level, string message, string filename = "") {
            if (SwitchToVerboseOnError) {
                if (level < LogLevel.Warning) {
                    Level = LogLevel.Verbose;
                }
            }

            if (level > Level) {
                return;
            }

            Directory.CreateDirectory(LogsDir);

            lock (_Lock) {
                try {
                    // filename = filename == string.Empty ? LogFileBaseName : filename;
                    LogFileBaseName = string.IsNullOrEmpty(filename)? LogFileBaseName : filename;
                    LogFile = $@"{LogsDir}\{LogFileBaseName}.log";

                    string s = GenerateMessageContents(level, message);
                    bool wroteLog = RetryWriteLog(3, TimeSpan.FromMilliseconds(30), () => { // HERE 
                        using (StreamWriter sw = new StreamWriter(LogFile, true)) {
                            sw.WriteLine(s);
                        }
                    });

                    if (!wroteLog) {
                        using (StreamWriter sw = File.AppendText(LogsDir + filename + "_FileInUse.log")) {
                            sw.WriteLine(s);
                        }
                    }

                    FileInfo fileInfo = new FileInfo(LogFile);
                    if (fileInfo.Length > (_MaxFileSize * 1000)) {
                        RotateLog();
                    }

                } catch (Exception ex) {
                    
                    HandleProcessingError(ex.ToString());
                }
            }
        }

        private bool RetryWriteLog(int times, TimeSpan delay, Action operation) {
            int attempts = 0;
            bool completed = true;
            do {
                try {
                    attempts++;
                    operation();
                    break; // Success! Let's exit the loop!
                } catch (UnauthorizedAccessException unAuthEx) {
                    HandleProcessingError("Unauthorized Exception writing to the log file or directory: " + unAuthEx);
                    break;
                } catch (IOException ioEx) {
                    // file in use by another process. give it another try
                    if (!ioEx.Message.Contains("Could not find a part of the path")) {
                        if (attempts == times) {
                            completed = false;
                            break;
                        }
                        Task.Delay(delay).Wait();
                    }
                } catch (Exception ex) {
                    HandleProcessingError(ex.ToString());
                    break;
                }
            } while (attempts < times);

            return completed;
        }

        private string GenerateMessageContents(LogLevel level, string message) {
            string processNumber;
            try {
                processNumber = GetCurrentProcessorNumber().ToString();
            } catch (Exception) {
                processNumber = "?";
            }

            processNumber = processNumber.PadLeft(2, '0');

            string
                thread = Thread.CurrentThread.ManagedThreadId.ToString().PadLeft(3, '0');
            DateTime
                timestamp = DateTime.Now;
            string
                logLevel = level.ToString().ToUpper().PadRight("INFORMATION".Length, ' ');

            return $"{processNumber}T{thread}_{timestamp} {logLevel} {message}";
        }

        private void HandleProcessingError(string errMsg) {
            string s = $"{Environment.NewLine}{DateTime.Now} -- {errMsg}";
            try {
                File.AppendAllText($"{LogsDir}{LogFileBaseName}_CadLoggerError.log", s);
            } catch {
                // ignored
            }
        }

        private void RotateLog() {
            if (_RotatingLog) {
                return;
            }

            _RotatingLog = true;
            if (KeepLogsOnRotation) {
                File.Copy(LogFile, $"{LogsDir}{LogFileBaseName}_{DateTime.Now:yyyy-MM-dd_HH-mm-ss-ff}.log", true);
            }

            try {
                File.SetAttributes(LogFile, FileAttributes.Normal);
                File.Delete(LogFile);

                int count = 0;
                while (File.Exists(LogFile) & count < 200) {
                    Thread.Sleep(10);
                    count = count + 1;
                }

                if (!File.Exists(LogFile)) {
                    File.WriteAllText(LogFile, string.Empty);
                }
            } catch (Exception) {
                try {
                    File.WriteAllText(LogFile, string.Empty);
                } catch {
                    // ignored
                }
            }
            _RotatingLog = false;
        }
    }
}
