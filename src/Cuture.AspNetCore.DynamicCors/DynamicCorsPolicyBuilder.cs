#pragma warning disable IDE0130

using System.ComponentModel;
using Microsoft.Extensions.DependencyInjection;

namespace Cuture.AspNetCore.DynamicCors;

/// <summary>
/// 动态Cors策略构造器
/// </summary>
/// <param name="Services"></param>
/// <param name="PolicyName">没有策略名称时认为是默认策略</param>
[EditorBrowsable(EditorBrowsableState.Never)]
public record class DynamicCorsPolicyBuilder(IServiceCollection Services, string? PolicyName);
