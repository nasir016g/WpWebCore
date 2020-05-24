using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Wp.Core.Domain.Expenses;
using Wp.Services.Expenses;
using Wp.Services.ExportImport.Help;
using Wp.Core.Common;

namespace Wp.Services.ExportImport
{
    public class ImportExcelService : IImportExcelService
    {
        private readonly IExpenseService _expenseService;
        private readonly IExpenseAccountService _expenseAccountService;
        private readonly IExpenseCategoryService _expenseCategoryService;
        private readonly IExpenseTagService _expenseTagService;

        public ImportExcelService(IExpenseService expenseService, IExpenseAccountService expenseAccountService, IExpenseCategoryService expenseCategoryService, IExpenseTagService expenseTagService)
        {
            _expenseService = expenseService;
            _expenseAccountService = expenseAccountService;
            _expenseCategoryService = expenseCategoryService;
            _expenseTagService = expenseTagService;
        }

        private ExpenseCategory GetExpenseCategoryByName(string name = "")
        {
            ExpenseCategory expenseCategory = _expenseCategoryService.GetAll().First(x => x.Name == "Others");

            if (name == null)
                return expenseCategory;
           
            // BankCosts
            if (name.Contains("BasisPakket", StringComparison.InvariantCultureIgnoreCase)
               || name.Contains("BetaalGemak", StringComparison.InvariantCultureIgnoreCase))
            {
                expenseCategory = _expenseCategoryService.GetAll().FirstOrDefault(x => x.Name == "Bank Costs");
            }

            // Transportation
            else if (name.Contains("SHELL", StringComparison.InvariantCultureIgnoreCase)
                || name.Contains("CCV*BP", StringComparison.InvariantCultureIgnoreCase)
                || name.Contains("TOTAL", StringComparison.InvariantCultureIgnoreCase)
                )
            {
                expenseCategory = _expenseCategoryService.GetAll().FirstOrDefault(x => x.Name == "Gas" && x.Category == "Transportation");
            }
            else if (name.Contains("37-RDJ-5", StringComparison.InvariantCultureIgnoreCase)
                || name.Contains("16-SJD-2", StringComparison.InvariantCultureIgnoreCase))
            {
                expenseCategory = _expenseCategoryService.GetAll().FirstOrDefault(x => x.Name == "Tax" && x.Category == "Transportation");
            }
            else if (name.Contains("ANWB"))
            {
                expenseCategory = _expenseCategoryService.GetAll().FirstOrDefault(x => x.Name == "ANWB" && x.Category == "Transportation");
            }
            else if (name.Contains("ov-", StringComparison.InvariantCultureIgnoreCase))
            {
                expenseCategory = _expenseCategoryService.GetAll().FirstOrDefault(x => x.Name == "Public Transport");
            }
            else if (name.Contains("reiskosten ", StringComparison.InvariantCultureIgnoreCase))
            {
                expenseCategory = _expenseCategoryService.GetAll().FirstOrDefault(x => x.Name == "Travel Expenses Reimbursement");
            }

            // Clothing
            else if (name.Contains("Zalando", StringComparison.InvariantCultureIgnoreCase))
            {
                expenseCategory = _expenseCategoryService.GetAll().FirstOrDefault(x => x.Name == "Zalando");
            }
            else if (name.Contains("PRIMARK", StringComparison.InvariantCultureIgnoreCase))
            {
                expenseCategory = _expenseCategoryService.GetAll().FirstOrDefault(x => x.Name == "Primark");
            }

            // Credit Account
            else if (name.Contains("ICS-klantnummer 68037960017", StringComparison.InvariantCultureIgnoreCase))
            {
                expenseCategory = _expenseCategoryService.GetAll().FirstOrDefault(x => x.Name == "ICS (Nasir)");
            }

            // Groceries
            else if (name.Contains("ALBERT")
                || name.Contains("AH"))
            {
                expenseCategory = _expenseCategoryService.GetAll().FirstOrDefault(x => x.Name == "AH");
            }
            else if (name.Contains("Kruidvat", StringComparison.InvariantCultureIgnoreCase))
            {
                expenseCategory = _expenseCategoryService.GetAll().FirstOrDefault(x => x.Name == "Kruidvat");
            }
            else if (name.Contains("HEMA", StringComparison.InvariantCultureIgnoreCase))
            {
                expenseCategory = _expenseCategoryService.GetAll().FirstOrDefault(x => x.Name == "HEMA");
            }

            // Health Care
            else if (name.Contains("UNIVE ZORG", StringComparison.InvariantCultureIgnoreCase)) 
            {
                expenseCategory = _expenseCategoryService.GetAll().FirstOrDefault(x => x.Name == "Medical Insurance");
            }
            else if (name.Contains("Apotheek ", StringComparison.InvariantCultureIgnoreCase))
            {
                expenseCategory = _expenseCategoryService.GetAll().FirstOrDefault(x => x.Name == "Medication");
            }

            // Household Goods
            else if (name.Contains("Intratuin", StringComparison.InvariantCultureIgnoreCase))
            {
                expenseCategory = _expenseCategoryService.GetAll().FirstOrDefault(x => x.Name == "Intratuin");
            }
            else if (name.Contains("Coolblue", StringComparison.InvariantCultureIgnoreCase))              
            {
                expenseCategory = _expenseCategoryService.GetAll().FirstOrDefault(x => x.Name == "Coolblue");
            }
            else if (name.Contains("bol.com", StringComparison.InvariantCultureIgnoreCase))              
            {
                expenseCategory = _expenseCategoryService.GetAll().FirstOrDefault(x => x.Name == "Bol.com");
            }
            else if (name.Contains("Alipay", StringComparison.InvariantCultureIgnoreCase))
            {
                expenseCategory = _expenseCategoryService.GetAll().FirstOrDefault(x => x.Name == "Alipay");
            }
            else if (name.Contains("Ikea", StringComparison.InvariantCultureIgnoreCase))
            {
                expenseCategory = _expenseCategoryService.GetAll().FirstOrDefault(x => x.Name == "Ikea");
            }

            // Non-Medical Insurance
            else if (name.Contains("UNIVE", StringComparison.InvariantCultureIgnoreCase))
            {
                expenseCategory = _expenseCategoryService.GetAll().FirstOrDefault(x => x.Name == "UNIVE" && x.Category == "Non-Medical Insurance");
            }

            // Housing
            else if (name.Contains("ST11107003621530001", StringComparison.InvariantCultureIgnoreCase)) // hypotheek
            {
                expenseCategory = _expenseCategoryService.GetAll().FirstOrDefault(x => x.Name == "Mortgage");
            }
            else if (name.Contains("verzekering 410656062", StringComparison.InvariantCultureIgnoreCase)) // opstalverzekering
            {
                expenseCategory = _expenseCategoryService.GetAll().FirstOrDefault(x => x.Name == "Home insurance");
            }
            else if (name.Contains("Vereniging Eigen Huis", StringComparison.InvariantCultureIgnoreCase))
            {
                expenseCategory = _expenseCategoryService.GetAll().FirstOrDefault(x => x.Name == "Vereniging Eigen Huis");
            }
            else if (name.Contains("Gemeentebelasting", StringComparison.InvariantCultureIgnoreCase))
            {
                expenseCategory = _expenseCategoryService.GetAll().FirstOrDefault(x => x.Name == "Municipal Taxes");
            }

            // Vacation
            else if (name.Contains("CCV Group BV", StringComparison.InvariantCultureIgnoreCase))
            {
                expenseCategory = _expenseCategoryService.GetAll().FirstOrDefault(x => x.Name == "Schiphol Parking");
            }

            // Utilities
            else if (name.Contains("BEN NEDERLAND", StringComparison.InvariantCultureIgnoreCase))               
            {
                expenseCategory = _expenseCategoryService.GetAll().FirstOrDefault(x => x.Name == "Mobile");
            }
            else if (name.Contains("ESSENT", StringComparison.InvariantCultureIgnoreCase))
            {
                expenseCategory = _expenseCategoryService.GetAll().FirstOrDefault(x => x.Name == "Gas/Electricity");
            }
            else if (name.Contains("VITENS", StringComparison.InvariantCultureIgnoreCase))
            {
                expenseCategory = _expenseCategoryService.GetAll().FirstOrDefault(x => x.Name == "Water");
            }
            else if (name.Contains("GBLT", StringComparison.InvariantCultureIgnoreCase))
            {
                expenseCategory = _expenseCategoryService.GetAll().FirstOrDefault(x => x.Name == "Water Tax");
            }
            else if (name.Contains("NETFLIX", StringComparison.InvariantCultureIgnoreCase))
            {
                expenseCategory = _expenseCategoryService.GetAll().FirstOrDefault(x => x.Name == "NETFLIX");
            }
            else if (name.Contains("Telfort Thuis", StringComparison.InvariantCultureIgnoreCase))
            {
                expenseCategory = _expenseCategoryService.GetAll().FirstOrDefault(x => x.Name == "Internet/Tv/Phone");
            }

            // must be the last
            else if (name.Contains("GEA")) 
            {
                expenseCategory = _expenseCategoryService.GetAll().FirstOrDefault(x => x.Name == "Cash");
            }
            else if (name.Contains("BEA"))
            {
                expenseCategory = _expenseCategoryService.GetAll().FirstOrDefault(x => x.Name == "BEA");
            }



            return expenseCategory;
        }

