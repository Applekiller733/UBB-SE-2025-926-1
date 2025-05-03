// <copyright file="IFeedService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace LoanShark.Service.SocialService.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using LoanShark.Data;
    using LoanShark.Domain;

    /// <summary>
    /// Provides methods to retrieve feed content.
    /// </summary>
    public interface IFeedService
    {
        /// <summary>
        /// Retrieves the content of the feed.
        /// </summary>
        /// <returns>A list of posts representing the feed content.</returns>
        List<Post> GetFeedContent();
    }
}
