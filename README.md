## Distributed E-Commerce Full Stack Project

12 Faktör Uygulama prensiplerine uygun, API Gateway üzerinden haberleşen, event-driven (olay güdümlü) .NET mikroservisleri (Auth, Product, Log) ile bu servisleri kullanan, birbirinden bağımsız geliştirilip deploy edilebilen mikro-frontend (Home, Cart) uygulamasını Docker üzerinde ayağa kaldırmaya uğraştığımız bu projede 3 ayrı mikroservisle auth product logs fonksiyonelliklerini her mikroservis kendi içerisinde onion arc içerecek şekilde sağladık. Front-end kısmında da 2 farklı frontend uygulamasını ayrı ayrı deploy edilebilecek şekilde geliştirip multi-zone mimarisi kullanarak senkron hale getirdik. İletişimi custom event sayesinde yapıp aynı verilerle çakışmalarını sağladık.

## 🚀 Özellikler
- Onion Architecture (Core - Application - Infrastructure - API)
- CQRS pattern: Command/Query ayrımı
- JWT ve refresh token mekanizması (Auth Servisi)
- RabbitMQ ile event-driven iletişim (Product → Log / Cart)
- Log Servisi: Merkezi ve yapısal loglama (Serilog)
- API Gateway (YARP) üzerinden yönlendirme ve rate limiting
- Rol ve policy tabanlı yetkilendirme
- Redis cache ile ürün listeleme ve cache invalidation
- PostgreSQL veritabanı
- Global exception handling
- SOLID prensiplerine tam uyum
- Tüm sistem Docker ile containerize edilmiş

## Frontend (Next.js Mikro-Frontend)
- Next.js 14 + TypeScript + App Router
- Mikro-Frontend yapısı: Multi-Zone mimarisi
- Home Uygulaması (Port 3000): Ürünleri listeler, filtreleme ve sıralama sağlar
- Cart Uygulaması (Port 3001): Sepet yönetimi ve senkronizasyon
- Log Servisi: Merkezi ve yapısal loglama (Serilog)
- State paylaşımı custom events üzerinden
- TailwindCSS ile responsive ve profesyonel UI
- next-intl ile çok dilli destek
- next/image ile lazy loading görseller
- Home ve Cart uygulamaları bağımsız deploy edilebilir

## 📦 Gereksinimler

Yeni bir bilgisayarda çalıştırmak için aşağıdaki araçların kurulu olması gerekir:
- .NET 9.08 SDK
- PostgreSQL + PGADMIN4 
- Visual Studio 2022 veya VS Code
- Node.js 20+
- pnpm (önerilen) veya npm / yarn
- Docker & Docker Compose
- Entity Framework Core CLI:
 ```bash
dotnet tool install --global dotnet-ef
```

## ⚙️ Kurulum
1. Projeyi klonla:
 ```bash
git clone --branch prod/v1.0.0 --single-branch https://github.com/ErenSeven/productApp.git
```
2. Api servislerindeki appsettings.json'ları düzenle:
 ```bash
  "ConnectionStrings": {
    "AuthDb": "Host=host.docker.internal;Port=5432;Database=AuthDb;Username=postgres;Password=1234"
  },
```
Infrastructure servislerindeki appsettings.json'ları düzenle:
 ```bash
  "ConnectionStrings": {
    "AuthDb": "Host=localhost;Port=5432;Database=AuthDb;Username=postgres;Password=1234"
  },
```
3. mikro-frontend içinde cart ve home dizinlerine .env dosyasını ekle
 ```bash
HOME_URL="http://localhost:3000"
BLOG_URL="http://localhost:3001"
NEXT_PUBLIC_API_BASE_URL=http://localhost:5179
```

