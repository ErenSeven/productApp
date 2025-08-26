## Product API
Bu proje, katmanlı mimari (Controller - Service - Repository) kullanılarak geliştirilmiş bir .NET 8 Web API uygulamasıdır.

##🚀 Özellikler

Yeni bir bilgisayarda çalıştırmak için aşağıdaki araçların kurulu olması gerekir:
- Katmanlı mimari (Controller, Service, Repository)
- Entity Framework Core ile MSSQL veritabanı
- Ürün ekleme ve listeleme (CRUD)
- DTO kullanımı
- async/await ile asenkron işlemler
- Migration yönetimi (dotnet ef migrations)
- Swagger UI entegrasyonu

##📦 Gereksinimler

Yeni bir bilgisayarda çalıştırmak için aşağıdaki araçların kurulu olması gerekir:
- .NET 9.08 SDK
- SQL Server (veya Docker üzerinde MSSQL)
- Visual Studio 2022 veya VS Code
- Entity Framework Core CLI:
 ```bash
dotnet tool install --global dotnet-ef
```

##⚙️ Kurulum

1. Projeyi klonla:
 ```bash
git clone https://github.com/ErenSeven/productApp.git
cd productApp
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
##📖 Swagger UI

API çalıştığında Swagger UI otomatik olarak açılır:
👉 http://localhost:5118/swagger/index.html
