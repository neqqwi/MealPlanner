using System.ComponentModel.DataAnnotations;

namespace MealPlanner.ViewModels.Accounts;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Введите email")]
    [EmailAddress(ErrorMessage = "Некорректный email")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Введите пароль")]
    [DataType(DataType.Password)]
    [MinLength(6, ErrorMessage = "Пароль должен содержать минимум 6 символов")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*\d).+$",
        ErrorMessage = "Пароль должен содержать хотя бы одну строчную латинскую букву и одну цифру")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Подтвердите пароль")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Пароли не совпадают")]
    public string ConfirmPassword { get; set; } = string.Empty;
}
