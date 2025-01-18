import React from "react";
import { useAuth } from "../contexts/AuthContext";
import "../styles/AuthPage.css";
import userLoginIcon from "../assets/user-login.png";  // ✅ Import logo correctly

function DashboardPage() {
  const { logout } = useAuth();

  const customers = [
    { id: 1, name: "Alice Johnson" },
    { id: 2, name: "Bob Smith" },
    { id: 3, name: "Charlie Davis" },
    { id: 4, name: "Daisy Brown" },
  ];

  return (
    <div className="auth-page">
      <div className="auth-card">
        <div className="dashboard-icon">
          <img src={userLoginIcon} alt="Dashboard Icon" />
        </div>
        <h2>Customer Waiting List</h2>
        <ul className="customer-list">
          {customers.map((customer) => (
            <li key={customer.id} className="customer-item">
              {customer.name}
            </li>
          ))}
        </ul>
        <button onClick={logout} className="auth-button">Logout</button>
      </div>
    </div>
  );
}

export default DashboardPage;
