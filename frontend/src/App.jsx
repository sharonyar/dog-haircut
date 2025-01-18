import React from "react";
import { Routes, Route } from "react-router-dom"; // ✅ Do NOT import BrowserRouter here
import LoginPage from "./pages/LoginPage";
import DashboardPage from "./pages/DashboardPage";
import ProtectedRoute from "./components/ProtectedRoute";

function App() {
  return (
    <Routes>  {/* ✅ Only Routes here, no extra BrowserRouter */}
      <Route path="/" element={<LoginPage />} />
      <Route path="/dashboard" element={
        <ProtectedRoute>
          <DashboardPage />
        </ProtectedRoute>
      } />
    </Routes>
  );
}

export default App;
