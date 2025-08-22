import Link from "next/link";

export default function Home() {
  return (
    <main className="mx-auto max-w-3xl px-4 py-20 text-center">
      <h1 className="text-5xl font-extrabold text-gray-200 leading-snug">
        Product Frontend
      </h1>
      <p className="mt-4 text-lg text-gray-200 max-w-xl mx-auto">
        Ürünleri görüntülemek veya yeni ürün eklemek için aşağıdaki sayfalara gidin.
      </p>
      <div className="mt-8 flex flex-wrap items-center justify-center gap-4">
        <Link
          href="/products"
          className="rounded-2xl bg-gradient-to-r from-blue-500 to-indigo-600 px-6 py-3 text-white font-semibold shadow-lg hover:from-blue-600 hover:to-indigo-700 transition"
        >
          Ürünler
        </Link>
        <Link
          href="/products/new"
          className="rounded-2xl border border-gray-300 px-6 py-3 text-gray-200 font-semibold hover:bg-gray-100 hover:text-gray-700 transition"
        >
          Yeni Ürün
        </Link>
      </div>
    </main>
  );
}
