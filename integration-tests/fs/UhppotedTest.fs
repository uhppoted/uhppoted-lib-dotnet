namespace FSharp.Tests

open System
open NUnit.Framework
open stub

[<TestFixture>]
type TestClass() =
    [<OneTimeSetUp>]
    member this.Initialise() = 
        TestContext.Error.WriteLine("=========>OneTimeSetUp")

    [<OneTimeTearDown>]
    member this.Terminate() = 
        TestContext.Error.WriteLine("=========>OneTimeTearDown")

    [<SetUp>]
    member this.Setup() = 
        printfn ">>>> setup"

    [<TearDown>]
    member this.TearDown() = 
        printfn ">>>> teardown"

    [<Test>]
    member this.TestEvenSequence() = Assert.That(true, Is.True)

    [<Test>]
    member this.TestEvenSequence2() = Assert.That(true, Is.True)
