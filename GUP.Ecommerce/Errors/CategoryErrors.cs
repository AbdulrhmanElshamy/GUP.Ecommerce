using GUP.Ecommerce.Abstractions;

namespace GUP.Ecommerce.Errors;

public record CategoryErrors
{

    public static readonly Error CategoryNotFound =
    new("Category.CategoryNotFound", "Category is not found", StatusCodes.Status404NotFound);

    public static readonly Error CannotDeleteCategory =
new("Category.CannotDeleteCategory", "Cannot delete category with associated products", StatusCodes.Status400BadRequest);

}