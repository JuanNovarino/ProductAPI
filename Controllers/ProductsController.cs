using Microsoft.AspNetCore.Mvc;
using ProductApi.Models.DTOs.Requests;
using ProductApi.Services.Implementations;

namespace ProductApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    // Requisito etapas 3 a 5: instanciar con new
    private ProductService _service = new ProductService();

    [HttpGet]
    public IActionResult GetAll()
    {
        var products = _service.GetAllProducts();
        return Ok(products);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var product = _service.GetProductById(id);
        if (product == null)
        {
            return NotFound();
        }

        return Ok(product);
    }

    [HttpPost]
    public IActionResult Create([FromBody] ProductForCreateDto dto)
    {
        var createdProduct = _service.CreateProduct(dto);
        return CreatedAtAction(nameof(GetById), new { id = createdProduct.Id }, createdProduct);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] ProductForUpdateDto dto)
    {
        var existing = _service.GetProductById(id);
        if (existing == null)
        {
            return NotFound();
        }

        _service.UpdateProduct(id, dto);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var existing = _service.GetProductById(id);
        if (existing == null)
        {
            return NotFound();
        }

        _service.DeleteProduct(id);
        return NoContent();
    }
}