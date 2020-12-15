using System;
using System.Text;
using System.IO;
using System.Security;
using System.Security.Cryptography;


namespace Utilities.Common
{

    public static class EncryptDecryptHelper
    {
        private static readonly byte[] SALT = { 0x26, 0xdc, 0xff, 0x00, 0xad, 0xed, 0x7a, 0xee, 0xc5, 0xfe, 0x07, 0xaf, 0x4d, 0x08, 0x22, 0x3c };
        private static readonly string PASSWORD = "HAWADDS";

        private static Rijndael CreateKey()
        {
            Rijndael rijndael = Rijndael.Create();
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(PASSWORD, SALT);
            rijndael.Key = pdb.GetBytes(32);
            rijndael.IV = pdb.GetBytes(16);

            return rijndael;
        }

        ///<summary>
        /// Encrypts a file using Rijndael algorithm.
        ///</summary>
        ///<param name="inputFile"></param>
        ///<param name="outputFile"></param>
        public static void EncryptFile(string inputFile, string outputFile)
        {
            var key = CreateKey();

            string cryptFile = outputFile;
            FileStream fsCrypt = new FileStream(cryptFile, FileMode.Create);

            CryptoStream cs = new CryptoStream(fsCrypt,
                key.CreateEncryptor(),
                CryptoStreamMode.Write);

            FileStream fsIn = new FileStream(inputFile, FileMode.Open);

            int data;
            while ((data = fsIn.ReadByte()) != -1)
            {
                cs.WriteByte((byte)data);
            }

            fsIn.Close();
            cs.Close();
            fsCrypt.Close();

        }

        ///<summary>
        /// Decrypts a file using Rijndael algorithm.
        ///</summary>
        ///<param name="inputFile"></param>
        ///<param name="outputFile"></param>
        public static void DecryptFile(string inputFile, string outputFile)
        {
            var key = CreateKey();

            FileStream fsCrypt = new FileStream(inputFile, FileMode.Open);

            CryptoStream cs = new CryptoStream(fsCrypt,
                 key.CreateDecryptor(),
                CryptoStreamMode.Read);

            FileStream fsOut = new FileStream(outputFile, FileMode.Create);

            int data;
            while ((data = cs.ReadByte()) != -1)
            {
                fsOut.WriteByte((byte)data);
            }

            fsOut.Close();
            cs.Close();
            fsCrypt.Close();
        }

        ///<summary>
        /// Encrypts bytes array using Rijndael algorithm.
        ///</summary>
        ///<param name="input"></param>
        public static byte[] EncryptBytes(byte[] input)
        {
            var key = CreateKey();

            using (MemoryStream stream = new MemoryStream())
            {
                using (AesCryptoServiceProvider aesProvider = new AesCryptoServiceProvider())
                {
                    using (CryptoStream cs = new CryptoStream(stream, key.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(input, 0, input.Length);
                    }
                }
                var result = stream.ToArray();
                return result;
            }
        }

        ///<summary>
        /// Decrypts bytes array using Rijndael algorithm.
        ///</summary>
        ///<param name="encryptBytes"></param>
        public static byte[] DecryptBytes(byte[] encryptBytes)
        {
            var key = CreateKey();

            using (MemoryStream stream = new MemoryStream())
            {
                using (AesCryptoServiceProvider aesProvider = new AesCryptoServiceProvider())
                {
                    using (CryptoStream cs = new CryptoStream(stream, key.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(encryptBytes, 0, encryptBytes.Length);
                    }
                }
                var result = stream.ToArray();
                return result;
            }
        }
    }
}
