using Cuture.AspNetCore.DynamicCors;

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
                    //.SyncAllowedOriginsFromConfiguration("Cors:AllowedOrigins")
                    .SyncAllowedOriginsWithOptions<SampleCorsOptions>(m => m.AllowedOrigins)
                    ;
                });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapSwaggerUI();
}

app.UseCors();

app.MapGet("/hello", () => "world")
   .WithName("Hello")
   .WithOpenApi()
   .RequireCors();

app.Run();

internal class SampleCorsOptions
{
    #region Public 属性

    public string? AllowedOrigins { get; set; }

    #endregion Public 属性
}
