using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Cuture.AspNetCore.DynamicCors.Internal.OriginsSync;

internal sealed class OptionsMonitorAllowedOriginsSyncSource<TOptions>
    : AllowedOriginsSyncSource
    where TOptions : class
{
    #region Private 字段

    private readonly IOptionsMonitor<TOptions> _optionsMonitor;

    private readonly IDisposable? _optionsMonitorDisposer;

    private readonly Func<TOptions, object?> _propertySelectAction;

    #endregion Private 字段

    #region Public 构造函数

    public OptionsMonitorAllowedOriginsSyncSource(string? policyName,
                                                  IOptionsMonitor<TOptions> optionsMonitor,
                                                  Func<TOptions, object?> propertySelectAction)
        : base(policyName)
    {
        ArgumentNullException.ThrowIfNull(optionsMonitor);
        ArgumentNullException.ThrowIfNull(propertySelectAction);

        _optionsMonitor = optionsMonitor;
        _propertySelectAction = propertySelectAction;

        _optionsMonitorDisposer = optionsMonitor.OnChange(_ =>
        {
            NotifySourceChanged();
        });
    }

    #endregion Public 构造函数

    #region Public 方法

    /// <inheritdoc/>
    public override string ToString() => $"Options: [{typeof(TOptions)}]";

    protected override Task<IEnumerable<string>> InnerGetOriginsAsync(CancellationToken cancellationToken)
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

    #endregion Public 方法

    #region Protected 方法

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        _optionsMonitorDisposer?.Dispose();
    }

    #endregion Protected 方法
}
