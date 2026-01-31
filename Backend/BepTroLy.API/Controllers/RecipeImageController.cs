using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using BepTroLy.Application.Interfaces;
using BepTroLy.API.Services;

namespace BepTroLy.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RecipeImageController : ControllerBase
    {
        private readonly IRecipeService _recipeService;
        private readonly IImageUploadService _imageUploadService;

        public RecipeImageController(IRecipeService recipeService, IImageUploadService imageUploadService)
        {
            _recipeService = recipeService;
            _imageUploadService = imageUploadService;
        }

        [HttpPost("{recipeId}/upload")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadImage(int recipeId, IFormFile image)
        {
            if (image == null || image.Length == 0)
                return BadRequest(new { message = "Image file is required" });

            try
            {
                var recipe = await _recipeService.GetRecipeByIdAsync(recipeId);
                if (recipe == null)
                    return NotFound(new { message = "Recipe not found" });

                if (!string.IsNullOrEmpty(recipe.ImageUrl))
                {
                    await _imageUploadService.DeleteImageAsync(recipe.ImageUrl);
                }

                var imageUrl = await _imageUploadService.UploadImageAsync(image, "recipes");
                await _recipeService.UpdateRecipeImageAsync(recipeId, imageUrl);

                return Ok(new { message = "Image uploaded successfully", imageUrl });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpDelete("{recipeId}")]
        public async Task<IActionResult> DeleteImage(int recipeId)
        {
            try
            {
                var recipe = await _recipeService.GetRecipeByIdAsync(recipeId);
                if (recipe == null)
                    return NotFound(new { message = "Recipe not found" });

                if (string.IsNullOrEmpty(recipe.ImageUrl))
                    return BadRequest(new { message = "Recipe has no image" });

                await _imageUploadService.DeleteImageAsync(recipe.ImageUrl);
                await _recipeService.UpdateRecipeImageAsync(recipeId, null);

                return Ok(new { message = "Image deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
