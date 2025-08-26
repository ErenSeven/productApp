import Link from "next/link";
import ProductCard from "@/components/ProductCard";
import { Product } from "@/types/product";

export const revalidate = 0;

async function getProducts(): Promise<Product[]> {
  const res = await fetch(`${process.env.API_BASE_URL}/api/products`, { cache: "no-store" });
  if (!res.ok) throw new Error(`Ürünler alınamadı: ${res.status}`);
  const json = await res.json();
  return json.data ?? [];
}

export default async function ProductsPage() {
  const products = await getProducts();

  return (
    <main className="mx-auto max-w-6xl px-6 py-10">
      <header className="mb-8 flex items-center justify-between">
        <h1 className="text-3xl font-bold tracking-tight text-gray-200">Ürünler</h1>
        <Link
          href="/products/new"
          className="rounded-2xl bg-blue-600 px-5 py-2 text-white hover:bg-blue-700 transition"
        >
          Yeni Ürün Ekle
        </Link>
      </header>

      {products.length === 0 ? (
        <div className="rounded-xl border p-8 text-center text-gray-600 bg-white shadow">
          Henüz ürün yok. İlk ürünü eklemek için sağ üstteki butona tıklayın.
        </div>
      ) : (
        <div className="grid grid-cols-1 gap-6 sm:grid-cols-2 lg:grid-cols-3">
          {products.map((p) => (
            <ProductCard key={p.id} p={p} />
          ))}
        </div>
      )}
    </main>
  );
}
