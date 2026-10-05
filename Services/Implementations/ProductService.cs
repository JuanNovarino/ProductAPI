using ProductApi.Entities;
using ProductApi.Models.DTOs.Requests;
using ProductApi.Models.DTOs.Responses;
using ProductApi.Repositories.Implementations;
using ProductApi.Services.Interfaces;

namespace ProductApi.Services.Implementations;

public class ProductService : IProductService
{
    private ProductRepository _repository = new ProductRepository();

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
}