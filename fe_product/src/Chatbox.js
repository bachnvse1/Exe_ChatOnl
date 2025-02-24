// import React, { useState, useEffect, useRef } from "react";
// import chatIcon from "./images/chat-icon.png";
// import "./css/chatbox.css";

// const Chatbox = () => {
//   const [isOpen, setIsOpen] = useState(false);
//   const [messages, setMessages] = useState([]);
//   const [inputs, setInputs] = useState({});
//   const [chatWindows, setChatWindows] = useState({});
//   const ws = useRef(null);
//   // Lấy username từ sessionStorage
//   const savedUsername = sessionStorage.getItem("username");
//   const isAdmin = sessionStorage.getItem("isAdmin") === "true";

//   const handleInputChange = (receiver, value) => {
//     setInputs((prevInputs) => ({
//       ...prevInputs,
//       [receiver]: value, // Lưu tin nhắn theo user
//     }));
//   };

//   const username = savedUsername
//     ? savedUsername // Nếu đã đăng nhập, lấy username từ sessionStorage
//     : isAdmin
//     ? "admin" // Nếu là admin nhưng sessionStorage trống, đặt là admin
//     : "guest_" + Math.random().toString(36).substring(7); // Nếu chưa đăng nhập, tạo guest

//   // Đảm bảo lưu lại vào sessionStorage nếu chưa có
//   if (!savedUsername) {
//     sessionStorage.setItem("username", username);
//   }
//   const [openChats, setOpenChats] = useState([]); // Danh sách cửa sổ chat mở

//   useEffect(() => {
//     const connectWebSocket = () => {
//       if (!ws.current || ws.current.readyState === WebSocket.CLOSED) {
//         ws.current = new WebSocket("ws://127.0.0.1:8181");

//         ws.current.onopen = () => {
//           console.log(`✅ [${username}] WebSocket đã kết nối!`);
//           ws.current.send(`USERNAME:${username}`); // Gửi username ngay khi kết nối
//         };

//         ws.current.onmessage = (event) => {
//           try {
//             const receivedMessage = JSON.parse(event.data);
//             console.log("📩 Tin nhắn nhận được:", receivedMessage);

//             // Cập nhật tin nhắn nhận được
//             setMessages((prev) => [...prev, receivedMessage]);

//             // Nếu tin nhắn đến từ admin, hiển thị đúng user
//             if (receivedMessage.sender === "admin") {
//               setChatWindows((prev) => ({
//                 ...prev,
//                 admin: [...(prev["admin"] || []), receivedMessage],
//               }));
//             } else {
//               // Nếu tin nhắn đến từ user, admin cập nhật vào cửa sổ chat của user đó
//               setChatWindows((prev) => ({
//                 ...prev,
//                 [receivedMessage.sender]: [
//                   ...(prev[receivedMessage.sender] || []),
//                   receivedMessage,
//                 ],
//               }));

//               // Nếu chưa có cửa sổ chat với user này, thêm vào danh sách
//               setOpenChats((prev) =>
//                 prev.includes(receivedMessage.sender)
//                   ? prev
//                   : [...prev, receivedMessage.sender]
//               );
//             }
//           } catch (error) {
//             console.error("❌ Lỗi khi xử lý tin nhắn:", error);
//           }
//         };

//         ws.current.onerror = (error) =>
//           console.error("❌ WebSocket lỗi:", error);
//         ws.current.onclose = () => {
//           console.log("🔴 WebSocket đóng, thử kết nối lại...");
//           setTimeout(connectWebSocket, 3000);
//         };
//       }
//     };

//     connectWebSocket();

//     // Chỉ đóng WebSocket khi unmount toàn bộ trang (không phải khi đóng chatbox)
//     return () => {
//       if (!isAdmin) {
//         ws.current?.close();
//       }
//     };
//   }, [isAdmin, username]);

//   const closeChat = (user) => {
//     setOpenChats((prev) => prev.filter((u) => u !== user));
//   };

//   const sendMessage = (receiver) => {
//     if (inputs.trim() && ws.current?.readyState === WebSocket.OPEN) {
//       const messageData = {
//         text: inputs,
//         sender: username, // Đảm bảo sender là username
//         receiver: isAdmin ? receiver : "admin",
//       };

//       console.log("📤 Gửi tin nhắn:", messageData); // Kiểm tra log

//       // Cập nhật state để hiển thị tin nhắn ngay lập tức
//       if (isAdmin) {
//         setChatWindows((prev) => ({
//           ...prev,
//           [receiver]: [...(prev[receiver] || []), messageData],
//         }));
//       } else {
//         setMessages((prev) => [...prev, messageData]);
//         setChatWindows((prev) => ({
//           ...prev,
//           admin: [...(prev["admin"] || []), messageData],
//         }));
//       }

