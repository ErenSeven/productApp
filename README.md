## Full Stack Project Backend Setup
Bu proje, Onion Architecture ve CQRS pattern’ini kullanan bir .NET backend çözümüdür.
Proje, Auth Servisi ve Product Servisi içerir; Product Servisi Redis cache ile optimize edilmiştir.
JWT tabanlı kimlik doğrulama kullanılır ve Redis cache invalidation stratejisi uygulanmıştır.

## 🚀 Özellikler

- Onion Architecture (Core - Application - Infrastructure - API)
- CQRS pattern: Command/Query ayrımı
- JWT ile kimlik doğrulama (Auth Servisi)
- Redis cache ile hızlandırılmış ürün listeleme
- Ürün ekleme/güncelleme/silme işlemlerinde cache invalidation
- PostgreSQL veritabanı
- Serilog ile basit loglama
- Global exception handling
- SOLID prensiplerine tam uyum
  
## 📦 Gereksinimler

Yeni bir bilgisayarda çalıştırmak için aşağıdaki araçların kurulu olması gerekir:
- .NET 9.08 SDK
- PostgreSQL + PGADMIN4 
- Visual Studio 2022 veya VS Code
- Entity Framework Core CLI:
 ```bash
dotnet tool install --global dotnet-ef
```

## ⚙️ Kurulum

1. Projeyi klonla:
 ```bash
git clone --branch test/v1.0.0 --single-branch https://github.com/ErenSeven/productApp.git
cd productApp
cd api
```
2.ECommerce.API ve ECommerce.Infrastructure içindeki appsettings.json dosyasını yapılandır:
 ```bash
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=ECommerceDb;Username=postgres;Password=1234",
    "Redis": "localhost:6379,abortConnect=false",
    "_comments": {
      "DefaultConnection": "PostgreSQL bağlantısı. Değiştirilecek alanlar: Host (sunucu adresi), Port (PostgreSQL portu), Database (veritabanı adı), Username (kullanıcı adı), Password (şifre)",
      "Redis": "Redis bağlantısı. Değiştirilecek alanlar: localhost (sunucu adresi), port (6379 varsayılan). abortConnect=false, bağlantı hatasında uygulamanın başlatılmasını sağlar"
    }
  }
```
3. Migration oluştur ve veritabanını güncelle:
 ```bash
dotnet ef database update
```
4. Projeyi Çalıştır:
 ```bash
  cd ECommerce.API
  dotnet watch run
```
## 📖 Swagger UI

API çalıştığında Swagger UI otomatik olarak açılır:
👉 http://localhost:5206/swagger/index.html
Redis cache’in çalıştığını doğrulamak için:
 ```bash
  redis-cli ping
```
## Product Frontend (Next.js)
Bu proje, Next.js 14 (App Router), TypeScript, TailwindCSS, next-intl (çok dilli destek) ve RTK (Redux Toolkit) kullanılarak geliştirilmiş bir frontend uygulamasıdır.
Amaç, backend’in Auth ve Product servislerini kullanarak tam işlevsel bir e-ticaret arayüzü sunmaktır.

## 🚀 Özellikler

- Kimlik doğrulama (JWT) via Auth Servisi
- Ürün listeleme, filtreleme ve sıralama
- Dinamik ürün detay sayfası (/products/:id)
- Sepet yönetimi RTK (global state) ile
- SSR veya ISR ile SEO uyumlu sayfalar
- next-intl ile çok dilli destek
- next/image ile lazy loading görseller

## 📦 Gereksinimler

Yeni bir bilgisayarda çalıştırmak için aşağıdaki araçların kurulu olması gerekir:
- Node.js 20+
- pnpm (önerilen) veya npm / yarn

## ⚙️ Kurulum

1. Projeyi Klonla:
 ```bash
git clone --branch dev/v1.0.0 --single-branch https://github.com/ErenSeven/productApp.git
 cd productApp
 cd frontend
```
2. Bağımlılıkları yükle:
 ```bash
npm install
```
veya
 ```bash
pnpm install
```
3. .env dosyasını oluştur:
 ```bash
NEXT_PUBLIC_API_BASE_URL=http://localhost:5206
```
4. geliştirme ortamında çalıştır:
 ```bash
 npm run dev
```
5. terminalde dönüş yapılan adresten uygulama arayüzüne eriş
