using LoanShark.Domain;
using LoanShark.EF.EFModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanShark.EF.Mappers
{
    public static class PostMapper
    {
        public static Post ToDomainPost(PostEF postEF)
        {
            if (postEF == null)
            {
                throw new ArgumentNullException(nameof(postEF));
            }

            return new Post(postEF.PostID, postEF.Title, postEF.Category, postEF.Content, postEF.Timestamp);
        }
    }
}
