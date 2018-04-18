using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Model
{
    /// <summary>
    /// 角色的数据模型
    /// </summary>
    public class UserModel
    {
        public int id; //唯一ID
        public string name;
        public int been;
        public int lv;
        public int exp;
        public int winCount;
        public int loseCount;
        public int runCount;


        public int accountID;

        public UserModel(int id,string name,int accountID)
        {
            this.id = id;
            this.name = name;
            this.been = 1000;
            this.lv = 1;
            this.exp = 0;
            this.winCount = 0;
            this.loseCount = 0;
            this.runCount = 0;
            this.accountID = accountID;
        }
    }
}
