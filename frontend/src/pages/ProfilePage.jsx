import React, { useState, useEffect } from "react";

function ProfilePage() {
  const [name, setName] = useState("");
  const token = localStorage.getItem("token");

  useEffect(() => {
    fetch("http://localhost:5000/api/customers/me", {
      headers: {
        "Authorization": `Bearer ${token}`
      }
    })
    .then(response => response.json())
    .then(data => setName(data.name || ""))
    .catch(error => console.error("Error fetching profile:", error));
  }, []);

  const handleUpdate = () => {
    fetch("http://localhost:5000/api/customers/me", {
      method: "PUT",
      headers: {
        "Authorization": `Bearer ${token}`,
        "Content-Type": "application/json"
      },
      body: JSON.stringify({ name })
    })
    .then(response => response.json())
    .then(data => alert(data.message)) // ✅ Show success message
    .catch(error => console.error("Error updating profile:", error));
  };

  return (
    <div>
      <h2>Edit Profile</h2>
      <label>
        Name: 
        <input 
          type="text" 
          value={name} 
          onChange={(e) => setName(e.target.value)}
        />
      </label>
      <button onClick={handleUpdate}>Save</button>
    </div>
  );
}

export default ProfilePage;
