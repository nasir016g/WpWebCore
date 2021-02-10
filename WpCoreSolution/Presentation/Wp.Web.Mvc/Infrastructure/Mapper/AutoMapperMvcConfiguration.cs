using AutoMapper;

namespace Wp.Web.Mvc.Infrastructure.Mapper
{
    /// <summary>
    /// AutoMapper configuration
    /// </summary>
    public static class AutoMapperMvcConfiguration
    {
        /// <summary>
        /// Mapper
        /// </summary>
        public static IMapper Mapper { get; private set; }

        /// <summary>
        /// Mapper configuration
        /// </summary>
        public static MapperConfiguration MapperConfiguration { get; private set; }

        /// <summary>
        /// Initialize mapper
        /// </summary>
        /// <param name="config">Mapper configuration</param>
        public static void Init()
        {
            var config = new MapperConfiguration(cfg =>
            {
                //cfg.AddProfile<AdminProfile>();
                cfg.AddProfile(typeof(WebMvcProfile));
            });
            MapperConfiguration = config;
            Mapper = config.CreateMapper();
        }
    }
}
