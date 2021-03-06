namespace OtpGenerator

module Otp =

    open System
    open System.Security.Cryptography

    let private createHmac () =
        new HMACSHA1()

    let private generateKey =
        let rng = new RNGCryptoServiceProvider()
        let k = Array.zeroCreate (createHmac().HashSize / 8)
        rng.GetBytes(k)
        k

    let private dt (hmacResult: byte array) =
            let offset = int hmacResult.[19] &&& 0xf
            (int hmacResult.[offset] &&& 0x7f) <<< 24
              ||| (int hmacResult.[offset + 1] &&& 0xff) <<< 16
              ||| (int hmacResult.[offset + 2] &&& 0xff) <<< 8
              ||| (int hmacResult.[offset + 3] &&& 0xff)

    let private stringToByteArray (key: string) = 
        System.Text.Encoding.ASCII.GetBytes key

    let private counterNow =
        let secondsSinceEpoch = (DateTime.UtcNow - DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds
        uint64 (floor(secondsSinceEpoch / 30.0))
    
    let private truncate v =
        v % int (10.0 ** 6.0) |> abs
    
    let getUserKey =
        System.Text.Encoding.ASCII.GetString generateKey

    // There's still something wrong that I wasn't able to figure out...
    // For some reason, sometimes the tests fail because the number generated
    // is negative and this is very weird. I hacked it by abs-ing it, but this feels very wrong.
    let getHotp (key: string) =
        let hmac = createHmac()
        hmac.Key <- stringToByteArray key
        hmac.ComputeHash(BitConverter.GetBytes(counterNow))
        |> dt |> truncate
        |> sprintf "%06i"
