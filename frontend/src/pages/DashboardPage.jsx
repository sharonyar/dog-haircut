import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";

function DashboardPage() {
  const [customers, setCustomers] = useState([]);
  const token = localStorage.getItem("token");
  const loggedInUserId = parseInt(localStorage.getItem("userId")); // ✅ Get logged-in user ID
  const navigate = useNavigate();

  useEffect(() => {
    fetch("http://localhost:5000/api/customers", {
      headers: {
        "Authorization": `Bearer ${token}`,
        "Content-Type": "application/json"
      }
    })
    .then(response => {
      if (!response.ok) throw new Error("Failed to fetch customers");
      return response.json();
    })
    .then(data => setCustomers(data))
    .catch(error => console.error("Error fetching customers:", error));
  }, []);

  return (
    <div>
      <h2>Customer List (Waiting for Dog Haircut)</h2>
      <ul>
        {customers.length > 0 ? (
          customers.map(customer => (
            <li key={customer.id}>
              {customer.name}
              {/* ✅ Show Edit Profile button only for logged-in user */}
              {customer.id === loggedInUserId && (
                <button onClick={() => navigate("/profile")}>Edit Profile</button>
              )}
            </li>
          ))
        ) : (
          <p>No customers found.</p>
        )}
      </ul>
    </div>
  );
}

export default DashboardPage;
