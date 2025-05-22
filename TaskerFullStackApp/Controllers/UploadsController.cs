﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using TaskerFullStackApp.Data;
using TaskerFullStackApp.Models;

namespace TaskerFullStackApp.Controllers
{
    [Route("uploads")]
    [ApiController]
    public class UploadsController(ApplicationDbContext _context) : ControllerBase
    {
        [HttpGet("{id:guid}")]
        [OutputCache(VaryByRouteValueNames = ["id"], Duration = 60 * 60 * 24)]
        public async Task<IActionResult> GetImage(Guid id)
        {
            ImageUpload? image = await _context.Images.FirstOrDefaultAsync(i => i.Id == id);
            if (image is null) return NotFound();

            //return byte array to the frontend for conversion into a src tag displaying on the page
            return File(image.Data!, image.Type!);
        }
    }
}
