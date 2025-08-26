import { Product } from "@/types/product";
import Image from "next/image";
import AddToCartButton from "@/components/AddToCartButton";
import { notFound } from "next/navigation";
import {getTranslations} from 'next-intl/server';

interface ProductPageProps {
  params: { id: string };
}

export const revalidate = 60;

async function getProduct(id: string): Promise<Product | null> {
  const res = await fetch(`${process.env.NEXT_PUBLIC_API_BASE_URL}/api/Product/${id}`, { next: { revalidate: 60 } });
  if (!res.ok) return null;
  return res.json();
}

export default async function ProductPage({ params }: { params: { id: string } }) {
  const { id } = await Promise.resolve(params);
  const product = await getProduct(id);
  const t = await getTranslations('ProductPage');

  if (!product) notFound();

  return (
    <main className="max-w-4xl mx-auto px-6 py-10 space-y-6">
      <h1 className="text-4xl font-bold">{product.name}</h1>

      <Image
        src={product.imageUrl}
        width={400}
        height={400}
        alt={product.name}
        className="rounded-lg shadow-md"
      />

      <div className="flex flex-col sm:flex-row sm:justify-between sm:items-center gap-4">
        <p className="text-2xl font-semibold text-gray-800">
          {new Intl.NumberFormat('tr-TR', { style: 'currency', currency: 'TRY' }).format(product.price)}
        </p>
        <AddToCartButton product={product} />
      </div>

      <div className="space-y-2">
        <p><span className="font-semibold">{t("category")}:</span> {product.category}</p>
        <p><span className="font-semibold">{t("stock")}:</span> {product.stock}</p>
      </div>

      <div className="mt-4">
        <h2 className="text-xl font-bold mb-2">{t("descriptionTitle")}</h2>
        <p>{product.description}</p>
      </div>
    </main>
  );
}

export async function generateMetadata({ params }: { params: { id: string } }) {
  // params.id'yi await ile alıyoruz
  const { id } = await Promise.resolve(params);
  const product = await getProduct(id);
  if (!product) return { title: "Ürün bulunamadı" };

  return {
    title: `${product.name} | Ürün Detayı`,
    description: product.description,
    keywords: `${product.name}, ${product.category}, alışveriş, ürün`,
    authors: [{ name: "ECommerce" }],
    openGraph: {
      title: product.name,
      description: product.description,
      images: [product.imageUrl],
      type: "website", // "product" yerine geçerli değer
    },
    twitter: {
      card: "summary_large_image",
      title: product.name,
      description: product.description,
      images: [product.imageUrl],
    },
  };
}