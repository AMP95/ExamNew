using System.Security.Cryptography;
using System.Text;
using Utilities.Interfaces;

namespace Services
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
