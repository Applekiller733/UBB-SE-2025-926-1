using LoanShark.Domain;
using LoanShark.EF.EFModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanShark.EF.Mappers
{
    public static class ChatMapper
    {
        public static Chat ToDomainChat(ChatEF chatEF)
        {
            if (chatEF == null)
            {
                throw new ArgumentNullException(nameof(chatEF));
            }

            List<int> userIds = chatEF.ChatUsers.Select(chatUser => chatUser.UserId).ToList();
            return new Chat(chatEF.Id, chatEF.ChatName, userIds);
        }
    }
}
