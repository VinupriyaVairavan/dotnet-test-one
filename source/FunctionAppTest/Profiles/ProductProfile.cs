using AutoMapper;
using FunctionAppTest.Data;
using FunctionAppTest.Models.Request;
using FunctionAppTest.Models.Response;

namespace FunctionAppTest.Profiles;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<CreateProductRequest, Product>();
        CreateMap<UpdateProductRequest, Product>();
        
        CreateMap<CreateProductItemRequest, ProductItem>();
        CreateMap<UpdateProductItemRequest, ProductItem>();
        
        CreateMap<Product, CreateProductResponse>();
        CreateMap<Product, GetProductResponse>();
        CreateMap<Product, UpdateProductResponse>();
        
        CreateMap<ProductItem, CreateProductItemResponse>();
        CreateMap<ProductItem, GetProductItemResponse>();
    }
}