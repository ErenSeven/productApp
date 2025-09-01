"use client";

import { useState, FormEvent } from "react";
import { useDispatch } from "react-redux";
import { useRouter } from "next/navigation";
import { login } from "@/store/userSlice";
import { AppDispatch } from "@/store";
import { useTranslations } from 'next-intl';
import { decodeJWT } from "@/utils/jwt";
import { dispatchCrossAppEvent } from "@/utils/events"; // 🔹 ekledik

export default function LoginPage() {
  const dispatch = useDispatch<AppDispatch>();
  const router = useRouter();
  const t = useTranslations('LoginPage');

  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState<string | null>(null);

  const handleSubmit = async (e: FormEvent) => {
    e.preventDefault();
    setError(null);

    try {
      const res = await fetch(
        `${process.env.NEXT_PUBLIC_API_BASE_URL}/api/Auth/login`,
        {
          method: "POST",
          headers: { "Content-Type": "application/json" },
          body: JSON.stringify({ Email: email, Password: password }),
        }
      );

      if (!res.ok) {
        const text = await res.text();
        throw new Error(text || "Giriş başarısız");
      }

      const data = await res.json(); // { token }

      // token'ı localStorage'a kaydet
      localStorage.setItem("token", `Bearer ${data.token}`);
      const payload = decodeJWT(data.token);
      const name = payload?.username || "User";

      // 🔹 Redux'a normal dispatch (sadece bu app için)
      dispatch(login({ name, token: `Bearer ${data.token}` }));

      // 🔹 Cart app'e de event fırlat
      dispatchCrossAppEvent("user-event", {
        type: "login",
        payload: { name, token: `Bearer ${data.token}` },
      });

      router.push("/");
    } catch (err: any) {
      setError(err.message ?? "Bilinmeyen hata");
    }
  };

  return (
    <main className="max-w-md mx-auto mt-20 p-6 bg-gray-100 rounded-xl">
      <h1 className="text-2xl font-bold mb-6 text-gray-500">{t("title")}</h1>
      {error && <p className="text-red-600 mb-4">{error}</p>}
      <form onSubmit={handleSubmit} className="space-y-4">
        <input
          type="email"
          placeholder={t("emailPlaceholder")}
          value={email}
          onChange={(e) => setEmail(e.target.value)}
          className="w-full px-3 py-2 rounded border text-gray-800"
          required
        />
        <input
          type="password"
          placeholder={t("passwordPlaceholder")}
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          className="w-full px-3 py-2 rounded border text-gray-800"
          required
        />
        <button
          type="submit"
          className="w-full bg-blue-600 text-white py-2 rounded hover:bg-blue-700 transition"
        >
          {t("submitButton")}
        </button>
      </form>
    </main>
  );
}
