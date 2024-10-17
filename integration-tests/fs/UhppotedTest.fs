namespace FSharp.Tests

open System
open NUnit.Framework
open stub

[<TestFixture>]
type TestClass() =

    [<Test>]
    member this.TestEvenSequence() = Assert.That(true, Is.True)
