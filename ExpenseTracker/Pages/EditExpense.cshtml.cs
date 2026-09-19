using ExpenseTracker.Data;
using ExpenseTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class EditExpenseModel : PageModel
{
    private readonly ExpenseContext _context;

    public EditExpenseModel(ExpenseContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Expense Expense { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var expense = await _context.Expenses.FindAsync(id);

        if (expense == null)
        {
            return NotFound();
        }

        Expense = expense;

        return Page();
    }

    // Update the expense with the information submitted by the user.
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        _context.Expenses.Update(Expense);
        await _context.SaveChangesAsync();

        return RedirectToPage("/ExpenseList");
    }
}