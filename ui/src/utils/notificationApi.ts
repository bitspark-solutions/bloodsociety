// Utility functions for interacting with the backend notification API

export async function sendNotification(message: string): Promise<Response> {
  const response = await fetch("/api/notification/send", {
    method: "POST",
    headers: {
      "Content-Type": "application/json"
    },
    body: JSON.stringify(message)
  });
  return response;
}

export async function getNotificationHistory(): Promise<Response> {
  const response = await fetch("/api/notification/history");
  return response;
}