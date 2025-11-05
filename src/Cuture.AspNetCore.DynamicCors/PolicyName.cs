namespace Cuture.AspNetCore.DynamicCors;

/// <summary>
/// 策略名称 (此类型为DI容器创建对象时定位参数提供支持)
/// </summary>
/// <param name="Name"></param>
public record PolicyName(string? Name)
{
    /// <inheritdoc/>
    public override string ToString() => Name ?? string.Empty;

    /// <summary>
    /// 隐式类型转换
    /// <para/>
    /// 由类型 <see cref="string"/> 对象转换到类型 <see cref="PolicyName"/> 对象
    /// </summary>
    /// <param name="name">源对象</param>
    public static implicit operator PolicyName(string? name)
    {
        return new(name);
    }

    /// <summary>
    /// 隐式类型转换
    /// <para/>
    /// 由类型 <see cref="PolicyName"/> 对象转换到类型 <see cref="string"/> 对象
    /// </summary>
    /// <param name="value">源对象</param>
    public static implicit operator string?(PolicyName value)
    {
        return value.Name;
    }
}
