using Nsr.RestClient.Models.Localization;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nsr.RestClient.RestClients.Localization
{
    public interface ILanguageWebApi
    {
        [Get("/api/language")]
        Task<List<Language>> GetAll();

        [Get("/api/language/{id}")]
        Task<Language> GetById([AliasAs("id")] int id);

        [Put("/api/language/{id}")]
        Task Update(int id, Language model);

        [Delete("/api/language/{id}")]
        Task Delete([AliasAs("id")] int id);      

    }
}
