namespace Uhppoted.Tests

open System
open System.Net

open NUnit.Framework
open uhppoted

[<TestFixture>]
type TestResolve() =
    [<Test>]
    member this.TestResolveUint32() =
        let expected: C =
            { controller = 405419896u
              endpoint = None
              protocol = None }

        match Uhppoted.resolve (405419896u) with
        | Ok response -> Assert.That(response, Is.EqualTo(expected))
        | Error err -> Assert.Fail(err)

    [<Test>]
    member this.TestResolveC() =
        let expected: C =
            { controller = 405419896u
              endpoint = None
              protocol = None }

        let controller =
            { controller = 405419896u
              endpoint = None
              protocol = None }

        match Uhppoted.resolve (controller) with
        | Ok response -> Assert.That(response, Is.EqualTo(expected))
        | Error err -> Assert.Fail(err)

    [<Test>]
    member this.TestResolveCWithEndPoint() =
        let expected: C =
            { controller = 405419896u
              endpoint = Some(IPEndPoint.Parse("192.169.1.100:60000"))
              protocol = None }

        let controller =
            { controller = 405419896u
              endpoint = Some(IPEndPoint.Parse("192.169.1.100:60000"))
              protocol = None }

        match Uhppoted.resolve (controller) with
        | Ok response -> Assert.That(response, Is.EqualTo(expected))
        | Error err -> Assert.Fail(err)

    [<Test>]
    member this.TestResolveCWithEndPointAndProtocol() =
        let expected: C =
            { controller = 405419896u
              endpoint = Some(IPEndPoint.Parse("192.169.1.100:60000"))
              protocol = Some("tcp") }

        let controller =
            { controller = 405419896u
              endpoint = Some(IPEndPoint.Parse("192.169.1.100:60000"))
              protocol = Some("tcp") }

        match Uhppoted.resolve (controller) with
        | Ok response -> Assert.That(response, Is.EqualTo(expected))
        | Error err -> Assert.Fail(err)

    [<Test>]
    member this.TestResolveInt32() =
        let expected =
            "unsupported controller type (System.Int32) - expected uint32 or struct"

        match Uhppoted.resolve (405419896) with
        | Ok response -> Assert.Fail()
        | Error err -> Assert.That(err, Is.EqualTo(expected))
