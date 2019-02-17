//#region init
#r @".\packages\xunit.assert\lib\netstandard1.1\xunit.assert.dll"
#r @".\packages\FSharp.Data\lib\netstandard2.0\FSharp.Data.dll"
#load  @".\packages\SchlenkR.FsHttp\lib\netstandard2.0\FsHttp.fsx"

open FSharp.Data
open FSharp.Data.JsonExtensions
open FsHttp
open FsHttp.Dsl
open Xunit
//#endregion

http {
    GET "http://localhost:8080/persons"
}
|> toJson
|> JsonExtensions.AsArray
|> fun arr -> arr.Length > 2
|> Assert.True












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
