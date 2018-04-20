using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Dto
{
    /// <summary>
    /// 匹配房间Dto
    /// </summary>
    [Serializable]
    public class MatchRoomDto
    {
        /// <summary>
        /// 用户ID   用户数据模型
        /// </summary>
        public Dictionary<int, UserDto> uIdUserDtoDict { get; set; }
        /// <summary>
        /// 准备的玩家ID
        /// </summary>
        public List<int> readyUIdList { get; set; }

        public MatchRoomDto()
        {
            this.uIdUserDtoDict = new Dictionary<int, UserDto>();
            this.readyUIdList = new List<int>();
        }
        //public MatchRoomDto()
        //{

        //} 

        public void Add(UserDto dto)
        {
            uIdUserDtoDict.Add(dto.id,dto);
        }

        public void Leave(int userID)
        {
            uIdUserDtoDict.Remove(userID);
        }

        public void Ready(int userId)
        {
            readyUIdList.Add(userId);
        }
    }
}
