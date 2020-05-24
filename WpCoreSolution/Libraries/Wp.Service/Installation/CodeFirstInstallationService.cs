using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Wp.Core;
using Wp.Core.Domain.Common;
using Wp.Core.Domain.Expenses;
using Wp.Core.Domain.Localization;
using Wp.Core.Domain.Sections;
using Wp.Core.Domain.Security;
using Wp.Core.Domain.Seo;
using Wp.Core.Domain.Tenants;
using Wp.Core.Domain.WebPages;
using Wp.Core.Security;
using Wp.Data;
using Wp.Service.Helpers;
using Wp.Service.Security;
using Wp.Services.Configuration;
using Wp.Services.Expenses;
using Wp.Services.Localization;

namespace Wp.Services.Installation
{
    public partial interface IInstallationService
    {
        void InstallTenants();
        Task InstallData();
        void InstallExpenses();
    }

    public class CodeFirstInstallationService : IInstallationService
    {
        #region Fields
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISettingService _settingService;
        private readonly IExpenseService _expenseService;
        private readonly IExpenseCategoryService _expenseCategoryService;
        private readonly IExpenseAccountService _expenseAccountService;
        private readonly IBaseRepository<WebPage> _webPageRepo;
        private readonly IBaseRepository<WebPageRole> _webPageRoleRepo;
        private readonly IBaseRepository<UrlRecord> _urlRecordRepo;
        private readonly IBaseRepository<Language> _languageRepo;
        private readonly IBaseRepository<Section> _sectionRepo;

        public UserManager<ApplicationUser> _userManager;
        public RoleManager<IdentityRole> _roleManager;
        public IHostingEnvironment _hostingEnvironment;
        private readonly ITenantService _tenantService;
        private readonly IClaimProvider _claimProvider;


        #endregion

        #region Ctor

        public CodeFirstInstallationService(IUnitOfWork unitOfWork,
            ISettingService settingService,
            IExpenseService expenseService,
            IExpenseCategoryService expenseCategoryService,
            IExpenseAccountService expenseAccountService,
            IBaseRepository<WebPage> webPageRepo,
            IBaseRepository<WebPageRole> webPageRoleRepo,
            IBaseRepository<UrlRecord> urlRecordRepo,
            IBaseRepository<Language> languageRepo,
            IBaseRepository<Section> sectionRepo,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IHostingEnvironment hostingEnvironment,
            ITenantService tenantService,
            IClaimProvider claimProvider)
        {
            _unitOfWork = unitOfWork;
            this._webPageRepo = webPageRepo;
            this._webPageRoleRepo = webPageRoleRepo;
            this._urlRecordRepo = urlRecordRepo;
            this._languageRepo = languageRepo;
            this._sectionRepo = sectionRepo;
            _settingService = settingService;
            this._expenseService = expenseService;
            this._expenseCategoryService = expenseCategoryService;
            this._expenseAccountService = expenseAccountService;
            _userManager = userManager;
            _roleManager = roleManager;
            _hostingEnvironment = hostingEnvironment;

            _tenantService = tenantService;
            _claimProvider = claimProvider;

        }

        #endregion

        #region Utilities

        private void InstallWebPages()
        {

            var sections = new List<Section>
            {
                new HtmlContentSection { Html = "test html section" }
            };
            var pages = new List<WebPage>
            {
                new WebPage { ParentId = 0, MetaTitle = "Home", NavigationName = "Home", VirtualPath = "Home" ,DisplayOrder = 1, Sections = sections },
                new WebPage { ParentId = 0, MetaTitle = "Contact", NavigationName = "Contact", VirtualPath = "Contact",  DisplayOrder = 2 },
                new WebPage { ParentId = 2, MetaTitle = "About", NavigationName = "About", VirtualPath = "Contact-About",  DisplayOrder = 7 }
            };

            foreach (var page in pages)
            {
                var p = _webPageRepo.Table.Where(x => x.VirtualPath == page.VirtualPath).FirstOrDefault();
                if (p == null)
                {
                    _webPageRepo.Add(page);
                    var urlRecord = new UrlRecord()
                    {
                        EntityId = page.Id,
                        EntityName = "WebPage",
                        Slug = page.VirtualPath,
                        LanguageId = 0,
                        IsActive = true,
                    };
                    _urlRecordRepo.Add(urlRecord);
                }
            }
            //_unitOfWork.Complete();

        }

        private void InstallLanguages()
        {
            if (_languageRepo.Table.Count() == 0)
            {
                var languages = new List<Language>()
                {
                    new Language { Name = "English", LanguageCulture = "en-Us", UniqueSeoCode = "en", FlagImageFileName = "us.png", Published = true },
                    new Language { Name = "Nederlands", LanguageCulture = "nl-NL", UniqueSeoCode = "nl", FlagImageFileName = "nl.png", Published = true }
                };

                languages.ForEach(l => _languageRepo.Add(l));
                //_unitOfWork.Complete();

                //InstallLocaleResources();
            }
        }

