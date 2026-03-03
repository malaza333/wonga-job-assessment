import { useEffect, useMemo, useState } from "react";
import {
  Box,
  Paper,
  Typography,
  Stack,
  TextField,
  InputAdornment,
  Table,
  TableHead,
  TableRow,
  TableCell,
  TableBody,
  Chip,
  Divider,
  Alert,
  CircularProgress,
} from "@mui/material";
import SearchIcon from "@mui/icons-material/Search";
import { getAudit } from "../services/auditService";

export default function AuditLogs() {
  const [rows, setRows] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");
  const [query, setQuery] = useState("");

  useEffect(() => {
    const fetchAudit = async () => {
      setLoading(true);
      setError("");

      try {
        const data = await getAudit();

        // Map API response to UI model
        const mapped = data.map((x) => ({
          id: x.id,
          eventType: x.eventType,
          email: x.email,
          createdAt: new Date(x.createdAtUtc).toLocaleString(),
        }));

        setRows(mapped);
      } catch (err) {
        const msg =
          err?.response?.data?.message || "Failed to load audit logs.";
        setError(msg);
      } finally {
        setLoading(false);
      }
    };

    fetchAudit();
  }, []);

  const filtered = useMemo(() => {
    const q = query.trim().toLowerCase();
    if (!q) return rows;

    return rows.filter(
      (r) =>
        r.eventType.toLowerCase().includes(q) ||
        r.email.toLowerCase().includes(q) ||
        r.createdAt.toLowerCase().includes(q)
    );
  }, [query, rows]);

  return (
    <Box
      sx={{ width: "100%", p: { xs: 2, sm: 3 }, maxWidth: 1000, mx: "auto" }}
    >
      <Paper
        sx={{
          p: { xs: 2.5, sm: 3 },
          borderRadius: 3,
          border: "1px solid",
          borderColor: "divider",
          boxShadow: "0 10px 30px rgba(0,0,0,0.06)",
        }}
      >
        <Stack
          direction={{ xs: "column", sm: "row" }}
          spacing={2}
          alignItems={{ xs: "flex-start", sm: "center" }}
          justifyContent="space-between"
        >
          <Box>
            <Typography variant="h5" fontWeight={800}>
              Audit Logs
            </Typography>
            <Typography variant="body2" color="text.secondary">
              Track system activity.
            </Typography>
          </Box>

          <TextField
            value={query}
            onChange={(e) => setQuery(e.target.value)}
            placeholder="Search logs..."
            size="small"
            sx={{ width: { xs: "100%", sm: 320 } }}
            disabled={loading}
            InputProps={{
              startAdornment: (
                <InputAdornment position="start">
                  <SearchIcon fontSize="small" />
                </InputAdornment>
              ),
            }}
          />
        </Stack>

        <Divider sx={{ my: 3 }} />

        {error && <Alert severity="error">{error}</Alert>}

        {loading ? (
          <Box sx={{ py: 6, display: "flex", justifyContent: "center" }}>
            <CircularProgress />
          </Box>
        ) : (
          <Table size="small">
            <TableHead>
              <TableRow>
                <TableCell sx={{ fontWeight: 800 }}>Date</TableCell>
                <TableCell sx={{ fontWeight: 800 }}>Event</TableCell>
                <TableCell sx={{ fontWeight: 800 }}>Email</TableCell>
              </TableRow>
            </TableHead>

            <TableBody>
              {filtered.length === 0 ? (
                <TableRow>
                  <TableCell colSpan={3}>
                    <Typography variant="body2" color="text.secondary">
                      No audit logs found.
                    </Typography>
                  </TableCell>
                </TableRow>
              ) : (
                filtered.map((row) => (
                  <TableRow key={row.id} hover>
                    <TableCell>{row.createdAt}</TableCell>
                    <TableCell>
                      <Chip
                        size="small"
                        label={row.eventType}
                        variant="outlined"
                      />
                    </TableCell>
                    <TableCell>{row.email}</TableCell>
                  </TableRow>
                ))
              )}
            </TableBody>
          </Table>
        )}
      </Paper>
    </Box>
  );
}
