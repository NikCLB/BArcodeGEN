using System;
using ZXing;
using ZXing.Common;
using System.Drawing;
using System.Text;
using System.Linq;
using ZXing.QrCode.Internal;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
class Program
{
   
    public static int CalculateEan13Checksum(string barcode)
    {
        if (barcode.Length != 12)
        {
            throw new ArgumentException("Barcode must be 12 digits long", "barcode");
        }

        int oddSum = 0, evenSum = 0;

        for (int i = 0; i < barcode.Length; i++)
        {
            if (i % 2 == 0)
            {
                oddSum += int.Parse(barcode[i].ToString());
            }
            else
            {
                evenSum += int.Parse(barcode[i].ToString());
            }
        }

        int total = oddSum + evenSum * 3;
        int checksum = (10 - total % 10) % 10;

        return checksum;
    }
    
    static void Main(string[] args)
    {
        long startbarcode = 205001032728;
        int g = 100;
        string barcode = "205001032728";
        int checksum = CalculateEan13Checksum(barcode);
        string barcodestring = "";
        barcode = barcode + checksum.ToString();
        string A1 = barcode; // Ваша исходная строка
        string result = "";
        
        for (int i = 0; i < g; i++)
        {
            barcode = startbarcode.ToString();
            checksum = CalculateEan13Checksum(barcode);
            Console.WriteLine("The checksum is: " + checksum);
            barcode = barcode + checksum.ToString();
            A1 = barcode; // Ваша исходная строка
            result = "";
            result += A1.Substring(0, 1);
            //result += Char.ConvertFromUtf32(Int32.Parse(A1.Substring(0, 1)));
            result += Char.ConvertFromUtf32(Int32.Parse(A1.Substring(1, 1)) + 65);

            if (Int32.Parse(A1.Substring(0, 1)) < 4)
                result += Char.ConvertFromUtf32(Int32.Parse(A1.Substring(2, 1)) + 65);
            else
                result += Char.ConvertFromUtf32(Int32.Parse(A1.Substring(2, 1)) + 75);

            if (new int[] { 0, 4, 7, 8 }.Contains(Int32.Parse(A1.Substring(0, 1))))
                result += Char.ConvertFromUtf32(Int32.Parse(A1.Substring(3, 1)) + 65);
            else
                result += Char.ConvertFromUtf32(Int32.Parse(A1.Substring(3, 1)) + 75);

            if (new int[] { 0, 1, 4, 5, 9 }.Contains(Int32.Parse(A1.Substring(0, 1))))
                result += Char.ConvertFromUtf32(Int32.Parse(A1.Substring(4, 1)) + 65);
            else
                result += Char.ConvertFromUtf32(Int32.Parse(A1.Substring(4, 1)) + 75);

            if (new int[] { 0, 2, 5, 6, 7 }.Contains(Int32.Parse(A1.Substring(0, 1))))
                result += Char.ConvertFromUtf32(Int32.Parse(A1.Substring(5, 1)) + 65);
            else
                result += Char.ConvertFromUtf32(Int32.Parse(A1.Substring(5, 1)) + 75);

            if (new int[] { 0, 3, 6, 8, 9 }.Contains(Int32.Parse(A1.Substring(0, 1))))
                result += Char.ConvertFromUtf32(Int32.Parse(A1.Substring(6, 1)) + 65);
            else
                result += Char.ConvertFromUtf32(Int32.Parse(A1.Substring(6, 1)) + 75);
            result += '*';
            result += Char.ConvertFromUtf32(Int32.Parse(A1.Substring(7, 1)) + 97);
            result += Char.ConvertFromUtf32(Int32.Parse(A1.Substring(8, 1)) + 97);
            result += Char.ConvertFromUtf32(Int32.Parse(A1.Substring(9, 1)) + 97);
            result += Char.ConvertFromUtf32(Int32.Parse(A1.Substring(10, 1)) + 97);
            result += Char.ConvertFromUtf32(Int32.Parse(A1.Substring(11, 1)) + 97);
            result += Char.ConvertFromUtf32(Int32.Parse(A1.Substring(12, 1)) + 97);
            result += '+';
            startbarcode = startbarcode + 1;
            if (i % 4 == 0)
                barcodestring = barcodestring + "\n" + "\n" + result.ToString();
            else
                barcodestring = barcodestring + " " + result.ToString();
            Document pdfDoc = new Document();

            // Создайте новый PDF-писатель
            PdfWriter.GetInstance(pdfDoc, new FileStream(@"C:\Users\nikit\OneDrive\Documents\YourFile.pdf", FileMode.Create));

            // Откройте PDF-документ
            pdfDoc.Open();

            // Создайте новый шрифт
            BaseFont baseFont = BaseFont.CreateFont(@"C:\Users\nikit\AppData\Local\Microsoft\Windows\Fonts\ean13.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            Font font = new Font(baseFont, 36, Font.NORMAL, BaseColor.BLACK);

            // Добавьте текст в документ с указанным шрифтом
            pdfDoc.Add(new Paragraph(barcodestring, font));

            // Закройте PDF-документ
            pdfDoc.Close();

        }


    




        string output = result.ToString();
        Console.WriteLine(barcodestring);
        Console.ReadKey();
    }
}