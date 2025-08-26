import { createSlice, PayloadAction } from "@reduxjs/toolkit";

interface UserState {
  isLoggedIn: boolean;
  name?: string;
  token?: string;
}

const initialState: UserState = {
  isLoggedIn: false,
  name: undefined,
  token: undefined,
};

export const userSlice = createSlice({
  name: "user",
  initialState,
  reducers: {
    login: (state, action: PayloadAction<{ name: string; token: string }>) => {
      state.isLoggedIn = true;
      state.name = action.payload.name;
      state.token = action.payload.token;
    },
    logout: (state) => {
      state.isLoggedIn = false;
      state.name = undefined;
      state.token = undefined;
    },
    rehydrate: (state, action: PayloadAction<{ name: string; token: string }>) => {
      state.isLoggedIn = true;
      state.name = action.payload.name;
      state.token = action.payload.token;
    },
  },
});

export const { login, logout, rehydrate } = userSlice.actions;
export default userSlice.reducer;
