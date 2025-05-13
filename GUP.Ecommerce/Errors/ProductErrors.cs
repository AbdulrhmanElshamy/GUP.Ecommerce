using GUP.Ecommerce.Abstractions;

namespace GUP.Ecommerce.Errors;

public record ProductErrors
{

    public static readonly Error ProductNotFound =
    new("Product.ProductNotFound", "Product is not found", StatusCodes.Status404NotFound);

    public static readonly Error ProducImageRequired=
new("Product.ProducImageRequired", "Product image is required", StatusCodes.Status400BadRequest);

    public static readonly Error ProducImageInvalid=
new("Product.ProducImageInvalid", "Product image is invalid", StatusCodes.Status400BadRequest);

    public static readonly Error CannotDeleteProduct =
new("Product.CannotDeleteProduct", "Cannot delete Product with associated order", StatusCodes.Status400BadRequest);

}
