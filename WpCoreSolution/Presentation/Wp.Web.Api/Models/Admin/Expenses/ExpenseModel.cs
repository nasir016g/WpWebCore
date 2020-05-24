using System;

namespace Wp.Web.Api.Models.Admin
{
    public class ExpenseModel 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public string Date { get; set; }       
        public string ExpenseTags { get; set; }
        public ExpenseCategoryModel ExpenseCategory { get; set; }
        public ExpenseAccountModel ExpenseAccount { get; set; }      
    }   
}
