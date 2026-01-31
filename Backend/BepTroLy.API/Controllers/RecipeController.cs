using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BepTroLy.Application.DTOs;
using BepTroLy.Application.Interfaces;
using BepTroLy.API.Services;

namespace BepTroLy.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RecipeController : ControllerBase
    {
        private readonly IRecipeService _recipeService;
        private readonly IImageUploadService _imageUploadService;

        public RecipeController(IRecipeService recipeService, IImageUploadService imageUploadService)
        {
            _recipeService = recipeService;
            _imageUploadService = imageUploadService;
        }

        // ============= Recipe Category Endpoints =============
        [HttpGet("categories")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<RecipeCategoryDto>>> GetAllCategories()
        {
            var categories = await _recipeService.GetAllCategoriesAsync();
            return Ok(categories);
        }

        [HttpGet("categories/{categoryId}")]
        [AllowAnonymous]
        public async Task<ActionResult<RecipeCategoryDto>> GetCategoryById(int categoryId)
        {
            try
            {
                var category = await _recipeService.GetCategoryByIdAsync(categoryId);
                return Ok(category);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost("categories")]
        public async Task<ActionResult<RecipeCategoryDto>> CreateCategory([FromBody] CreateRecipeCategoryRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.CategoryName))
                return BadRequest(new { message = "Category name is required" });

            var category = await _recipeService.CreateCategoryAsync(request);
            return CreatedAtAction(nameof(GetCategoryById), new { categoryId = category.CategoryId }, category);
        }

        [HttpPut("categories/{categoryId}")]
        public async Task<IActionResult> UpdateCategory(int categoryId, [FromBody] CreateRecipeCategoryRequest request)
        {
            try
            {
                await _recipeService.UpdateCategoryAsync(categoryId, request);
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
                await _recipeService.DeleteCategoryAsync(categoryId);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        // ============= Recipe Endpoints =============
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<RecipeDto>>> GetAllRecipes()
        {
            var recipes = await _recipeService.GetAllRecipesAsync();
            return Ok(recipes);
        }

        [HttpGet("{recipeId}")]
        [AllowAnonymous]
        public async Task<ActionResult<RecipeDto>> GetRecipeById(int recipeId)
        {
            try
            {
                var recipe = await _recipeService.GetRecipeByIdAsync(recipeId);
                return Ok(recipe);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<RecipeDto>> CreateRecipe([FromBody] CreateRecipeRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.RecipeName))
                return BadRequest(new { message = "Recipe name is required" });

            var recipe = await _recipeService.CreateRecipeAsync(request);
            return CreatedAtAction(nameof(GetRecipeById), new { recipeId = recipe.RecipeId }, recipe);
        }

        [HttpPut("{recipeId}")]
        public async Task<IActionResult> UpdateRecipe(int recipeId, [FromBody] UpdateRecipeRequest request)
        {
            try
            {
                await _recipeService.UpdateRecipeAsync(recipeId, request);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpDelete("{recipeId}")]
        public async Task<IActionResult> DeleteRecipe(int recipeId)
        {
            try
            {
                await _recipeService.DeleteRecipeAsync(recipeId);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        // ============= Search Endpoint =============
        [HttpPost("search")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<RecipeDto>>> SearchRecipes([FromBody] SearchRecipeRequest request)
        {
            try
            {
                var recipes = await _recipeService.SearchRecipesAsync(request);
                return Ok(recipes);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
