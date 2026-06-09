using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using TestDealershipApi.Api.ResponseModels;

namespace TestDealershipApi.Api
{
    public static class RequestTemplates
    {
        const string ERROR_MESSAGE = "Произошла ошибка, попробуйте еще раз";

        public static async Task<ApiResponse<T>> HandleResponse<T>(HttpResponseMessage response, bool isDelete = false)
        {
            if (response.IsSuccessStatusCode)
            {
                if (isDelete) return ApiResponse<T>.SuccessWithoutData();

                var result = await response.Content.ReadFromJsonAsync<T>();
                return ApiResponse<T>.Success(result!);
            }

            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();
                return ApiResponse<T>.Fail(error?.Message ?? "Некорректные данные");
            }

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return ApiResponse<T>.Fail("Неверный логин или пароль");
            }

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                var entityName = typeof(T).Name.ToLower();
                return ApiResponse<T>.Fail($"{entityName} не найден");
            }

            return ApiResponse<T>.Fail($"Ошибка сервера: {response.StatusCode}");
        }


        public static async Task<ApiResponse<TResponse>> PostAsync<TResponse>(HttpClient httpClient, string url, object request)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync(url, request);
                return await HandleResponse<TResponse>(response);
            }
            catch (Exception)
            {
                return ApiResponse<TResponse>.Fail(ERROR_MESSAGE);
            }
        }


        public static async Task<ApiResponse<TResponse>> PutAsync<TResponse>(HttpClient httpClient, string url, object request)
        {
            try
            {
                var response = await httpClient.PutAsJsonAsync(url, request);
                return await HandleResponse<TResponse>(response);
            }
            catch (Exception)
            {
                return ApiResponse<TResponse>.Fail(ERROR_MESSAGE);
            }
        }


        public static async Task<ApiResponse<TResponse>> GetAsync<TResponse>(HttpClient httpClient, string url)
        {
            try
            {
                var response = await httpClient.GetAsync(url);
                return await HandleResponse<TResponse>(response);
            }
            catch (Exception)
            {
                return ApiResponse<TResponse>.Fail(ERROR_MESSAGE);
            }
        }


        public static async Task<ApiResponse<TResponse>> DeleteAsync<TResponse>(HttpClient httpClient, string url)
        {
            try
            {
                var response = await httpClient.DeleteAsync(url);
                return await HandleResponse<TResponse>(response, isDelete: true);
            }
            catch (Exception)
            {
                return ApiResponse<TResponse>.Fail(ERROR_MESSAGE);
            }
        }
    }
}
