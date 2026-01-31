using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using BepTroLy.Application.DTOs;
using BepTroLy.Application.Interfaces;

namespace BepTroLy.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RecommendationController : ControllerBase
    {
        private readonly IRecommendationService _recommendationService;

        public RecommendationController(IRecommendationService recommendationService)
        {
            _recommendationService = recommendationService;
        }

        /// <summary>
        /// Get recipe recommendations based on available pantry ingredients
        /// </summary>
        /// <returns>List of recipes scored by ingredient match percentage</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RecipeDto>>> GetRecommendations()
        {
            var userId = GetUserIdFromToken();
            if (userId <= 0)
                return Unauthorized(new { message = "Invalid user token" });

            var recommendations = await _recommendationService.GetRecommendationsAsync(userId);
            return Ok(recommendations);
        }

        private int GetUserIdFromToken()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            return int.TryParse(userIdClaim?.Value, out var userId) ? userId : -1;
        }
    }
}
