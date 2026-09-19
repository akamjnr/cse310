using ExpenseTracker.Data;
using ExpenseTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

public class ExpenseListModel : PageModel
{
    private readonly ExpenseContext _context;

    public ExpenseListModel(ExpenseContext context)
    {
        _context = context;
    }

    public List<Expense> Expenses { get; set; } = new();

    // Retrieve all saved expenses from the database
    // and make them available to the web page.
    public async Task OnGetAsync()
    {
        Expenses = await _context.Expenses
            .OrderByDescending(e => e.Date)
            .ToListAsync();
    }

    // Delete the selected expense from the database.
    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        var expense = await _context.Expenses.FindAsync(id);

        if (expense == null)
        {
            return NotFound();
        }

        _context.Expenses.Remove(expense);
        await _context.SaveChangesAsync();

        return RedirectToPage("/ExpenseList");
    }
}