using Microsoft.EntityFrameworkCore;

using Hub.Server.Models;
using Hub.Server.Services;
using Hub.Server.SignalR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Hub.Server;
using Hub.Server.Interfaces.Service.Voice;
using Hub.Server.Interfaces.Service.Hub;
using Hub.Server.Interfaces.Database.Voice;
using Hub.Server.Repository.Voice;
using System.Text;
using Hub.Server.Common;
using Hub.Server.Interfaces.Service;
using Hub.Server.DBContext;

var builder = WebApplication.CreateBuilder(args);

// 1⃣ 저장소(Repository) 먼저 등록
builder.Services.AddScoped<iInMemoryVoiceRepository, InMemoryVoiceRepository>();
builder.Services.AddScoped<iRdbmsVoiceRepository, RdbmsVoiceRepository>();
builder.Services.AddScoped<iVoiceBroadcastRepository, VoiceBroadcastRepository>();

// 2⃣ 서비스 등록 (Repository를 사용하는 서비스)
builder.Services.AddTransient<iXpHubService, XpHubLoginService>();
builder.Services.AddTransient<iVoiceBroadcastService, VoiceBraodcastService>();  
builder.Services.AddTransient<iVoiceReservationService, VoiceReservationService>();
builder.Services.AddScoped<iNotificationService, NotificationService>();  // 👈 서비스 등록

//3  API 컨트롤러 추가
builder.Services.AddControllers();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

//4 InMemoryDatabase
builder.Services.AddDbContext<XpVoiceInMemoryDbContext>(options =>
    options.UseInMemoryDatabase("ReservationCache"));

// 5 RDBMS
builder.Services.AddDbContext<XpVoiceDbContext>(options =>
options.UseSqlServer("Server=localhost;Database=DB_HUB;User Id=xphub;Password=cpzm"));


// 6   google firebase auth
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = $@"https://securetoken.google.com/{Common.projectId}"; // Firebase Auth 사용 시
        options.Audience = Common.projectId;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Common.secretKey)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];
                if (!string.IsNullOrEmpty(accessToken) &&
                    context.HttpContext.WebSockets.IsWebSocketRequest)
                {
                    context.Token = accessToken;
                }
                return Task.CompletedTask;
            }
        };
    });

// 7 Signal R 설정

builder.Services.AddSignalR(options =>
{
    // 서버에서 클라이언트로 KeepAlive 신호를 보내는 간격
    options.KeepAliveInterval = TimeSpan.FromMinutes(10);
    // 클라이언트의 응답 대기 시간 (1시간)
    options.ClientTimeoutInterval = TimeSpan.FromHours(1);
});

// 5  CORS 설정
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.WithOrigins("https://localhost:7017", "https://localhost:5000")
               .AllowAnyHeader()
               .AllowAnyMethod()
               .AllowCredentials(); // SignalR은 Credentials 필요
    });
});

//builder.Services.AddEndpointsApiExplorer();
// 6 Background Hosted Service
builder.Services.AddHostedService<ServerTimeNotifier>();
builder.Services.AddHostedService<SessionTimeoutService>();

var app = builder.Build();
app.Use(async (context, next) =>
{
    Console.WriteLine($"Request Path: {context.Request.Path}");
    await next();
});
// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();

}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

// ✅ 6. 미들웨어 순서 올바르게 정리
app.UseRouting(); // 먼저 라우팅 활성화
app.UseCors("AllowAll"); // CORS 설정 적용

app.UseAuthentication(); // 인증 먼저 실행
app.UseAuthorization();  // 권한 검사 실행
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();
// ✅ 7. API, SignalR, Blazor 라우팅 등록
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers(); // API 컨트롤러 매핑
    endpoints.MapHub<NotificationHub>("/notifications"); // SignalR 허브 매핑
    endpoints.MapFallbackToFile("index.html"); // Blazor SPA 처리
});
/*

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseCors("AllowAll"); // CORS 설정을 한 번만 호출
app.UseRouting();
//app.UseHttpsRedirection(); // HTTPS 리디렉션 설정
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers(); // API 먼저 처리
    endpoints.MapHub<NotificationHub>("/notifications"); // NotificationHub를 UseEndpoints 내에 위치시킴
    endpoints.MapFallbackToFile("index.html"); // Blazor SPA 처리
});
*/

app.Run();
