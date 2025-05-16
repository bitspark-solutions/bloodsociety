"use client";
import { useState } from "react";
import { sendNotification } from "../utils/notificationApi";
import { Button, TextField, Paper, Typography, CircularProgress, Alert } from "@mui/material";

export default function NotificationForm() {
  const [message, setMessage] = useState("");
  const [status, setStatus] = useState<string | null>(null);
  const [loading, setLoading] = useState(false);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    setStatus(null);
    try {
      const response = await sendNotification(message);
      if (response.ok) {
        setStatus("Notification sent successfully.");
        setMessage("");
      } else {
        setStatus("Failed to send notification.");
      }
    } catch (error) {
      setStatus("Error sending notification.");
    } finally {
      setLoading(false);
    }
  };

  return (
    <Paper component="form" onSubmit={handleSubmit} sx={{ p: 3, maxWidth: 400, mx: "auto", mt: 4, display: "flex", flexDirection: "column", gap: 2 }} elevation={3}>
      <Typography variant="h6" fontWeight="bold">Send Notification</Typography>
      <TextField
        multiline
        minRows={4}
        value={message}
        onChange={e => setMessage(e.target.value)}
        placeholder="Enter notification message"
        required
        variant="outlined"
        fullWidth
      />
      <Button
        type="submit"
        variant="contained"
        color="primary"
        disabled={loading || !message.trim()}
        sx={{ alignSelf: "flex-end", minWidth: 160 }}
      >
        {loading ? <><CircularProgress size={20} sx={{ mr: 1 }} /> Sending...</> : "Send Notification"}
      </Button>
      {status && <Alert severity={status.includes("success") ? "success" : "error"} sx={{ mt: 1 }}>{status}</Alert>}
    </Paper>
  );
}