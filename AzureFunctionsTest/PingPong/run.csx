#r "Newtonsoft.Json"

using System;
using System.Net;
using Newtonsoft.Json;

public static async Task<object> Run(HttpRequestMessage req, TraceWriter log)
{
    log.Info($"Webhook was triggered!");

    string jsonContent = await req.Content.ReadAsStringAsync();
    dynamic data = JsonConvert.DeserializeObject(jsonContent);

    if (data.ping == null) {
        return req.CreateResponse(HttpStatusCode.BadRequest, new {
            error = "Please pass ping property in the input object"
        });
    }

    return req.CreateResponse(HttpStatusCode.OK, new {
        pong = data.ping
    });
}
