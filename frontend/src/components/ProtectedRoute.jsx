import { Outlet, Navigate } from "react-router-dom";

function ProtectedRoute() {
  const token = localStorage.getItem("token"); // ✅ Check if user is logged in

  return token ? <Outlet /> : <Navigate to="/login" />; // ✅ Redirect if no token
}

export default ProtectedRoute;
