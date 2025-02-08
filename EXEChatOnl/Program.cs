using Container.DependencyInjection;
using Microsoft.OpenApi.Models;
using Fleck;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

// Đăng ký các dịch vụ từ ServiceContainer
builder.Services.InfrastructureServices(builder.Configuration);

// Thêm các dịch vụ cơ bản
builder.Services.AddControllers();
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

var app = builder.Build();

// 🔹 Pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// 🔹 CORS
app.UseCors("AllowAll");

// 🔹 Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// 🌟 Chạy WebSocket Server trên một Task riêng
Task.Run(() => StartWebSocketServer());

app.Run();


/*// ✅ Chạy WebSocket Server mà không block main thread
void StartWebSocketServer()
{
    var server = new WebSocketServer("ws://0.0.0.0:8181");
    var wsConnections = new Dictionary<IWebSocketConnection, string>();
    var lastMessagedUsers = new Dictionary<IWebSocketConnection, IWebSocketConnection>();

    server.Start(ws =>
    {
        ws.OnOpen = () =>
        {
            Console.WriteLine("🟢 Client connected");
            wsConnections[ws] = "Guest";
        };

        ws.OnClose = () =>
        {
            if (wsConnections.TryGetValue(ws, out string username))
            {
                Console.WriteLine($"🔴 Client {username} disconnected");
                wsConnections.Remove(ws);
            }

            lastMessagedUsers.Remove(ws);
        };

        ws.OnMessage = message =>
        {
            try
            {
                if (message.StartsWith("USERNAME:"))
                {
                    string username = message.Substring(9).Trim();
                    wsConnections[ws] = username;
                    Console.WriteLine($"👤 User {username} connected");
                    return;
                }

                if (!wsConnections.ContainsKey(ws))
                {
                    ws.Send("{\"error\": \"Bạn chưa đặt username.\"}");
                    return;
                }

                string sender = wsConnections[ws];

                if (sender == "admin")
                {
                    var jsonData = JsonConvert.DeserializeObject<Dictionary<string, string>>(message);
                    if (jsonData.ContainsKey("receiver") && jsonData.ContainsKey("text"))
                    {
                        string receiver = jsonData["receiver"];
                        string text = jsonData["text"];

                        var userConnection = wsConnections.FirstOrDefault(c => c.Value == receiver).Key;
                        if (userConnection != null)
                        {
                            var jsonMessage = new
                            {
                                sender = "admin",
                                text = text
                            };
                            string jsonString = JsonConvert.SerializeObject(jsonMessage);
                            userConnection.Send(jsonString);
                        }
                        else
                        {
                            ws.Send(JsonConvert.SerializeObject(new { error = "User không tồn tại hoặc offline." }));
                        }
                    }
                }

                else
                {
                    var adminConnection = wsConnections.FirstOrDefault(c => c.Value == "admin").Key;
                    if (adminConnection != null)
                    {
                        var jsonResponse = new { sender, text = message };
                        adminConnection.Send(JsonConvert.SerializeObject(jsonResponse));
                        lastMessagedUsers[adminConnection] = ws;
                        Console.WriteLine($"📨 {sender} gửi tin nhắn đến admin: {message}");
                    }
                    else
                    {
                        ws.Send("{\"error\": \"Admin không online.\"}");
                    }
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
}*/

