"use client";
import { useEffect, useState } from "react";
import { getNotificationHistory } from "../utils/notificationApi";

export default function NotificationHistory() {
  const [history, setHistory] = useState<string[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    async function fetchHistory() {
      setLoading(true);
      setError(null);
      try {
        const response = await getNotificationHistory();
        if (response.ok) {
          const data = await response.json();
          // Adjust parsing based on actual backend response
          setHistory(Array.isArray(data) ? data : [data]);
        } else {
          setError("Failed to fetch notification history.");
        }
      } catch (err) {
        setError("Error fetching notification history.");
      } finally {
        setLoading(false);
      }
    }
    fetchHistory();
  }, []);

  if (loading) return <div>Loading notification history...</div>;
  if (error) return <div className="text-red-600">{error}</div>;

  return (
    <div className="max-w-md mx-auto p-4 border rounded bg-white shadow mt-6">
      <h2 className="text-lg font-bold mb-2">Notification History</h2>
      {history.length === 0 ? (
        <div>No notifications found.</div>
      ) : (
        <ul className="space-y-2">
          {history.map((item, idx) => (
            <li key={idx} className="border-b pb-2 text-sm">
              {typeof item === "string" ? item : JSON.stringify(item)}
            </li>
          ))}
        </ul>
      )}
    </div>
  );
}