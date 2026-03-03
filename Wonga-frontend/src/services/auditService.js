import apiClient from "./apiClient";

export const getAudit = async () => {
  const response = await apiClient.get(`/auth/me/audit-logs?take=12`);
  return response.data;
};