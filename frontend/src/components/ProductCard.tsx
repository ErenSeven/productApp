import { Product } from "@/types/product";

export default function ProductCard({ p }: { p: Product }) {
  return (
    <div className="rounded-xl bg-white p-5 shadow hover:shadow-lg transition">
      <h2 className="text-xl font-semibold text-gray-900">{p.name}</h2>
      <p className="mt-2 text-gray-700">{p.description}</p>
      <div className="mt-4 flex items-center justify-between text-gray-800 font-medium">
        <span>Stok: {p.stock}</span>
        <span>₺{p.price.toLocaleString()}</span>
      </div>
    </div>
  );
}
