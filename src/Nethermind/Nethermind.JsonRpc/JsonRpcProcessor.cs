/*
 * Copyright (c) 2018 Demerzel Solutions Limited
 * This file is part of the Nethermind library.
 *
 * The Nethermind library is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Lesser General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * The Nethermind library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 * GNU Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public License
 * along with the Nethermind. If not, see <http://www.gnu.org/licenses/>.
 */


using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nethermind.Core;
using Nethermind.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Nethermind.JsonRpc
{
    public class JsonRpcProcessor : IJsonRpcProcessor
    {
        private readonly IJsonRpcService _jsonRpcService;
        private readonly IJsonSerializer _jsonSerializer;
        private readonly JsonSerializer _traceSerializer;
        private readonly ILogger _logger;

        public JsonRpcProcessor(IJsonRpcService jsonRpcService, IJsonSerializer jsonSerializer, ILogManager logManager)
        {
            _jsonRpcService = jsonRpcService ?? throw new ArgumentNullException(nameof(jsonRpcService));
            _jsonSerializer = jsonSerializer ?? throw new ArgumentNullException(nameof(jsonSerializer));
            var jsonSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            foreach (var converter in _jsonRpcService.Converters)
            {
                jsonSettings.Converters.Add(converter);
            }

            _traceSerializer = JsonSerializer.Create(jsonSettings);
            _logger = logManager?.GetClassLogger() ?? throw new ArgumentNullException(nameof(logManager));
        }

        public async Task<JsonRpcResult> ProcessAsync(string request)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            (JsonRpcRequest Model, List<JsonRpcRequest> Collection) rpcRequest;
            try
            {
                rpcRequest = _jsonSerializer.DeserializeObjectOrArray<JsonRpcRequest>(request);
            }
            catch (Exception ex)
            {
                Metrics.JsonRpcRequestDeserializationFailures++;
                if (_logger.IsError) _logger.Error($"Error during parsing/validation, request: {request}", ex);
                var response = _jsonRpcService.GetErrorResponse(ErrorType.ParseError, "Incorrect message");
                TraceResult(response);
                return JsonRpcResult.Single(response);
            }

            if (rpcRequest.Model != null)
            {
                if (_logger.IsInfo) _logger.Info($"JSON RPC request {rpcRequest.Model.Method}");
                
                Metrics.JsonRpcRequests++;
                var response = await _jsonRpcService.SendRequestAsync(rpcRequest.Model);
                if (response.Error != null)
                {
                    if (_logger.IsError)
                        _logger.Error($"Failed to respond to {rpcRequest.Model.Method} {response.Error.Message}");
                    Metrics.JsonRpcErrors++;
                }
                else
                {
                    if (_logger.IsDebug) _logger.Debug($"Responded to {rpcRequest.Model.Method}");
                    Metrics.JsonRpcSuccesses++;
                }

                TraceResult(response);
                stopwatch.Stop();
                if (_logger.IsInfo) _logger.Info($"  {rpcRequest.Model.Method} handled in {stopwatch.ElapsedMilliseconds}ms");
                return JsonRpcResult.Single(response);
            }

            if (rpcRequest.Collection != null)
            {
                if (_logger.IsInfo) _logger.Info($"{rpcRequest.Collection.Count} JSON RPC requests");
                
                var responses = new List<JsonRpcResponse>();
                int requestIndex = 0;
                foreach (var jsonRpcRequest in rpcRequest.Collection)
                {
                    if (_logger.IsInfo) _logger.Info($"  {requestIndex}/{rpcRequest.Collection.Count} JSON RPC request - {jsonRpcRequest.Method}");
                    
                    Metrics.JsonRpcRequests++;
                    var response = await _jsonRpcService.SendRequestAsync(jsonRpcRequest);
                    if (response.Error != null)
                    {
                        if (_logger.IsError)
                            _logger.Error($"Failed to respond to {jsonRpcRequest.Method} {response.Error.Message}");
                        Metrics.JsonRpcErrors++;
                    }
                    else
                    {
                        if (_logger.IsDebug) _logger.Debug($"Responded to {jsonRpcRequest.Method}");
                        Metrics.JsonRpcSuccesses++;
                    }

                    responses.Add(response);
                }

                TraceResult(responses);
                stopwatch.Stop();
                if (_logger.IsInfo) _logger.Info($"  {rpcRequest.Collection.Count} requests handled in {stopwatch.ElapsedMilliseconds}ms");
                return JsonRpcResult.Collection(responses);
            }

            Metrics.JsonRpcInvalidRequests++;
            var errorResponse = _jsonRpcService.GetErrorResponse(ErrorType.InvalidRequest, "Invalid request");
            TraceResult(errorResponse);
            stopwatch.Stop();
            if (_logger.IsInfo) _logger.Info($"  Failed request handled in {stopwatch.ElapsedMilliseconds}ms");
            return JsonRpcResult.Single(errorResponse);
        }

        private void TraceResult(JsonRpcResponse response)
        {
            if (_logger.IsTrace)
            {
                var builder = new StringBuilder();
                using (var stringWriter = new StringWriter(builder))
                using (var jsonWriter = new JsonTextWriter(stringWriter))
                {
                    _traceSerializer.Serialize(jsonWriter, response);
                }
                
                _logger.Trace($"Sending JSON RPC response: {builder}");
            }
        }
        
        private void TraceResult(List<JsonRpcResponse> responses)
        {
            if (_logger.IsTrace)
            {
                var builder = new StringBuilder();
                using (var stringWriter = new StringWriter(builder))
                using (var jsonWriter = new JsonTextWriter(stringWriter))
                {
                    _traceSerializer.Serialize(jsonWriter, responses);
                }
                
                _logger.Trace($"Sending JSON RPC response: {builder}");
            }
        }
    }
}