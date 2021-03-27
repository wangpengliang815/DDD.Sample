using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace DotNetCore.Infra.Safety
{
    internal enum CryptoMode
    {
        AES,
        DES
    }
    internal class SymmetricCrypto
    {
        private readonly SymmetricAlgorithm m_symAlgorithm = null;
        public string Seed { get; set; } = "undefinedseed";

        public SymmetricCrypto(CryptoMode cryptoMode)
        {
            switch (cryptoMode)
            {
                case CryptoMode.AES:
                    m_symAlgorithm = new RijndaelManaged();
                    m_symAlgorithm.GenerateKey();
                    m_symAlgorithm.GenerateIV();
                    break;
                case CryptoMode.DES:
                    m_symAlgorithm = new DESCryptoServiceProvider();
                    m_symAlgorithm.GenerateKey();
                    m_symAlgorithm.GenerateIV();
                    break;
                default:
                    throw new Exception("It doesn't support this SymmetricAlgorithm！");
            }
        }

        /// <summary>
        /// 加密文件流
        /// </summary>
        /// <param name="inStream"></param>
        /// <param name="outStream"></param>
        public void Encrypt(Stream inStream, Stream outStream)
        {
            Encrypt(inStream, outStream, Seed);
        }
        /// <summary>
        ///  加密文件流
        /// </summary>
        /// <param name="inStream"></param>
        /// <param name="outStream"></param>
        /// <param name="seed"></param>
        public void Encrypt(Stream inStream, Stream outStream, string seed)
        {
            byte[] Key = GetKey(seed);
            byte[] IV = GetIV(seed);

            ICryptoTransform transform = m_symAlgorithm.CreateEncryptor(Key, IV);
            SymmetricCrypt(inStream, outStream, transform);
        }

        /// <summary>
        /// 解密文件流
        /// </summary>
        /// <param name="inStream"></param>
        /// <param name="outStream"></param>
        public void Decrypt(Stream inStream, Stream outStream)
        {
            Decrypt(inStream, outStream, Seed);
        }
        /// <summary>
        /// 解密文件流
        /// </summary>
        /// <param name="inStream"></param>
        /// <param name="outStream"></param>
        /// <param name="seed"></param>
        public void Decrypt(Stream inStream, Stream outStream, string seed)
        {
            byte[] Key = GetKey(seed);
            byte[] IV = GetIV(seed);

            ICryptoTransform transform = m_symAlgorithm.CreateDecryptor(Key, IV);
            SymmetricCrypt(inStream, outStream, transform);
        }

        /// <summary>
        /// 加密字符串
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public string EncryptString(string source)
        {
            return EncryptString(source, Seed);
        }
        /// <summary>
        /// 加密字符串
        /// </summary>
        /// <param name="source">待加密的串</param>
        /// <param name="seed">加密种子</param>
        /// <returns>经过加密的串</returns>
        public string EncryptString(string source, string seed)
        {
            if (string.IsNullOrEmpty(source))
            {
                return string.Empty;
            }

            string result = "";

            MemoryStream inStream = null;
            MemoryStream outStream = null;
            try
            {
                byte[] byteIn = UTF8Encoding.UTF8.GetBytes(source);
                inStream = new MemoryStream(byteIn, 0, byteIn.Length);
                outStream = new MemoryStream();
                Encrypt(inStream, outStream, seed);
                byte[] bytOut = outStream.ToArray();
                result = Convert.ToBase64String(bytOut);
            }
            finally
            {
                if (outStream != null)
                {
                    outStream.Close();
                }

                if (inStream != null)
                {
                    inStream.Close();
                }
            }

            return result;
        }

        /// <summary>
        /// 解密字符串
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public string DecryptString(string source)
        {
            return DecryptString(source, Seed);
        }
        /// <summary>
        /// 解密字符串
        /// </summary>
        /// <param name="source">待解密的串</param>
        /// <param name="seed">加密种子</param>
        /// <returns>经过解密的串</returns>
        public string DecryptString(string source, string seed)
        {
            if (string.IsNullOrEmpty(source))
            {
                return string.Empty;
            }

            string result = "";
            MemoryStream inStream = null;
            MemoryStream outStream = null;
            try
            {
                byte[] bytIn = Convert.FromBase64String(source);
                inStream = new MemoryStream(bytIn, 0, bytIn.Length);
                outStream = new MemoryStream();
                Decrypt(inStream, outStream, seed);
                byte[] byteOut = outStream.ToArray();
                result = UTF8Encoding.UTF8.GetString(byteOut);
            }
            finally
            {
                if (outStream != null)
                {
                    outStream.Close();
                }

                if (inStream != null)
                {
                    inStream.Close();
                }
            }

            return result;
        }

        /// <summary>
        /// 对称加密文件流
        /// </summary>
        /// <param name="inStream"></param>
        /// <param name="outStream"></param>
        /// <param name="transform"></param>
        private void SymmetricCrypt(Stream inStream, Stream outStream, ICryptoTransform transform)
        {
            /* set to begin */
            if (inStream.Position != 0 && inStream.CanSeek)
            {
                inStream.Seek(0, SeekOrigin.Begin);
            }
            /* clear out stream */
            outStream.SetLength(0);
            //Create cryptoStream
            CryptoStream cryptoStream = new CryptoStream(outStream, transform, CryptoStreamMode.Write);
            //Read from the input file, then encrypt and write to the output file.
            CopyStream(inStream, cryptoStream);
            /* Flush stream */
            cryptoStream.FlushFinalBlock();
        }

        private byte[] GetKey(string seed)
        {
            byte[] key = Encoding.UTF8.GetBytes(seed);
            if (key.Length != m_symAlgorithm.Key.Length)
            {
                Array.Resize(ref key, m_symAlgorithm.Key.Length);
            }
            return key;
        }

        private byte[] GetIV(string seed)
        {
            byte[] iv = Encoding.UTF8.GetBytes(seed);
            Array.Reverse(iv);
            if (iv.Length != m_symAlgorithm.IV.Length)
            {
                Array.Resize(ref iv, m_symAlgorithm.IV.Length);
            }
            return iv;
        }

        private void CopyStream(Stream inStream, Stream outStream)
        {
            const int IO_Block_Size = 1024;
            int readBytes = 0;
            byte[] buffer = new byte[IO_Block_Size];

            while (true)
            {
                readBytes = inStream.Read(buffer, 0, IO_Block_Size);
                if (readBytes > 0)
                {
                    outStream.Write(buffer, 0, readBytes);
                }
                else
                {
                    break;
                }
            } //while
            outStream.Flush();
        }
    }
}
