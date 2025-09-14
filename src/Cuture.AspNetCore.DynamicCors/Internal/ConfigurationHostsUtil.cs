namespace Cuture.AspNetCore.DynamicCors.Internal;

internal static class ConfigurationHostsUtil
{
    #region Public 方法

    public static string[] SplitAsHosts(this string value)
    {
        return value.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
    }

    #endregion Public 方法
}
