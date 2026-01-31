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
    public class IngredientController : ControllerBase
    {
        private readonly IIngredientService _ingredientService;

        public IngredientController(IIngredientService ingredientService)
        {
            _ingredientService = ingredientService;
        }

        // ============= Ingredient Category Endpoints =============
        [HttpGet("categories")]
        public async Task<ActionResult<IEnumerable<IngredientCategoryDto>>> GetAllCategories()
        {
            var categories = await _ingredientService.GetAllCategoriesAsync();
            return Ok(categories);
        }

        [HttpGet("categories/{categoryId}")]
        public async Task<ActionResult<IngredientCategoryDto>> GetCategoryById(int categoryId)
        {
            try
            {
                var category = await _ingredientService.GetCategoryByIdAsync(categoryId);
                return Ok(category);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost("categories")]
        public async Task<ActionResult<IngredientCategoryDto>> CreateCategory([FromBody] CreateIngredientCategoryRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.CategoryName))
                return BadRequest(new { message = "Category name is required" });

            var category = await _ingredientService.CreateCategoryAsync(request);
            return CreatedAtAction(nameof(GetCategoryById), new { categoryId = category.CategoryId }, category);
        }

        [HttpPut("categories/{categoryId}")]
        public async Task<IActionResult> UpdateCategory(int categoryId, [FromBody] CreateIngredientCategoryRequest request)
        {
            try
            {
                await _ingredientService.UpdateCategoryAsync(categoryId, request);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpDelete("categories/{categoryId}")]
        public async Task<IActionResult> DeleteCategory(int categoryId)
        {
            try
            {
                await _ingredientService.DeleteCategoryAsync(categoryId);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        // ============= Ingredient Endpoints =============
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<IngredientDto>>> GetAllIngredients()
        {
            var ingredients = await _ingredientService.GetAllIngredientsAsync();
            return Ok(ingredients);
        }

        [HttpGet("{ingredientId}")]
        public async Task<ActionResult<IngredientDto>> GetIngredientById(int ingredientId)
        {
            try
            {
                var ingredient = await _ingredientService.GetIngredientByIdAsync(ingredientId);
                return Ok(ingredient);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<IngredientDto>> CreateIngredient([FromBody] CreateIngredientRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.IngredientName))
                return BadRequest(new { message = "Ingredient name is required" });

            var ingredient = await _ingredientService.CreateIngredientAsync(request);
            return CreatedAtAction(nameof(GetIngredientById), new { ingredientId = ingredient.IngredientId }, ingredient);
        }

        [HttpPut("{ingredientId}")]
        public async Task<IActionResult> UpdateIngredient(int ingredientId, [FromBody] UpdateIngredientRequest request)
        {
            try
            {
                await _ingredientService.UpdateIngredientAsync(ingredientId, request);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpDelete("{ingredientId}")]
        public async Task<IActionResult> DeleteIngredient(int ingredientId)
        {
            try
            {
                await _ingredientService.DeleteIngredientAsync(ingredientId);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        // ============= User Pantry Endpoints =============
        [HttpGet("pantry")]
        public async Task<ActionResult<IEnumerable<UserIngredientDto>>> GetUserPantry()
        {
            var userId = GetUserIdFromToken();
            if (userId <= 0)
                return Unauthorized(new { message = "Invalid user token" });

            var pantry = await _ingredientService.GetUserPantryAsync(userId);
            return Ok(pantry);
        }

        [HttpGet("pantry/{userIngredientId}")]
        public async Task<ActionResult<UserIngredientDto>> GetUserIngredient(int userIngredientId)
        {
            try
            {
                var userIngredient = await _ingredientService.GetUserIngredientAsync(userIngredientId);
                
                // Verify ownership
                var userId = GetUserIdFromToken();
                if (userIngredient.UserId != userId)
                    return Forbid();

                return Ok(userIngredient);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost("pantry")]
        public async Task<ActionResult<UserIngredientDto>> AddIngredientToPantry([FromBody] AddUserIngredientRequest request)
        {
            var userId = GetUserIdFromToken();
            if (userId <= 0)
                return Unauthorized(new { message = "Invalid user token" });

            try
            {
                var userIngredient = await _ingredientService.AddIngredientToPantryAsync(userId, request);
                return CreatedAtAction(nameof(GetUserIngredient), new { userIngredientId = userIngredient.UserIngredientId }, userIngredient);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPut("pantry/{userIngredientId}")]
        public async Task<IActionResult> UpdatePantryIngredient(int userIngredientId, [FromBody] UpdateUserIngredientRequest request)
        {
            try
            {
                var userIngredient = await _ingredientService.GetUserIngredientAsync(userIngredientId);
                
                // Verify ownership
                var userId = GetUserIdFromToken();
                if (userIngredient.UserId != userId)
                    return Forbid();

                await _ingredientService.UpdatePantryIngredientAsync(userIngredientId, request);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpDelete("pantry/{userIngredientId}")]
        public async Task<IActionResult> RemoveIngredientFromPantry(int userIngredientId)
        {
            try
            {
                var userIngredient = await _ingredientService.GetUserIngredientAsync(userIngredientId);
                
                // Verify ownership
                var userId = GetUserIdFromToken();
                if (userIngredient.UserId != userId)
                    return Forbid();

                await _ingredientService.RemoveIngredientFromPantryAsync(userIngredientId);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        // ============= Helper Methods =============
        private int GetUserIdFromToken()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            return int.TryParse(userIdClaim?.Value, out var userId) ? userId : -1;
        }
    }
}
