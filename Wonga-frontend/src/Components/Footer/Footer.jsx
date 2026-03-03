import React from "react";
import { Box, Container, Typography, Link, Stack } from "@mui/material";

const Footer = () => {
  return (
    <Box
      component="footer"
      sx={{
        backgroundColor: "#111827",
        color: "#fff",
        mt: 8,
        py: 4,
      }}
    >
      <Container maxWidth="lg">
        {/* Divider */}
        <Box
          sx={{
            textAlign: "center",
          }}
        >
          <Typography variant="body2">
            © {new Date().getFullYear()} Wonga Finance. All rights reserved.
          </Typography>
        </Box>
      </Container>
    </Box>
  );
};

export default Footer;
