
#r "netstandard"
#r "./packages/Suave/lib/netstandard2.0/Suave.dll"
#r @"./packages/Newtonsoft.Json/lib/netstandard2.0/Newtonsoft.Json.dll"

open System
open System.Reflection
open System.Threading

open Newtonsoft.Json
open Suave
open Suave.Filters
open Suave.Utils.Collections
open Suave.Operators
open Suave.Successful

[<AutoOpen>]
module Helper =
    let keyNotFoundString = "KEY_NOT_FOUND"
    let query key (r: HttpRequest) = defaultArg (Option.ofChoice (r.query ^^ key)) keyNotFoundString
    let header key (r: HttpRequest) = defaultArg (Option.ofChoice (r.header key)) keyNotFoundString
    let form key (r: HttpRequest) = defaultArg (Option.ofChoice (r.form ^^ key)) keyNotFoundString
    let toJson (o:obj) = o |> JsonConvert.SerializeObject
    let writeJson = Writers.setMimeType "application/json; charset=utf-8"
    let fromJson<'a> json = JsonConvert.DeserializeObject(json, typeof<'a>) :?> 'a
    let getJson<'a> (req : HttpRequest) =
      let getString rawForm = System.Text.Encoding.UTF8.GetString(rawForm)
      req.rawForm |> getString |> fromJson<'a>

    let startServer app =
        let cts = new CancellationTokenSource()
        let config =
            { defaultConfig with
                cancellationToken = cts.Token
                bindings = [ HttpBinding.createSimple HTTP "127.0.0.1" 8080 ] }
        let _,server = startWebServerAsync config app
        Async.Start(server, cts.Token)
        fun() ->
            cts.Cancel()
            cts.Dispose()

type Person = { name:string; age:int }

let mutable persons = [
    { name = "Steffi"; age = 25 }
    { name = "Hans"; age = 45 }
    { name = "Gabi"; age = 43 }
    { name = "Jonas"; age = 32 }
]

let app =
    choose [
        GET >=> choose [
            path "/info" >=> OK "Hello world server v0.1"
            path "/persons"
                >=> request (
                    fun r -> persons |> List.sortBy (fun p -> p.name) |> toJson
                    >> OK
                )
                >=> writeJson
            // now: Extend our API with basic filters
            // path "/persons" >=> request (
            //     fun r ->
            //         let filter = r |> query "filter"
            //         let propName,value = (String.split ',' filter) |> (fun x -> x.[0],x.[1])

            //         let prop =
            //             typeof<Person>.GetProperties()
            //             |> Seq.filter (fun p -> p.Name = propName)
            //             |> Seq.exactlyOne

            //         persons
            //         |> Seq.map (fun p -> prop.GetValue p)
            //         |> Seq.map (fun p -> string p)
            //         |> Seq.filter (fun v -> v = value)
            //         |> toJson
            //     >> OK
            // )
        ]
        PUT >=> choose [
            path "/persons" 
            >=> request (fun r ->
                let person = getJson<Person> r
                persons <- persons @ [person]
                OK ""
            )
        ]
    ]

let stopServer = startServer app
