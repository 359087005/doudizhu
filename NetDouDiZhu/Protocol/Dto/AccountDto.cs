using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// 账户数据的传输模型
/// </summary>
namespace Protocol.Dto
{

    [Serializable]
    public class AccountDto
    {
        public string Account;
        public string Password;

        public AccountDto()
        {

        }

        public AccountDto(string acc, string pas)
        {
            this.Account = acc;
            this.Password = pas;
        }
    }
}
