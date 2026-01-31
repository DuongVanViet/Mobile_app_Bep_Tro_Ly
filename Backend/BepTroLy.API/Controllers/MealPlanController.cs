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
    public class MealPlanController : ControllerBase
    {
        private readonly IMealPlanService _mealPlanService;

        public MealPlanController(IMealPlanService mealPlanService)
        {
            _mealPlanService = mealPlanService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MealPlanDto>>> GetUserMealPlans()
        {
            var userId = GetUserIdFromToken();
            if (userId <= 0)
                return Unauthorized(new { message = "Invalid user token" });

            var mealPlans = await _mealPlanService.GetUserMealPlansAsync(userId);
            return Ok(mealPlans);
        }

        [HttpGet("week")]
        public async Task<ActionResult<IEnumerable<MealPlanDto>>> GetWeekMealPlans([FromQuery] DateTime? startDate)
        {
            var userId = GetUserIdFromToken();
            if (userId <= 0)
                return Unauthorized(new { message = "Invalid user token" });

            var start = startDate ?? DateTime.Today;
            var mealPlans = await _mealPlanService.GetMealPlansForWeekAsync(userId, start);
            return Ok(mealPlans);
        }

        [HttpGet("{mealPlanId}")]
        public async Task<ActionResult<MealPlanDto>> GetMealPlanById(int mealPlanId)
        {
            try
            {
                var mealPlan = await _mealPlanService.GetMealPlanByIdAsync(mealPlanId);
                
                // Verify ownership
                var userId = GetUserIdFromToken();
                if (mealPlan.UserId != userId)
                    return Forbid();

                return Ok(mealPlan);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<MealPlanDto>> CreateMealPlan([FromBody] CreateMealPlanRequest request)
        {
            var userId = GetUserIdFromToken();
            if (userId <= 0)
                return Unauthorized(new { message = "Invalid user token" });

            try
            {
                var mealPlan = await _mealPlanService.CreateMealPlanAsync(userId, request);
                return CreatedAtAction(nameof(GetMealPlanById), new { mealPlanId = mealPlan.MealPlanId }, mealPlan);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPut("{mealPlanId}")]
        public async Task<IActionResult> UpdateMealPlan(int mealPlanId, [FromBody] UpdateMealPlanRequest request)
        {
            try
            {
                var mealPlan = await _mealPlanService.GetMealPlanByIdAsync(mealPlanId);
                
                // Verify ownership
                var userId = GetUserIdFromToken();
                if (mealPlan.UserId != userId)
                    return Forbid();

                await _mealPlanService.UpdateMealPlanAsync(mealPlanId, request);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpDelete("{mealPlanId}")]
        public async Task<IActionResult> DeleteMealPlan(int mealPlanId)
        {
            try
            {
                var mealPlan = await _mealPlanService.GetMealPlanByIdAsync(mealPlanId);
                
                // Verify ownership
                var userId = GetUserIdFromToken();
                if (mealPlan.UserId != userId)
                    return Forbid();

                await _mealPlanService.DeleteMealPlanAsync(mealPlanId);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        private int GetUserIdFromToken()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            return int.TryParse(userIdClaim?.Value, out var userId) ? userId : -1;
        }
    }
}
