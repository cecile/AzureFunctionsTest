#r "Newtonsoft.Json"

using System;
using System.Net;
using Newtonsoft.Json;

using System.Collections.Generic;

public static async Task<object> Run(HttpRequestMessage req, TraceWriter log)
{
    log.Info($"Webhook was triggered!");

    string jsonContent = await req.Content.ReadAsStringAsync();
    dynamic data = JsonConvert.DeserializeObject(jsonContent);

    if (data.key == null || data.msg == null || data.cipher == null ) {
        return req.CreateResponse(HttpStatusCode.BadRequest, new {
            error = "Please pass key, msg & cipher properties in the input object"
        });
    }

    Dictionary<string, string> cipher = new Dictionary<string, string>();

    foreach (var item in data.cipher){

        string name = (string) item.Name;
        string value = (string) item.Value;

        cipher.Add(value, name);
    }

    string msg = (string) data.msg;

    string result = "";

    for (int i = 0;i < (msg.Length/2); i++){
        var digit = msg.Substring(i*2,2);
        result += cipher[digit];
    }
   
    return req.CreateResponse(HttpStatusCode.OK, new {
        key = data.key,
        result = result
    });
}
