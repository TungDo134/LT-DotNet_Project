using System.Security.Cryptography;
using System.Text;

namespace WebBanLapTop.Helpers
{
    public static class MaHoaMK
    {


        public static string ToSHA1(string str)
        {

            try
            {
                using (SHA1 sha1 = SHA1.Create())
                {
                    // Chuyển đổi chuỗi thành mảng byte
                    byte[] dataBytes = Encoding.UTF8.GetBytes(str);

                    // Tính toán SHA-1
                    byte[] sha1Bytes = sha1.ComputeHash(dataBytes);

                    // Mã hóa mảng byte thành chuỗi Base64
                    string rs = Convert.ToBase64String(sha1Bytes);
                    return rs;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }

       
    }
}