//       ws.current.send(JSON.stringify(messageData)); // Gửi tin nhắn qua WebSocket
//       // Reset input chỉ cho người đang chat
//       setInputs((prevInputs) => ({
//         ...prevInputs,
//         [receiver]: "", // Đặt lại input của user đó thành rỗng
//       }));
//     }
//   };

//   return (
//     <div className="chat-container">
//       {!isAdmin && !isOpen && (
//         <button onClick={() => setIsOpen(true)} className="chat-icon">
//           <img src={chatIcon} alt="Chat" />
//         </button>
//       )}

//       {!isAdmin && isOpen && (
//         <div className="chatbox">
//           <div className="chat-header">
//             <span>Chat với quản lý</span>
//             <button onClick={() => setIsOpen(false)}>✖</button>
//           </div>
//           <div className="chat-body">
//             {messages.map((msg, index) => (
//               <div
//                 key={index}
//                 className={`message ${
//                   msg.sender === username ? "user" : "admin"
//                 }`}
//               >
//                 {msg.text}
//               </div>
//             ))}
//           </div>
//           <div className="chat-footer">
//             <input
//               type="text"
//               value={inputs.text}
//               onChange={(e) => handleInputChange(user, e.target.value)}
//               onKeyDown={(e) => e.key === "Enter" && sendMessage()}
//               placeholder="Nhập tin nhắn..."
//             />
//             <button onClick={sendMessage}>Gửi</button>
//           </div>
//         </div>
//       )}

//       {/* Hiển thị danh sách chat nếu là Admin */}
//       {isAdmin && (
//         <div className="chat-windows">
//           {openChats.map((user) => (
//             <div key={user} className="chatbox user-chat">
//               <div className="chat-header">
//                 <span>Chat với {user}</span>
//                 <button onClick={() => closeChat(user)}>✖</button>
//               </div>
//               <div className="chat-body">
//                 {chatWindows[user]?.map((msg, index) => (
//                   <div
//                     key={index}
//                     className={`message ${
//                       msg.sender === "admin" ? "admin" : "user"
//                     }`}
//                   >
//                     {msg.text}
//                   </div>
//                 ))}
//               </div>
//               <div className="chat-footer">
//                 <input
//                   type="text"
//                   value={inputs.text}
//                   onChange={(e) => setInputs(e.target.value)}
//                   onKeyDown={(e) => e.key === "Enter" && sendMessage(user)}
//                   placeholder="Nhập tin nhắn..."
//                 />
//                 <button onClick={() => sendMessage(user)}>Gửi</button>
//               </div>
//             </div>
//           ))}
//         </div>
//       )}
//     </div>
//   );
// };

// export default Chatbox;
import React, { useState, useEffect, useRef } from "react";
import chatIcon from "./images/chat-icon.png";
import "./css/chatbox.css";

