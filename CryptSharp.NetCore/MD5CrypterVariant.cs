﻿namespace CryptSharp.NetCore;

/// <summary>
///     Modified versions of the MD5 crypt algorithm.
/// </summary>
public enum MD5CrypterVariant
{
    /// <summary>
    ///     Standard MD5 crypt.
    /// </summary>
    Standard,

    /// <summary>
    ///     Apache htpasswd files have a different prefix.
    ///     Due to the nature of MD5 crypt, this also affects the crypted password.
    /// </summary>
    Apache
}