import React, { useEffect, useState } from "react";
import Footer from "../Components/Footer/Footer";
import Nav from "../Components/Nav/Nav";
import { Outlet, useLocation } from "react-router-dom";

const Layout = () => {
  const location = useLocation();
  const [hideLogout, setHideLogout] = useState(false);
  useEffect(() => {
    setHideLogout(
      location.pathname === "/login" || location.pathname === "/register"
    );
  }, [location.pathname]);

  return (
    <React.Fragment>
      <Nav hideLogout={hideLogout} />
      <main>
        <Outlet />
      </main>
      <Footer />
    </React.Fragment>
  );
};

export default Layout;