const Chatbox = () => {
  const [isOpen, setIsOpen] = useState(false);
  const [messages, setMessages] = useState([]);
  const [inputs, setInputs] = useState({}); // Dùng object để lưu tin nhắn của từng user
  const [chatWindows, setChatWindows] = useState({});
  const [openChats, setOpenChats] = useState([]);
  const ws = useRef(null);
  const handlePaste = (e, receiver) => {
    const items = e.clipboardData.items;
    for (let i = 0; i < items.length; i++) {
      if (items[i].type.indexOf("image") === 0) {
        const blob = items[i].getAsFile();
        const reader = new FileReader();

        reader.onloadend = () => {
          const imageUrl = reader.result;
          setInputs((prevInputs) => ({
            ...prevInputs,
            [receiver]:
              prevInputs[receiver] + `<img src="${imageUrl}" alt="image" />`, // Thêm ảnh vào nội dung tin nhắn
          }));
        };

        reader.readAsDataURL(blob);
        break; // Chỉ xử lý một ảnh đầu tiên
      }
    }
  };
  const savedUsername = sessionStorage.getItem("username");
  const isAdmin = sessionStorage.getItem("isAdmin") === "true";

  const username = savedUsername
    ? savedUsername
    : isAdmin
    ? "admin"
    : "guest_" + Math.random().toString(36).substring(7);

  if (!savedUsername) {
    sessionStorage.setItem("username", username);
  }

  useEffect(() => {
    const connectWebSocket = () => {
      if (!ws.current || ws.current.readyState === WebSocket.CLOSED) {
        ws.current = new WebSocket("ws://192.168.101.105:8181");
        ws.current.onopen = () => {
          console.log(`✅ [${username}] WebSocket đã kết nối!`);
          ws.current.send(`USERNAME:${username}`);
        };

        ws.current.onmessage = (event) => {
          try {
            const receivedMessage = JSON.parse(event.data);
            console.log("📩 Tin nhắn nhận được:", receivedMessage);

            setMessages((prev) => [...prev, receivedMessage]);

            if (receivedMessage.sender === "admin") {
              setChatWindows((prev) => ({
                ...prev,
                admin: [...(prev["admin"] || []), receivedMessage],
              }));
            } else {
              setChatWindows((prev) => ({
                ...prev,
                [receivedMessage.sender]: [
                  ...(prev[receivedMessage.sender] || []),
                  receivedMessage,
                ],
              }));

              setOpenChats((prev) =>
                prev.includes(receivedMessage.sender)
                  ? prev
                  : [...prev, receivedMessage.sender]
              );
            }
          } catch (error) {
            console.error("❌ Lỗi khi xử lý tin nhắn:", error);
          }
        };

        ws.current.onerror = (error) =>
          console.error("❌ WebSocket lỗi:", error);
        ws.current.onclose = () => {
          console.log("🔴 WebSocket đóng, thử kết nối lại...");
          setTimeout(connectWebSocket, 3000);
        };
      }
    };

    connectWebSocket();

    return () => {
      if (!isAdmin) {
        ws.current?.close();
      }
    };
  }, [isAdmin, username]);

  const closeChat = (user) => {
    setOpenChats((prev) => prev.filter((u) => u !== user));
  };

  const handleInputChange = (receiver, value) => {
    setInputs((prevInputs) => ({
      ...prevInputs,
      [receiver]: value, // Đảm bảo mỗi user có input riêng
    }));
  };

  const sendMessage = (receiver) => {
    if (inputs[receiver]?.trim() && ws.current?.readyState === WebSocket.OPEN) {
      const messageData = {
        text: inputs[receiver].trim(),
        sender: username,
        receiver: isAdmin ? receiver : "admin",
      };

      console.log("📤 Gửi tin nhắn:", messageData);

      if (isAdmin) {
        setChatWindows((prev) => ({
          ...prev,
          [receiver]: [...(prev[receiver] || []), messageData],
        }));
      } else {
        setMessages((prev) => [...prev, messageData]);
        setChatWindows((prev) => ({
          ...prev,
          admin: [...(prev["admin"] || []), messageData],
        }));
      }

      ws.current.send(JSON.stringify(messageData));

      setInputs((prevInputs) => ({
        ...prevInputs,
        [receiver]: "", // Reset input cho user cụ thể
      }));
    }
  };

  return (
    <div className="chat-container">
      {!isAdmin && !isOpen && (
        <button onClick={() => setIsOpen(true)} className="chat-icon">
          <img src={chatIcon} alt="Chat" />
        </button>
      )}

      {!isAdmin && isOpen && (
        <div className="chatbox">
          <div className="chat-header">
            <span>Chat với quản lý</span>
            <button onClick={() => setIsOpen(false)}>✖</button>
          </div>
          <div className="chat-body">
            {messages.map((msg, index) => (
              <div
                key={index}
                className={`message ${
                  msg.sender === username ? "user" : "admin"
                }`}
                dangerouslySetInnerHTML={{ __html: msg.text }} // Hiển thị ảnh từ HTML
              />
            ))}
          </div>
          <div className="chat-footer">
            <input
              type="text"
              value={inputs["admin"] || ""}
              onChange={(e) => handleInputChange("admin", e.target.value)}
              onPaste={(e) => handlePaste(e, "admin")} // Thêm sự kiện dán vào
              onKeyDown={(e) => e.key === "Enter" && sendMessage("admin")}
              placeholder="Nhập tin nhắn..."
            />
            <button onClick={() => sendMessage("admin")}>Gửi</button>
          </div>
        </div>
      )}

      {isAdmin && (
        <div className="chat-windows">
          {openChats.map((user) => (
            <div key={user} className="chatbox user-chat">
              <div className="chat-header">
                <span>Chat với {user}</span>
                <button onClick={() => closeChat(user)}>✖</button>
              </div>
              <div className="chat-body">
                {chatWindows[user]?.map((msg, index) => (
                  <div
                    key={index}
                    className={`message ${
                      msg.sender === "admin" ? "admin" : "user"
                    }`}
                    dangerouslySetInnerHTML={{ __html: msg.text }} // Hiển thị ảnh từ HTML
                  />
                ))}
              </div>
              <div className="chat-footer">
                <input
                  type="text"
                  value={inputs[user] || ""}
                  onChange={(e) => handleInputChange(user, e.target.value)}
                  onPaste={(e) => handlePaste(e, user)} // Thêm sự kiện dán vào
                  onKeyDown={(e) => e.key === "Enter" && sendMessage(user)}
                  placeholder="Nhập tin nhắn..."
                />
                <button onClick={() => sendMessage(user)}>Gửi</button>
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  );
};

export default Chatbox;
