namespace Uhppoted.Tests

open System
open NUnit.Framework
open uhppoted

[<TestFixture>]
type TestClass() =
    [<Test>]
    member this.TestEvenSequence() =
        printfn "** nothing to do **"
        Assert.True(true)
