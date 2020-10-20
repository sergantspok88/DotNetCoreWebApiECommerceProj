using AutoMapper;
using Ecommwebapi.Data.Dtos;
using Ecommwebapi.Data.Models;
using Ecommwebapi.Data.Services;
using Ecommwebapi.Entities;
using Ecommwebapi.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace Ecommwebapi.Controllers
{
    [Route("api")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService categoryService;
        private readonly IMapper mapper;
        private readonly ILogger<CategoriesController> logger;

        public CategoriesController(ICategoryService categoryService,
            IMapper mapper,
            ILogger<CategoriesController> logger)
        {
            this.mapper = mapper;
            this.categoryService = categoryService;
            this.logger = logger;
        }

        //Categories----------------------------------------
        [HttpGet("categories")]
        public ActionResult<IEnumerable<CategoryReadDto>> GetAllCategories()
        {
            var categories = categoryService.GetAllCategories();

            return Ok(mapper.Map<IEnumerable<CategoryReadDto>>(categories));
        }

        [Authorize(Roles = Role.Admin)]
        [HttpPost("categories")]
        public IActionResult AddCategory([FromBody] CategoryAddWriteDto model)
        {
            var category = mapper.Map<Category>(model);
            try
            {
                var newCategory = categoryService.CreateCategory(category);

                return Created(new Uri($"{Request.Path}/{newCategory.Id}", UriKind.Relative),
                    mapper.Map<Category, CategoryReadDto>(newCategory));
            }
            catch (AppException ex)
            {
                return BadRequest(new ResponseError
                {
                    Message = ex.Message
                });
            }
        }

        [Authorize(Roles = Role.Admin)]
        [HttpPut("categories/{id}")]
        //for now can use ProductAddWriteDto, ProductUpdateWriteModel would be the same
        public IActionResult UpdateCategory(int id, [FromBody] CategoryAddWriteDto model)
        {
            var category = mapper.Map<Category>(model);
            category.Id = id;

            try
            {
                categoryService.UpdateCategory(category);
                return Ok();
            }
            catch (AppException ex)
            {
                return BadRequest(new ResponseError
                {
                    Message = ex.Message
                });
            }
        }

        [Authorize(Roles = Role.Admin)]
        [HttpDelete("categories/{name}")]
        public IActionResult DeleteCategory(string name)
        {
            categoryService.DeleteCategory(name);
            return Ok();
        }
    }
}
