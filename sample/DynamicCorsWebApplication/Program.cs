using Cuture.AspNetCore.DynamicCors;
using DynamicCorsWebApplication;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddOptions<SampleCorsOptions>().BindConfiguration("Cors");

builder.Services.AddDynamicCors(builder =>
                {
                    builder.AddDefaultPolicy(policyBuilder =>
                    {
                        policyBuilder.AllowAnyMethod()
                                     .AllowCredentials()
                                     .AllowAnyHeader();
                    })
                    .SyncAllowedOriginsWithOptions<SampleCorsOptions>(m => m.AllowedOrigins)
                    .SyncAllowedOriginsFromConfiguration("Cors:AllowedOrigins2")
                    .AddAllowedOriginsSyncSource<SampleAllowedOriginsSyncSource>()
                    ;
                });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapSwaggerUI();
}

//可选
app.WaitDynamicCorsInit();

app.UseCors();

app.MapGet("/hello", () => "world")
   .WithName("Hello")
   .RequireCors();

app.Run();

internal class SampleCorsOptions
{
    #region Public 属性

    public string? AllowedOrigins { get; set; }

    #endregion Public 属性
}
