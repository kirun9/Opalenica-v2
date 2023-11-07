// Copyright (c) Krystian Pawełek PKMK. All rights reserved.

namespace Opalenica.Updater;
using System.Security.Cryptography;
using System.Text;

public static class Hasher
{
    [System.Diagnostics.DebuggerStepThrough()]
    public static string HashFile(string filePath, HashType type)
    {
        return type switch
        {
            HashType.MD5 => MakeHashString(CalculateHash(MD5.Create(), filePath)),
            HashType.SHA1 => MakeHashString(CalculateHash(SHA1.Create(), filePath)),
            HashType.SHA256 => MakeHashString(CalculateHash(SHA256.Create(), filePath)),
            HashType.SHA384 => MakeHashString(CalculateHash(SHA384.Create(), filePath)),
            HashType.SHA512 => MakeHashString(CalculateHash(SHA512.Create(), filePath)),
            _ => "",
        };
    }

    [System.Diagnostics.DebuggerStepThrough()]
    private static byte[] CalculateHash(HashAlgorithm argorithm, string filePath)
    {
        using FileStream stream = File.OpenRead(filePath);
        return argorithm.ComputeHash(stream);
    }

    [System.Diagnostics.DebuggerStepThrough()]
    private static string MakeHashString(byte[] hash)
    {
        StringBuilder sb = new StringBuilder(hash.Length * 2);
        foreach (byte b in hash)
            sb.Append(b.ToString("X2").ToLower());
        return sb.ToString();

    }
}