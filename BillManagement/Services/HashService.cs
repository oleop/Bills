using System.Security.Cryptography;
using System.Text;
using BillManagement.Services.Interfaces;

namespace BillManagement.Services;

public class HashService: IHashService
{
    public string Hash(string input)
    {
        using SHA256 sha256 = SHA256.Create();
        byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
        StringBuilder builder = new StringBuilder();

        foreach (byte b in hashBytes)
        {
            builder.Append(b.ToString("x2"));
        }

        return builder.ToString();
    }
}