using Microsoft.Build.ObjectModelRemoting;
using Nsr.RestClient.Models.WorkHistories;

namespace Wp.Web.Mvc.Services
{
    public class EducationService
    {
        private readonly HttpClient _httpClient;

        public EducationService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public Task<List<EducationModel>> GetEducations(int resumeId)
        {
            return null;

        }
    }
}
