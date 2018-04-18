using AhpilyServer;
using AhpilyServer.ConCurrent;
using GameServer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 角色缓存
/// </summary>
namespace GameServer.Cache
{
    public class UserCache
    {
        /// <summary>
        /// int  角色ID  model角色数据模型
        /// </summary>
        private Dictionary<int, UserModel> idModelDict = new Dictionary<int, UserModel>();

        /// <summary>
        /// 账号ID 和角色 ID
        /// </summary>
        private Dictionary<int, int> accIdUIdDict = new Dictionary<int, int>();

       /// <summary>
       /// 线程安全的int
       /// </summary>
        ConCurrentInt id = new ConCurrentInt(-1);

        /// <summary>
        /// 创建角色
        /// </summary>
        /// <param name="client">连接对象</param>
        /// <param name="name">角色名字</param>
        /// <param name="accountId">账号ID</param>
        public void Creat(ClientPeer client,string name,int accountId)
        {
            UserModel model = new UserModel(id.Add_Get(),name,accountId);
            idModelDict.Add(model.id,model);
            accIdUIdDict.Add(model.accountID,model.id);
        }
        /// <summary>
        /// 判断此账号下是否有角色
        /// </summary>
        public bool IsExist(int accountId)
        {
            return accIdUIdDict.ContainsKey(accountId);
        }

        /// <summary>
        /// 根据账号ID 返回角色模型
        /// </summary>
        public UserModel GetModelByAccountID(int accountID)
        {
            int userID = accIdUIdDict[accountID];
            UserModel model = idModelDict[userID];
            return model;
        }
    }
}
