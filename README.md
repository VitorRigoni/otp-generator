# OTP Generator

An implementation of the OTP generator algo in F#.
This uses the standard time-based implementation according to RFC 6238 https://tools.ietf.org/html/rfc6238

Uses standard 6 digit/30 seconds.

Usage:

```fsharp
open Otp

let key = getUserKey // Save this to your DB and present to the user (usually done as a QR Code)

// Verify the user input
let userKey = "" // you got this from your DB
let result = getHotp key
if result == userInput then
  // do your thing (login successful maybe?)

```

## Next steps

- Add QR code generator
- Add sample
- Tests?