using log4net.Appender;
using log4net.Config;
using log4net.Layout;

public class Logger
{
    /// <summary>
    ///  Configure logging to write to Logs\EventLog.txt and the Unity console output.
    /// </summary>
    public static void ConfigureAllLogging()
    {
        var patternLayout = new PatternLayout
        {
            ConversionPattern = "%date %-5level %logger - %message%newline"
        };
        patternLayout.ActivateOptions();

        //// setup the appender that writes to Log\EventLog.txt
        //var fileAppender = new RollingFileAppender
        //{
        //    AppendToFile = false,
        //    File = @"Logs\EventLog.txt",
        //    Layout = patternLayout,
        //    MaxSizeRollBackups = 5,
        //    MaximumFileSize = "1GB",
        //    RollingStyle = RollingFileAppender.RollingMode.Size,
        //    StaticLogFileName = true
        //};
        //fileAppender.ActivateOptions();

        var unityLogger = new UnityAppender
        {
            Layout = patternLayout
        };
        unityLogger.ActivateOptions();

        BasicConfigurator.Configure(unityLogger);
    }
}
