﻿using Refit;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wp.Web.Mvc.Profile.Models;
using Wp.Web.Mvc.Models.Resumes;

namespace Wp.Web.Mvc.Profile.RestClients
{
    public interface IResumesWebApi
    {
        [Get("/resume")]
        Task<List<ResumeAdminModel>> GetResume();

        [Get("/resume/{id}")]
        Task<ResumeAdminModel> GetResumeById([AliasAs("id")] int id);        

        [Put("/resume/{id}")]
        //[Put("/resume")]
        Task Update(int id, ResumeAdminModel model);

        [Delete("/resume/{id}")]
        Task Delete([AliasAs("id")] int id);

        [Get("/resume/details/{id}")]
        Task<ResumeModel> GetResumeDetails([AliasAs("id")] int id);

    }
}
