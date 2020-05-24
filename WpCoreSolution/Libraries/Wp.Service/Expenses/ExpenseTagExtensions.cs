using System;
using System.Collections.Generic;
using System.Text;

namespace Wp.Services.Expenses
{
    public static class ExpenseTagExtensions
    {
        public static string[] ParseExpenseTags(this string expenseTags)
        {
            var result = new List<string>();
            if (string.IsNullOrWhiteSpace(expenseTags))
                return result.ToArray();

            var values = expenseTags.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var val in values)
                if (!string.IsNullOrEmpty(val.Trim()))
                    result.Add(val.Trim());

            return result.ToArray();
        }
    }
}
