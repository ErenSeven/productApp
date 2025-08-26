"use client";

import { useSelector, useDispatch } from "react-redux";
import { RootState, AppDispatch } from "@/store";
import {
  removeFromCart,
  clearCart,
  increaseQuantity,
  decreaseQuantity,
} from "@/store/cartSlice";
import Image from "next/image";
import {useTranslations} from 'next-intl';

export default function CartPage() {
  const dispatch = useDispatch<AppDispatch>();
  const cart = useSelector((state: RootState) => state.cart);
  const t = useTranslations('CartPage');
  const handleRemove = (id: number) => {
    dispatch(removeFromCart(id));
  };

  const totalPrice = cart.items.reduce(
    (acc, item) => acc + item.price * item.quantity,
    0
  );

  return (
    <main className="max-w-6xl mx-auto px-6 py-10">
      <h1 className="text-3xl font-bold mb-6">{t('title')}</h1>

      {cart.items.length === 0 ? (
        <p className="text-gray-500">{t('empty')}</p>
      ) : (
        <>
          <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6">
            {cart.items.map((item) => (
              <div
                key={item.id}
                className="border rounded-lg p-4 flex flex-col items-center"
              >
                <Image
                  src={item.imageUrl}
                  width={150}
                  height={150}
                  alt={item.name}
                  className="rounded"
                />
                <h2 className="font-semibold mt-2">{item.name}</h2>
                <p className="mt-1 font-medium">
                  {item.price}₺ x {item.quantity} ={" "}
                  <span className="font-bold">{item.price * item.quantity}₺</span>
                </p>
                <p className="text-gray-500 mt-1">{t('stock')}: {item.stock}</p>

                {/* Adet arttırma / azaltma */}
                <div className="flex items-center gap-2 mt-2">
                  <button
                    onClick={() => dispatch(decreaseQuantity(item.id))}
                    className="px-2 py-1 bg-gray-300 rounded hover:bg-gray-400"
                  >
                    {t('decrease')}
                  </button>
                  <span className="px-2">{item.quantity}</span>
                  <button
                    onClick={() => dispatch(increaseQuantity(item.id))}
                    disabled={item.quantity >= (item.stock ?? 0)}
                    className={`px-2 py-1 rounded text-white ${
                      item.quantity < (item.stock ?? 0)
                        ? "bg-green-600 hover:bg-green-700"
                        : "bg-gray-400 cursor-not-allowed"
                    }`}
                  >
                    {t('increase')}
                  </button>
                </div>

                <button
                  onClick={() => handleRemove(item.id)}
                  className="mt-2 bg-red-600 px-3 py-1 rounded hover:bg-red-700 text-white transition"
                >
                  {t('remove')}
                </button>
              </div>
            ))}
          </div>

          <div className="mt-6 flex justify-between items-center">
            <p className="text-xl font-bold">{t('total')}: {totalPrice}{t('currency')}</p>
            <button
              onClick={() => dispatch(clearCart())}
              className="bg-red-600 px-4 py-2 rounded hover:bg-red-700 text-white transition"
            >
              {t('clearCart')}
            </button>
          </div>
        </>
      )}
    </main>
  );
}