using ExpenseTracker.Data;
using ExpenseTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

public class AddExpenseModel : PageModel
{
    private readonly ExpenseContext _context;

    public AddExpenseModel(ExpenseContext context)
    {
        _context = context;
    }

    [BindProperty]
    [Required(ErrorMessage = "Amount is required.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0.")]
    public decimal Amount { get; set; }

    [BindProperty]
    [Required(ErrorMessage = "Category is required.")]
    public string Category { get; set; } = "";

    [BindProperty]
    [Required(ErrorMessage = "Description is required.")]
    public string Description { get; set; } = "";

    [BindProperty]
    [Required(ErrorMessage = "Date is required.")]
    public DateTime? Date { get; set; }

    public void OnGet()
    {
        Date = DateTime.Today;
    }

    // Process the expense submitted by the user,
    // validate the information, save it to the database,
    // and then show the expense list.
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var expense = new Expense
        {
            Amount = Amount,
            Category = Category,
            Description = Description,
            Date = Date!.Value
        };

        _context.Expenses.Add(expense);
        await _context.SaveChangesAsync();

        return RedirectToPage("/ExpenseList");
    }
}