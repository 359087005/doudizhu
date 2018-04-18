using AhpilyServer;
using AhpilyServer.ConCurrent;
using GameServer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Cache
{

    /// <summary>
    /// 账号缓存 
    /// </summary>
    public class AccountCache
    {
        /// <summary>
        /// 账号  对应的  账号数据模型
        /// </summary>
        private Dictionary<string, AccountModel> accModelDict = new Dictionary<string, AccountModel>();

        /// <summary>
        /// 用来存储账号ID   后期通过数据库自己增加
        /// </summary>
        private ConCurrentInt id = new ConCurrentInt(-1);

        /// <summary>
        /// 判断否存在账号
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public bool IsExist(string account)
        {
            return accModelDict.ContainsKey(account);
        }
        /// <summary>
        /// 创建账号数据模型信息
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        public void Creat(string account, string password)
        {
            AccountModel model = new AccountModel(id.Add_Get(), account, password);
            accModelDict.Add(model.account, model);
        }

        /// <summary>
        /// 获取对应账号的数据模型      这里要100%保证数据存在的原因 是在逻辑层就要考虑好所有情况。
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public AccountModel GetModel(string account)
        {
            return accModelDict[account];
        }
        /// <summary>
        /// 账号密码是否匹配
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool IsMatch(string account, string password)
        {
            AccountModel model = accModelDict[account];
            return model.password == password;
        }
        /// <summary>
        /// 账号   连接对象   
        /// </summary>
        private Dictionary<string, ClientPeer> accClientDict = new Dictionary<string, ClientPeer>();
        private Dictionary<ClientPeer, string> clientAccDict = new Dictionary<ClientPeer, string>();
        /// <summary>
        /// 是否在线
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public bool IsOnline(string account)
        {
            return accClientDict.ContainsKey(account);
        }
        public bool IsOnline(ClientPeer client)
        {
            return clientAccDict.ContainsKey(client);
        }

        /// <summary>
        /// 上线
        /// </summary>
        /// <param name="account"></param>
        /// <param name="client"></param>
        public void OnLine(string account, ClientPeer client)
        {
            accClientDict.Add(account,client);
            clientAccDict.Add(client,account);
        }

        /// <summary>
        /// 下线
        /// </summary>
        /// <param name="account"></param>
        public void Offline(string account)
        {
            ClientPeer client = accClientDict[account];
            accClientDict.Remove(account);
            clientAccDict.Remove(client);
        }
        public void Offline(ClientPeer client)
        {
            string account = clientAccDict[client];
            accClientDict.Remove(account);
            clientAccDict.Remove(client);
        }
        /// <summary>
        /// 获取在线玩家的ID
        /// </summary>
        /// <returns></returns>
        public int GetId(ClientPeer client)
        {
            string account = clientAccDict[client];
            AccountModel model = accModelDict[account];
            return model.id;
        }

    }
}
