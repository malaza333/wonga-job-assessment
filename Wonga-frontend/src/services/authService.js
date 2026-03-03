import apiClient from "./apiClient";

export const login = async (data) => {
  const response = await apiClient.post(`/auth/login`, data);
  const token = response.data.token;
  localStorage.setItem("token", token);
  return response.data;
};

export const register = async (data) => {
  const response = await apiClient.post(`/auth/register`, data);
  console.log("response...........register", response);
  return response.data;
};