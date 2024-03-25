# Soru 1

 - Kullanıcıların kampanya dönemi içerisinde çeşitli kanallar üzerinden kullanabilecekleri kodlar üretilmesi için kullanılabilir algoritmalardır.

generate_codes prosedüründe genel olarak 8 karakter uzunluğunda kod oluşturulmaktadır. Prosedür kullanılırken kaç kod üretilmesi istenildiği @NumberOfCodes değeri ile belirtilerek işlem yapılır. 
``` sql
Exec [dbo].generate_codes 1000
``` 
generate_codes prosedüründe gerekli olan değişkenler tanımlanıyor.
Daha sonra üretilmek istenilen kod sayısınca bir döngü ile kodlar üretilir.
GeneratedCodes tablosuna kaydedilerek, işlemler sonunda çıktı olarak gösterilir.

Kod güvenliğini arttırmak için her kodda bir sayı olması ve aynı karakterlerden sadece bir tane kullanılması, çok sayıda tekrar etmemesi kontrolleri ile 8 karakterli random bir kod oluşturulur.


Oluşturulan kod unique durumu kontrol edilir. 
Unique değilse tekrar kod oluşturulur.
Unique olan code check_code prosedürü ile kontrol edilir.

check_code prosedürü kodun karakter uzunluğunu ve içerdiği karakterleri kontrol eder. Geçerli ise 1, geçersiz ise 0 response değeri döner.


> NOT:  Prosedürler aşağıda bulunmaktadır. Ekstra açıklamalar kod içerisinde de bulunmaktadır.


## Generate_codes Prosedürü
``` sql
CREATE PROCEDURE [dbo].[generate_codes]
@NumberOfCodes INT
AS
BEGIN
    DECLARE @CodeLength INT = 8; -- kod karakter uzunluğu
    DECLARE @CharacterSet NVARCHAR(30) = 'ACDEFGHKLMNPRTXYZ'; -- karakterler
    DECLARE @SpecialCharacters NVARCHAR(6) = '234579'; -- karakterler
    DECLARE @GeneratedCodes TABLE (Code NVARCHAR(8)); -- geçici olarak tutulacak tablo

    WHILE @NumberOfCodes > 0
    BEGIN
        DECLARE @Code NVARCHAR(8) = '';
	DECLARE @AvailableCharacters NVARCHAR(30) = @CharacterSet; -- Kullanılabilir karakterler listesi
        DECLARE @Index INT = 1;

        -- Sayı için (SpecialCharacters) rastgele pozisyon belirlenir
        DECLARE @SpecialIndex INT = CEILING(RAND() * @CodeLength);

        WHILE @Index <= @CodeLength
        BEGIN
            IF @Index = @SpecialIndex -- Pozisyon kontrol edilir
            BEGIN
                -- Özel karakterlerden birini seç ve kodun bu pozisyonuna ekle
                DECLARE @SpecialCharIndex INT = CEILING(RAND() * LEN(@SpecialCharacters));
                SET @Code = @Code + SUBSTRING(@SpecialCharacters, @SpecialCharIndex, 1);
            END
            ELSE
            BEGIN
                -- Diğer pozisyonlarda rastgele karakter eklenir
                DECLARE @RandomIndex INT = CEILING(RAND() * LEN(@AvailableCharacters));
		DECLARE @SelectedChar NVARCHAR(1) = SUBSTRING(@AvailableCharacters, @RandomIndex, 1);

		-- Seçilen karakteri kodun sonuna eklenir
		SET @Code = @Code + @SelectedChar;

		-- Seçilen karakteri kullanılabilir karakterler listesinden çıkartılır
		SET @AvailableCharacters = STUFF(@AvailableCharacters, CHARINDEX(@SelectedChar, @AvailableCharacters), 1, '');
            END
            SET @Index = @Index + 1;
        END;

        IF NOT EXISTS (SELECT 1 FROM @GeneratedCodes WHERE Code = @Code) --unique kontrolü
        BEGIN
	        -- check_code prosedür ile kod kontrol edilir
		DECLARE @IsValid INT;
		EXEC [dbo].[check_code] @Code, @IsValid OUT;

		-- Kod geçerli ise GeneratedCodes tablosuna değeri ekler
		IF @IsValid = 1
		BEGIN 
			INSERT INTO @GeneratedCodes (Code) VALUES (@Code);
			SET @NumberOfCodes = @NumberOfCodes - 1;
		END
        END;
    END;

    SELECT Code FROM @GeneratedCodes;
END;
```
##  Check_code Prosedürü
``` sql
CREATE PROCEDURE [dbo].[check_code]
@Code varchar(8),
@IsValid int out
as
BEGIN
    DECLARE @CodeLength INT = 8;
    DECLARE @CharacterSet NVARCHAR(30) = 'ACDEFGHKLMNPRTXYZ234579';

    IF LEN(@Code) <> @CodeLength OR @Code NOT LIKE '%[^' + @CharacterSet + ']%'
    BEGIN
        SET @IsValid = 0; -- Geçersiz kod
    END
    ELSE
    BEGIN
        SET @IsValid = 1; -- Geçerli kod
    END;
END;
```

# Soru 2

Bu soruda istenilenler proje içerisinde bu repoda bulunmaktadır.

Program.cs içerisinde kodlar vardır. 
İşlem tamamlandığında current lokasyonda output.json isimle dosyaya response kaydedilmektedir. Ekstra olarak console ile ekranda da gösterilmektedir.
Eğer istenirse bu aşamada veritabanı vs bir yerede kaydedilebilir.

- jsonData isimli örnek bir json proje içerisinde oluşturulmuştur. 
- Bu data OcrResponse modeline parse edilmektedir.
- Daha sonra dosyaya ve console yazılma işlemleri vardır.

``` c#
var data = JsonSerializer.Deserialize<List<OcrResponse>>(jsonData);

string outputPath = Path.Combine(Environment.CurrentDirectory, "output.json");
using (StreamWriter sw = new StreamWriter(outputPath))
{
    var header = "line | text";
    Console.WriteLine(header);
    sw.WriteLine(header);

    foreach (var item in data)
    {
        var text = item.Cord + "    | " + item.Desc;
        Console.WriteLine(text);
        sw.WriteLine(text);
    }
}
``` 
