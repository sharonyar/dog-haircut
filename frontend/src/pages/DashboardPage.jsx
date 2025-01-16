import React from "react";
import { useAuth } from "../contexts/AuthContext";

function DashboardPage() {
  const { logout } = useAuth();

  return (
    <div>
      <h1>Welcome to the Dashboard!</h1>
      <button onClick={logout}>Logout</button>
    </div>
  );
}

export default DashboardPage;
