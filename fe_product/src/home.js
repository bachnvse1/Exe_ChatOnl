import React, { useState, useEffect } from "react";

import Chatbox from "./Chatbox";
import "./css/style.css";
import "./css/bootstrap.min.css";
import "./css/tiny-slider.css";
import "@fortawesome/fontawesome-free/css/all.min.css";
import "bootstrap/dist/css/bootstrap.min.css";
import "bootstrap/dist/js/bootstrap.bundle.min";

// Import hình ảnh từ thư mục images
import logo from "./images/LogoGomXinh.jpg";
import product1 from "./images/product-1.png";
import product2 from "./images/product-2.png";
import product3 from "./images/product-3.png";
import config from "./config";
import couch from "./images/combogom2-removebg-preview.png";
import userIcon from "./images/user.svg";
import cartIcon from "./images/cart.svg";
import truckIcon from "./images/truck.svg";
import bagIcon from "./images/bag.svg";
import supportIcon from "./images/support.svg";
import returnIcon from "./images/return.svg";
import whyChooseUsImg from "./images/why-choose-us-img.jpg";
import blog1 from "./images/ComBo1.png";
import blog2 from "./images/blog2.jpg";
import blog3 from "./images/blog3.jpg";
import envelopeIcon from "./images/envelope-outline.svg";
import chatIcon from "./images/chat-icon.png";

const FixedBox = ({ content, onClose }) => (
  <div className="fixed-box-overlay">
    <div className="fixed-box">
      <div className="box-content">
        <button className="close-btn" onClick={onClose}>
          ✖
        </button>
        <p>{content}</p>
      </div>
    </div>
  </div>
);

