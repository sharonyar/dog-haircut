import React, { useEffect, useState } from "react";

function DashboardPage() {
  const [customers, setCustomers] = useState([]);

  useEffect(() => {
    fetch("http://localhost:5000/api/customers")
      .then(response => response.json())
      .then(data => setCustomers(data))
      .catch(error => console.error("Error fetching customers:", error));
  }, []);

  return (
    <div>
      <h2>Customer List</h2>
      <ul>
        {customers.map((customer) => (
          <li key={customer.id}>{customer.name}</li>
        ))}
      </ul>
    </div>
  );
}

export default DashboardPage;
