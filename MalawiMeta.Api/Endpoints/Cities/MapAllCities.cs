﻿using MalawiMeta.Api.Extensions;
using MalawiMeta.Api.TransferObjects;
using MalawiMeta.Api.UseCases.Cities;
using Microsoft.AspNetCore.Mvc;

namespace MalawiMeta.Api.Endpoints.Cities;

public static partial class CityEndpoints
{
    private static void MapAllCities(this IEndpointRouteBuilder app)
    {
        app.MapGet(
            "/",
            [ProducesResponseType(typeof(IEnumerable<CityResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        async (HttpRequest request, HttpResponse response) =>
            {
                var context = request.HttpContext;

                var fetchAllCities = await context.GetServiceOrThrowAsync<IFetchAllCitiesUseCase>();

                var result = await fetchAllCities.ExecuteAsync(null);

                result.SwitchFirst(
                    cities => response.WriteAsJsonAsync(cities),
                    error => response.WriteAsJsonAsync(new { Message = error.Description })
                );
            }).WithName("GetCities");
    }
}

