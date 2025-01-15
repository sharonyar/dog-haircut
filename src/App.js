import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import LoginPage from './components/LoginPage';
// import DashboardPage from './pages/DashboardPage';

function App() {
  return (
    <Router>
      <Routes>
        {/* Public Route */}
        <Route path="/" element={<LoginPage />} />

        {/* Private Route Example (to protect dashboard) */}
        {/* <Route path="/dashboard" element={<DashboardPage />} /> */}
      </Routes>
    </Router>
  );
}

export default App;
