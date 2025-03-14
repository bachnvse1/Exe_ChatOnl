import React from "react";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import { ToastContainer } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";

import Login from "./Authen/Login";
import Home from "./A_Home/home";
import Shop from "./A_Home/shop";
import Shop1 from "./A_Home/Shop1";
import OTP from "./Authen/otp";
import ProfilePage from "./Account/profile";
import Cart from "./B_Product/cart";
import Dashboard from "./Admin/adminDashboard";

function App() {
  return (
    <Router>
      <ToastContainer />{" "}
      {/* Đặt ở đây để hiển thị toast trên toàn bộ ứng dụng */}
      <Routes>
        <Route path="/" element={<Login />} />
        <Route path="/home" element={<Home />} />
        <Route path="/shop" element={<Shop />} />
        <Route path="/shop1" element={<Shop1 />} />
        <Route path="/otp" element={<OTP />} />
        <Route path="/profile" element={<ProfilePage />} />
        <Route path="/cart" element={<Cart />} />
        <Route path="/dashboard" element={<Dashboard />} />
      </Routes>
    </Router>
  );
}

export default App;
