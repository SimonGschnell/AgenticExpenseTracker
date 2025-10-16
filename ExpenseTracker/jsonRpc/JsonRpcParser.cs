namespace ExpenseTracker.jsonRpc;

using System;
using System.Text.Json;

public static class JsonRpcParser
{
    public static (JsonRpcMessage,JsonRpcMessageTypes) Parse(string json)
    {
        using var doc = JsonDocument.Parse(json);
        var root = doc.RootElement;

        if (root.TryGetProperty("method", out _))
        {
            return (JsonSerializer.Deserialize<JsonRpcRequest>(json)!,JsonRpcMessageTypes.JsonRpcRequest);
        }

        if (root.TryGetProperty("result", out _))
        {
            return (JsonSerializer.Deserialize<JsonRpcSuccessResponse>(json)!, JsonRpcMessageTypes.JsonRpcResponse);
        }

        if (root.TryGetProperty("error", out _))
        {
            return (JsonSerializer.Deserialize<JsonRpcErrorResponse>(json)!,JsonRpcMessageTypes.JsonRpcError);
        }

        throw new InvalidOperationException("Unknown JSON-RPC message type");
    }
}
