using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace Cuture.AspNetCore.DynamicCors.Internal.OriginsSync;

internal sealed class ConfigurationAllowedOriginsSyncSource(string? policyName, IConfiguration configuration)
    : IAllowedOriginsSyncSource
{
    #region Public 属性

    public string? PolicyName { get; } = policyName;

    #endregion Public 属性

    #region Public 方法

    public void Dispose()
    {
    }

    public Task<IEnumerable<string>> GetOriginsAsync(CancellationToken cancellationToken)
    {
        IEnumerable<string>? origins;

        if (configuration.Get<string[]>() is { } arrayValue)
        {
            origins = arrayValue.SelectMany(m => m?.SplitAsHosts() ?? []);
        }
        else
        {
            origins = (configuration as IConfigurationSection)?.Value?.SplitAsHosts();
        }

        origins = origins?.Where(static m => !string.IsNullOrWhiteSpace(m!));

        return Task.FromResult(origins ?? []);
    }

    public IChangeToken GetReloadToken() => configuration.GetReloadToken();

    #endregion Public 方法
}
