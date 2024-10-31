namespace TestAPI

open System
open System.IO
open System.Net

open FsHttp
open Thoth.Json.Net

open ThothSerializationCoders
open ThothDeserializationCoders

open MyFsToolkit
open System.Threading
open MyFsToolkit.Builders

//CLIENT CALL TEMPLATES
//REST API

module CallRestApiThoth =
        
    let private apiKey = "my-api-key"
    let private apiKeyTest = "test74764"

    //************************* GET ****************************

    let getFromRestApiTest () = 

        async 
            {
                let url = "http://kodis.somee.com/api/" // ensure trailing slash if required
            
                let! response = 
                    http 
                        {
                            GET url
                            header "X-API-KEY" apiKeyTest 
                        }
                    |> Request.sendAsync  
    
                match response.statusCode with
                | HttpStatusCode.OK 
                    -> 
                     let! jsonString = Response.toTextAsync response 

                     return
                         Decode.fromString decoderGetTest jsonString   
                         |> function
                             | Ok value  ->
                                          value                                
                             | Error err ->
                                          { 
                                              GetLinks = String.Empty
                                              Message = err
                                          } 
                | _ -> 
                     return 
                         { 
                             GetLinks = String.Empty
                             Message  = sprintf "Request failed with status code %d" (int response.statusCode)
                         } 
            }    

    let getFromRestApi () =

        async
            {
                let url = "http://localhost:8080/" 
                
                let! response = 
                    http 
                        {
                            GET url
                            header "X-API-KEY" apiKey 
                        }
                    |> Request.sendAsync                                

                //let! response = get >> Request.sendAsync <| url  
                let! jsonString = Response.toTextAsync response 
                
                return
                    Decode.fromString decoderGet jsonString   
                    |> function
                        | Ok value ->
                                    value
                        | Error _  ->  
                                    { 
                                        Message = String.Empty
                                        Timestamp = String.Empty
                                    }                
            }

    let getFromRestApiWithParam () =
    
            async
                {
                    let url = "http://localhost:8080/api/greetings/greet?name=Alice"
    
                    let! response = 
                        http 
                            {
                                GET url
                                header "X-API-KEY" apiKey 
                            }
                        |> Request.sendAsync 

                    //let! response = get >> Request.sendAsync <| url  
                    let! jsonString = Response.toTextAsync response 
                    
                    return
                        Decode.fromString decoderGet jsonString   
                        |> function
                            | Ok value ->
                                        value
                            | Error _  ->  
                                        { 
                                            Message = String.Empty
                                            Timestamp = String.Empty
                                        }                               
                }

    //************************* POST ****************************
    
    let postToRestApi () =
        async
            {
                //let url = "http://localhost:8080/"
                let url = "http://localhost:8080/api/greetings/greet"
                
                let payload = 
                    {
                        Name = "Alice"
                    }               
                
                let thothJsonPayload = Encode.toString 2 (encoderPost payload)

                let! response = 
                    http
                        {
                            POST url
                            header "X-API-KEY" apiKey 
                            body                              
                            json thothJsonPayload
                        }
                    |> Request.sendAsync            

                let! jsonString = Response.toTextAsync response 
                
                return
                    Decode.fromString decoderPost jsonString   
                    |> function
                        | Ok value ->
                                    value
                        | Error _  ->  
                                    { 
                                        Message2 = String.Empty     
                                    }                
            } 
    
    //************************* PUT ****************************

    let putToRestApiTest () =

        let getJsonString path =

            try
                pyramidOfDoom
                    {
                        let filepath = Path.GetFullPath path |> Option.ofNullEmpty 
                        let! filepath = filepath, Error (sprintf "%s%s" "Chyba při čtení cesty k souboru " path)

                        let fInfodat : FileInfo = FileInfo filepath
                        let! _ =  fInfodat.Exists |> Option.ofBool, Error (sprintf "Soubor %s nenalezen" path) 
                 
                        use fs = new FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.None) 
                        let! _ = fs |> Option.ofNull, Error (sprintf "%s%s" "Chyba při čtení dat ze souboru " filepath)                        
                    
                        use reader = new StreamReader(fs) //For large files, StreamReader may offer better performance and memory efficiency
                        let! _ = reader |> Option.ofNull, Error (sprintf "%s%s" "Chyba při čtení dat ze souboru " filepath) 
                
                        let jsonString = reader.ReadToEnd()
                        let! jsonString = jsonString |> Option.ofNullEmpty, Error (sprintf "%s%s" "Chyba při čtení dat ze souboru " filepath)                      
                                  
                        return Ok jsonString 
                    }
            with
            | ex -> Error (string ex.Message)

        async
            {
                let path = "CanopyResults/canopy_results.json"                
                let url = "http://kodis.somee.com/api/" 
                                                  
                let thothJsonPayload =                    
                    match getJsonString path with
                    | Ok jsonString -> jsonString                                  
                    | Error _       -> String.Empty            
           
                let! response = 
                    http
                        {
                            PUT url
                            header "X-API-KEY" apiKeyTest 
                            body 
                            json thothJsonPayload
                        }
                    |> Request.sendAsync       
                    
                match response.statusCode with
                | HttpStatusCode.OK 
                    -> 
                     let! jsonMsg = Response.toTextAsync response

                     return                          
                         Decode.fromString decoderPutTest jsonMsg   
                         |> function
                             | Ok value  -> value   
                             | Error err -> { Message1 = String.Empty; Message2 = err }      
                | _ -> 
                     return { Message1 = String.Empty; Message2 = sprintf "Request failed with status code %d" (int response.statusCode) }                                           
            } 

    let putToRestApi () =
        async
            {
                let url = "http://localhost:8080/user" // !!! /user 

                let payload = 
                    {
                        Id = 2
                        Name = "Robert"
                        Email = "robert@example.com"
                    }
 
                let thothJsonPayload = Encode.toString 2 (encoderPut payload)  

                let! response = 
                    http
                        {
                            PUT url
                            header "X-API-KEY" apiKey 
                            body 
                            json thothJsonPayload
                        }
                    |> Request.sendAsync   
                 
                let! jsonString = Response.toTextAsync response 
                                   
                return
                    Decode.fromString decoderPut jsonString   
                    |> function
                        | Ok value ->
                                    value
                        | Error _  ->  
                                    { 
                                        Message = String.Empty
                                        UpdatedDataTableInfo = 
                                            {
                                                Id = -1
                                                Name = String.Empty
                                                Email = String.Empty     
                                            }
                                    }                             
            } 

    let runRestApiThoth () = 

        printfn "*********************** PUT **********************************************"

        let response = putToRestApiTest () |> Async.RunSynchronously
        printfn "Message1 %s" response.Message1
        printfn "Message2 %s" response.Message2

        printfn "*********************** GET **********************************************"

        Thread.Sleep 5000

        let response = getFromRestApiTest () |> Async.RunSynchronously
        printfn "Message2 %s" response.Message
        printfn "Json %A \n" response.GetLinks

        (*
        let response = getFromRestApi () |> Async.RunSynchronously
        printfn "\n\nMessageGet: %s\nTimestamp: %s" response.Message response.Timestamp

        let response1 = getFromRestApiWithParam () |> Async.RunSynchronously
        printfn "\n\nMessageGetWithParam: %s\nTimestamp: %s" response1.Message response1.Timestamp

        let response2 = postToRestApi () |> Async.RunSynchronously
        printfn "\n\nMessagePost: %s" response2.Message 
        
        let response3 = putToRestApi () |> Async.RunSynchronously
        printfn "\n\nMessagePut: %s" response3.Message 
        printfn "Updated: %A" response3.UpdatedDataTableInfo
        *)