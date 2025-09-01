
"use client";

import { useEffect } from "react";
import { useDispatch } from "react-redux";
import { rehydrate } from "@/store/cartSlice";
import { rehydrate as rehydrateUser } from "@/store/userSlice";

export default function CartInitializer() {
  const dispatch = useDispatch();

  useEffect(() => {
    const savedCart = localStorage.getItem("cart");
    if (savedCart) {
      dispatch(rehydrate(JSON.parse(savedCart)));
    }

    const savedUser = localStorage.getItem("user");
    if (savedUser) {
      dispatch(rehydrateUser(JSON.parse(savedUser)));
    }
  }, [dispatch]);

  return null;
}
