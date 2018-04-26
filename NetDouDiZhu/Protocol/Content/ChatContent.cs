using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Content
{
    public static class ChatContent
    {
        private static Dictionary<int, string> typeTextDict;

        //private static string[] chatString;

        static ChatContent()
        {
            typeTextDict = new Dictionary<int, string>();
            //chatString = new string[8]
            //    {
            //       "" ,"大家好","合作愉快","快点","你真厉害","不要吵架","坚持到底","再见"
            //    };

            //for (int i = 1; i < 8; i++)
            //{
            //    typeTextDict.Add(i, chatString[i]);
            //}

            typeTextDict.Add(1,"大家好");
            typeTextDict.Add(2, "合作愉快");
            typeTextDict.Add(3, "快点");
            typeTextDict.Add(4, "你真厉害");
            typeTextDict.Add(5, "不要吵架");
            typeTextDict.Add(6, "坚持到底");
            typeTextDict.Add(7, "再见");
        }

        public static string GetText(int chatType)
        {
            return typeTextDict[chatType];
        }
    }
}
