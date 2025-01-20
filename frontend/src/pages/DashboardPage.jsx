import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";

function DashboardPage() {
  const [customers, setCustomers] = useState([]);
  const [searchName, setSearchName] = useState("");
  const [sortByTime, setSortByTime] = useState("none");
  const token = localStorage.getItem("token");
  const loggedInUserId = parseInt(localStorage.getItem("userId"));
  const navigate = useNavigate();

  useEffect(() => {
    fetch("http://localhost:5000/api/customers", {
      headers: {
        Authorization: `Bearer ${token}`,
        "Content-Type": "application/json",
      },
    })
      .then((response) => {
        if (!response.ok) throw new Error("Failed to fetch customers");
        return response.json();
      })
      .then((data) => {
        console.log("Fetched Customers:", data); // ✅ Debugging log
        setCustomers(data);
      })
      .catch((error) => console.error("Error fetching customers:", error));
  }, []);

  // ✅ Step 1: Filter customers by name
  const filteredCustomers = customers.filter(
    (customer) =>
      customer.name &&
      customer.name.toLowerCase().includes(searchName.toLowerCase())
  );

  // ✅ Step 2: Sort customers by appointment time
  const sortedCustomers = [...filteredCustomers].sort((a, b) => {
    if (!a.appointmentTime || !b.appointmentTime) return 0;

    const timeA = new Date(`1970-01-01T${a.appointmentTime}:00`).getTime();
    const timeB = new Date(`1970-01-01T${b.appointmentTime}:00`).getTime();

    if (sortByTime === "asc") return timeA - timeB;
    if (sortByTime === "desc") return timeB - timeA;
    return 0;
  });

  console.log("Sorted Customers:", sortedCustomers); // ✅ Debugging log

  return (
    <div>
      <h2>🐶 Dog Haircut Schedule</h2>

      {/* ✅ Filter Inputs */}
      <div className="filters">
        <input
          type="text"
          placeholder="Search by name..."
          value={searchName}
          onChange={(e) => setSearchName(e.target.value)}
          className="filter-input"
        />
        <select
          value={sortByTime}
          onChange={(e) => setSortByTime(e.target.value)}
          className="filter-select"
        >
          <option value="none">Sort by Time</option>
          <option value="asc">Earliest First</option>
          <option value="desc">Latest First</option>
        </select>
      </div>

      <table>
        <thead>
          <tr>
            <th>Name</th>
            <th>Appointment Time</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          {sortedCustomers.length > 0 ? (
            sortedCustomers.map((customer) => (
              <tr key={customer.id}>
                <td>{customer.name}</td>
                <td>
                  {customer.appointmentTime && customer.appointmentTime.trim() !== "" 
                    ? new Date(`1970-01-01T${customer.appointmentTime}:00`).toLocaleTimeString() 
                    : "No Appointment"}
                </td>
                <td>
                  {customer.id === loggedInUserId ? (
                    <>
                      <button
                        onClick={() => navigate("/profile")}
                        className="edit-button"
                      >
                        ✏️ Edit
                      </button>
                      <button className="delete-button">🗑️ Delete</button>
                    </>
                  ) : (
                    <span>🔒 Restricted</span>
                  )}
                </td>
              </tr>
            ))
          ) : (
            <tr>
              <td colSpan="3">No customers found.</td>
            </tr>
          )}
        </tbody>
      </table>
    </div>
  );
}

export default DashboardPage;
