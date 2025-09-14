using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

namespace Cuture.AspNetCore.DynamicCors.Internal.OriginsSync;

internal sealed class AllowedOriginsSynchronizer : IDisposable
{
    #region Private 字段

    private readonly List<SyncSourceOrigins> _allOrigins = [];

    private readonly CancellationTokenSource _cancellationTokenSource;

    private readonly List<IDisposable> _changeCallbackDisposers = [];

    private readonly IDomainNameCollectionContainer _domainNameCollectionContainer;

    private readonly ILogger _logger;

    private readonly DynamicCorsOptions _options;

    private readonly SemaphoreSlim _syncSemaphore = new(1);

    private bool _isDisposed;

    #endregion Private 字段

    #region Public 构造函数

    public AllowedOriginsSynchronizer(IDomainNameCollectionContainer domainNameCollectionContainer,
                                      IEnumerable<IAllowedOriginsSyncSource> sources,
                                      IOptions<DynamicCorsOptions> options,
                                      ILogger<AllowedOriginsSynchronizer> logger)
    {
        ArgumentNullException.ThrowIfNull(domainNameCollectionContainer);
        ArgumentNullException.ThrowIfNull(sources);
        ArgumentNullException.ThrowIfNull(options.Value);
        ArgumentNullException.ThrowIfNull(logger);

        _domainNameCollectionContainer = domainNameCollectionContainer;
        _options = options.Value;
        _logger = logger;
        _cancellationTokenSource = new CancellationTokenSource();

        var changeCallback = SourceChangeCallback;
        foreach (var source in sources)
        {
            var currentSourceOrigins = new SyncSourceOrigins(source.PolicyName ?? _options.DefaultPolicyName, source);
            _changeCallbackDisposers.Add(ChangeToken.OnChange(source.GetReloadToken, changeCallback, currentSourceOrigins));
            _allOrigins.Add(currentSourceOrigins);
        }

        var cancellationToken = _cancellationTokenSource.Token;

        //初始化
        Task.Run(async () =>
        {
            await _syncSemaphore.WaitAsync(cancellationToken);

            try
            {
                foreach (var sourceOrigins in _allOrigins)
                {
                    try
                    {
                        await sourceOrigins.LoadOriginsAsync(cancellationToken);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Init allowed origins from source {Source} failed.", sourceOrigins.Source);
                    }
                }

                foreach (var group in _allOrigins.GroupBy(m => m.PolicyName))
                {
                    ResetDomainNameCollection(group.Key, group.SelectMany(m => m.Origins));
                }
            }
            finally
            {
                _syncSemaphore.Release();
            }
        });
    }

    #endregion Public 构造函数

    #region Public 方法

    /// <inheritdoc/>
    public void Dispose()
    {
        if (_isDisposed)
        {
            return;
        }

        try
        {
            _cancellationTokenSource.Cancel();
        }
        catch { }

        _cancellationTokenSource.Dispose();

        foreach (var item in _changeCallbackDisposers)
        {
            item.Dispose();
        }

        _syncSemaphore.Dispose();

        _isDisposed = true;
    }

    #endregion Public 方法

    #region Private 方法

    private void ResetDomainNameCollection(string? policyName, IEnumerable<string> allowedOrigins)
    {
        policyName ??= _options.DefaultPolicyName;

        try
        {
            if (_domainNameCollectionContainer.Get(policyName) is not { } domainNameCollection)
            {
                _logger.LogWarning("Cannot get dynamic cors domainNameCollection for policy {Name}.", policyName);
            }
            else
            {
                domainNameCollection.Reset(allowedOrigins);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Reset allowed origins failed.");
        }
    }

    private void SourceChangeCallback(SyncSourceOrigins sourceOrigins)
    {
        var cancellationToken = _cancellationTokenSource.Token;

        Task.Run(async () =>
        {
            await _syncSemaphore.WaitAsync(cancellationToken);

            var policyName = sourceOrigins.PolicyName ?? _options.DefaultPolicyName;

            try
            {
                try
                {
                    _logger.LogInformation("Starting sync allowed origins for {Name} from source {Source}.", policyName, sourceOrigins.Source);

                    await sourceOrigins.LoadOriginsAsync(cancellationToken);

                    ResetDomainNameCollection(policyName, _allOrigins.Where(m => m.PolicyName == policyName).SelectMany(m => m.Origins));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Sync allowed origins for {Name} from source {Source} failed.", policyName, sourceOrigins.Source);
                }
            }
            finally
            {
                _syncSemaphore.Release();
            }
        }, cancellationToken);
    }

    #endregion Private 方法

    private record class SyncSourceOrigins(string PolicyName, IAllowedOriginsSyncSource Source)
    {
        public string[] Origins { get; private set; } = [];

        public async Task LoadOriginsAsync(CancellationToken cancellationToken)
        {
            var origins = await Source.GetOriginsAsync(cancellationToken);
            Origins = [.. origins];
        }
    }
}
