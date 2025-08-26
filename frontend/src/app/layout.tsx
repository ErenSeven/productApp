import { ReduxProvider } from "@/providers/ReduxProvider";
import Header from "@/components/Header";
import "./globals.css";
import {NextIntlClientProvider} from 'next-intl';

export default function RootLayout({ children }: { children: React.ReactNode }) {
  return (
    <html lang="tr">
      <body>
        <NextIntlClientProvider>
          <ReduxProvider>
            <Header />
            <main>{children}</main>
          </ReduxProvider>
        </NextIntlClientProvider>
      </body>
    </html>
  );
}