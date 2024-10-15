namespace Uhppoted.Tests

open System
open NUnit.Framework
open uhppoted

[<TestFixture>]
type TestClass() =
    [<Test>]
    member this.TestEvenSequence() =
        let expected = Seq.empty<int>
        let actual = Stub.squaresOfOdds [ 2; 4; 6; 8; 10 ]
        Assert.That(actual, Is.EqualTo(expected))
