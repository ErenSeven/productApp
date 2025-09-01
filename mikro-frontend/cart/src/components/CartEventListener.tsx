"use client";

import { useEffect } from "react";
import { useDispatch } from "react-redux";
import { addToCart } from "@/store/cartSlice";
import { AppDispatch } from "@/store";

export default function CartEventListener() {
  const dispatch = useDispatch<AppDispatch>();

  useEffect(() => {
    const handler = (event: Event) => {
      const customEvent = event as CustomEvent<{
        type: string;
        payload?: any;
      }>;

      if (customEvent.detail.type === "addToCart") {
        dispatch(addToCart(customEvent.detail.payload));
      }
    };

    window.addEventListener("cart-event", handler);

    return () => {
      window.removeEventListener("cart-event", handler);
    };
  }, [dispatch]);

  return null;
}
