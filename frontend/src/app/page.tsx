import ProductCard from "@/components/ProductCard";
import { Product } from "@/types/product";
import Link from "next/link";
import {getTranslations} from 'next-intl/server';

export const revalidate = 60;

async function getFilteredProducts(
  category?: string,
  minPrice?: number,
  maxPrice?: number,
  sort?: "asc" | "desc"
): Promise<Product[]> {
  const params = new URLSearchParams();
  if (category && category !== "all") params.append("category", category);
  if (minPrice !== undefined) params.append("minPrice", String(minPrice));
  if (maxPrice !== undefined) params.append("maxPrice", String(maxPrice));
  if (sort) {
    params.append("sortBy", "price");
    params.append("sortDirection", sort);
  }
  params.append("limit", "20");

  const res = await fetch(`${process.env.NEXT_PUBLIC_API_BASE_URL}/api/product?${params.toString()}`, {
    next: { revalidate: 60 },
  });

  if (!res.ok) return [];
  return res.json();
}

async function getCategories(): Promise<string[]> {
  const res = await fetch(`${process.env.NEXT_PUBLIC_API_BASE_URL}/api/product/categories`, {
    next: { revalidate: 60 },
  });
  if (!res.ok) return [];
  return res.json();
}

interface HomeProps {
  searchParams: {
    category?: string;
    minPrice?: string;
    maxPrice?: string;
    sort?: "asc" | "desc";
  };
}

export default async function Home({ searchParams }: HomeProps) {
  const params = await Promise.resolve(searchParams || {});
  const category = params.category;
  const minPrice = params.minPrice ? Number(params.minPrice) : undefined;
  const maxPrice = params.maxPrice ? Number(params.maxPrice) : undefined;
  const sort = params.sort;
  const t = await getTranslations('HomePage');

  const [products, categories] = await Promise.all([
    getFilteredProducts(category, minPrice, maxPrice, sort),
    getCategories(),
  ]);

  return (
    <main className="mx-auto max-w-6xl px-6 py-10">
      <div className="mb-10 text-center">
        <h1 className="text-3xl font-bold text-gray-800">{t('featuredProducts')}</h1>
        <p className="text-gray-600">{t('description')}</p>
      </div>

      <div className="mb-6 flex flex-wrap gap-4 justify-center items-center">
        <Link
          href="?category=all"
          className={`px-3 py-2 border rounded ${
            !category || category === "all" ? "bg-gray-200 font-bold text-gray-400" : "hover:bg-gray-100"
          }`}
        >
          {t('allCategories')}
        </Link>
        {categories.map((cat) => (
          <Link
            key={cat}
            href={`?category=${encodeURIComponent(cat)}`}
            className={`px-3 py-2 border rounded ${
              category === cat ? "bg-gray-200 font-bold" : "hover:bg-gray-100 text-gray-400"
            }`}
          >
            {cat}
          </Link>
        ))}
      </div>

      <form method="get" className="mb-8 flex flex-wrap gap-4 justify-center items-center">
        {category && category !== "all" && (
          <input type="hidden" name="category" value={category} />
        )}
        <input
          type="number"
          name="minPrice"
          placeholder="Min Fiyat"
          defaultValue={minPrice}
          className="px-3 py-2 border rounded w-28"
        />
        <input
          type="number"
          name="maxPrice"
          placeholder="Max Fiyat"
          defaultValue={maxPrice}
          className="px-3 py-2 border rounded w-28"
        />

        <select
          name="sort"
          defaultValue={sort || ""}
          className="px-3 py-2 border rounded text-gray-500 hover:text-gray-800"
        >
          <option value="">Sıralama Yok</option>
          <option value="asc">Fiyat (Artan)</option>
          <option value="desc">Fiyat (Azalan)</option>
        </select>

        <button
          type="submit"
          className="px-4 py-2 bg-blue-600 text-white rounded hover:bg-blue-700 transition"
        >
          {t('filter')}
        </button>
      </form>

      {products.length > 0 ? (
        <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6">
          {products.map((product) => (
            <div key={product.id} className="border rounded-xl p-4 hover:shadow-lg transition">
              <ProductCard product={product} />
            </div>
          ))}
        </div>
      ) : (
        <p className="text-center text-gray-500">{t('noProducts')}</p>
      )}
    </main>
  );
}