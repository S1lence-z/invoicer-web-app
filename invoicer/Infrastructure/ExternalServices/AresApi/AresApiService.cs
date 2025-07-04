﻿using System.Net;
using System.Text.Json;
using Application.ExternalServiceInterfaces;
using Refit;
using Application.Interfaces;
using Application.AresApi;

namespace Infrastructure.ExternalServices.AresApi
{
	public class AresApiService : IAresApiService
	{
		private readonly IAresApi _aresApi;
		private const string baseUrl = "https://ares.gov.cz/ekonomicke-subjekty-v-be/rest";
		private static readonly JsonSerializerOptions jsonSerializeOptions = new() { PropertyNameCaseInsensitive = true };

		public AresApiService()
		{
			_aresApi = RestService.For<IAresApi>(baseUrl);
		}

		public async Task<IApiResult<IAresApiResponse>> GetEntityInformationByIcoAsync(string ico)
		{
			try
			{
				HttpResponseMessage response = await _aresApi.GetEntityInformationByIcoAsync(ico);
				string content = await response.Content.ReadAsStringAsync();
				if (response.IsSuccessStatusCode)
				{
					SubjectInformation? data = JsonSerializer.Deserialize<SubjectInformation>(content, jsonSerializeOptions);
					return AresApiResult<SubjectInformation>.Success(data!);
				}
				AresApiErrorResponse? errorData = JsonSerializer.Deserialize<AresApiErrorResponse>(content, jsonSerializeOptions);
				return HandleErrorResponse(response.StatusCode, errorData!);
			}
			catch (Exception ex)
			{
				return AresApiResult<IAresApiResponse>.Failure(null, $"Unexpected error: {ex.Message}", 500);
			}
		}

		private static IApiResult<IAresApiResponse> HandleErrorResponse(HttpStatusCode statusCode, AresApiErrorResponse errorResponse)
		{
			string message = $"{(int)statusCode}: {errorResponse?.Popis ?? "Unknown Error"}";
			return AresApiResult<AresApiErrorResponse>.Failure(errorResponse, message, (int)statusCode);
		}
	}
}
