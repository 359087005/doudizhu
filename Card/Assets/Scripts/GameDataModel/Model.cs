using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.GameDataModel
{
  public static  class Model
    {
        public static GameModel gameModel;

         static Model()
        {
            gameModel = new GameModel();
        }
    }
}