        private string[] GetExpenseTagsByName(string name)
        {
            var result = new List<string>();
           // if (name.Contains("BEN NEDERLAND", StringComparison.InvariantCultureIgnoreCase)) { result.Add("mobile"); } 

            return result.ToArray();
        }

        public static IList<PropertyByName<T>> GetPropertiesByExcelCells<T>(ExcelWorksheet worksheet)
        {
            var properties = new List<PropertyByName<T>>();
            var poz = 1;
            while (true)
            {
                try
                {
                    var cell = worksheet.Cells[1, poz];

                    if (string.IsNullOrEmpty(cell?.Value?.ToString()))
                        break;

                    poz += 1;
                    properties.Add(new PropertyByName<T>(cell.Value.ToString()));
                }
                catch
                {
                    break;
                }
            }

            return properties;
        }


        public void ImportExpensesFromXlsx(Stream stream)
        {
            using (var xlPackage = new ExcelPackage(stream))
            {
                // get the first worksheet in the workbook
                var worksheet = xlPackage.Workbook.Worksheets.FirstOrDefault();
                if (worksheet == null)
                    throw new Exception("No worksheet found");

                //the columns
                var properties = GetPropertiesByExcelCells<Expense>(worksheet);

                var manager = new PropertyManager<Expense>(properties);

                var iRow = 2;
                var setSeName = properties.Any(p => p.PropertyName == "Notifications");
                var expenseCategory = _expenseCategoryService.GetAll().FirstOrDefault();
                while (true)
                {
                    var allColumnsAreEmpty = manager.GetProperties
                        .Select(property => worksheet.Cells[iRow, property.PropertyOrderPosition])
                        .All(cell => cell?.Value == null || string.IsNullOrEmpty(cell.Value.ToString()));

                    if (allColumnsAreEmpty)
                        break;

                    manager.ReadFromXlsx(worksheet, iRow);



                    string descriptionName = "Notifications";
                    string dateName = "Date";
                    if (!manager.GetProperties.Any(x => x.PropertyName == "Notifications"))
                    {
                        descriptionName = "Omschrijving";
                        dateName = "Transactiedatum";
                    }


                    var expense = _expenseService.GetByDescription(manager.GetProperty(descriptionName).StringValue, manager.GetProperty(dateName).DateTime);

                    var isNew = expense == null;

                    expense = expense ?? new Expense();

                    if (isNew)
                    {
                        expense.CreatedOn = DateTime.Now;
                        expense.UpdatedOn = DateTime.Now;


                        foreach (var property in manager.GetProperties)
                        {
                            switch (property.PropertyName)
                            {
                                case "Date": //ing
                                case "Transactiedatum": //abn 
                                    expense.Date = property.DateTime;
                                    break;
                                case "Name / Description": //ing
                                    expense.Name = property.StringValue;
                                    break;
                                case "Account": //ing
                                case "Rekeningnummer": //abn 
                                    expense.ExpenseAccount = _expenseAccountService.GetByAccount(property.StringValue);
                                    break;
                                case "Amount (EUR)": //ing                                    
                                    expense.Amount = property.DecimalValueCommaDecimalSeparator;
                                    if (manager.GetProperty("Debit/credit").StringValue == "Debit")
                                    {
                                        expense.Amount *= -1;
                                    }
                                    break;
                                case "Transactiebedrag": //abn                                   
                                    expense.Amount = property.DecimalValue;
                                    break;
                                case "Notifications": //ing
                                    expense.Description = property.StringValue;
                                    break;
                                case "Omschrijving": //abn
                                    expense.Description = property.StringValue;
                                    if (expense.Description.Contains("Naam:"))
                                    {
                                        expense.Name = expense.Description.GetBetween("Naam:", "  ");
                                    }
                                    else if (expense.Description.Contains("NAME/"))
                                    {
                                        expense.Name = expense.Description.GetBetween("NAME/", "/");
                                    }
                                    else
                                    {
                                        if (expense.Description.Length > 50)
                                            expense.Name = expense.Description.Substring(0, 50);
                                        else
                                            expense.Name = expense.Description;
                                    }
                                    break;
                            }
                        }

                        expense.ExpenseCategory = GetExpenseCategoryByName(expense.Name + expense.Description);

                        if (expense.Amount > 0 && expense.ExpenseCategory.Name == "Others")
                        {
                            expense.ExpenseCategory = _expenseCategoryService.GetAll().FirstOrDefault(x => x.Name == "Income");
                        }

                        _expenseService.Insert(expense);
                        _expenseTagService.UpdateExpenseTags(expense, GetExpenseTagsByName(expense.Name + expense.Description));

                    }
                    iRow++;
                }
            }
        }
    }
}
