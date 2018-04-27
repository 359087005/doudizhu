using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AhpilyServer;
using Protocol;
using GameServer.Cache;
using GameServer.Model;
using Protocol.Dto;

/// <summary>
/// 用户数据逻辑处理类
/// </summary>
namespace GameServer.Logic
{
    public class UserHandler : IHandler
    {
        UserCache userCache = Caches.user;
        AccountCache accountCache = Caches.account;

        public void OnDisConnect(ClientPeer client)
        {
            if (userCache.IsOnLine(client))
                userCache.OffLine(client);
        }

        public void OnReceive(ClientPeer client, int subCode, object value)
        {
            switch (subCode)
            {
                case UserCode.CREAT_CREQ:
                    Creat(client,value.ToString());
                    break;
                case UserCode.GET_INFO_CREQ:
                    GetInfo(client);
                    break;
                case UserCode.ONLINE_CREQ:
                    OnLine(client);
                    break;
            }
        }
        /// <summary>
        /// 创建角色
        /// </summary>
        /// <param name="client"></param>
        /// <param name="name"></param>
        private void Creat(ClientPeer client, string name)
        {
            SingleExecute.Instance.Execute(delegate 
            {
                //判断客户端是否非法登录
                if (!accountCache.IsOnline(client))
                {
                    client.Send(OpCode.USER, UserCode.CREAT_SRES, -1); //非法登录
                    return;
                }
                //客户端正常登录   获取ACCOUNTID
                int accountId = accountCache.GetId(client);
                //根据id判断是否有角色
                if (userCache.IsExist(accountId))
                {
                    client.Send(OpCode.USER, UserCode.CREAT_SRES, -2); //已经有角色 无法重复创建
                    return;
                }
                //创建ing
                userCache.Creat(name, accountId);
                client.Send(OpCode.USER, UserCode.CREAT_SRES, 0); //成功
            });
        }
        /// <summary>
        /// 获取角色信息
        /// </summary>
        /// <param name="client"></param>
        private void GetInfo(ClientPeer client)
        {
            SingleExecute.Instance.Execute(delegate
            {
                //判断是否非法登录
                //if (!accountCache.IsOnline(client))
                //{
                //    client.Send(OpCode.USER, UserCode.GET_INFO_SRES, -1); //非法登录
                //    return;
                //}
                int accountId = accountCache.GetId(client);

                if (userCache.IsExist(accountId) == false)
                {
                    client.Send(OpCode.USER, UserCode.GET_INFO_SRES, null); //没有角色
                    return;
                }

                //代码执行到这里 代表有角色
                if (userCache.IsOnLine(client) == false) //防止二次调用上线的方法 所以进行二次判断
                {
                    OnLine(client);
                }
                //发送到客户端角色信息
                UserModel model = userCache.GetModelByAccountID(accountId);
                UserDto dto = new UserDto(model.id,model.name,model.been,model.lv,model.exp,model.winCount,model.loseCount,model.runCount);
                client.Send(OpCode.USER, UserCode.GET_INFO_SRES, dto); //成功
            });
           
        }

        /// <summary>
        /// 角色上线
        /// </summary>
        private void OnLine(ClientPeer client)
        {
            SingleExecute.Instance.Execute(delegate
            {
                //判断客户端是否登录
                if (!accountCache.IsOnline(client))
                {
                    client.Send(OpCode.USER, UserCode.ONLINE_SRES, -1); //非法登录
                    return;
                }
                int accountId = accountCache.GetId(client);

                if (userCache.IsExist(accountId) == false)
                {
                    client.Send(OpCode.USER, UserCode.ONLINE_SRES, -2);//无角色 不能上线
                    return;
                }
                int userId = userCache.GetIdByAccId(accountId);
                userCache.OnLine(client, userId);
                client.Send(OpCode.USER, UserCode.ONLINE_SRES, 0);//成功
            });
           
        }
    }
}
