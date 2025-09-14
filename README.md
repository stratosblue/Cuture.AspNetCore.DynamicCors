# Cuture.AspNetCore.DynamicCors

A `dynamic Cors` support library based on `Microsoft.AspNetCore.Cors`. 基于 `Microsoft.AspNetCore.Cors` 的 `动态Cors` 支持库。

## 1. Feature

- 运行时动态调整允许跨域的域名列表
- 基于 `Microsoft.AspNetCore.Cors` 工作，通过代理原始 `ICorsPolicyProvider` 实现功能，作为其补充
- 支持`泛域名`配置
- 简化`允许源`的重新加载

-------
 
### Note
与原始的 `Microsoft.AspNetCore.Cors` 相比，当前存在如下差异

- 不验证端口号
- 不验证协议

-------

## 2. 快速开始
安装包
```C#
<PackageReference Include="Cuture.AspNetCore.DynamicCors" Version="1.0.0-*" />
```
添加服务
```C#
services.AddDynamicCors(builder =>
        {
            builder.AddDefaultPolicy(policyBuilder =>
            {
                //注意不允许配置 Origin
                policyBuilder.AllowAnyMethod()
                             .AllowCredentials()
                             .AllowAnyHeader();
            })
            //.SyncAllowedOriginsFromConfiguration("Cors:AllowedOrigins") //从 IConfiguration 同步允许跨域的域名列表
            .SyncAllowedOriginsWithOptions<SampleCorsOptions>(m => m.AllowedOrigins)  //从 IOptions<SampleCorsOptions> 同步允许跨域的域名列表
            ;
        });
```
启用Cors
```C#
app.UseCors();
```

## 3. 自定义

### 3.1 自定义`允许源`的动态变更

使用 `IDomainNameCollectionContainer.Get` 方法获取指定策略的域名集合进行操作

```C#
var collection = collectionContainer.Get("policyName"); //获取指定策略名的域名集合
collection.Add("http://sample.com");  //添加指定域名
collection.Remove("http://sample.com"); //从集合中移除指定域名
collection.Reset(["http://sample.com", "http://sample2.com"]); //重置为指定集合
collection.Contains("http://sample.com"); //检查是否包含指定域名
collection.Clear(); //清空集合
```
