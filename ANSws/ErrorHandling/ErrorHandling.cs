using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace ANSws
{
    public class ErrorHandling
    {
        /// <summary>
        /// This method is for preapare the error massage on base of Exception Object
        /// </summary>
        /// <param name="serviceException"></param>
        /// <returns></returns>
        public static string CreateErrorMessage(Exception serviceException)
        {
            StringBuilder messageBuilder = new StringBuilder();

            messageBuilder.AppendLine("====================================================================");

            try
            {
                messageBuilder.AppendLine("[" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.ffff") + "] The Exception is:-");

                messageBuilder.AppendLine("Exception :: " + serviceException);
                if (serviceException.InnerException != null)
                {
                    messageBuilder.AppendLine("InnerException :: " + serviceException.InnerException);
                }
                return messageBuilder.ToString();
            }
            catch
            {
                messageBuilder.AppendLine("Exception:: Unknown Exception.");
                return messageBuilder.ToString();
            }

        }

        /// <summary>
        /// This method is for writting the Log file with message parameter
        /// </summary>
        /// <param name="message"></param>
        public static void LogFileWrite(string message)
        {
            FileStream fileStream = null;
            StreamWriter streamWriter = null;
            try
            {
                string logFilePath = HttpContext.Current.Server.MapPath("~/ErrorHandling/log/");

                logFilePath = logFilePath + "ProgramLog" + "-" + DateTime.Today.ToString("yyyyMMdd") + "." + "txt";

                if (logFilePath.Equals("")) return;

                #region Create the Log file directory if it does not exists

                FileInfo logFileInfo = new FileInfo(logFilePath);
                var logDirInfo = new DirectoryInfo(logFileInfo.DirectoryName);
                if (!logDirInfo.Exists) logDirInfo.Create();
                #endregion Create the Log file directory if it does not exists

                if (!logFileInfo.Exists)
                {
                    fileStream = logFileInfo.Create();
                }
                else
                {
                    fileStream = new FileStream(logFilePath, FileMode.Append);
                }
                streamWriter = new StreamWriter(fileStream);
                streamWriter.WriteLine(message);
            }
            finally
            {
                if (streamWriter != null) streamWriter.Close();
                if (fileStream != null) fileStream.Close();
            }

        }

        public static void LogException(Exception ex)
        {
            LogFileWrite(CreateErrorMessage(ex));
        }
    }
}