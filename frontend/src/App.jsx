import { Routes, Route, Navigate } from "react-router-dom";
import DashboardPage from "./pages/DashboardPage";
import ProfilePage from "./pages/ProfilePage";
import LoginPage from "./pages/LoginPage"
import SignUpPage from "./pages/SignUpPage";
import ProtectedRoute from "./components/ProtectedRoute"; // ✅ Import the authentication check

function App() {
  return (
    <Routes>
      {/* ✅ Default route should be login */}
      <Route path="/" element={<Navigate to="/login" />} />

      {/* ✅ Public routes */}
      <Route path="/login" element={<LoginPage />} />
      <Route path="/signup" element={<SignUpPage />} />

      {/* ✅ Protected routes (only logged-in users can access) */}
      <Route element={<ProtectedRoute />}>
        <Route path="/dashboard" element={<DashboardPage />} />
        <Route path="/profile" element={<ProfilePage />} />
      </Route>
    </Routes>
  );
}

export default App;