        private void InstallLocaleResources()
        {
            var webRoot = _hostingEnvironment.WebRootPath;
            //var file = System.IO.Path.Combine(webRoot, "test.txt");
            foreach (var language in _languageRepo.Table.ToList())
            {
                foreach (var filePath in System.IO.Directory.EnumerateFiles(Path.Combine(webRoot, "App_Data/Localization/"), string.Format("*.{0}.res.xml", language.UniqueSeoCode), SearchOption.TopDirectoryOnly))
                {
                    // 
                    string xmlText = File.ReadAllText(filePath);
                    var localizationService = ServiceLocator.Instance.GetService<ILocalizationService>();
                    localizationService.ImportResourcesFromXml(language, xmlText);
                }
            }
        }

        private async Task InstallUsersAndRoles()
        {
            // Add Users and Roles

            //Create Role Administrators if it does not exist
            var admin = await _roleManager.FindByNameAsync(SystemRoleNames.Administrators);
            if (admin == null)
                await _roleManager.CreateAsync(new IdentityRole(SystemRoleNames.Administrators));

            //Create Role Users if it does not exist
            var fUser = await _roleManager.FindByNameAsync(SystemRoleNames.Users);
            if (fUser == null)
                await _roleManager.CreateAsync(new IdentityRole(SystemRoleNames.Users));

            //Create User=test with password=test
            if (_userManager.FindByNameAsync("test").Result == null)
            {
                var user = new ApplicationUser { UserName = "test", Email = "test@test.nl" };
                var userResult = await _userManager.CreateAsync(user, "test12");

                // Add User test to Role Administrators
                if (userResult.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, SystemRoleNames.Administrators);
                }
                else
                {
                    throw new Exception(userResult.Errors.First().ToString());
                }
            }

            // install default claims
            var defaultClaims = _claimProvider.GetDefaultClaims().ToList();
            foreach (var dc in defaultClaims)
            {
                var role = _roleManager.Roles.FirstOrDefault(x => x.Name == dc.RoleName);
                if (role == null)
                {
                    await _roleManager.CreateAsync(new IdentityRole(dc.RoleName));
                    role = _roleManager.Roles.FirstOrDefault(x => x.Name == dc.RoleName);
                }
                foreach (var cr in dc.ClaimRecords)
                {
                    await _roleManager.AddClaimAsync(role, new System.Security.Claims.Claim(cr.ClaimType, cr.ClaimValue));
                }

            }
        }

        private void InstallRolesAtAPage()
        {
            _unitOfWork.Complete();
            var firstPage = _webPageRepo.Table.First();
            var roles = _roleManager.Roles;

            List<WebPageRole> wrList = new List<WebPageRole>();
            foreach (var role in roles)
            {
                WebPageRole pr = new WebPageRole();
                pr.WebPage = firstPage;
                pr.Name = role.Name;

                wrList.Add(pr);

            }
            _webPageRoleRepo.AddRange(wrList);

            _unitOfWork.Complete();
        }

        private void InstallSettings()
        {
            if (_settingService.GetAll().Count() == 0)
            {
                _settingService.SaveSetting(new WebsiteSettings()
                {
                    WebsiteName = "Default",
                    Theme = "Grey",
                });

                _settingService.SaveSetting(new LocalizationSettings()
                {
                    DefaultAdminLanguageId = _languageRepo.Table.Single(x => x.Name == "English").Id,
                    UseImagesForLanguageSelection = false,
                });
            }
        }

        #endregion

        public void InstallTenants()
        {
            _tenantService.InstallTenants();
        }

        public async Task InstallData()
        {
            InstallWebPages();
            InstallLanguages();
            await InstallUsersAndRoles();
            InstallRolesAtAPage();
            //InstallSettings();
            InstallExpenses();
        }

