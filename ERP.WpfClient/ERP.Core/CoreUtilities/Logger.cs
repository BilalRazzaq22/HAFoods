using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Repository.Hierarchy;
using System;

namespace PrizeBondChecker.Core.CoreUtilities
{
    public class Logger
    {
        #region Constants
        private const string LOGGER_NAME = "PrizeBondChecker";
        private const string INFO_LOGGER_NAME = "PrizeBondChecker";
        private const string APPENDER_NAME = "RollingFileAppender";

        #endregion

        #region Member Variables

        private static readonly ILog _log = LogManager.GetLogger(LOGGER_NAME);
        private static readonly ILog _info = LogManager.GetLogger(INFO_LOGGER_NAME);

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Logger"/> class.
        /// </summary>
        static Logger()
        {
            XmlConfigurator.Configure();
            Hierarchy hierarchy = LogManager.GetRepository() as Hierarchy;
            if (hierarchy != null)
            {
                RollingFileAppender fileAppender = (RollingFileAppender)hierarchy.GetLogger(LOGGER_NAME, hierarchy.LoggerFactory).GetAppender(APPENDER_NAME);
                if (fileAppender != null)
                {
                    fileAppender.ActivateOptions();
                }

                RollingFileAppender infoFileAppender = (RollingFileAppender)hierarchy.GetLogger(INFO_LOGGER_NAME, hierarchy.LoggerFactory).GetAppender(APPENDER_NAME);
                if (infoFileAppender != null)
                {
                    infoFileAppender.ActivateOptions();
                }
            }
        }
        #endregion

        #region Public Methods

        public static void Error(object message, Exception ex)
        {

            var exM = string.Empty;
            var ixm = ex.InnerException;
            while (ixm != null)
            {
                exM += String.Format("\r\n---------------\r\n{0}\r\n------------------", ixm.Message);
                ixm = ixm.InnerException;
            }

            _log.Error(message + exM, ex);
        }

        public static void Info(object message, string methodName)
        {
            _log.Info(message + " in " + methodName + " method");
        }

        public static void Error(string message)
        {
            _log.Error(message);
        }
        public static void Warn(string message)
        {
            _info.Warn(message);
        }
        public static void Debug(string message)
        {
            _info.Debug(message);
        }

        public static void Fatal(string message)
        {
            _info.Fatal(message);
        }
        #endregion
    }

}
