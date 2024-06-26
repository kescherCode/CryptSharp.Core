﻿#region License

/*
CryptSharp
Copyright (c) 2011 James F. Bellinger <http://www.zer7.com/software/cryptsharp>

Permission to use, copy, modify, and/or distribute this software for any
purpose with or without fee is hereby granted, provided that the above
copyright notice and this permission notice appear in all copies.

THE SOFTWARE IS PROVIDED "AS IS" AND THE AUTHOR DISCLAIMS ALL WARRANTIES
WITH REGARD TO THIS SOFTWARE INCLUDING ALL IMPLIED WARRANTIES OF
MERCHANTABILITY AND FITNESS. IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR
ANY SPECIAL, DIRECT, INDIRECT, OR CONSEQUENTIAL DAMAGES OR ANY DAMAGES
WHATSOEVER RESULTING FROM LOSS OF USE, DATA OR PROFITS, WHETHER IN AN
ACTION OF CONTRACT, NEGLIGENCE OR OTHER TORTIOUS ACTION, ARISING OUT OF
OR IN CONNECTION WITH THE USE OR PERFORMANCE OF THIS SOFTWARE.
*/

#endregion

using System;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using CryptSharp.NetCore.Utility;

namespace CryptSharp.NetCore.Demo.BlowfishTest;

internal static class TestVectors
{
    public static void Test()
    {
        Console.Write("Testing Blowfish");
        using (var stream =
               Assembly.GetEntryAssembly()!.GetManifestResourceStream("CryptSharp.NetCore.Demo.Blowfish.TestVectors.txt")!)
        {
            using StreamReader reader = new(stream);
            string line;
            while ((line = reader.ReadLine()!) is { })
            {
                var match = Regex.Match(line, @"^([0-9A-z]{16})\s*([0-9A-z]{16})\s*([0-9A-z]{16})$");
                if (!match.Success) { continue; }

                string key = match.Groups[1].Value, clear = match.Groups[2].Value, cipher = match.Groups[3].Value;
                var keyBytes = Base16Encoding.Hex.GetBytes(key);
                var clearBytes = Base16Encoding.Hex.GetBytes(clear);

                Console.Write(".");
                using var fish = BlowfishCipher.Create(keyBytes);
                var testCipherBytes = new byte[8];
                fish.Encipher(clearBytes, 0, testCipherBytes, 0);
                var testCipher = Base16Encoding.Hex.GetString(testCipherBytes);
                if (cipher != testCipher)
                {
                    Console.WriteLine("WARNING: Encipher failed test ({0} became {1})", cipher, testCipher);
                }

                var testClearBytes = new byte[8];
                fish.Decipher(testCipherBytes, 0, testClearBytes, 0);
                var testClear = Base16Encoding.Hex.GetString(testClearBytes);
                if (clear != testClear)
                {
                    Console.WriteLine("WARNING: Decipher failed ({0} became {1})", clear, testClear);
                }
            }
        }

        Console.WriteLine("done.");
    }
}