import React, { useState } from "react";
import Chatbox from "./Chatbox";
import "./css/style.css";
import "./css/bootstrap.min.css";

// Import hình ảnh từ thư mục images
import product1 from "./images/product-1.png";
import product2 from "./images/product-2.png";
import product3 from "./images/product-3.png";
import couch from "./images/couch.png";
import userIcon from "./images/user.svg";
import cartIcon from "./images/cart.svg";
import truckIcon from "./images/truck.svg";
import bagIcon from "./images/bag.svg";
import supportIcon from "./images/support.svg";
import returnIcon from "./images/return.svg";
import whyChooseUsImg from "./images/why-choose-us-img.jpg";
import blog1 from "./images/post-1.jpg";
import blog2 from "./images/post-2.jpg";
import blog3 from "./images/post-3.jpg";
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

  return (
    <div className="home-container">
      {/* Navigation Bar */}
      <nav className="custom-navbar navbar navbar-expand-md navbar-dark bg-dark">
        <div className="container">
          <a className="navbar-brand" href="index.html">
            Furni<span>.</span>
          </a>
          <button
            className="navbar-toggler"
            type="button"
            data-bs-toggle="collapse"
            data-bs-target="#navbarNav"
          >
            <span className="navbar-toggler-icon"></span>
          </button>
          <div className="collapse navbar-collapse" id="navbarNav">
            <ul className="custom-navbar-nav navbar-nav ms-auto">
              <li className="nav-item active">
                <a className="nav-link" href="index.html">
                  Home
                </a>
              </li>
              <li>
                <a className="nav-link" href="shop.html">
                  Shop
                </a>
              </li>
              <li>
                <a className="nav-link" href="about.html">
                  About us
                </a>
              </li>
              <li>
                <a className="nav-link" href="services.html">
                  Services
                </a>
              </li>
              <li>
                <a className="nav-link" href="blog.html">
                  Blog
                </a>
              </li>
              <li>
                <a className="nav-link" href="contact.html">
                  Contact us
                </a>
              </li>
            </ul>
            <ul className="custom-navbar-cta navbar-nav ms-5">
              <li>
                <a className="nav-link" href="#">
                  <img src={userIcon} alt="User" />
                </a>
              </li>
              <li>
                <a className="nav-link" href="#">
                  <img src={cartIcon} alt="Cart" />
                </a>
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
              <h1>
                Modern Interior <span className="d-block">Design Studio</span>
              </h1>
              <p>
                Donec vitae odio quis nisl dapibus malesuada. Nullam ac aliquet
                velit.
              </p>
              <p>
                <a href="#" className="btn btn-secondary me-2">
                  Shop Now
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
          <h2>Why Choose Us</h2>
          <div className="row mt-4">
            <div className="col-md-3">
              <img src={truckIcon} alt="Fast Shipping" className="img-icon" />
              <h3>Fast & Free Shipping</h3>
            </div>
            <div className="col-md-3">
              <img src={bagIcon} alt="Easy Shopping" className="img-icon" />
              <h3>Easy to Shop</h3>
            </div>
            <div className="col-md-3">
              <img src={supportIcon} alt="24/7 Support" className="img-icon" />
              <h3>24/7 Support</h3>
            </div>
            <div className="col-md-3">
              <img
                src={returnIcon}
                alt="Hassle Free Returns"
                className="img-icon"
              />
              <h3>Hassle Free Returns</h3>
            </div>
          </div>
        </div>
      </section>

      {/* Blog Section */}
      <section className="blog-section container mt-5">
        <h2 className="text-center mb-4">Recent Blog</h2>
        <div className="row">
          <div className="col-md-4">
            <img src={blog1} className="img-fluid" alt="Blog 1" />
            <h3>First Time Home Owner Ideas</h3>
          </div>
          <div className="col-md-4">
            <img src={blog2} className="img-fluid" alt="Blog 2" />
            <h3>How To Keep Your Furniture Clean</h3>
          </div>
          <div className="col-md-4">
            <img src={blog3} className="img-fluid" alt="Blog 3" />
            <h3>Small Space Furniture Apartment Ideas</h3>
          </div>
        </div>
      </section>

      {/* Footer */}
      <footer className="footer-section bg-dark text-light text-center p-3 mt-5">
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
          <p>&copy; {new Date().getFullYear()} Furni. All rights reserved.</p>
        </div>
      </footer>

      <div className="home-container">
        <nav className="custom-navbar navbar navbar-expand-md navbar-dark bg-dark">
          <div className="container">
            <a className="navbar-brand" href="#">
              Furni<span className="text-primary">.</span>
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
    </div>
  );
};

export default Home;
