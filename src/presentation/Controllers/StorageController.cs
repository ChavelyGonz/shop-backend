using Domain.Models;
using Application.Features.GeneralPropose.Helpers;
using Application.Features.Storage.DTOs;
using Application.Features.Storage.Commands;
using Application.Features.Storage.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StorageController : ApiBaseController
    {
        private readonly Helper<Product> _productHelper;
        public StorageController(ISender sender, Helper<Product> productHelper) : base(sender) 
        { 
            _productHelper = productHelper;
        }

        /// <summary>
        /// Creates a new product and associates it with specified stores.
        /// </summary>
        /// <param name="command">Contains product details and the list of store IDs where the product should be available.</param>
        /// <returns>The created product dto, including its generated ID.</returns>
        /// <response code="200">Returns the created product.</response>
        [HttpPost("AddProduct")]
        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Create a new product", Description = "Creates a new product and links it to specified stores.")]
        public async Task<IActionResult> AddProduct(CreateProductCommand command)
        {
            var result = await Sender.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves all products with optional pagination.
        /// </summary>
        /// <param name="pageNumber">Page number (default is 1)</param>
        /// <param name="pageSize">Page size (default is 10)</param>
        /// <returns>List of products as DTOs.</returns>
        /// <response code="200">Returns list of products.</response>
        [HttpGet("GetAllProducts")]
        [ProducesResponseType(typeof(List<ProductDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Get all products", Description = "Retrieves paginated list of products.")]
        public async Task<IActionResult> GetAllProducts([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var products = await Sender.Send(new GetAllProductsQuery(pageNumber, pageSize));
            return Ok(products);
        }


        /// <summary>
        /// Get the total number of products in the database.
        /// </summary>
        /// <remarks>
        /// This endpoint retrieves the total count of products without loading them into memory.
        /// It's optimized: performs a direct SQL COUNT(*) query in the database.
        /// </remarks>
        /// <returns>
        /// Returns an integer representing the total number of products.
        /// </returns>
        /// <response code="200">Returns the total number of products</response>
        [HttpGet("TotalCount")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Get total product count",
            Description = "Returns the total number of products in the database using an efficient COUNT(*) query."
        )]
        public async Task<IActionResult> GetTotalProductCount()
        {
            // Efficient call to DB: SELECT COUNT(*) FROM products
            var totalCount = await _productHelper.TotalCount();

            // Return HTTP 200 OK with the total count
            return Ok(totalCount);
        }


        /// <summary>
        /// Deletes a product by its ID.
        /// </summary>
        /// <param name="id">ID of the product to delete.</param>
        /// <response code="200">Product was successfully deleted.</response>
        /// <response code="404">Product not found.</response>
        [HttpDelete("DeleteProductById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            Summary = "Delete product by ID",
            Description = "Deletes an existing product by its ID. Returns 404 if product not found."
        )]
        public async Task<IActionResult> DeleteProductById([FromQuery] int id)
        {
            await _productHelper.DeleteAsync(id);
            return Ok();
        }

        /// <summary>
        /// Edits an existing product.
        /// </summary>
        /// <param name="productDto">Product DTO containing updated details. Must include the product ID.</param>
        /// <response code="200">Product was successfully updated.</response>
        /// <response code="404">Product not found.</response>
        [HttpPut("EditProduct")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            Summary = "Edit existing product",
            Description = "Updates product details. The productDto must include an existing product ID."
        )]
        public async Task<IActionResult> EditProduct([FromBody] ProductDto productDto)
        {
            await _productHelper.EditAsync(productDto);
            return Ok();
        }
    }
}
