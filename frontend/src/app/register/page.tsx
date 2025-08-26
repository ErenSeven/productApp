"use client";

import { useState, FormEvent } from "react";
import { useRouter } from "next/navigation";
import {useTranslations} from 'next-intl';

export default function RegisterPage() {
    const router = useRouter();
    const t = useTranslations('RegisterPage');
    const [name, setName] = useState("");   
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [error, setError] = useState<string | null>(null);

    const handleSubmit = async (e: FormEvent) => {
        e.preventDefault();
        setError(null);

        try {
        const res = await fetch(`${process.env.NEXT_PUBLIC_API_BASE_URL}/api/Auth/register`, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ UserName: name,Email: email,Password: password}),
        });

        if (!res.ok) {
            const text = await res.text();
            throw new Error(text || "Kayıt başarısız");
        }

        router.push("/login"); // kayıt sonrası login sayfasına yönlendir
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
                type="text"
                placeholder={t("namePlaceholder")}
                value={name}
                onChange={e => setName(e.target.value)}
                className="w-full px-3 py-2 rounded border text-gray-800"
                required
                />
                <input
                type="email"
                placeholder={t("emailPlaceholder")}
                value={email}
                onChange={e => setEmail(e.target.value)}
                className="w-full px-3 py-2 rounded border text-gray-800"
                required
                />
                <input
                type="password"
                placeholder={t("passwordPlaceholder")}
                value={password}
                onChange={e => setPassword(e.target.value)}
                className="w-full px-3 py-2 rounded border text-gray-800"
                required 
                />
                <button
                type="submit"
                className="w-full bg-green-600 text-white py-2 rounded hover:bg-green-700 transition"
                >
                {t("submitButton")}
                </button>
            </form>
        </main>
    );
}