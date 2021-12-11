using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Nsr.Common.Core;
using Nsr.Common.Core.Localization;
using Nsr.Common.Data;
using Nsr.Common.Data.Repositories;
using Nsr.Common.Service.Configuration;
using Nsr.Common.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Wp.Core;
using Wp.Core.Domain.Common;
using Wp.Core.Domain.Sections;
using Wp.Core.Domain.Security;
using Wp.Core.Domain.Seo;
using Wp.Core.Domain.WebPages;
using Wp.Core.Domain.Websites;
using Wp.Core.Security;
using Wp.Service.Security;

namespace Wp.Services.Installation
{
    public partial interface IInstallationService
    {
        void InstallTenants();
        Task InstallData();
    }

    public class CodeFirstInstallationService : IInstallationService
    {
        #region Fields
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICommonUnitOfWork _commonUnitOfWork;
        private readonly ISettingService _settingService;
        private readonly IBaseRepository<Website> _websiteRepo;
        private readonly IBaseRepository<WebPage> _webPageRepo;
        private readonly IBaseRepository<WebPageRole> _webPageRoleRepo;
        private readonly IBaseRepository<UrlRecord> _urlRecordRepo;
        private readonly IBaseRepository<Section> _sectionRepo;

        public UserManager<ApplicationUser> _userManager;
        public RoleManager<IdentityRole> _roleManager;
        public IHostingEnvironment _hostingEnvironment;
        private readonly ITenantService _tenantService;
        private readonly IClaimProvider _claimProvider;


        #endregion

        #region Ctor

        public CodeFirstInstallationService(IUnitOfWork unitOfWork,
            ICommonUnitOfWork commonUnitOfWork,
            ISettingService settingService,
            IBaseRepository<Website> websiteRepo,
            IBaseRepository<WebPage> webPageRepo,
            IBaseRepository<WebPageRole> webPageRoleRepo,
            IBaseRepository<UrlRecord> urlRecordRepo,
            IBaseRepository<Section> sectionRepo,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IHostingEnvironment hostingEnvironment,
            ITenantService tenantService,
            IClaimProvider claimProvider
            )
        {
            _unitOfWork = unitOfWork;
            _commonUnitOfWork = commonUnitOfWork;
            this._webPageRepo = webPageRepo;
            this._webPageRoleRepo = webPageRoleRepo;
            this._urlRecordRepo = urlRecordRepo;
            this._sectionRepo = sectionRepo;
            _settingService = settingService;
            _websiteRepo = websiteRepo;
            _userManager = userManager;
            _roleManager = roleManager;
            _hostingEnvironment = hostingEnvironment;

            _tenantService = tenantService;
            _claimProvider = claimProvider;
        }

        #endregion

        #region Utilities

        private void InstallWebsite()
        {
            if(_websiteRepo.Table.Any()) return;

            var website = new Website();
            website.WebsiteName = "Default";
            website.Theme = "Default";

            _websiteRepo.Add(website);
            _unitOfWork.Complete();


        }

        private void InstallWebPages()
        {
            if (_webPageRepo.Table.Count() > 0) return;

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
                    page.CreatedOn = DateTime.Now;
                    page.UpdatedOn = DateTime.Now;
                    _webPageRepo.Add(page);
                    _unitOfWork.Complete();
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

        private async Task InstallUsersAndRoles()
        {
            if (_userManager.Users.Any()) return;
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
                var user = new ApplicationUser { UserName = "test@test.nl", Email = "test@test.nl" };
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
            if (_webPageRoleRepo.Table.Any()) return;

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
                //_settingService.SaveSetting(new WebsiteSettings()
                //{
                //    WebsiteName = "Default",
                //    Theme = "Darkly",
                //});

                _settingService.SaveSetting(new LocalizationSettings()
                {
                    DefaultAdminLanguageId = 1,  // _languageRepo.Table.Single(x => x.Name == "English").Id,
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
            InstallWebsite();
            InstallWebPages();
            await InstallUsersAndRoles();
            InstallRolesAtAPage();
            InstallSettings();
        }       
    }
}
