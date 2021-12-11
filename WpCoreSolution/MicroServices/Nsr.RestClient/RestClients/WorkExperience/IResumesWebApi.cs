using Nsr.RestClient.Models.WorkHistories;
using Refit;

namespace Nsr.RestClient.RestClients
{
    public interface IResumesWebApi
    {
        [Get("/resume")]
        Task<List<ResumeModel>> GetResume();

        [Get("/resume/{id}")]
        Task<ResumeModel> GetResumeById([AliasAs("id")] int id);        

        [Put("/resume/{id}")]
        //[Put("/resume")]
        Task Update(int id, ResumeModel model);

        [Delete("/resume/{id}")]
        Task Delete([AliasAs("id")] int id);

        [Get("/resume/details/{id}/{languageId}")]
        Task<WorkHistoryDetailsModel> GetResumeDetails([AliasAs("id")] int id, int languageId);

    }
}
