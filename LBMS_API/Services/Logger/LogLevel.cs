using System.Diagnostics;

namespace Logging {
    public enum LogLevel {
        /// <summary>
        /// Fatal error or application crash, likely requires immediate attention
        /// </summary>
        Critical = 1,
        /// <summary>
        /// Recoverable error
        /// </summary>
        Error = 2,
        /// <summary>
        /// Non-Critical problem
        /// </summary>
        Warning = 4,
        /// <summary>
        /// Informational message
        /// </summary>
        Information = 8,
        /// <summary>
        /// Debugging Trace
        /// </summary>
        Verbose = 16,
        /// <summary>
        /// More verbose debugging trace?
        /// </summary>
        Debug = 32,
    }

    /// <summary>
    /// Temporary extension methods to help transition out of using <see cref="TraceEventType"/>
    /// </summary>
    public static class LogLevelExtensions {

        public static LogLevel ToLogLevel(this TraceEventType logLevel) {
            LogLevel result = LogLevel.Information;
            switch (logLevel) {
                case TraceEventType.Critical:
                    result = LogLevel.Critical;
                    break;
                case TraceEventType.Error:
                    result = LogLevel.Error;
                    break;
                case TraceEventType.Information:
                    result = LogLevel.Information;
                    break;
                case TraceEventType.Verbose:
                    result = LogLevel.Verbose;
                    break;
                case TraceEventType.Warning:
                    result = LogLevel.Warning;
                    break;
            }
            return result;
        }

        public static TraceEventType ToTraceEventType(this LogLevel logLevel) {
            TraceEventType result = TraceEventType.Information;
            switch (logLevel) {
                case LogLevel.Critical:
                    result = TraceEventType.Critical;
                    break;
                case LogLevel.Error:
                    result = TraceEventType.Error;
                    break;
                case LogLevel.Warning:
                    result = TraceEventType.Warning;
                    break;
                case LogLevel.Information:
                    result = TraceEventType.Information;
                    break;
                case LogLevel.Verbose:
                    result = TraceEventType.Verbose;
                    break;
                case LogLevel.Debug:
                    result = TraceEventType.Verbose;
                    break;
            }
            return result;
        }

    }
}
