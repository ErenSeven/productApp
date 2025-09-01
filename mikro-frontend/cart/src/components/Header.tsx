"use client";

import Link from "next/link";
import { useSelector, useDispatch } from "react-redux";
import { RootState, AppDispatch } from "@/store";
import { logout, rehydrate } from "@/store/userSlice";
import { useEffect } from "react";
import { decodeJWT } from "@/utils/jwt";
import {useTranslations} from 'next-intl';

export default function Header() {
  const dispatch = useDispatch<AppDispatch>();
  const user = useSelector((state: RootState) => state.user);
  const cart = useSelector((state: RootState) => state.cart);
  const t = useTranslations('Header');
  const handleLogout = () => {
    localStorage.removeItem("token");
    dispatch(logout());
  };

  useEffect(() => {
    const token = localStorage.getItem("token");
    if (token) {
      const payload = decodeJWT(token);
      const name = payload?.username || "User";
      dispatch(rehydrate({ name, token }));
    }
  }, [dispatch]);

  return (
    <header className="flex justify-between items-center p-6 bg-gray-800 text-white">
      <div className="flex items-center justify-between max-w-7xl w-full mx-auto">
        <a href="/" className="text-2xl font-bold">
          LOGO
        </a>

        <div className="flex items-center gap-4">
          {!user.isLoggedIn ? (
            <>
              <a href="/login" className="hover:underline">
                {t("login")}
              </a>
              <a href="/register" className="hover:underline">
                {t("register")}
              </a>
            </>
          ) : (
            <>
              <span>Hello, {user.name}</span>
              <button
                onClick={handleLogout}
                className="bg-red-600 px-3 py-1 rounded hover:bg-red-700 transition"
              >
                {t("logout")}
              </button>
              <a
                href="/cart"
                className="bg-blue-600 px-3 py-1 rounded hover:bg-blue-700 transition"
              >
                {t("cart", { count: cart.items.length })}
              </a>
            </>
          )}
        </div>
      </div>
    </header>
  );
}