const Home = () => {
  const [boxContent, setBoxContent] = useState("");
  const [showChat, setShowChat] = useState(false);

  // State để lưu danh sách sản phẩm
  const [products, setProducts] = useState([]);

  useEffect(() => {
    fetch(`${config.API_BASE_URL}/Products`, {
      headers: {
        "ngrok-skip-browser-warning": "true", // Bỏ qua cảnh báo ngrok
      },
    })
      .then((response) => response.json())
      .then((data) => setProducts(data))
      .catch((error) => console.error("Lỗi khi lấy sản phẩm:", error));
  }, []);

  return (
    <>
      {/* Navigation Bar */}
      <nav
        class="custom-navbar navbar navbar navbar-expand-md navbar-dark"
        arial-label="Furni navigation bar"
      >
        <div class="container">
          <a class="navbar-brand d-flex align-items-center" href="/home">
            <img src={logo} alt="Logo" class="logo" />
            <span class="brand-text">
              Gom Xinh<span>.</span>
            </span>
          </a>

          <button
            class="navbar-toggler"
            type="button"
            data-bs-toggle="collapse"
            data-bs-target="#navbarsFurni"
            aria-controls="navbarsFurni"
            aria-expanded="false"
            aria-label="Toggle navigation"
          >
            <span class="navbar-toggler-icon"></span>
          </button>

          <div class="collapse navbar-collapse" id="navbarsFurni">
            <ul class="custom-navbar-nav navbar-nav ms-auto mb-2 mb-md-0">
              <li class="nav-item ">
                <a class="nav-link" href="/home">
                  Trang chủ
                </a>
              </li>
              <li class="nav-item ">
                <a class="nav-link" href="/shop">
                  Cửa hàng
                </a>
              </li>
              <li class="nav-item ">
                <a class="nav-link" href="about.html">
                  Câu chuyện thương hiệu
                </a>
              </li>
              <li class="nav-item ">
                <a class="nav-link" href="services.html">
                  Dịch vụ
                </a>
              </li>
              <li>
                <a class="nav-link" href="blog.html">
                  Blog
                </a>
              </li>
              <li>
                <a class="nav-link" href="contact.html">
                  Liên hệ
                </a>
              </li>
            </ul>

            <ul class="custom-navbar-cta navbar-nav mb-2 mb-md-0 ms-5">
              <li>
                {" "}
                <a className="nav-link" href="#">
                  {" "}
                  <img src={userIcon} alt="User" />{" "}
                </a>{" "}
              </li>{" "}
              <li>
                {" "}
                <a className="nav-link" href="#">
                  {" "}
                  <img src={cartIcon} alt="Cart" />{" "}
                </a>{" "}
              </li>
            </ul>
          </div>
        </div>
      </nav>

      {/* Hero Section */}
      <section className="hero">
        <div className="container">
          <div className="row justify-content-between">
            <div className="col-lg-5">
              <h1 style={{ fontSize: "2rem" }}>
                Modern Interior Combined With Tradition{" "}
                <span className="d-block" style={{ fontSize: "1.8rem" }}>
                  Design Studio
                </span>
              </h1>

              <p>
                Mang nét đẹp văn hoá đồ gốm decor hiện đại tới với mọi người
              </p>
              <p>
                <a href="/shop" className="btn btn-secondary me-2">
                  Mua ngay
                </a>
                <a href="#" className="btn btn-outline-dark">
                  Explore
                </a>
              </p>
            </div>
            <div className="col-lg-7">
              <div className="hero-img-wrap">
                <img src={couch} className="img-fluid" alt="Hero Couch" />
              </div>
            </div>
          </div>
        </div>
      </section>

      {/* Why Choose Us Section */}
      <section className="why-choose-section text-center mt-5">
        <div className="container">
          <h2>Tại sao nên là chúng tôi</h2>
          <div className="row mt-4">
            <div className="col-md-3">
              <img src={truckIcon} alt="Fast Shipping" className="img-icon" />
              <h3>Vận chuyển nhanh gọn</h3>
            </div>
            <div className="col-md-3">
              <img src={bagIcon} alt="Easy Shopping" className="img-icon" />
              <h3>Dễ dàng mua sắm</h3>
            </div>
            <div className="col-md-3">
              <img src={supportIcon} alt="24/7 Support" className="img-icon" />
              <h3>24/7 Hỗ trợ</h3>
            </div>
            <div className="col-md-3">
              <img
                src={returnIcon}
                alt="Hassle Free Returns"
                className="img-icon"
              />
              <h3>Đổi trả dễ dàng</h3>
            </div>
          </div>
        </div>
      </section>

      {/* Blog Section */}
      <section className="blog-section container mt-5">
        <h2 className="text-center mb-4">Góc decor nổi bật</h2>
        <div className="row">
          <div className="col-md-4 d-flex flex-column align-items-center">
            <img src={blog3} className="img-fluid rounded-3" alt="Blog 3" />
          </div>
          <div className="col-md-4 d-flex flex-column align-items-center">
            <img src={blog1} className="img-fluid rounded-3" alt="Blog 1" />
          </div>
          <div className="col-md-4 d-flex flex-column align-items-center">
            <img src={blog2} className="img-fluid rounded-3" alt="Blog 2" />
          </div>
        </div>
      </section>

      {/* Danh sách combo */}
      <section className="container mt-5">
        <h2 className="text-center mb-4">Sản phẩm nổi bật</h2>
        <div className="row">
          {products.slice(0, 3).map((product) => (
            <div key={product.id} className="col-md-4 d-flex">
              <div className="card">
                <img
                  src={product.imageUrl}
                  className="card-img-top product-image"
                  alt={product.name}
                />
                <div className="card-body text-center">
                  <h5 className="card-title">{product.name}</h5>
                  <p className="card-text">{product.description}</p>
                  <p className="text-danger">
                    {product.price.toLocaleString("vi-VN")} VNĐ
                  </p>
                  <a
                    href={product.shopeeUrl}
                    target="_blank"
                    rel="noopener noreferrer"
                    className="btn btn-success"
                  >
                    Mua ngay
                  </a>
                </div>
              </div>
            </div>
          ))}
        </div>
      </section>

      {/* Footer */}
      <footer className="footer-section text-light text-center p-3 mt-5">
        <div className="container">
          <h3>Subscribe to Newsletter</h3>
          <form className="row g-3 justify-content-center">
            <div className="col-auto">
              <input
                type="text"
                className="form-control"
                placeholder="Enter your name"
              />
            </div>
            <div className="col-auto">
              <input
                type="email"
                className="form-control"
                placeholder="Enter your email"
              />
            </div>
            <div className="col-auto">
              <button className="btn btn-primary">
                <img src={envelopeIcon} alt="Subscribe" />
              </button>
            </div>
          </form>
          <p>&copy; {new Date().getFullYear()} Gom Xinh Potter</p>
        </div>
      </footer>

      <div className="home-container">
        <nav className="custom-navbar navbar navbar-expand-md navbar-dark bg-dark">
          <div className="container">
            <a className="navbar-brand" href="#">
              Gom Xinh Potter<span className="text-primary">.</span>
            </a>
            <button
              className="navbar-toggler"
              type="button"
              data-bs-toggle="collapse"
              data-bs-target="#navbarNav"
            >
              <span className="navbar-toggler-icon"></span>
            </button>
          </div>
        </nav>

        {boxContent && (
          <FixedBox content={boxContent} onClose={() => setBoxContent("")} />
        )}
        {showChat && <Chatbox />}

        <button className="chat-toggle" onClick={() => setShowChat(!showChat)}>
          <img src={chatIcon} alt="Chat" />
        </button>
      </div>
    </>
  );
};

export default Home;
