import type { NextConfig } from "next";
import createNextIntlPlugin from "next-intl/plugin";

const nextConfig: NextConfig = {
  assetPrefix: "/cart-static", 
  images: {
    domains: ["picsum.photos"],
    unoptimized: true, // 🔴 optimize etmesin
  },
  eslint: {
    ignoreDuringBuilds: true,
  },
};


const withNextIntl = createNextIntlPlugin();

export default withNextIntl(nextConfig);
