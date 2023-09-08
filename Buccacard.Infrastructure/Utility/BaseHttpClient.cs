using Buccacard.Infrastructure.DTO;
using Buccacard.Infrastructure.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

public interface IBaseHttpClient
{
    Task<ServiceResponse<T>> JSendPostAsync<T>(string baseUrl, string url, object postdata, string clientId, string accessToken);
}

public class BaseHttpClient : IBaseHttpClient
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IResponseService _responseService;
    private readonly ILogger<object>? _logger;

    public BaseHttpClient(IHttpClientFactory httpClientFactory, IResponseService responseService, ILogger<object>? logger = null)
    {
        _httpClientFactory = httpClientFactory;
        _responseService = responseService;
        _logger = logger;
    }

    public async Task<ServiceResponse<T>> JSendPostAsync<T>(string baseUrl, string url, object postdata, string clientId, string accessToken)
    {
        var client = CreateClient(clientId, accessToken);
        url = $"{baseUrl}{url}";
        using var httpResponse = await client.PostAsJsonAsync(url, postdata);
        return await ParseHttpResponse<T>(httpResponse, true);
    }

    private HttpClient CreateClient(string clientId, string accessToken)
    {
        var client = _httpClientFactory.CreateClient();
        client.Timeout = TimeSpan.FromMinutes(30);
        client.DefaultRequestHeaders.Clear();

        if (!string.IsNullOrEmpty(clientId)) client.DefaultRequestHeaders.Add("client-id", clientId);

        if (!string.IsNullOrEmpty(accessToken))
            client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Bearer {accessToken}");

        return client;
    }

    private async Task<ServiceResponse<T>> ParseHttpResponse<T>(HttpResponseMessage httpResponse,
        bool isJsendResp = false, bool useNewtonToDes = false)
    {
        switch (httpResponse.StatusCode)
        {
            case HttpStatusCode.BadRequest:
                return _responseService.ErrorResponse<T>("Your request is invalid. Check your input and try again");
            case HttpStatusCode.NotFound:
                return _responseService.ErrorResponse<T>("Kindly contact system administrator");
            case HttpStatusCode.InternalServerError:
                return _responseService.ErrorResponse<T>("An error occured on one of our systems. Please try again later");
        }

        if (!httpResponse.IsSuccessStatusCode)
        {
            if ((int)httpResponse.StatusCode == StatusCodes.Status302Found)
                return _responseService.ErrorResponse<T>("User already created");
            if ((int)httpResponse.StatusCode == StatusCodes.Status400BadRequest)
                return _responseService.ErrorResponse<T>($"Invalid model sent to sso");
            if ((int)httpResponse.StatusCode == StatusCodes.Status500InternalServerError)
                return _responseService.ErrorResponse<T>("An error occured on server");

            return _responseService.ErrorResponse<T>($"An error occured - from ssoo");
        }

        T returnValue;
        if (!isJsendResp)
        {
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            returnValue = stringResponse.FromJson<T>();
            return _responseService.SuccessResponse(returnValue, "success");
        }

        var apiResponse = await httpResponse.Content.ReadFromJsonAsync<ServiceResponse<T>>();
        if (!apiResponse?.Status ?? true) return _responseService.ErrorResponse<T>(apiResponse?.Message ?? "");
        if (apiResponse != null && apiResponse.Data != null)
        {
            return _responseService.SuccessResponse(apiResponse.Data, apiResponse?.Message ?? "success");
        }
        return _responseService.ErrorResponse<T>();
    }
}
