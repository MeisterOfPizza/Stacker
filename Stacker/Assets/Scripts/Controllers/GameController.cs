using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stacker.Controllers
{

    class GameController : Controller<GameController>
    {

        #region Static properties

        public static int TotalStars { get; private set; }

        #endregion

        public static void GivePlayerStars(int stars)
        {
            TotalStars += stars;
        }

    }

}
