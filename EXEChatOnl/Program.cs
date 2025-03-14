using Container.DependencyInjection;
using Microsoft.OpenApi.Models;
using Fleck;
using Microsoft.AspNetCore.OData;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

// Đăng ký các dịch vụ từ ServiceContainer
builder.Services.InfrastructureServices(builder.Configuration);

// Thêm các dịch vụ cơ bản
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Nhập 'Bearer' [space] token của bạn. Ví dụ: Bearer abc123xyz"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

// Thêm CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins(
                    "https://6f8f-14-232-61-47.ngrok-free.app",  // Production (ngrok)
                    "http://localhost:3001"                       // Localhost FE
                )
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});


var app = builder.Build();

// Áp dụng CORS
app.UseCors(MyAllowSpecificOrigins);
app.UseAuthentication();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// 🔥 Khởi động WebSocket server không chặn main thread
Task.Run(() => StartWebSocketServer());

app.Run();

// ------------------------------------
// 🚀 WebSocket Server chạy song song với API
void StartWebSocketServer()
{
    var server = new WebSocketServer("ws://0.0.0.0:8181")
    {
        RestartAfterListenError = true
    };

    var wsConnections = new Dictionary<IWebSocketConnection, string>();
    int guestCounter = 1;  // Đếm số lượng Guest

    server.Start(ws =>
    {
        ws.OnOpen = () =>
        {
            lock (wsConnections)
            {
                if (!wsConnections.ContainsKey(ws))
                {
                    wsConnections[ws] = "Guest_" + guestCounter++;
                }
                Console.WriteLine($"🟢 Client kết nối: {wsConnections[ws]}");
            }
        };

        ws.OnClose = () =>
        {
            lock (wsConnections)
            {
                if (wsConnections.TryGetValue(ws, out string username))
                {
                    Console.WriteLine($"🔴 Client {username} đã ngắt kết nối");
                    wsConnections.Remove(ws);
                }
            }
        };

        ws.OnMessage = message =>
        {
            try
            {
                lock (wsConnections)
                {
                    if (message.StartsWith("USERNAME:"))
                    {
                        string username = message.Substring(9).Trim();

                        if (string.IsNullOrEmpty(username) || wsConnections.ContainsValue(username))
                        {
                            ws.Send("{\"error\": \"Username đã tồn tại hoặc không hợp lệ.\"}");
                        }
                        else
                        {
                            wsConnections[ws] = username;
                            Console.WriteLine($"👤 User {username} đã kết nối");
                        }
                        return;
                    }

                    if (!wsConnections.ContainsKey(ws))
                    {
                        ws.Send("{\"error\": \"Bạn chưa đặt username. Vui lòng gửi 'USERNAME:TênCủaBạn' trước.\"}");
                        return;
                    }

                    string sender = wsConnections[ws];

                    // 🛠 Kiểm tra JSON hợp lệ
                    var jsonData = JsonConvert.DeserializeObject<Dictionary<string, string>>(message);
                    if (jsonData == null || !jsonData.ContainsKey("receiver") || !jsonData.ContainsKey("text"))
                    {
                        ws.Send("{\"error\": \"Dữ liệu tin nhắn không hợp lệ.\"}");
                        return;
                    }

                    string receiver = jsonData["receiver"];
                    string text = jsonData["text"];

                    // 👉 Kiểm tra người nhận có online không
                    var receiverConnection = wsConnections.FirstOrDefault(c => c.Value == receiver).Key;
                    if (receiverConnection == null)
                    {
                        ws.Send(JsonConvert.SerializeObject(new { error = "Người nhận không tồn tại hoặc offline." }));
                        return;
                    }

                    // 👉 Gửi tin nhắn đến người nhận
                    var jsonMessage = new { sender, text };
                    receiverConnection.Send(JsonConvert.SerializeObject(jsonMessage));
                    Console.WriteLine($"📨 {sender} gửi tin nhắn đến {receiver}: {text}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Lỗi xử lý tin nhắn: {ex.Message}");
                ws.Send("{\"error\": \"Lỗi xử lý tin nhắn.\"}");
            }
        };
    });

    Console.WriteLine("🚀 WebSocket server started on ws://0.0.0.0:8181");
}
