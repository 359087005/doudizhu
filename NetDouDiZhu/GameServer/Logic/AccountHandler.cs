using System;
using System.Collections.Generic;
using System.Text;
using AhpilyServer;
using Protocol;
using Protocol.Dto;
using GameServer.Cache;

namespace GameServer.Logic
{
    /// <summary>
    /// 账号处理的逻辑层
    /// </summary>
    public class AccountHandler : IHandler
    {
        AccountCache accountCache = Caches.account; //写成static  保证accountcache的唯一性

        public void OnDisConnect(ClientPeer client)
        {
            if(accountCache.IsOnline(client))
            accountCache.Offline(client);
        }

        public void OnReceive(ClientPeer client, int subCode, object value)
        {
                switch (subCode)
                {
                    case AccountCode.REGIST_CREQ:
                        {
                            AccountDto dto = value as AccountDto;
                        // Console.WriteLine(msg.Account);
                        // Console.WriteLine(msg.Password);
                            Regist(client, dto.Account, dto.Password);
                        }
                        break;
                    case AccountCode.LOGIN:
                        {
                            AccountDto dto = value as AccountDto;
                            //Console.WriteLine(dto.Account);
                            //Console.WriteLine(dto.Password);
                            Login(client, dto.Account, dto.Password);
                        }
                        break;
                }
        }
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="client"></param>
        /// <param name="account"></param>
        /// <param name="password"></param>
        private void Regist(ClientPeer client, string account, string password)
        {
            SingleExecute.Instance.Execute(() =>
            {
                if (accountCache.IsExist(account))
                {
                    //client.Send(OpCode.ACCOUNT, AccountCode.REGIST_SRES, "账号已存在");
                    client.Send(OpCode.ACCOUNT, AccountCode.REGIST_SRES, -1);
                    return; //账号已存在
                }
                if (string.IsNullOrEmpty(account))
                {
                   // client.Send(OpCode.ACCOUNT, AccountCode.REGIST_SRES, "账号为空");
                    client.Send(OpCode.ACCOUNT, AccountCode.REGIST_SRES, -2);
                    return;//账号为空
                }
                if (string.IsNullOrEmpty(password))
                {
                    //client.Send(OpCode.ACCOUNT, AccountCode.REGIST_SRES, "密码不合法");
                    client.Send(OpCode.ACCOUNT, AccountCode.REGIST_SRES, -3);
                    return;//密码不合法
                }
                
                accountCache.Creat(account, password);
                //client.Send(OpCode.ACCOUNT, AccountCode.REGIST_SRES, "注册成功");
                client.Send(OpCode.ACCOUNT, AccountCode.REGIST_SRES, 0);
            });
        }

        private void Login(ClientPeer client, string account, string password)
        {
            SingleExecute.Instance.Execute(() =>
            {
                if (accountCache.IsExist(account) == false)
                {
                    //账号不存在
                    //client.Send(OpCode.ACCOUNT, AccountCode.LOGIN, "账号错误");
                    client.Send(OpCode.ACCOUNT, AccountCode.LOGIN, -1);
                    return;
                }
                if (accountCache.IsMatch(account, password) == false)
                {
                    //账号密码不匹配
                    //client.Send(OpCode.ACCOUNT, AccountCode.LOGIN, "账号密码不匹配");
                    client.Send(OpCode.ACCOUNT, AccountCode.LOGIN, -2);
                    return;
                }
                if (accountCache.IsOnline(account))
                {
                    //账号已经登录
                    //client.Send(OpCode.ACCOUNT, AccountCode.LOGIN, "账号已登录");
                    client.Send(OpCode.ACCOUNT, AccountCode.LOGIN, -3);
                    return;
                }

                accountCache.OnLine(account, client);
                //client.Send(OpCode.ACCOUNT, AccountCode.LOGIN, "登录成功");
                client.Send(OpCode.ACCOUNT, AccountCode.LOGIN, 0);
            });
        }
    }
}
