import React, { createContext, useState, useContext } from "react";
import axios from "axios";

const AuthContext = createContext();

export function AuthProvider({ children }) {
  const [token, setToken] = useState(localStorage.getItem("token") || null);

  // Login function
  const login = async (username, password) => {
    try {
      const response = await axios.post("http://localhost:5000/api/auth/login", {
        username,
        password,
      });

      console.log("Login Response:", response.data);

      const userToken = response.data?.Token || response.data?.token;
      if (userToken) {
        setToken(userToken);
        localStorage.setItem("token", userToken);
        return true; // ✅ Success
      } else {
        throw new Error("No token received");
      }
    } catch (error) {
      console.error("Login failed:", error.response?.data || error.message);
      throw new Error(error.response?.data || "Invalid username or password"); // ✅ Return error
    }
  };

  // Logout function
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

// ✅ Export useAuth to access auth state
export function useAuth() {
  return useContext(AuthContext);
}
