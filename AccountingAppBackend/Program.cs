using System.Text;
using System.Text.Json;
using AccountingAppBackend.DataAccess;
using AccountingAppBackend.Services.INF;
using AccountingAppBackend.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<dbContext>(options =>
{
    //  替換成你的 MySQL 連線字串
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),  //  從 appsettings.json 讀取連線字串
        new MySqlServerVersion(new Version(8, 0, 29)) //  或你的 MySQL Server 版本
    );
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy =>
        {
            policy.WithOrigins("https://brave-water-0ffc2560f.6.azurestaticapps.net")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

// 1.  設定 Authentication (驗證) 服務
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme) //  使用 JWT Bearer 作為預設驗證方案
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true, //  是否驗證 Token 簽發者
            ValidateAudience = false, //  是否驗證 Token 接收者 (通常 Web API 不驗證 Audience)
            ValidateLifetime = true, //  是否驗證 Token 是否過期
            ValidateIssuerSigningKey = true, //  是否驗證簽名金鑰
            ValidIssuer = builder.Configuration["JWT:Issuer"], //  Token 簽發者
            //ValidAudience = builder.Configuration["JWT:Audience"], // Token 接收者 (通常 Web API 不驗證 Audience)
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:KEY"] ?? "")) // 簽名金鑰 (務必與產生 Token 的金鑰一致)
        };
        
        // 新增事件處理
        options.Events = new JwtBearerEvents
        {
            OnChallenge = async context =>
            {
                context.HandleResponse(); // 阻止預設的挑戰行為

                string errorMessage = "Unauthorized";
                if (context.AuthenticateFailure != null)
                {
                    errorMessage = context.AuthenticateFailure switch
                    {
                        SecurityTokenExpiredException => "Token expired",
                        SecurityTokenInvalidSignatureException => "Invalid token signature",
                        _ => "Invalid token"
                    };
                }

                context.Response.StatusCode = 401;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonSerializer.Serialize(new 
                {
                    error = errorMessage,
                    details = context.AuthenticateFailure?.Message
                }));
            }
        };
    });

// 2.  設定 Authorization (授權) 服務 (可選，如果您需要基於角色或權限的授權)
builder.Services.AddAuthorization();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<IDropdownOptionsService, DropdownOptionsService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

var app = builder.Build();

app.UseCors("AllowReactApp");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
