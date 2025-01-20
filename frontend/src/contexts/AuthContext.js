import React, { createContext, useState, useContext } from "react";
import axios from "axios";

const AuthContext = createContext();

export function AuthProvider({ children }) {
  const [token, setToken] = useState(localStorage.getItem("token") || null);

  const login = async (username, password) => {
    try {
      const response = await axios.post("http://localhost:5000/api/auth/login", {
        username,
        password,
      });

      console.log("Login Response:", response.data);

      const userToken = response.data?.token || response.data?.Token;
      if (userToken) {
        setToken(userToken);
        localStorage.setItem("token", userToken);
        return true; // ✅ Return success
      } else {
        throw new Error("Invalid response from server");
      }
    } catch (error) {
      console.error("Login failed:", error.response?.data || error.message);
      throw new Error(error.response?.data || "Invalid username or password");
    }
  };

  return (
    <AuthContext.Provider value={{ token, login }}>
      {children}
    </AuthContext.Provider>
  );
}

// ✅ Ensure `useAuth()` is exported properly
export function useAuth() {
  return useContext(AuthContext);
}
