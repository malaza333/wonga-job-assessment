import { Navigate, Outlet, useLocation } from "react-router-dom";

const getToken = () => localStorage.getItem("token"); // or sessionStorage

export default function ProtectedRoute() {
  const token = getToken();
  const location = useLocation();

  // No token => kick to login
  if (!token) {
    return <Navigate to="/login" replace state={{ from: location }} />;
  }

  return <Outlet />;
}
