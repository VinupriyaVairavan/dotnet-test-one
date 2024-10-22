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
        CreateMap<Product, CreateProductResponse>();
        CreateMap<Product, GetProductResponse>();
        CreateMap<Product, UpdateProductResponse>();
    }
}