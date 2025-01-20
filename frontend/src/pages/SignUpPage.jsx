import React, { useState } from "react";
import { useNavigate, Link } from "react-router-dom"; // ✅ Import Link
import "../styles/AuthPage.css";
import addUserIcon from "../assets/add-user-male.png"; // ✅ Import the local image

function SignUpPage() {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [firstname, setFirstname] = useState("");
  const [errorMessage, setErrorMessage] = useState(""); // ✅ Store error message
  const navigate = useNavigate();

  const handleSignUp = async (e) => {
    e.preventDefault();
    setErrorMessage(""); // ✅ Clear previous errors

    console.log("Attempting to sign up with:", { username, firstname, password });

    try {
      const response = await fetch("http://localhost:5000/api/auth/register", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ username, name: firstname, password }), // ✅ Match backend model
      });

      if (!response.ok) {
        const errorData = await response.text();
        throw new Error(errorData);
      }

      console.log("Signup successful");
      alert("Signup successful! You can now log in.");
      navigate("/"); // ✅ Redirect to login
    } catch (error) {
      console.error("Signup error:", error);
      setErrorMessage(error.message); // ✅ Display error in UI
    }
  };

  return (
    <div className="auth-page">
      <div className="auth-card">
        <div className="auth-icon">
          <img src={addUserIcon} alt="Add User Icon" />
        </div>
        <h2>Sign Up</h2>

        {/* ✅ Display error message */}
        {errorMessage && <p className="error-message">{errorMessage}</p>}

        <form onSubmit={handleSignUp} className="auth-form">
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
          Already have an account? <Link to="/">Login</Link> {/* ✅ Add login link */}
        </p>
      </div>
    </div>
  );
}

export default SignUpPage;
