using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using BepTroLy.Application.Interfaces;
using BepTroLy.Application.DTOs;

namespace BepTroLy.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ShoppingListController : ControllerBase
    {
        private readonly IShoppingListService _shoppingListService;

        public ShoppingListController(IShoppingListService shoppingListService)
        {
            _shoppingListService = shoppingListService;
        }

        [HttpPost("generate")]
        public async Task<IActionResult> GenerateShoppingList([FromBody] GenerateShoppingListRequest request)
        {
            if (request == null || request.StartDate >= request.EndDate)
                return BadRequest("Invalid date range");

            try
            {
                var userId = GetUserIdFromToken();
                if (userId != request.UserId)
                    return Forbid();

                var shoppingList = await _shoppingListService.GenerateFromMealPlanAsync(
                    request.UserId,
                    request.StartDate,
                    request.EndDate
                );

                return Ok(shoppingList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetShoppingList(int id)
        {
            try
            {
                var userId = GetUserIdFromToken();
                var shoppingList = await _shoppingListService.GetShoppingListByIdAsync(id);

                if (shoppingList.UserId != userId)
                    return Forbid();

                return Ok(shoppingList);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("user/all")]
        public async Task<IActionResult> GetUserShoppingLists()
        {
            try
            {
                var userId = GetUserIdFromToken();
                var shoppingLists = await _shoppingListService.GetUserShoppingListsAsync(userId);
                return Ok(shoppingLists);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateShoppingList([FromBody] CreateShoppingListRequest request)
        {
            if (request == null)
                return BadRequest("Invalid request");

            try
            {
                var userId = GetUserIdFromToken();
                if (userId != request.UserId)
                    return Forbid();

                var shoppingList = await _shoppingListService.CreateShoppingListAsync(request);
                return CreatedAtAction(nameof(GetShoppingList), new { id = shoppingList.ShoppingListId }, shoppingList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPut("{id}/item")]
        public async Task<IActionResult> UpdateShoppingListItem(int id, [FromBody] UpdateShoppingListItemRequest request)
        {
            if (request == null)
                return BadRequest("Invalid request");

            try
            {
                var userId = GetUserIdFromToken();
                var shoppingList = await _shoppingListService.GetShoppingListByIdAsync(id);

                if (shoppingList.UserId != userId)
                    return Forbid();

                var updated = await _shoppingListService.UpdateShoppingListItemAsync(id, request);
                return Ok(updated);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShoppingList(int id)
        {
            try
            {
                var userId = GetUserIdFromToken();
                var success = await _shoppingListService.DeleteShoppingListAsync(id, userId);

                if (!success)
                    return NotFound(new { message = "Shopping list not found or not authorized" });

                return Ok(new { message = "Shopping list deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        private int GetUserIdFromToken()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            return int.Parse(userIdClaim?.Value ?? "0");
        }
    }
}
