﻿using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace 关键字排名查询
{
    /// <summary>
    /// MD5
    /// </summary>
    public class MD5_
    {
        public MD5_()
        { 
        }
        public string Encrypt(string Source)
        {
            byte[] data = UTF8Encoding.UTF8.GetBytes(Source);
            // This is one implementation of the abstract class MD5.
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] result = md5.ComputeHash(data);
            return  Convert.ToBase64String(result);

        }
        public byte[] Encrypt(byte[] Source)
        {          
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] result = md5.ComputeHash(Source);
            return result;
        }
    }
}
