"use client";

import Link from "next/link";
import { useState, FormEvent } from "react";
import { useRouter } from "next/navigation";

type NewProduct = {
  name: string;
  price: number | "";
  stock: number | "";
  description?: string;
};

export default function NewProductPage() {
  const router = useRouter();
  const [form, setForm] = useState<NewProduct>({
    name: "",
    price: "",
    stock: "",
    description: "",
  });
  const [submitting, setSubmitting] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const onSubmit = async (e: FormEvent) => {
    e.preventDefault();
    setError(null);

    // Basit doğrulama
    if (!form.name || form.price === "" || form.stock === "") {
      setError("Lütfen zorunlu alanları doldurun.");
      return;
    }

    setSubmitting(true);
    try {
      const res = await fetch("/api/products", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({
          name: form.name,
          price: Number(form.price),
          stock: Number(form.stock),
          description: form.description || undefined,
        }),
      });

      if (!res.ok) {
        const text = await res.text();
        throw new Error(text || `İstek başarısız: ${res.status}`);
      }

      // Başarılı -> listeye dön
      router.push("/products");
      router.refresh();
    } catch (err: any) {
      setError(err.message ?? "Bilinmeyen hata");
    } finally {
      setSubmitting(false);
    }
  };

  return (
    <main className="mx-auto max-w-xl px-6 py-10 bg-gray-100 rounded-xl mt-10">
      <header className="mb-8 flex items-center justify-between">
        <h1 className="text-3xl font-bold tracking-tight text-gray-900">Yeni Ürün Ekle</h1>
        <Link
          href="/products"
          className="rounded-2xl bg-gray-500 px-5 py-2 text-white hover:bg-gray-700 transition"
        >
          Ürünleri Görüntüle
        </Link>
      </header>
      <form className="space-y-5">
        <div>
          <label className="block text-lg font-medium text-gray-700">Ürün Adı *</label>
          <input className="mt-1 w-full rounded-xl border px-3 py-2 text-gray-900" />
        </div>
        <div className="grid grid-cols-1 gap-4 sm:grid-cols-2">
          <div>
            <label className="block text-lg font-medium text-gray-700">Fiyat (₺) *</label>
            <input className="mt-1 w-full rounded-xl border px-3 py-2 text-gray-900" />
          </div>
          <div>
            <label className="block text-lg font-medium text-gray-700">Stok *</label>
            <input className="mt-1 w-full rounded-xl border px-3 py-2 text-gray-900" />
          </div>
        </div>
        <div>
          <label className="block text-lg font-medium text-gray-700">Açıklama</label>
          <textarea className="mt-1 w-full rounded-xl border px-3 py-2 text-gray-900" rows={4} />
        </div>
        <div className="flex gap-4">
          <button className="rounded-xl bg-blue-600 px-5 py-2 text-white hover:bg-blue-700 transition cursor-pointer">Kaydet</button>
          <button className="rounded-xl border px-5 py-2 text-gray-800 hover:bg-gray-200 transition cursor-pointer">İptal</button>
        </div>
      </form>
    </main>
  );
}
