using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using clickfly.ViewModels;
using QRCoder;

namespace clickfly
{
    //Extension method to convert Bitmap to Byte Array
    public static class BitmapExtension
    {
        public static byte[] BitmapToByteArray(this Bitmap bitmap)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                bitmap.Save(ms, ImageFormat.Png);
                return ms.ToArray();
            }
        }
    }
    
    public class Utils : IUtils
    {
        public string RandomBytes(int length)
        {
            string chars = "abcdefghjklmnopqrstuvwxyz0123456789";
            var stringChars = new char[length];
            Random random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            string password = new String(stringChars);

            return password;
        }

        public PaginationResult<Type> CreatePaginationResult<Type>(List<Type> data, PaginationFilter filter, int totalRecords)
        {
            PaginationResult<Type> result = new PaginationResult<Type>(data, filter.page_number, filter.page_size);
            double totalPages = ((double)totalRecords / (double)filter.page_size);
            
            result.total_pages = Convert.ToInt32(Math.Ceiling(totalPages));
            result.total_records = totalRecords;

            return result;
        }
    
        public string GetFieldsSql(GetFieldsSqlParams getFieldsSqlParams)
        {
            string _as = getFieldsSqlParams._as;
            string action = getFieldsSqlParams.action;
            string[] fields = getFieldsSqlParams.fields;

            if(_as != null)
            {
                _as += ".";
            }

            string fieldsToUpdate = "";
            for (int index = 0; index < fields.Length; index++)
            {
                string field = fields[index];
                bool isLastField = index == fields.Length - 1;

                if(action == "UPDATE")
                {
                    fieldsToUpdate += $"{_as}{field} = @{field}";
                }
                else
                {
                    fieldsToUpdate += $"{_as}{field}";
                }
                
                if(!isLastField)
                {
                    fieldsToUpdate += $", ";
                }
            }

            Console.WriteLine(fieldsToUpdate);
            return fieldsToUpdate;
        }
    
        public string GetBulkSql(string[] ids)
        {
            string bulkSql = "{";
            for (int i = 0; i < ids.Length; i++)
            {
                bool isLastId = i == ids.Length - 1;

                string id = ids[i];
                bulkSql += $"\"{id}\"";

                if(!isLastId)
                {
                    bulkSql += ",";
                }
            }

            bulkSql += "}";

            return bulkSql;
        }

        public Installment[] GetInstallments(float subtotal, int max = 8)
        {
            List<Installment> installments = new List<Installment>();

            // VERIFICAR SE O TOTAL É PARCELÁVEL
            if(subtotal < 100)
            {
                Installment installment = new Installment();
                installment.number = 1;
                installment.value = subtotal;
                installments.Add(installment);
            }

            int maxLength = subtotal < 800 ? (int)Math.Truncate(subtotal / 100) : max;

            for (int i = 1; i <= maxLength; i++)
            {
                float value = subtotal / i;
                Installment installment = new Installment();
                installment.number = i;
                installment.value = value;

                installments.Add(installment);
            }

            return installments.ToArray();
        }

        public string GetBookingStatus(string chargeStatus)
        {
            string bookingStatus = "";

            if(chargeStatus == "paid" || chargeStatus == "overpaid")
            {
                bookingStatus = "approved";
            }
            if(chargeStatus == "processing" || chargeStatus == "pending" || chargeStatus == "underpaid")
            {
                bookingStatus = "pending";
            }
            if(chargeStatus == "failed")
            {
                bookingStatus = "not_approved";
            }

            return bookingStatus;
        }

        public string RemoveWhiteSpaces(string value)
        {
            value = String.Join("", value.Split(" "));
            return value;
        }
    
        public bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    string domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException e)
            {
                return false;
            }
            catch (ArgumentException e)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        public string GetExpCard(int exp_month, int exp_year)
        {
            string readable_exp_month = exp_month.ToString().Length == 1 ? $"0{exp_month}" : exp_month.ToString();
            string readable_exp_year = exp_year.ToString().Split("0")[1];
            string readable_exp_card = $"{readable_exp_month}/{readable_exp_year}";

            return readable_exp_card;
        }
    
        public string DateTimeToSql(DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public string GenerateQRCode(string value)
        {
            QRCodeGenerator qRCodeGenerator = new QRCodeGenerator();
            QRCodeData qRCodeData = qRCodeGenerator.CreateQrCode(value, QRCodeGenerator.ECCLevel.Q);

            QRCode qRCode = new QRCode(qRCodeData);
            Bitmap qRBitmap = qRCode.GetGraphic(60);

            byte[] BitmapArray = qRBitmap.BitmapToByteArray();
            string qRUri = string.Format("data:image/png;base64,{0}", Convert.ToBase64String(BitmapArray));

            return qRUri;
        }
    
        public string RandomHexString()
        {
            Random random = new Random();
            Byte[] bytes = new Byte[6];
            random.NextBytes(bytes);

            string[] hexArray = Array.ConvertAll(bytes, x => x.ToString("X2"));
            string hexStr = String.Concat(hexArray);

            return hexStr.ToLower();
        }
    }
}
