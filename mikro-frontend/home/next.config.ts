import type { NextConfig } from "next";
import createNextIntlPlugin from "next-intl/plugin";
const { BLOG_URL } = process.env;

const nextConfig: NextConfig = {
  async rewrites() {
    return [
      {
        source: "/cart",
        destination: `${BLOG_URL}/cart`,
      },
      {
        source: "/cart/:path+",
        destination: `${BLOG_URL}/cart/:path+`,
      },
      {
        source: "/cart-static/_next/:path+",
        destination: `${BLOG_URL}/cart-static/_next/:path+`,
      }
    ];
  },
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
