using Protocol;
using Protocol.Content;
using Protocol.Dto.Fight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Net.Impl
{
    public class ChatHandler : HandlerBase
    {
        private ChatMsg msg = new ChatMsg();

        public override void OnReceive(int subCode, object value)
        {
            switch (subCode)
            {
                case ChatCode.SRES:
                    ChatDto dto = value as ChatDto;
                    int userId = dto.userId;
                    int chatType = dto.chatType;
                    string text = ChatContent.GetText(chatType);

                    msg.userId = userId;
                    msg.chatType = chatType;
                    msg.text = text;
                    Dispatch(AreaCode.UI,UIEvent.PLAYER_CHAT,msg);
                    Dispatch(AreaCode.AUDIO,AudioEvent.PLAY_EFFECT_AUDIO,"Chat/Chat_" + chatType);
                    break;
                default:
                    break;
            }
        }
    }
}
