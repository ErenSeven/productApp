"use client";

import { useState, useEffect } from "react";
import { useRouter } from "next/navigation";
import { useTranslations } from "next-intl";

interface Product {
  id: string;
  name: string;
  description: string;
  price: number;
  category?: string;
  imageUrl?: string;
  stock?: number;
}

export default function AdminProductPage() {
  const t = useTranslations("AdminProductPage");
  const [products, setProducts] = useState<Product[]>([]);
  const [newProduct, setNewProduct] = useState({
    name: "",
    description: "",
    price: 0,
    category: "",
    imageUrl: "",
    stock: 0,
  });
  const [editingProduct, setEditingProduct] = useState<Product | null>(null);
  const [loading, setLoading] = useState(true);
  const [successMessage, setSuccessMessage] = useState("");
  
  const router = useRouter();
  const API_URL = `${process.env.NEXT_PUBLIC_API_BASE_URL}/api/Products`;
  const token = typeof window !== "undefined" ? localStorage.getItem("token") : null;

  const authHeader: Record<string, string> = token
    ? { Authorization: `${token}` }
    : {};

  useEffect(() => {
    if (!token) {
      router.push("/login");
    } else {
      const payloadBase64 = token.split(".")[1];
      try {
        const payload = JSON.parse(atob(payloadBase64));
        if (payload["userType"] !== "admin") {
          router.push("/");
        }
      } catch {
        router.push("/login");
      }
    }
  }, [token, router]);

  const fetchProducts = async () => {
    try {
      const res = await fetch(API_URL, { headers: authHeader });
      if (!res.ok) throw new Error("Ürünler alınamadı");
      const data = await res.json();
      setProducts(data);
    } catch (err) {
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  const handleAddProduct = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      const res = await fetch(API_URL, {
        method: "POST",
        headers: { "Content-Type": "application/json", ...authHeader },
        body: JSON.stringify(newProduct),
      });

      if (res.ok) {
        setNewProduct({ name: "", description: "", price: 0, category: "", imageUrl: "", stock: 0 });
        await fetchProducts();
        setSuccessMessage(t("product_added"));
        setTimeout(() => setSuccessMessage(""), 3000);
      }
    } catch (err) {
      console.error("Ürün eklenemedi", err);
    }
  };

  const handleUpdateProduct = async (id: string) => {
    if (!editingProduct) return;
    try {
      const res = await fetch(`${API_URL}/${id}`, {
        method: "PUT",
        headers: { "Content-Type": "application/json", ...authHeader },
        body: JSON.stringify(editingProduct),
      });
      const text = await res.text();
      if (!res.ok) console.error("PUT başarısız:", res.status, text);
      if (res.ok) {
        setEditingProduct(null);
        await fetchProducts();
      }
    } catch (err) {
      console.error("Güncelleme başarısız", err);
    }
  };

  const handleDeleteProduct = async (id: string) => {
    if (!confirm(t("delete") + "?")) return;
    try {
      const res = await fetch(`${API_URL}/${id}`, {
        method: "DELETE",
        headers: authHeader,
      });
      if (res.ok) {
        await fetchProducts();
      }
    } catch (err) {
      console.error("Silme başarısız", err);
    }
  };

  useEffect(() => {
    fetchProducts();
  }, []);

  return (
    <div className="max-w-6xl mx-auto p-4">
      <h1 className="text-2xl font-bold mb-4">{t("admin_product_page_title")}</h1>

      {successMessage && <p className="text-green-600 mb-2">{successMessage}</p>}

      {/* Yeni Ürün Ekleme */}
      <form onSubmit={handleAddProduct} className="mb-6 flex flex-wrap gap-4 items-center">
        <input
          type="text"
          placeholder={t("product_name")}
          value={newProduct.name}
          onChange={(e) => setNewProduct({ ...newProduct, name: e.target.value })}
          className="border px-3 py-1 rounded"
          required
        />
        <input
          type="text"
          placeholder={t("product_description")}
          value={newProduct.description}
          onChange={(e) => setNewProduct({ ...newProduct, description: e.target.value })}
          className="border px-3 py-1 rounded"
        />
        <input
          type="number"
          placeholder={t("product_price")}
          value={newProduct.price}
          onChange={(e) => setNewProduct({ ...newProduct, price: Number(e.target.value) })}
          className="border px-3 py-1 rounded"
          required
        />
        <input
          type="text"
          placeholder={t("product_category")}
          value={newProduct.category}
          onChange={(e) => setNewProduct({ ...newProduct, category: e.target.value })}
          className="border px-3 py-1 rounded"
        />
        <input
          type="text"
          placeholder={t("product_image_url")}
          value={newProduct.imageUrl}
          onChange={(e) => setNewProduct({ ...newProduct, imageUrl: e.target.value })}
          className="border px-3 py-1 rounded"
        />
        <input
          type="number"
          placeholder={t("product_stock")}
          value={newProduct.stock}
          onChange={(e) => setNewProduct({ ...newProduct, stock: Number(e.target.value) })}
          className="border px-3 py-1 rounded"
        />
        <button type="submit" className="bg-blue-600 text-white px-4 py-1 rounded">
          {t("add_product")}
        </button>
      </form>

      {/* Ürün Listesi */}
      <table className="min-w-full bg-white border border-gray-300 text-gray-500">
        <thead>
          <tr className="bg-gray-100">
            <th className="py-2 px-4 border-b">{t("product_name")}</th>
            <th className="py-2 px-4 border-b">{t("product_description")}</th>
            <th className="py-2 px-4 border-b">{t("product_price")}</th>
            <th className="py-2 px-4 border-b">{t("product_category")}</th>
            <th className="py-2 px-4 border-b">{t("product_image_url")}</th>
            <th className="py-2 px-4 border-b">{t("product_stock")}</th>
            <th className="py-2 px-4 border-b"></th>
          </tr>
        </thead>
        <tbody>
          {products.map((product) =>
            editingProduct?.id === product.id ? (
              <tr key={product.id} className="hover:bg-gray-50">
                <td className="py-2 px-4 border-b">
                  <input
                    type="text"
                    value={editingProduct.name}
                    onChange={(e) => setEditingProduct({ ...editingProduct, name: e.target.value })}
                    className="border px-2 py-1 rounded"
                  />
                </td>
                <td className="py-2 px-4 border-b">
                  <input
                    type="text"
                    value={editingProduct.description}
                    onChange={(e) => setEditingProduct({ ...editingProduct, description: e.target.value })}
                    className="border px-2 py-1 rounded"
                  />
                </td>
                <td className="py-2 px-4 border-b">
                  <input
                    type="number"
                    value={editingProduct.price}
                    onChange={(e) => setEditingProduct({ ...editingProduct, price: Number(e.target.value) })}
                    className="border px-2 py-1 rounded"
                  />
                </td>
                <td className="py-2 px-4 border-b">
                  <input
                    type="text"
                    value={editingProduct.category || ""}
                    onChange={(e) => setEditingProduct({ ...editingProduct, category: e.target.value })}
                    className="border px-2 py-1 rounded"
                  />
                </td>
                <td className="py-2 px-4 border-b">
                  <input
                    type="text"
                    value={editingProduct.imageUrl || ""}
                    onChange={(e) => setEditingProduct({ ...editingProduct, imageUrl: e.target.value })}
                    className="border px-2 py-1 rounded"
                  />
                </td>
                <td className="py-2 px-4 border-b">
                  <input
                    type="number"
                    value={editingProduct.stock || 0}
                    onChange={(e) => setEditingProduct({ ...editingProduct, stock: Number(e.target.value) })}
                    className="border px-2 py-1 rounded"
                  />
                </td>
                <td className="py-2 px-4 border-b space-x-2">
                  <button
                    type="button"
                    onClick={() => handleUpdateProduct(product.id)}
                    className="bg-green-600 text-white px-3 py-1 rounded"
                  >
                    {t("update")}
                  </button>
                  <button
                    type="button"
                    onClick={() => setEditingProduct(null)}
                    className="bg-gray-400 text-white px-3 py-1 rounded"
                  >
                    {t("cancel")}
                  </button>
                </td>
              </tr>
            ) : (
              <tr key={product.id} className="hover:bg-gray-50">
                <td className="py-2 px-4 border-b">{product.name}</td>
                <td className="py-2 px-4 border-b">{product.description}</td>
                <td className="py-2 px-4 border-b">{product.price} ₺</td>
                <td className="py-2 px-4 border-b">{product.category}</td>
                <td className="py-2 px-4 border-b">{product.imageUrl}</td>
                <td className="py-2 px-4 border-b">{product.stock}</td>
                <td className="py-2 px-4 border-b space-x-2">
                  <button
                    onClick={() => setEditingProduct(product)}
                    className="bg-yellow-500 text-white px-2 py-1 rounded"
                  >
                    {t("update")}
                  </button>
                  <button
                    onClick={() => handleDeleteProduct(product.id)}
                    className="bg-red-600 text-white px-2 py-1 rounded"
                  >
                    {t("delete")}
                  </button>
                </td>
              </tr>
            )
          )}
        </tbody>
      </table>
    </div>
  );
}
