

using Microsoft.AspNetCore.SignalR.Client;
using System.Timers;
using Microsoft.JSInterop;
using Radzen;
using Hub.Shared;
using Hub.Shared.Model;
using NetworkLibrary;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components;
using Blazored.LocalStorage;
using System.Linq.Dynamic.Core.Tokenizer;
using Hub.Client;
using Hub.Shared.Model.Hub.Login;

public class SignalRService : IAsyncDisposable
{
    private HubConnection? _hubConnection; // 수정: nullable로 변경
    private DateTime _lastActivityTime; // 마지막 사용자 활동 시간
    private readonly System.Timers.Timer _inactivityTimer; // 비활성 타이머
    private readonly ILogger<SignalRService> _logger;
    public event Action<string>? OnMessageReceived;
    private ILocalStorageService _localStorageService;
    private NavigationManager _navigationManager;


    public event Action<string, string, NotificationSeverity>? OnNotificationReceived;
    public SignalRService(ILogger<SignalRService> logger)
    {
        _logger = logger;
        // 마지막 활동 시간 초기화
        _lastActivityTime = DateTime.Now;
        // 비활성 타이머 초기화 (1분 간격으로 체크)
        _inactivityTimer = new System.Timers.Timer(60000); // 1분 = 60,000ms
        _inactivityTimer.Elapsed += CheckInactivity;
        _inactivityTimer.Start();
    }
    public void Initialize(NavigationManager navigationManager, ILocalStorageService localStorageService)
    {
        _navigationManager = navigationManager;
        _localStorageService = localStorageService;
        ResetActivityTimer();
        // 필요한 초기화 작업 수행
        _logger.LogInformation("SignalRService Initialized");
    }


    private void ConfigureEventHandlers()
    {
        // 메시지 수신 처리
        _hubConnection.On<string>("ReceiveNotification", message =>
        {
            SignalRMessage<string> temp = message.DecryptData<SignalRMessage<string>>();
            if (temp.type == MessageType.Alert)
            {

            }
            OnMessageReceived?.Invoke(temp.body);
            ResetActivityTimer(); // 메시지를 받을 때도 활동 시간 갱신
            ShowNotification(temp.title, temp.body, NotificationSeverity.Info);
        });

        // 연결 종료 시 처리
        _hubConnection.Closed += async (error) =>
        {
            ShowNotification("Disconnected", "The connection has been stopped.", NotificationSeverity.Warning);
            await _localStorageService.RemoveItemAsync("jwtToken"); // JWT 삭제
            await _localStorageService.RemoveItemAsync("userData"); 
            await _localStorageService.RemoveItemAsync("loginData"); // JWT 삭제



            //NavigationManager.NavigateTo("/login"); // 로그인 페이지로 이동

        };
        //강제 로그아웃시 처리
        _hubConnection.On("ForceLogout", async () =>
        {
            await HandleLogoutAsync();
        });
    }


    private async Task InitStart()
    {
        try
        {
            await _localStorageService.SetItemAsync("jwtToken", Common.loginData.securityToken);
            await _localStorageService.SetItemAsync("userData", Common.userData);
            await _localStorageService.SetItemAsync("loginData", Common.loginData);

            var token = await _localStorageService.GetItemAsync<string>("jwtToken");
            // HubConnection 객체가 없거나 연결이 끊어졌을 때만 새로 시작
            if (_hubConnection == null || _hubConnection.State == HubConnectionState.Disconnected)
            {
                _hubConnection = new HubConnectionBuilder()
                    .WithUrl("https://localhost:7017/notifications", options =>
                    {
                        if (!string.IsNullOrEmpty(token))
                        {
                            options.AccessTokenProvider = async () =>
                            {
                                return await Task.FromResult(token); 
                            };
                        }
                    })
                    .WithAutomaticReconnect()
                    .Build();
                //서버 에서 주는 method 이벤트 핸들러
                ConfigureEventHandlers();
                await _hubConnection.StartAsync();

                // 연결 시작
                ResetActivityTimer(); // 연결 시 활동 시간 갱신

            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error starting SignalR connection: {ex.Message}");
        }
    }

    public async Task StartConnectionAsync()
    {
        await InitStart();
    }

    private async Task HandleLogoutAsync()
    {
        // 로그아웃 처리 로직
        _logger.LogInformation("Logout requested by the server.");
        // 연결 종료
        await StopConnectionAsync();
        // 로그인 페이지로 리디렉션
        _navigationManager.NavigateTo("/", forceLoad: true);
    }

    public async Task StopConnectionAsync()
    {
        if (_hubConnection?.State == HubConnectionState.Connected)
        {
            await _hubConnection.StopAsync();
        }
    }

    public async Task SendMethodAsync<TIn>(string methodName, TIn data)
    {
        if (_hubConnection?.State == HubConnectionState.Connected)
        {
            await _hubConnection.SendAsync(methodName, data);
            ResetActivityTimer(); // 메시지를 보낼 때 활동 시간 갱신
        }
        else
        {
            _logger.LogInformation("Connection is not established. Cannot send message.");
        }
    }

    public async Task<TOut?> SendMethodAsync<TIn, TOut>(string methodName, TIn data)
    {
        if (_hubConnection?.State == HubConnectionState.Connected)
        {
            TOut? result = await _hubConnection.InvokeAsync<TOut>(methodName, data);
            ResetActivityTimer(); // 메시지를 보낼 때 활동 시간 갱신
            return result;
        }
        else
        {
            _logger.LogInformation("Connection is not established. Cannot send message.");
            return default;
        }
    }


    public async Task JoinGroupAsync(string groupName)
    {
        if (_hubConnection?.State == HubConnectionState.Connected)
        {
            await _hubConnection.SendAsync("JoinGroup", groupName);
            ResetActivityTimer(); // 그룹에 참여할 때 활동 시간 갱신
        }
    }

    private void ResetActivityTimer()
    {
        _lastActivityTime = DateTime.Now; // 마지막 활동 시간 갱신
    }

    private async void CheckInactivity(object? sender, ElapsedEventArgs e)
    {
        // 1시간 동안 활동이 없으면 연결 종료
        if (DateTime.Now - _lastActivityTime > TimeSpan.FromHours(1))
        {
            Console.WriteLine("No activity for 1 hour. Disconnecting...");
            await StopConnectionAsync();
            _inactivityTimer.Stop(); // 타이머 중지
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_hubConnection is not null)
        {
            await _hubConnection.DisposeAsync();
        }

        _inactivityTimer?.Dispose();
    }

    private void ShowNotification(string title, string message, NotificationSeverity severity)
    {
        OnNotificationReceived?.Invoke(title, message, severity);

    }

}
