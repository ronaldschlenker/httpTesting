module Tests

open System
open Xunit
open FSharp.Data
open FSharp.Data.JsonExtensions
open FsHttp


[<Fact>]
let ``Persons can be retrieved`` () =
    http {
        GET "http://localhost:8080/persons"
    }
    |> toJson
    |> JsonExtensions.AsArray
    |> fun arr -> arr.Length > 2
    |> Assert.True


[<Fact>]
let ``Persons are ordered`` () =
    let names =
        http {
            GET "http://localhost:8080/persons"
        }
        |> (toJson >> JsonExtensions.AsArray)
        |> Array.map (fun x -> x?name.AsString())
    let orderedNames = names |> Array.sort

    Seq.zip names orderedNames
    |> Seq.map (fun (a,b) -> a = b)
    |> Seq.forall (fun x -> x = true)
    |> Assert.True
    
