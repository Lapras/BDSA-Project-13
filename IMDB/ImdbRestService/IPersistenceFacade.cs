﻿using System.Threading.Tasks;
using DtoSubsystem;

namespace ImdbRestService
{
    public interface IPersistenceFacade
    {
        /// <summary>
        /// Get content from the database
        /// </summary>
        /// <param name="data">Name of the data to get</param>
        /// <param name="requestedDto">Dto format requested</param>
        /// <returns>Result</returns>
        Task<ResponseData> Get(string data, Dto requestedDto);

        /// <summary>
        /// Post content to the database
        /// </summary>
        /// <param name="path">Path send in the post request including the posted data</param>
        /// <param name="requestedDto">Dto format to post for send in</param>
        /// <returns>Result</returns>
        Task<ResponseData> Post(string path, Dto requestedDto);
    }
}