import { ReduxProvider } from "@/providers/ReduxProvider";
import Header from "@/components/Header";
import CartEventListener from "@/components/CartEventListener"; 
import CartInitializer from './CartInitializer';
import "./globals.css";
import { NextIntlClientProvider } from "next-intl";

export default function RootLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  return (
    <html lang="en">
      <body>
        <NextIntlClientProvider>
          <ReduxProvider>
            <CartEventListener />  
            <CartInitializer />
            <Header />
            <main>{children}</main>
          </ReduxProvider>
        </NextIntlClientProvider>
      </body>
    </html>
  );
}
