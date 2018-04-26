using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Dto.Fight
{
    [Serializable]
   public  class ChatDto
    {
        public int userId;
        public int chatType;

        public ChatDto()
        { }

        public ChatDto(int userId, int chatType)
        {
            this.userId = userId;
            this.chatType = chatType;
        }
    }
}
