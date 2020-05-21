using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace PARLIB
{
    public class Cry : IDisposable
    {
        TripleDESCryptoServiceProvider tdes;
        ICryptoTransform Encryptor;
        ICryptoTransform Decryptor;
        public Cry()
        {
            tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = UTF8Encoding.UTF8.GetBytes("Leninsnameshalln");
            //                                                   1234567890123456 
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;
            Encryptor = tdes.CreateEncryptor();
            Decryptor = tdes.CreateDecryptor();
        }
        ~Cry()
        {
            Dispose(false);
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool _disposing)
        {
            if (_disposing)
            {
                if (tdes == null)
                    return;
                tdes.Clear();
                tdes.Dispose();
                tdes = null;
            }
        }
        public string to(string _s)
        {
            byte[] toEncryptArray = System.Text.Encoding.Default.GetBytes(_s);
            byte[] resultArray = Encryptor.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return (ByteArrayToString(resultArray));
        }
        public string from(string _s)
        {
            byte[] resultArray = null;
            try
            {
                byte[] toEncryptArray = StringToByteArray(_s);
                resultArray = Decryptor.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            }
            catch
            {
                return (null);
            }
            return (System.Text.Encoding.Default.GetString(resultArray));
        }
        public string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }
        public byte[] StringToByteArray(String hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < bytes.Length; i++)
                bytes[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
            return bytes;
        }
    }
}
