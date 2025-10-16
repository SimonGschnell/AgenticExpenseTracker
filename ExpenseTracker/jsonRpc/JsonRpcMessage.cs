namespace ExpenseTracker.jsonRpc;

using System;
using System.Text.Json;
using System.Text.Json.Serialization;

public record JsonRpcMessage
{
    [JsonPropertyName("jsonrpc")]
    public string JsonRpc { get; set; } = "2.0";

    [JsonPropertyName("id")]
    public int? Id { get; set; }
}

public record JsonRpcRequest : JsonRpcMessage
{
    [JsonPropertyName("method")]
    public string Method { get; set; }

    [JsonPropertyName("params")]
    public JsonElement? Params { get; set; }
}

public record JsonRpcSuccessResponse : JsonRpcMessage
{
    [JsonPropertyName("result")]
    public JsonElement? Result { get; set; }
}

public record JsonRpcErrorResponse : JsonRpcMessage
{
    [JsonPropertyName("error")]
    public JsonRpcError Error { get; set; }
}

public record JsonRpcError
{
    [JsonPropertyName("code")]
    public int Code { get; set; }

    [JsonPropertyName("message")]
    public string Message { get; set; }

    [JsonPropertyName("data")]
    public JsonElement? Data { get; set; }
}
