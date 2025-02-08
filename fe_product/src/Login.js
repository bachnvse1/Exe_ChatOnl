import React, { useState } from "react";
import axios from "axios";
import "./css/login.css";
import { useNavigate } from "react-router-dom";

const Login = () => {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [email, setEmail] = useState("");
  const [isRegistering, setIsRegistering] = useState(false);
  const [error, setError] = useState(null);
  const navigate = useNavigate();

  const handleLogin = async (e) => {
    e.preventDefault();
    setError(null);

    try {
      const response = await axios.post(
        "https://localhost:5001/api/Users/login",
        {
          username,
          password,
        }
      );

      if (response.data?.success) {
        const userInfo = response.data;

        // Lưu username và isAdmin vào sessionStorage
        sessionStorage.setItem("username", userInfo.username);
        sessionStorage.setItem("isAdmin", userInfo.isAdmin ? "true" : "false"); // Đảm bảo lưu đúng kiểu dữ liệu

        console.log("Đăng nhập thành công:", userInfo.username);
        navigate("/home");
      } else {
        alert(response.data?.message || "Đăng nhập thất bại!");
      }
    } catch (error) {
      setError(
        error.response?.data?.message || "Lỗi đăng nhập. Vui lòng thử lại nhé!"
      );
    }
  };

  const handleRegister = async (e) => {
    e.preventDefault();
    setError(null);

    try {
      const response = await axios.post(
        "https://localhost:5001/api/Users/register",
        {
          username,
          email,
          password,
        }
      );

      if (response.data.flag === true) {
        alert("Registration successful. Please log in.");
        setIsRegistering(false);
      } else {
        alert(response.data.message);
      }
    } catch (error) {
      setError(error.response?.data?.message || "Registration failed");
    }
  };

  return (
    <div className="login-container">
      <div className="login-box">
        {isRegistering ? (
          <>
            <h2>Sign Up</h2>
            {error && <p className="error-text">{error}</p>}
            <form onSubmit={handleRegister}>
              <input
                type="text"
                className="login-input"
                placeholder="Username"
                value={username}
                onChange={(e) => setUsername(e.target.value)}
                required
              />
              <input
                type="email"
                className="login-input"
                placeholder="Email"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
                required
              />
              <input
                type="password"
                className="login-input"
                placeholder="Password"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                required
              />
              <button type="submit" className="login-button">
                Sign Up
              </button>
            </form>
            <p>
              Already have an account?{" "}
              <span
                className="toggle-form"
                onClick={() => setIsRegistering(false)}
              >
                Login
              </span>
            </p>
          </>
        ) : (
          <>
            <h2>Login</h2>
            {error && <p className="error-text">{error}</p>}
            <form onSubmit={handleLogin}>
              <input
                type="text"
                className="login-input"
                placeholder="Username"
                value={username}
                onChange={(e) => setUsername(e.target.value)}
                required
              />
              <input
                type="password"
                className="login-input"
                placeholder="Password"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                required
              />
              <button type="submit" className="login-button">
                Login
              </button>
            </form>
            <a href="#" className="forgot-password">
              Forgot Password?
            </a>
            <p>
              Don't have an account?{" "}
              <span
                className="toggle-form"
                onClick={() => setIsRegistering(true)}
              >
                Sign Up
              </span>
            </p>
          </>
        )}
      </div>
    </div>
  );
};

export default Login;
