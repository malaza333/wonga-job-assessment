import { useState } from "react";
import {
  Box,
  Typography,
  TextField,
  Stack,
  Divider,
  InputAdornment,
  IconButton,
  Alert,
  Link as MuiLink,
} from "@mui/material";
import LoadingButton from "@mui/lab/LoadingButton";
import { Link as RouterLink, useNavigate, useLocation } from "react-router-dom";
import Visibility from "@mui/icons-material/Visibility";
import VisibilityOff from "@mui/icons-material/VisibilityOff";
import AuthCard from "../Components/Auth/AuthCard";
import { login } from "../services/authService";

export default function Login() {
  const navigate = useNavigate();
  const location = useLocation();

  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");

  const [showPassword, setShowPassword] = useState(false);
  const [error, setError] = useState("");
  const [loading, setLoading] = useState(false); // ✅ add this

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError("");

    if (!email.trim() || !password.trim()) {
      setError("Please enter your email and password.");
      return;
    }

    setLoading(true); // ✅ start loading
    try {
      const response = await login({ email: email.trim(), password });
      console.log("response--------from Login", response);

      // If your login returns token, store it (only if not already done in authService)
      // if (response?.token) localStorage.setItem("token", response.token);

      const from = location.state?.from?.pathname || "/";
      navigate(from, { replace: true });
    } catch (err) {
      console.log("login error:", err);

      const msg =
        err?.response?.data?.message ||
        err?.response?.data ||
        "Login failed. Please check your details and try again.";
      setError(
        typeof msg === "string"
          ? msg
          : "Login failed. Please check your details and try again."
      );
    } finally {
      setLoading(false); // ✅ stop loading
    }
  };

  return (
    <AuthCard>
      <Stack spacing={2.5}>
        <Box>
          <Typography
            variant="h4"
            fontWeight={800}
            sx={{ letterSpacing: -0.5 }}
          >
            Welcome back
          </Typography>
          <Typography variant="body2" color="text.secondary" sx={{ mt: 0.5 }}>
            Sign in to continue to your profile.
          </Typography>
        </Box>

        {error ? <Alert severity="error">{error}</Alert> : null}

        <Box component="form" onSubmit={handleSubmit}>
          <Stack spacing={2}>
            <TextField
              label="Email"
              type="email"
              autoComplete="email"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              fullWidth
              disabled={loading}
            />

            <TextField
              label="Password"
              type={showPassword ? "text" : "password"}
              autoComplete="current-password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              fullWidth
              disabled={loading}
              InputProps={{
                endAdornment: (
                  <InputAdornment position="end">
                    <IconButton
                      aria-label="toggle password visibility"
                      onClick={() => setShowPassword((s) => !s)}
                      edge="end"
                      disabled={loading}
                    >
                      {showPassword ? <VisibilityOff /> : <Visibility />}
                    </IconButton>
                  </InputAdornment>
                ),
              }}
            />

            <LoadingButton
              type="submit"
              variant="contained"
              size="large"
              loading={loading}
              loadingPosition="center"
              sx={{ py: 1.2, borderRadius: 2 }}
            >
              Sign in
            </LoadingButton>

            <Divider />

            <Typography
              variant="body2"
              color="text.secondary"
              textAlign="center"
            >
              Don’t have an account?{" "}
              <MuiLink component={RouterLink} to="/register" underline="hover">
                Create one
              </MuiLink>
            </Typography>
          </Stack>
        </Box>
      </Stack>
    </AuthCard>
  );
}
