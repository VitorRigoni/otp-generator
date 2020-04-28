namespace OtpGenerator.UnitTests

module OtpTests =

    open Xunit
    open OtpGenerator.Otp
    open FsUnit.TopLevelOperators

    [<Fact>]
    let ``Getting a HOTP should have length 6`` () =
        getUserKey
        |> getHotp
        |> should haveLength 6

    [<Fact>]
    let ``A HOTP should always be a positive number`` () =
        for _ in [1..1000] do
            getUserKey
            |> getHotp
            |> int
            |> should greaterThanOrEqualTo 0