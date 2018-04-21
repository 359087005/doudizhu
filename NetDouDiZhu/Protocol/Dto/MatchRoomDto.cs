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
        public List<int> readyUIdList;
        /// <summary>
        /// 玩家的顺序list
        /// </summary>
        public List<int> uIdList;
        public MatchRoomDto()
        {
            this.uIdUserDtoDict = new Dictionary<int, UserDto>();
            this.readyUIdList = new List<int>();
            this.uIdList = new List<int>();
        }
        //public MatchRoomDto()
        //{

        //} 

        public void Add(UserDto dto)
        {
            uIdUserDtoDict.Add(dto.id,dto);
            uIdList.Add(dto.id);
        }

        public void Leave(int userID)
        {
            uIdUserDtoDict.Remove(userID);
            uIdList.Add(userID);
        }

        public void Ready(int userId)
        {
            readyUIdList.Add(userId);
        }

        public int leftId;
        public int rightId;

        /// <summary>
        /// 重置位置   按照逆时针方向  根据房间位置list的数目  根据自己的位置  得出别人的位置
        /// </summary>
        public void ResetPositon(int myUserId)
        {
            leftId = -1;
            rightId = -1;

            if (uIdList.Count == 1)
            {

            }
            else if (uIdList.Count == 2)
            {
                if (uIdList[0] == myUserId)
                {
                    rightId = uIdList[1];
                }

                if (uIdList[1] == myUserId)
                {
                    leftId = uIdList[0];
                }
            }
            else if (uIdList.Count == 3)
            {
                if (uIdList[0] == myUserId)
                {
                    rightId = uIdList[1];
                    leftId = uIdList[2];
                }
                else if (uIdList[1] == myUserId)
                {
                    rightId = uIdList[2];
                    leftId = uIdList[0];
                }
                else if (uIdList[2] == myUserId)
                {
                    rightId = uIdList[0];
                    leftId = uIdList[1];
                }
            }

        }
    }
}
