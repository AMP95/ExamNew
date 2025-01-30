﻿using MediatorServices.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace Exam.FileManager
{
    public class HashService : IHashService
    {
        public string GetHash(string original)
        {
            SHA256 sha = SHA256.Create();
            byte[] hashed = sha.ComputeHash(Encoding.UTF8.GetBytes(original));
            string qwe = BitConverter.ToString(hashed).Replace("-", "").ToLowerInvariant();
            sha.Clear();
            return qwe;
        }
    }
}
