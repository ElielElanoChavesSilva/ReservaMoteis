export interface GetUserDTO {
  id?: string;
  name?: string;
  email?: string;
  profileId?: number;
  password?: string;
}

export interface CreateUserDTO {
  name: string;
  email: string;
  profileId: number;
  password: string;
}
