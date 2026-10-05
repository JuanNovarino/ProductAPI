using Microsoft.AspNetCore.Mvc;
using ProductApi.Models.DTOs.Requests;
using ProductApi.Services.Interfaces;

namespace ProductApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _service;

    // Constructor con Inyección de Dependencias (Etapa 9)
    public ProductsController(IProductService service)
    {
        _service = service;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var products = _service.GetAllProducts();
        return Ok(products);
    }

    [HttpGet("{id:int}")]
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
        try
        {
            var createdProduct = _service.CreateProduct(dto);
            return CreatedAtAction(nameof(GetById), new { id = createdProduct.Id }, createdProduct);
        }
        catch (InvalidOperationException ex)
        {
            // Retorna 409 Conflict si el producto ya existe (Etapa 8)
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
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

    [HttpDelete("{id:int}")]
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

    // GET api/products/search?name=xxx (Etapa 7.1)
    [HttpGet("search")]
    public IActionResult Search([FromQuery] string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return BadRequest(new { message = "El término de búsqueda no puede estar vacío." });
        }

        var results = _service.SearchProductsByName(name);
        return Ok(results);
    }

    // GET api/products/stats (Etapa 7.2)
    [HttpGet("stats")]
    public IActionResult GetStats()
    {
        var stats = _service.GetStats();
        return Ok(stats);
    }
}