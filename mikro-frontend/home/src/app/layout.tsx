import { ReduxProvider } from "@/providers/ReduxProvider";
import Header from "@/components/Header";
import "./globals.css";
import {NextIntlClientProvider} from 'next-intl';
import CartInitializer from './CartInitializer';

export default function RootLayout({ children }: { children: React.ReactNode }) {
  return (
    <html lang="tr">
      <body>
        <NextIntlClientProvider>
          <ReduxProvider>
            <CartInitializer />
            <Header />
            <main>{children}</main>
          </ReduxProvider>
        </NextIntlClientProvider>
      </body>
    </html>
  );
}