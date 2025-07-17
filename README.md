# 🏥 HospitalDB_Test - Veritabanı Kurulum Rehberi

Bu dosya, `HospitalDB_Test.sql` script dosyasının Microsoft SQL Server ortamına nasıl ekleneceğini anlatır.

## 📦 Gereksinimler

- Microsoft SQL Server (SQL Server Express veya üstü)
- SQL Server Management Studio (SSMS)

## 🔧 Kurulum Adımları

1. **SSMS (SQL Server Management Studio)** uygulamasını başlatın.
2. Sol üstteki veritabanı listesinden **master** veritabanını seçin.
3. Bu klasördeki `HospitalDB_Test.sql` dosyasını SSMS'de açın.
4. Tüm kodu seçmek için `Ctrl + A` tuşlarına basın.
5. Script'i çalıştırmak için `F5` tuşuna basın.
6. Kurulumdan sonra veritabanı `HospitalDB_Test` adıyla oluşturulacaktır.

## ⚠️ Notlar

- Eğer aşağıdaki satır hata verirse, başına `--` ekleyerek yorum satırı yapabilirsiniz:
  ```sql
  EXEC [HospitalDB_Test].[dbo].[sp_fulltext_database] @action = 'enable'
