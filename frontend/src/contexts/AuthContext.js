import React, { createContext, useState, useContext, useEffect } from "react";
import axios from "axios";

// Create authentication context
const AuthContext = createContext();

export function AuthProvider({ children }) {
  const [token, setToken] = useState(localStorage.getItem("token") || null);

  // Function to login and store token
  const login = async (username, password) => {
    try {
      const response = await axios.post("http://localhost:5000/api/auth/login", {
        username,
        password,
      });
      const userToken = response.data.Token;
      setToken(userToken);
      localStorage.setItem("token", userToken);
    } catch (error) {
      console.error("Login failed:", error.response?.data || error.message);
    }
  };

  // Function to logout
  const logout = () => {
    setToken(null);
    localStorage.removeItem("token");
  };

  return (
    <AuthContext.Provider value={{ token, login, logout }}>
      {children}
    </AuthContext.Provider>
  );
}

// Custom hook to use AuthContext
export function useAuth() {
  return useContext(AuthContext);
}