/*void StartWebSocketServer()
{
    var server = new WebSocketServer("ws://0.0.0.0:8181");
    var wsConnections = new Dictionary<IWebSocketConnection, string>();

    server.Start(ws =>
    {
        ws.OnOpen = () =>
        {
            Console.WriteLine("🟢 Client connected");
            wsConnections[ws] = "Guest";
        };

        ws.OnClose = () =>
        {
            if (wsConnections.TryGetValue(ws, out string username))
            {
                Console.WriteLine($"🔴 Client {username} disconnected");
                wsConnections.Remove(ws);
            }
        };

        ws.OnMessage = message =>
        {
            try
            {
                if (message.StartsWith("USERNAME:"))
                {
                    string username = message.Substring(9).Trim();
                    wsConnections[ws] = username;
                    Console.WriteLine($"👤 User {username} connected");
                    return;
                }

                if (!wsConnections.ContainsKey(ws))
                {
                    ws.Send("{\"error\": \"Bạn chưa đặt username.\"}");
                    return;
                }

                string sender = wsConnections[ws];

                // 👉 Kiểm tra JSON hợp lệ
                var jsonData = JsonConvert.DeserializeObject<Dictionary<string, string>>(message);
                if (!jsonData.ContainsKey("receiver") || !jsonData.ContainsKey("text"))
                {
                    ws.Send("{\"error\": \"Dữ liệu tin nhắn không hợp lệ.\"}");
                    return;
                }

                string receiver = jsonData["receiver"];
                string text = jsonData["text"];

                // 👉 Kiểm tra xem người nhận có tồn tại không
                var receiverConnection = wsConnections.FirstOrDefault(c => c.Value == receiver).Key;
                if (receiverConnection == null)
                {
                    ws.Send(JsonConvert.SerializeObject(new { error = "Người nhận không tồn tại hoặc offline." }));
                    return;
                }

                // 👉 Gửi tin nhắn đi
                var jsonMessage = new
                {
                    sender = sender,
                    text = text
                };
                string jsonString = JsonConvert.SerializeObject(jsonMessage);
                receiverConnection.Send(jsonString);
                Console.WriteLine($"📨 {sender} gửi tin nhắn đến {receiver}: {text}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Lỗi xử lý tin nhắn: {ex.Message}");
                ws.Send("{\"error\": \"Lỗi xử lý tin nhắn.\"}");
            }
        };
    });

    Console.WriteLine("🚀 WebSocket server started on ws://0.0.0.0:8181");
}*/
void StartWebSocketServer()
{
    var server = new WebSocketServer("ws://0.0.0.0:8181");
    var wsConnections = new Dictionary<IWebSocketConnection, string>();
    int guestCounter = 1;  // Đếm số lượng Guest

    server.Start(ws =>
    {
        ws.OnOpen = () =>
        {
            if (!wsConnections.ContainsKey(ws))
            {
                wsConnections[ws] = "Guest_" + guestCounter++;
            }
            Console.WriteLine($"🟢 Client kết nối: {wsConnections[ws]}");
        };

        ws.OnClose = () =>
        {
            if (wsConnections.TryGetValue(ws, out string username))
            {
                Console.WriteLine($"🔴 Client {username} đã ngắt kết nối");
                wsConnections.Remove(ws);
            }
        };

        ws.OnMessage = message =>
        {
            try
            {
                // Xử lý khi user đặt tên
                if (message.StartsWith("USERNAME:"))
                {
                    string username = message.Substring(9).Trim();

                    if (!string.IsNullOrEmpty(username) && !wsConnections.ContainsValue(username))
                    {
                        wsConnections[ws] = username;
                        Console.WriteLine($"👤 User {username} đã kết nối");
                    }
                    else
                    {
                        ws.Send("{\"error\": \"Username đã tồn tại hoặc không hợp lệ.\"}");
                    }
                    return;
                }

                // Kiểm tra user có username hợp lệ không
                if (!wsConnections.ContainsKey(ws))
                {
                    ws.Send("{\"error\": \"Bạn chưa đặt username. Vui lòng gửi 'USERNAME:TênCủaBạn' trước.\"}");
                    return;
                }

                string sender = wsConnections[ws];
                var jsonData = JsonConvert.DeserializeObject<Dictionary<string, string>>(message);

                if (!jsonData.ContainsKey("receiver") || !jsonData.ContainsKey("text"))
                {
                    ws.Send("{\"error\": \"Dữ liệu tin nhắn không hợp lệ.\"}");
                    return;
                }

                string receiver = jsonData["receiver"];
                string text = jsonData["text"];

                // Tìm kết nối của người nhận
                var receiverConnection = wsConnections.FirstOrDefault(c => c.Value == receiver).Key;
                if (receiverConnection == null)
                {
                    ws.Send(JsonConvert.SerializeObject(new { error = "Người nhận không tồn tại hoặc offline." }));
                    return;
                }

                // Gửi tin nhắn đến người nhận
                var jsonMessage = new { sender = sender, text = text };
                receiverConnection.Send(JsonConvert.SerializeObject(jsonMessage));
                Console.WriteLine($"📨 {sender} gửi tin nhắn đến {receiver}: {text}");
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


