# Planor backend

## Development

Aşağıdaki komut ile API uygulamasını çalıştırabilirsin.

```bash

cd src/Planor.WebUI
dotnet watch run

```

Docker ile mysql ayağa kaldırmak için aşağıdaki komut kullanılabilir. Servis bağımlılık olarak mysql veritabanına ihtiyaç duyar, mysql connection ile ilgili bilgiler appsettings.json içerisinde bulunabilir, eğer erişilebilir bir mysql veritabanı bulamazsa uygulama ayağa kalkarken durdurulur. 

```bash

docker compose -f docker-compose.development.yml up -d
docker compose -f docker-compose.development.yml down

```

## Docker

Mysql bağımlılıkları eklenmiş bir şekilde tek bir komutla localhost:5050 adresinden kullanılabilecek uygulamayı erişime sunar. Bilgisayar Docker kurulu olması gerekmektedir.

```bash

docker compose build
docker compose up -d
docker compose down

```