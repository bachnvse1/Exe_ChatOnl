import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import Login from "./Authen/Login";
import Home from "./Home/home";
import Shop from "./Home/shop";
import OTP from "./Authen/otp";

function App() {
  return (
    <Router>
      <Routes>
        <Route path="/" element={<Login />} />
        <Route path="/home" element={<Home />} />
        <Route path="/shop" element={<Shop />} />
        <Route path="/otp" element={<OTP />} />
      </Routes>
    </Router>
  );
}

export default App;
