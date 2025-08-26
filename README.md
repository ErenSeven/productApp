## Product API
Bu proje, katmanlı mimari (Controller - Service - Repository) kullanılarak geliştirilmiş bir .NET 8 Web API uygulamasıdır.

## 🚀 Özellikler

- Katmanlı mimari (Controller, Service, Repository)
- Entity Framework Core ile MSSQL veritabanı
- Ürün ekleme ve listeleme (CRUD)
- DTO kullanımı
- async/await ile asenkron işlemler
- Migration yönetimi (dotnet ef migrations)
- Swagger UI entegrasyonu

## 📦 Gereksinimler

Yeni bir bilgisayarda çalıştırmak için aşağıdaki araçların kurulu olması gerekir:
- .NET 9.08 SDK
- SQL Server (veya Docker üzerinde MSSQL)ve SSSMS21(Gerekli değil ama veritabanı kontrolü için opsiyonel)
- Visual Studio 2022 veya VS Code
- Entity Framework Core CLI:
 ```bash
dotnet tool install --global dotnet-ef
```

## ⚙️ Kurulum

1. Projeyi klonla:
 ```bash
git clone --branch dev/v1.0.0 --single-branch https://github.com/ErenSeven/productApp.git
cd productApp
cd api
```
2. appsettings.json dosyasını yapılandır:
 ```bash
  "ConnectionStrings": {
    "Data Source={sunucu bağlantisinda pc adi}\\{sunucu bağlantisinda sqlexpress adi};Initial Catalog={olusturulmus};Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"
  },
```
3. Migration oluştur ve veritabanını güncelle:
 ```bash
dotnet ef database update
```
4. Projeyi Çalıştır:
 ```bash
  dotnet watch run
```
## 📖 Swagger UI

API çalıştığında Swagger UI otomatik olarak açılır:
👉 http://localhost:5118/swagger/index.html

## Product Frontend (Next.js)
Bu proje, Next.js 14 (App Router), TypeScript ve TailwindCSS kullanılarak geliştirilmiş bir frontend uygulamasıdır.

## 🚀 Özellikler

- Next.js 14 (App Router)
- TypeScript
- TailwindCSS
- Ürün listeleme sayfası (GET /products)
- Ürün ekleme sayfası (POST /products)
- .env dosyası ile yapılandırma (Backend API adresi gibi)

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
3. .env.local dosyasını oluştur:
 ```bash
API_BASE_URL=http://localhost:5081
```
4. geliştirme ortamında çalıştır:
 ```bash
 npm run dev
```
5. terminalde dönüş yapılan adresten uygulama arayüzüne eriş

## 📖 Sayfalar

Ürün Listeleme
- Backend’den GET /products endpoint’ini çağırır ve ürünleri listeler.

Ürün Ekleme
- Form aracılığıyla ürün bilgilerini alır ve POST /products endpoint’ine gönderir.