        public void InstallExpenses()
        {
            // create categories
            if (_expenseCategoryService.GetAll().Count() == 0)
            {
                var categories = new List<ExpenseCategory>()
                {
                    new ExpenseCategory { Name = "BEA", Category = "ATM", Description = "ATM" },
                    new ExpenseCategory { Name = "Cash", Category = "ATM", Description = "ATM" },

                    new ExpenseCategory { Name = "Bank Costs", Category = "Bank" , Description = "Bank costs" },

                    new ExpenseCategory { Name = "Tax", Category = "Transportation", Description = "Cars gas, tax and etc." },
                    new ExpenseCategory { Name = "Gas", Category = "Transportation", Description = "" },
                    new ExpenseCategory { Name = "ANWB", Category = "Transportation", Description = "" },
                    new ExpenseCategory { Name = "Car Maintenance", Category = "Transportation", Description = "" },
                    new ExpenseCategory { Name = "Public Transport", Category = "Transportation" , Description = "Public Transport" },
                    new ExpenseCategory { Name = "Travel Expenses Reimbursement", Category = "Transportation", Description = "reiskostenvergoeding" },

                    new ExpenseCategory { Name = "Zalando", Category = "Clothing", Description = "Clothes" },
                    new ExpenseCategory { Name = "Primark", Category = "Clothing", Description = "Clothes" },

                    new ExpenseCategory { Name = "ICS (Nasir)", Category = "Credit Account", Description = "Credit Account" },

                    new ExpenseCategory { Name = "Education", Category = "Education", Description = "Education" },
                    
                    new ExpenseCategory { Name = "Electronics", Category = "Electronics", Description = "Cell phones, computers, tv and etc." },
                    //new ExpenseCategory { Name = "Go out", Color = "#5e94ff", Description = "Go out" },
                    
                    new ExpenseCategory { Name = "AH", Category = "Groceries", Description = "Includes super markets, kruidvat and etc." },
                    new ExpenseCategory { Name = "Kruidvat", Category = "Groceries", Description = "" },
                    new ExpenseCategory { Name = "HEMA", Category = "Groceries", Description = "" },

                    new ExpenseCategory { Name = "Medical Insurance", Category = "Health Care", Description = "Insurance, deductible etc." },
                    new ExpenseCategory { Name = "Medication", Category = "Health Care", Description = "" },

                    new ExpenseCategory { Name = "Intratuin", Category = "Household Items", Description = "Household Goods" },
                    new ExpenseCategory { Name = "Coolblue", Category = "Household Items", Description = "Household Goods" },
                    new ExpenseCategory { Name = "Bol.com", Category = "Household Items", Description = "Household Goods" },
                    new ExpenseCategory { Name = "Alipay", Category = "Household Items", Description = "Household Goods" },
                    new ExpenseCategory { Name = "Ikea", Category = "Household Items", Description = "Household Goods" },
                    
                    new ExpenseCategory { Name = "Income", Category = "Income", Description = "Income" },                    

                    new ExpenseCategory { Name = "UNIVE", Category = "Non-Medical Insurance", Description = "Car, house, legal, insurances" },

                    new ExpenseCategory { Name = "Mortgage", Category = "Housing", Description = "hypotheek" },
                    new ExpenseCategory { Name = "Home insurance", Category = "Housing", Description = "opstalverzekering" },
                    new ExpenseCategory { Name = "Vereniging Eigen Huis", Category = "Housing", Description = "" },
                    new ExpenseCategory { Name = "Municipal Taxes", Category = "Housing", Description = "Gemeentebelasting" },
                    
                    new ExpenseCategory { Name = "Schiphol Parking", Category = "Vacation", Description = "Vacation" },

                    new ExpenseCategory { Name = "Mobile", Category = "Utilities", Description = "Utilities includes gas, electricity, water, cellphone, internet and tv, netflix, spotify and etc." },
                    new ExpenseCategory { Name = "Gas/Electricity", Category = "Utilities", Description = "ESSENT" },
                    new ExpenseCategory { Name = "Water", Category = "Utilities", Description = "VITENS" },
                    new ExpenseCategory { Name = "Water Tax", Category = "Utilities", Description = "GBLT" },
                    new ExpenseCategory { Name = "NETFLIX", Category = "Utilities", Description = "" },
                    new ExpenseCategory { Name = "Internet/Tv/Phone", Category = "Utilities", Description = "Telfort Thuis" },

                    new ExpenseCategory { Name = "Others", Category = "Others", Description = "Others" }
                  };

                categories.ForEach(category => _expenseCategoryService.Insert(category));
            }

            // create accounts
            if (_expenseAccountService.GetAll().Count() == 0)
            {
                var accounts = new List<ExpenseAccount>()
                {
                    new ExpenseAccount { Name = "Bank (Nasir Ing private)", Account = "NL13INGB0007076421" },
                     new ExpenseAccount { Name = "Bank (Nasir Abn amro private)", Account = "410656062" },
                    new ExpenseAccount { Name = "Bank (Zarghona private)" },
                };

                accounts.ForEach(account => _expenseAccountService.Insert(account));
            }

            //var nasirAccount = _expenseAccountService.GetByName("Bank (Nasir private)");

            //Random r = new Random();
            //for (int i = 0; i < 3; i++)
            //{
            //    Expense expense = new Expense
            //    {
            //        Name = "expense " + i.ToString(),
            //        Amount = r.Next(5, 20),
            //        Date = DateTime.Now,
            //        ExpenseCategory = _expenseCategoryService.GetAll()[i],
            //        ExpenseAccount = nasirAccount
            //    };
            //    _expenseService.Insert(expense);
            //}
        }
    }
}
