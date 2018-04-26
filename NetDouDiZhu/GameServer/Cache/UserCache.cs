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
        /// <param name="name">角色名字</param> 
        /// <param name="accountId">账号ID</param>
        public void Creat(string name, int accountId)
        {
            UserModel model = new UserModel(id.Add_Get(), name, accountId);
            idModelDict.Add(model.id, model);
            accIdUIdDict.Add(model.accountID, model.id);
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

        /// <summary>
        /// 根据账号ID获取角色ID
        /// </summary>
        /// <param name="accId"></param>
        public int GetIdByAccId(int accId)
        {
            return accIdUIdDict[accId];
        }

        private Dictionary<int, ClientPeer> idClientDict = new Dictionary<int, ClientPeer>();
        private Dictionary<ClientPeer, int> clientIdDict = new Dictionary<ClientPeer, int>();
        /// <summary>
        /// 判断玩家是否在线
        /// </summary>
        public bool IsOnLine(ClientPeer client)
        {
            return clientIdDict.ContainsKey(client);
        }
        public bool IsOnLine(int id)
        {
            return idClientDict.ContainsKey(id);
        }
        /// <summary>
        /// 玩家上线
        /// </summary>
        /// <param name="client"></param>
        /// <param name="id"></param>
        public void OnLine(ClientPeer client, int id)
        {
            idClientDict.Add(id,client);
            clientIdDict.Add(client,id);
        }
        /// <summary>
        /// 玩家下线
        /// </summary>
        /// <param name="client"></param>
        public void OffLine(ClientPeer client)
        {
            int id = clientIdDict[client];
            clientIdDict.Remove(client);
            idClientDict.Remove(id);
        }
        /// <summary>
        /// 根据连接对象获取角色模型
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public UserModel GetModelByClient(ClientPeer client)
        {
            int id = clientIdDict[client];
           return idModelDict[id];
        }
        /// <summary>
        /// 根据连接对象ID获取角色模型
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public UserModel GetModelByUserId(int userId)
        {
            return idModelDict[userId];
        }

        /// <summary>
        /// 根据角色ID获取连接对象
        /// </summary>
        /// <returns></returns>
        public ClientPeer GetClientById(int id)
        {
            return idClientDict[id];
        }
        /// <summary>
        /// 根据连接对象获取ID
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public int GetIdByClient(ClientPeer client)
        {
            return clientIdDict[client];
        }

        /// <summary>
        /// 更新角色数据
        /// </summary>
        public void Update(UserModel model)
        {
            idModelDict[model.id] = model;
        }
    }
}
