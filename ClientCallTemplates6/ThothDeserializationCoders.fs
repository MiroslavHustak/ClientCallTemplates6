namespace TestAPI

//Compiler-independent template suitable for Shared as well

//************************************************

//Compiler directives
#if FABLE_COMPILER
open Thoth.Json
#else
open Thoth.Json.Net
#endif

//**************** GET ********************

type ResponseGet = 
    {
        GetLinks : string
        Message : string
    } 

type HelloResponseGetThoth = 
    {
        Message : string
        Timestamp : string
    } 

//**************** POST ********************

type HelloResponsePostThoth = 
    {
        Message2 : string
    }  

//**************** PUT ********************

type UserPayloadThoth =
    {
        Id : int
        Name : string
        Email : string
    }

type UserResponsePutThoth = 
    {
        Message : string
        UpdatedDataTableInfo : UserPayloadThoth
    }

type ResponsePut = 
    {
        Message : string
    }

module ThothDeserializationCoders =   

    //**************** GET ********************
    
    let internal decoderGetTest : Decoder<ResponseGet> =
        Decode.object
            (fun get ->
                      {
                          GetLinks  = get.Required.Field "GetLinks" Decode.string
                          Message  = get.Required.Field "Message" Decode.string
                      }
            )

    let internal decoderGet : Decoder<HelloResponseGetThoth> =
        Decode.object
            (fun get ->
                      {
                          Message = get.Required.Field "Message" Decode.string
                          Timestamp = get.Required.Field "Timestamp" Decode.string
                      }
            ) 

    //**************** POST ********************
     
    let internal decoderPost : Decoder<HelloResponsePostThoth> =
        Decode.object
            (fun get ->
                      {
                          Message2 = get.Required.Field "Message2" Decode.string                     
                      }
            )

    //**************** PUT ********************
        
    let internal decoderPut : Decoder<UserResponsePutThoth> =
        Decode.object
            (fun get ->
                      {
                          Message = get.Required.Field "Message" Decode.string
                          UpdatedDataTableInfo =
                              get.Required.Field "UpdatedDataTableInfo"
                                  (Decode.object
                                      (fun get1 ->
                                                 {
                                                     Id = get1.Required.Field "Id" Decode.int
                                                     Name = get1.Required.Field "Name" Decode.string
                                                     Email = get1.Required.Field "Email" Decode.string
                                                 }
                                      )
                                  )
                      }
            )

    let internal decoderPutTest : Decoder<ResponsePut> =
        Decode.object
            (fun get ->
                      {
                          Message = get.Required.Field "Message" Decode.string
                      }
            )