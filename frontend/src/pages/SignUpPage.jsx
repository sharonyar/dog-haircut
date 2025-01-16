import React, { useState } from "react";
import { useNavigate, Link } from "react-router-dom";  // ✅ Import Link
import axios from "axios";
import "../styles/AuthPage.css";
import addUserIcon from "../assets/add-user-male.png";  // ✅ Import the local image

function SignUpPage() {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [firstname, setFirstname] = useState("");
  const navigate = useNavigate();
  

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      await axios.post("http://localhost:5000/api/auth/signup", {
        username,
        password,
        firstname,
      });
      alert("Signup successful! You can now log in.");
      navigate("/"); // ✅ Redirect to login after successful signup
    } catch (error) {
      alert("Signup failed: " + (error.response?.data?.message || error.message));
    }
  };

  return (
    <div className="auth-page">
      <div className="auth-card">
        <div className="auth-icon">
        <img src={addUserIcon} alt="Add User Icon" />
        </div>
        <h2>Sign Up</h2>
        <form onSubmit={handleSubmit} className="auth-form">
          <input
            type="text"
            placeholder="Username *"
            value={username}
            onChange={(e) => setUsername(e.target.value)}
            required
            className="auth-input"
          />
          <input
            type="text"
            placeholder="Firstname *"
            value={firstname}
            onChange={(e) => setFirstname(e.target.value)}
            required
            className="auth-input"
          />
          <input
            type="password"
            placeholder="Password *"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            required
            className="auth-input"
          />
          <button type="submit" className="auth-button">Sign Up</button>
        </form>
        <p className="auth-switch">
          Already have an account? <Link to="/">Login</Link>  {/* ✅ Add login link */}
        </p>
      </div>
    </div>
  );
}

export default SignUpPage;
