"use client";

import Image from "next/image";
import Link from "next/link";
import { Product } from "@/types/product";
import { useDispatch } from "react-redux";
import { addToCart } from "@/store/cartSlice";
import {useTranslations} from 'next-intl';

export default function ProductCard({ product }: { product: Product }) {
    const dispatch = useDispatch();
    const t = useTranslations('ProductCard');
    const handleAddToCart = () => {
        dispatch(addToCart(product));
    };

    return (
        <Link href={`/products/${product.id}`}>
            <div className="rounded-xl bg-white p-5 shadow hover:shadow-lg transition cursor-pointer">
                <Image 
                    src={product.imageUrl} 
                    width={200} 
                    height={200} 
                    alt={product.name} 
                    className="rounded-lg" 
                />
                <h2 className="text-xl font-semibold text-gray-900">{product.name}</h2>
                <p className="mt-2 text-gray-700">{product.price}₺</p>
                <button
                    onClick={(e) => {
                        e.preventDefault();
                        handleAddToCart();
                    }}
                    className="mt-2 rounded-xl bg-blue-600 px-4 py-2 text-white hover:bg-blue-700 transition"
                >
                    {t("addToCart")}
                </button>
            </div>
        </Link>
    );
}
