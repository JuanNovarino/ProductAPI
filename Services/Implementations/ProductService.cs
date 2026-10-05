using ProductApi.Entities;
using ProductApi.Models.DTOs.Requests;
using ProductApi.Models.DTOs.Responses;
using ProductApi.Repositories.Interfaces;
using ProductApi.Services.Interfaces;

namespace ProductApi.Services.Implementations;

public class ProductService : IProductService
{
    private readonly IProductRepository _repository;

    // Constructor con Inyección de Dependencias (Etapa 9)
    public ProductService(IProductRepository repository)
    {
        _repository = repository;
    }

    public List<ProductForReadDto> GetAllProducts()
    {
        return _repository.GetAllProducts()
            .Select(p => new ProductForReadDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price
            })
            .ToList();
    }

    public ProductForReadDto? GetProductById(int id)
    {
        var product = _repository.GetProductById(id);
        if (product == null) return null;

        return new ProductForReadDto
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price
        };
    }

    public ProductForReadDto CreateProduct(ProductForCreateDto dto)
    {
        // Regla de negocio: no permitir nombres repetidos (Etapa 8)
        bool exists = _repository.GetAllProducts()
            .Any(p => p.Name.Equals(dto.Name, StringComparison.OrdinalIgnoreCase));

        if (exists)
        {
            throw new InvalidOperationException("Ya existe un producto con el mismo nombre.");
        }

        var entity = new Product
        {
            Name = dto.Name,
            Price = dto.Price
        };

        _repository.AddProduct(entity);

        return new ProductForReadDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Price = entity.Price
        };
    }

    public void UpdateProduct(int id, ProductForUpdateDto dto)
    {
        var entity = new Product
        {
            Id = id,
            Name = dto.Name,
            Price = dto.Price
        };

        _repository.UpdateProduct(entity);
    }

    public void DeleteProduct(int id)
    {
        var product = _repository.GetProductById(id);
        if (product != null)
        {
            _repository.DeleteProduct(product);
        }
    }

    public List<ProductForReadDto> SearchProductsByName(string name)
    {
        return _repository.SearchProductsByName(name)
            .Select(p => new ProductForReadDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price
            })
            .ToList();
    }

    public ProductStatsDto GetStats()
    {
        var products = _repository.GetAllProducts();

        // Evita excepción si la lista está vacía (Etapa 7.2)
        if (products.Count == 0)
        {
            return new ProductStatsDto
            {
                Total = 0,
                AveragePrice = 0m,
                MostExpensiveName = "N/A"
            };
        }

        return new ProductStatsDto
        {
            Total = products.Count,
            AveragePrice = Math.Round(products.Average(p => p.Price), 2),
            MostExpensiveName = products.OrderByDescending(p => p.Price).First().Name
        };
    }
}