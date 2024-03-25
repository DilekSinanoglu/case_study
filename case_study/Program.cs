using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace case_study
{
    public class Program
    {
        static void Main(string[] args)
        {

            var jsonData = @"[ 
                            {""cord"": ""1"", ""desc"" : ""TEŞEKKÜRLER""},
                            {""cord"": ""2"", ""desc"" : ""GUNEYDOĞU TEKS. GIDA INS SAN. LTD.STI.""},
                            {""cord"": ""3"" , ""desc"" : ""ORNEKTEPE MAH.ETIBANK CAD.SARAY APT.""},
                            {""cord"": ""4"" , ""desc"" : "" N:43-1 BEYOĞLU/ISTANBUL""},
                            {""cord"": ""5"" , ""desc"" : "" GÜNEŞLİ V.D. 4350078928 V. NO.""},
                            {""cord"": ""6"" , ""desc"" : ""TARIH : 26.08.2020""},
                            {""cord"": ""7"" , ""desc"" : ""SAAT : 15:27""},
                            {""cord"": ""8"" , ""desc"" : ""FİŞ NO : 0082""},
                            {""cord"": ""9"" , ""desc"" : "" 54491250""},
                            {""cord"": ""10"" , ""desc"" : "" 2 ADx 2,75"" },
                            {""cord"": ""11"" , ""desc"" : "" CC.COCA COLA CAM 200 08 *5,50"" },
                            {""cord"": ""12"" , ""desc"" : "" 2701084"" },
                            {""cord"": ""13"" , ""desc"" :  ""1,566 KGx 1,99"" },
                            {""cord"": ""14"" , ""desc"" :  ""MANAV DOMATES PETEME *3,32"" },
                            {""cord"": ""15"" , ""desc"" :  ""2701076"" },
                            {""cord"": ""16"" , ""desc"" :  ""0,358 KGx 4,99"" },
                            {""cord"": ""17"" , ""desc"" : ""MANAV BIBER CARLISTO 08 *1,79"" },
                            {""cord"": ""18"" , ""desc"" :  ""4"" },
                            {""cord"": ""19"" , ""desc"" :  ""EKMEK CIFTLI 01 *1,25"" },
                            {""cord"": ""20"" , ""desc"" :  ""TOPKDV *0,80"" },
                            {""cord"": ""21"" , ""desc"" :  ""TOPLAM *11,86"" },
                            {""cord"": ""22"" , ""desc"" :  ""NAKİT *20,00"" },
                            {""cord"": ""23"" , ""desc"" :  ""KDV KDV TUTARI KDV'LI TOPLAM"" },
                            {""cord"": ""24"" , ""desc"" : "" 01 *0,01 *1,25"" },
                            {""cord"": ""25"" , ""desc"" :  ""08 *0,79 *10,61"" },
                            {""cord"": ""26"" , ""desc"" : ""KASİYER : SUPERVISOR"" },
                            {""cord"": ""27"" , ""desc"" :  ""00 0001/020/000084/1//82/"" },
                            {""cord"": ""28"" , ""desc"" :  ""PARA USTÜ *8,14"" },
                            {""cord"": ""29"" , ""desc"" :  ""KASİYER: 1"" },
                            {""cord"": ""30"" , ""desc"" :  ""2 NO:1330 EKÜ NO:0001"" },
                            {""cord"": ""31"" , ""desc"" :  ""MF YAB 15017876"" }
                       ]";

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

            Console.ReadLine();
        }
    }
}
