using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
/// <summary>
/// user数据的传输模型
/// </summary>
namespace Protocol.Dto
{
    [Serializable]
   public class UserDto
    {
        public int id;
        public string name;
        public int been;
        public int lv;
        public int exp;
        public int winCount;
        public int loseCount;
        public int runCount;

        public UserDto()
        { }
        public UserDto(int id,string name,int been,int lv,int exp,int win,int lose ,int runCount)
        {
            this.id = id;
            this.name = name;this.been = been;this.lv = lv;this.exp = exp;
            this.winCount = win;this.loseCount = lose;this.runCount = runCount;
        }
    }
}
