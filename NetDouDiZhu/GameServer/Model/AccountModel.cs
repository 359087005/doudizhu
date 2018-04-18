using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Model
{
    /// <summary>
    /// 账号的数据模型
    /// </summary>
   public class AccountModel
    {
        public int id;
        public string account;
        public string password;
        //....创建日期   电话~

        public AccountModel(int id, string account, string password)
        {
            this.id = id;
            this.account = account;
            this.password = password;
        }
    }
}
