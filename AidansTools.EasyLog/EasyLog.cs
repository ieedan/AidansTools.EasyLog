using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AidansTools.Logging
{
    /// <summary>
    /// Allows you to easily log data with built in events for errors
    /// </summary>
    public class EasyLog
    {
        private string FilePath;
        /// <summary>
        /// Creates new instance of easy log named as 'AidansTools.EasyLog'
        /// </summary>
        public EasyLog()
        {
            FilePath = AppDomain.CurrentDomain.BaseDirectory + $"AidansTools.EasyLog.txt";
        }

        /// <summary>
        /// Specify the name of your log
        /// </summary>
        /// <param name="name"></param>
        public EasyLog(string name)
        {
            FilePath = AppDomain.CurrentDomain.BaseDirectory + $"{name}.txt";
        }

        /// <summary>
        /// Allows you to add a name of a folder to store the log in no need to include backslashes
        /// 
        /// <example>
        ///     \Logs\Logs.txt
        /// </example>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fileInDirectory"></param>
        public EasyLog(string name, string fileInDirectory)
        {
            string fullPath = AppDomain.CurrentDomain.BaseDirectory + fileInDirectory;
            if (!Directory.Exists(fullPath))
            {
                Directory.CreateDirectory(fullPath);
            }
            FilePath = fullPath + @"\" + $"{name}.txt";
        }

        /// <summary>
        /// Logs info with date-time stamp to FilePath specified from the constuctor
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public Task LogInfo(object value)
        {
            string fulltext = $"[{DateTime.Now}] {value.ToString()}";

            File.AppendAllText(FilePath, fulltext + Environment.NewLine);

            System.Diagnostics.Debug.WriteLine(fulltext);

            NewInfoLogged?.Invoke();

            return Task.CompletedTask;
        }

        /// <summary>
        /// Logs info with date-time stamp to FilePath specified from the constuctor in all caps to be identified as an error
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public Task LogError(object value, [CallerMemberName] string name = null)
        {
            string fulltext = $"[{DateTime.Now}] [Called by: {name}] {value.ToString()}".ToUpper();

            File.AppendAllText(FilePath, fulltext + Environment.NewLine);

            System.Diagnostics.Debug.WriteLine(fulltext);

            NewErrorLogged?.Invoke(0);

            return Task.CompletedTask;
        }

        /// <summary>
        /// Logs info with date-time stamp to FilePath specified from the constuctor in all caps to be identified as an error allows for error code
        /// </summary>
        /// <param name="text"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public Task LogError(object value, int code, [CallerMemberName] string name = null)
        {
            string fulltext = $"[{DateTime.Now}] [Error Code {code}] [Called by: {name}] {value.ToString()}".ToUpper();

            File.AppendAllText(FilePath, fulltext + Environment.NewLine);

            System.Diagnostics.Debug.WriteLine(fulltext);

            NewErrorLogged?.Invoke(code);

            return Task.CompletedTask;
        }

        /// <summary>
        /// Clears the logs from the specified file
        /// </summary>
        /// <returns></returns>
        public Task FlushLog()
        {
            File.WriteAllText(FilePath, String.Empty);

            System.Diagnostics.Debug.WriteLine($"Clearing logs in {FilePath}");

            LogCleared?.Invoke();

            return Task.CompletedTask;
        }

        /// <summary>
        /// Fires when logs are cleared
        /// </summary>
        public event Action LogCleared;

        /// <summary>
        /// Fires when new info is logged
        /// </summary>
        public event Action NewInfoLogged;

        /// <summary>
        /// Will fire when error is logged and will return an error code if included
        /// </summary>
        public event Action<int> NewErrorLogged;
    }
}
