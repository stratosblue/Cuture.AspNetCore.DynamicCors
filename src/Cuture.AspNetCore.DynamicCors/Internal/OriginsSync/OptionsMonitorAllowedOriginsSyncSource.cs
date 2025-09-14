using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

namespace Cuture.AspNetCore.DynamicCors.Internal.OriginsSync;

internal sealed class OptionsMonitorAllowedOriginsSyncSource<TOptions>
    : IAllowedOriginsSyncSource
    where TOptions : class
{
    #region Private 字段

    private readonly IOptionsMonitor<TOptions> _optionsMonitor;

    private readonly IDisposable? _optionsMonitorDisposer;

    private readonly Func<TOptions, object?> _propertySelectAction;

    private ConfigurationReloadToken _changeToken = new();

    #endregion Private 字段

    #region Public 属性

    public string? PolicyName { get; }

    #endregion Public 属性

    #region Public 构造函数

    public OptionsMonitorAllowedOriginsSyncSource(string? policyName, IOptionsMonitor<TOptions> optionsMonitor, Func<TOptions, object?> propertySelectAction)
    {
        ArgumentNullException.ThrowIfNull(optionsMonitor);
        ArgumentNullException.ThrowIfNull(propertySelectAction);

        PolicyName = policyName;
        _optionsMonitor = optionsMonitor;
        _propertySelectAction = propertySelectAction;

        _optionsMonitorDisposer = optionsMonitor.OnChange(_ =>
        {
            var previousToken = Interlocked.Exchange(ref _changeToken, new ConfigurationReloadToken());
            previousToken.OnReload();
        });
    }

    #endregion Public 构造函数

    #region Public 方法

    public void Dispose() => _optionsMonitorDisposer?.Dispose();

    public Task<IEnumerable<string>> GetOriginsAsync(CancellationToken cancellationToken)
    {
        var value = _propertySelectAction(_optionsMonitor.CurrentValue);

        IEnumerable<string?>? origins;

        if (value is IEnumerable<string?> arrayValue)
        {
            origins = arrayValue.SelectMany(m => m?.SplitAsHosts() ?? []);
        }
        else
        {
            origins = (value as string)?.SplitAsHosts();
        }

        origins = origins?.Where(static m => !string.IsNullOrWhiteSpace(m!));

        return Task.FromResult(origins ?? [])!;
    }

    public IChangeToken GetReloadToken() => _changeToken;

    #endregion Public 方法
}