4. PostgreSQL’in pg_hba.conf dosyasını bul host.docker.internal veya IP için izin ekle
 ```bash
C:\Program Files\PostgreSQL\<version>\data\pg_hba.conf
```
Dosyanın sonuna şunu ekle(cmd'ye ipconfig yazıp ipv4 adresine ulaş):
 ```bash
# Docker konteynerinden host makinedeki PostgreSQL'e erişim
host    all             all             0.0.0.0/0               scram-sha-256
```
Windows hizmetlerden postgresql'i yeniden başlat
postgresql'e şu 3 db'i ekle:
 ```bash
LogDb
ProductDb
AuthDb
```
Ardından bütün apı servisleri için .Infrastructer dizinine gelip terminalden database güncelle:
 ```bash
dotnet ef database update
```
5. Tüm api mikroservislerine .dockerignore ekle:
```bash
**/bin/
**/obj/
```
6.Infrastructure servislerindeki appsettings.json'ları düzenle:
 ```bash
  "ConnectionStrings": {
    "AuthDb": "Host=host.docker.internal;Port=5432;Database=AuthDb;Username=postgres;Password=1234"
  },
```
7. Terminalden docker'ı ayağa kaldır(Docker desktop açık olsun)
```bash
docker-compose up --build
```
8. Ürün eklemek için uğraşmamak istiyorsan postgresqlden insert et:
```bash
INSERT INTO public."Products"(
    "Id", "Name", "Description", "Price", "Stock", "Category", "ImageUrl", "CreatedAt", "UpdatedAt"
) VALUES
('e57f9456-9d42-49c8-8f87-ec93f0f8c8f1', 'Wireless Mouse', 'A high-quality wireless mouse with ergonomic design', 25.99, 150, 'Electronics', 'https://picsum.photos/300/200?random=1', '2023-08-01 10:00:00', '2023-08-01 10:00:00'),
('cae8abf2-5bde-409d-8f6c-e3b2208d6a16', 'Bluetooth Headphones', 'Noise-cancelling Bluetooth headphones with long battery life', 89.99, 75, 'Electronics', 'https://picsum.photos/300/200?random=2', '2023-08-05 11:30:00', '2023-08-05 11:30:00'),
('b7d7fe33-b973-44b5-b66a-4bcb49f28b22', 'Gaming Keyboard', 'RGB gaming keyboard with mechanical switches', 55.49, 50, 'Electronics', 'https://picsum.photos/300/200?random=3', '2023-08-10 12:00:00', '2023-08-10 12:00:00'),
('d5288b62-58f1-419d-b139-d3425738a327', 'Smartphone', 'Latest model with 128GB storage and 6GB RAM', 599.99, 200, 'Electronics', 'https://picsum.photos/300/200?random=4', '2023-08-15 14:00:00', '2023-08-15 14:00:00'),
('fa302b13-d91e-4b84-bf8c-ff4f6daffecb', 'Coffee Maker', 'Automatic coffee maker with multiple brewing options', 39.99, 120, 'Home Appliances', 'https://picsum.photos/300/200?random=5', '2023-08-20 08:30:00', '2023-08-20 08:30:00'),
('c2e2c9d1-73f9-41c1-b278-2f03e6314215', 'Office Chair', 'Ergonomic office chair with adjustable height and lumbar support', 159.99, 40, 'Furniture', 'https://picsum.photos/300/200?random=6', '2023-08-22 09:00:00', '2023-08-22 09:00:00'),
('e51dce1e-b4f7-4ca4-bd3d-f2b1f3d8a4b6', 'Air Purifier', 'HEPA filter air purifier with UV-C light', 199.99, 60, 'Home Appliances', 'https://picsum.photos/300/200?random=7', '2023-08-23 10:30:00', '2023-08-23 10:30:00'),
('d2319936-ec52-4db7-9e6f-7da54b3d8b89', 'Electric Fan', 'Portable electric fan with three-speed settings', 29.99, 180, 'Home Appliances', 'https://picsum.photos/300/200?random=8', '2023-08-25 12:00:00', '2023-08-25 12:00:00'),
('b698b8b7-e00a-4189-b19e-1894a0f84974', 'Dining Table', 'Modern dining table with wooden finish', 299.99, 35, 'Furniture', 'https://picsum.photos/300/200?random=9', '2023-08-30 13:30:00', '2023-08-30 13:30:00'),
('c9f9e631-91c3-4c7e-b20c-91fe649b8295', 'Sofa Set', 'Comfortable 3-piece sofa set with plush cushions', 899.99, 20, 'Furniture', 'https://picsum.photos/300/200?random=10', '2023-09-01 15:00:00', '2023-09-01 15:00:00'),
('a7bc5f2d-f8e1-4793-9f0e-3c1251bb9e64', 'Cooking Set', '10-piece non-stick cookware set with heat-resistant handles', 79.99, 95, 'Kitchenware', 'https://picsum.photos/300/200?random=11', '2023-09-02 14:45:00', '2023-09-02 14:45:00'),
('dda67c0f-9848-41d0-8bb2-153aa423ca99', 'Blender', 'High-speed blender with multiple preset functions', 119.99, 100, 'Kitchenware', 'https://picsum.photos/300/200?random=12', '2023-09-05 16:30:00', '2023-09-05 16:30:00'),
('75f62300-d1b0-4e6f-b0c9-78c5c10f47b3', 'Electric Grill', 'Compact electric grill for indoor cooking', 65.49, 150, 'Kitchenware', 'https://picsum.photos/300/200?random=13', '2023-09-07 17:00:00', '2023-09-07 17:00:00'),
('0637c1ca-b497-47db-9ecf-f84a2a437559', 'Yoga Mat', 'Non-slip yoga mat with comfortable thickness', 25.99, 200, 'Sports', 'https://picsum.photos/300/200?random=14', '2023-09-10 08:00:00', '2023-09-10 08:00:00'),
('45327d9d-b469-4b66-8047-d8e43f9a3c25', 'Dumbbell Set', 'Adjustable dumbbell set with a range of weights', 79.99, 60, 'Sports', 'https://picsum.photos/300/200?random=15', '2023-09-12 11:30:00', '2023-09-12 11:30:00'),
('39b3502d-3cf9-46de-b30f-63f21490bba0', 'Treadmill', 'Foldable treadmill with heart rate monitor and multiple workout programs', 499.99, 30, 'Sports', 'https://picsum.photos/300/200?random=16', '2023-09-15 14:00:00', '2023-09-15 14:00:00'),
('c78106d7-35d0-4e57-8185-8e358318f738', 'Smartwatch', 'Fitness smartwatch with step tracking and heart rate monitor', 129.99, 150, 'Wearables', 'https://picsum.photos/300/200?random=17', '2023-09-18 09:30:00', '2023-09-18 09:30:00'),
('1b3376cc-28b2-4c3b-9750-bbd3a15a1f75', 'Bluetooth Speaker', 'Portable Bluetooth speaker with waterproof design', 45.99, 200, 'Wearables', 'https://picsum.photos/300/200?random=18', '2023-09-20 10:00:00', '2023-09-20 10:00:00'),
('d7101c76-0c52-4682-bb76-f8c88c21be87', 'VR Headset', 'Virtual reality headset with immersive 3D experience', 199.99, 70, 'Wearables', 'https://picsum.photos/300/200?random=19', '2023-09-22 12:00:00', '2023-09-22 12:00:00'),
('f1226fae-228b-4fc6-b0d5-29baf48b95a3', 'Smart Glasses', 'Stylish smart glasses with built-in audio and touch controls', 299.99, 50, 'Wearables', 'https://picsum.photos/300/200?random=20', '2023-09-25 13:30:00', '2023-09-25 13:30:00');

```
