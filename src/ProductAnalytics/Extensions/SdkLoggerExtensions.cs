namespace ProductAnalytics.Extensions;

public static class SdkLoggerExtensions
{
    public static void Trace(this ISdkLogger logger, string message) =>
        logger.Log(SdkLogLevel.Trace, message);

    public static void Trace(this ISdkLogger logger, string message, params object[] args) =>
        logger.Log(SdkLogLevel.Trace, string.Format(message, args));

    public static void Debug(this ISdkLogger logger, string message) =>
        logger.Log(SdkLogLevel.Debug, message);

    public static void Debug(this ISdkLogger logger, string message, params object[] args) =>
        logger.Log(SdkLogLevel.Debug, string.Format(message, args));

    public static void Info(this ISdkLogger logger, string message) =>
        logger.Log(SdkLogLevel.Information, message);

    public static void Info(this ISdkLogger logger, string message, params object[] args) =>
        logger.Log(SdkLogLevel.Information, string.Format(message, args));

    public static void Warn(this ISdkLogger logger, string message) =>
        logger.Log(SdkLogLevel.Warning, message);

    public static void Warn(this ISdkLogger logger, string message, params object[] args) =>
        logger.Log(SdkLogLevel.Warning, string.Format(message, args));

    public static void Warn(this ISdkLogger logger, Exception exception) =>
        logger.Log(SdkLogLevel.Warning, exception.ToString());

    public static void Error(this ISdkLogger logger, string message) =>
        logger.Log(SdkLogLevel.Error, message);

    public static void Error(this ISdkLogger logger, string message, params object[] args) =>
        logger.Log(SdkLogLevel.Error, string.Format(message, args));

    public static void Error(this ISdkLogger logger, Exception exception) =>
        logger.Log(SdkLogLevel.Error, exception.ToString());

    public static void Critical(this ISdkLogger logger, string message) =>
        logger.Log(SdkLogLevel.Critical, message);

    public static void Critical(this ISdkLogger logger, string message, params object[] args) =>
        logger.Log(SdkLogLevel.Critical, string.Format(message, args));

    public static void Critical(this ISdkLogger logger, Exception exception) =>
        logger.Log(SdkLogLevel.Critical, exception.ToString());
}