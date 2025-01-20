import React, { useState } from "react";
import { useNavigate, Link } from "react-router-dom";
import "../styles/AuthPage.css"; // ✅ Use common styling
import userLoginIcon from "../assets/user-login.png"; // ✅ Import login icon

function LoginPage() {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [errorMessage, setErrorMessage] = useState(""); // ✅ Store error message
  const navigate = useNavigate();

  const handleLogin = async (e) => {
    e.preventDefault();
    setErrorMessage(""); // ✅ Clear previous errors

    try {
      const response = await fetch("http://localhost:5000/api/auth/login", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ username, password }),
      });

      // ✅ Handle non-JSON responses (like plain text "Invalid username or password")
      const text = await response.text();
      let data;
      try {
        data = JSON.parse(text);
      } catch (error) {
        data = { message: text }; // If response is not JSON, assume plain text
      }

      if (!response.ok) {
        throw new Error(data.message || "Login failed.");
      }

      // ✅ Store token and navigate if login is successful
      if (data.token) {
        localStorage.setItem("token", data.token);
        localStorage.setItem("userId", data.userId);
        navigate("/dashboard");
      }
    } catch (error) {
      console.error("Error logging in:", error);
      setErrorMessage(error.message); // ✅ Display error in UI
    }
  };

  return (
    <div className="auth-page">
      <div className="auth-card">
        <div className="auth-icon">
          <img src={userLoginIcon} alt="User Login" />
        </div>
        <h2>Login</h2>

{/* ✅ Display error message with class */}
{errorMessage && <p className="error-message">{errorMessage}</p>}

        <form onSubmit={handleLogin} className="auth-form">
          <input
            type="text"
            placeholder="Username"
            value={username}
            onChange={(e) => setUsername(e.target.value)}
            required
            className="auth-input"
          />
          <input
            type="password"
            placeholder="Password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            required
            className="auth-input"
          />
          <button type="submit" className="auth-button">Login</button>
        </form>

        <p className="auth-switch">
          Don't have an account? <Link to="/signup">Sign Up</Link>
        </p>
      </div>
    </div>
  );
}

export default LoginPage;
