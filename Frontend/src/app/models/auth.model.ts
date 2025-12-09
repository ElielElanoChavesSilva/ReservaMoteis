export interface Login {
  email?: string;
  password?: string;
}

export interface AuthResponse {
  token?: string;
  userId?: string;
  name?: string;
  email?: string;
  role?: string;
}
