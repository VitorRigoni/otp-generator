module GenerateOtp

open System.Security.Cryptography
open System

let createHmac = new HMACSHA1()

let generateKey =
    let rng = new RNGCryptoServiceProvider()
    let k = Array.zeroCreate (createHmac.HashSize / 8)
    rng.GetBytes(k)
    k

let dt (hmacResult: byte array) =
    let offset = int hmacResult.[19] &&& 0xf
    (int hmacResult.[offset] &&& 0x7f) <<< 24
      ||| (int hmacResult.[offset + 1] &&& 0xff) <<< 16
      ||| (int hmacResult.[offset + 2] &&& 0xff) <<< 8
      ||| (int hmacResult.[offset + 3] &&& 0xff)

let truncate (hmacResult: byte array) = dt hmacResult % int (10.0 ** 6.0)

let counterNow =
    let secondsSinceEpoch = (DateTime.UtcNow - DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds
    uint64 (Math.Floor(secondsSinceEpoch / 30.0))

let HOTP (key: byte array) (C: uint64) =
    let hmac = createHmac
    hmac.Key <- key
    hmac.ComputeHash(BitConverter.GetBytes(C)) |> truncate
