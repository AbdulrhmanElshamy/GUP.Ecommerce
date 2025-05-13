namespace GUP.Ecommerce.Errors;

public record DiscountErrors
{

    public static readonly Error DiscountNotFound =
    new("Discount.DiscountNotFound", "Discount is not found", StatusCodes.Status404NotFound);

    public static readonly Error DiscountNotValidAmount =
   new("Discount.DiscountNotValidAmount", "Discount Amount must be more than 0 and less than 100 with persintage type", StatusCodes.Status400BadRequest);


    public static readonly Error DiscountNotValid =
  new("Discount.DiscountNotValidAmount", "Discount Amount must be more than 0 ", StatusCodes.Status400BadRequest);



    public static readonly Error DiscountNotValidDate =
new("Discount.DiscountNotValidDate", "Discount end date must be gretter than start date ", StatusCodes.Status400BadRequest);

}