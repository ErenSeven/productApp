"use client";

import { Product } from "@/types/product";
import { useDispatch, useSelector } from "react-redux";
import { addToCart } from "@/store/cartSlice";
import { RootState, AppDispatch } from "@/store";
import { useTranslations } from 'next-intl';
import { dispatchCrossAppEvent } from "@/utils/events";

export default function AddToCartButton({ product }: { product: Product }) {
    const dispatch = useDispatch<AppDispatch>();
    const user = useSelector((state: RootState) => state.user);
    const t = useTranslations('AddToCartButton');

    const handleAdd = () => {
        if (!user.isLoggedIn) {
            alert(t("loginAlert"));
            return;
        }

        dispatch(addToCart(product));

        dispatchCrossAppEvent("cart-event", { type: "addToCart", payload: product });

        alert(t("addedAlert"));
    };

    return (
        <button
            onClick={handleAdd}
            className="mt-4 rounded-xl bg-blue-600 px-5 py-2 text-white hover:bg-blue-700 transition"
        >
            {t("buttonText")}
        </button>
    );
}
