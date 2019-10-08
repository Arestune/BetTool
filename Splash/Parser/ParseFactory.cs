using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Splash.Parser.Basketball;
using Splash.Parser.ESport;
using Splash.Item;
namespace Splash.Parser
{
    class ParseFactory
    {
        public static BaseParser GetParser( SportID sportId,string webName)
        {
            switch(sportId)
            {
                case SportID.SID_BASKETBALL:
                    if (webName == StaticData.webNames[(int)WebID.WID_IM])
                    {
                        return new BasketballParser_Yabo();
                    }
                    if (webName == StaticData.webNames[(int)WebID.WID_YAYOU])
                    {
                        return new BasketballParser_Yayou();
                    }
                    break;
                case SportID.SID_ESPORT:
                    if (webName == StaticData.webNames[(int)WebID.WID_IM])
                    {
                        return new ESportParser_IM();
                    }
                    else if (webName == StaticData.webNames[(int)WebID.WID_YAYOU])
                    {
                        return new ESportParser_Yayou();
                    }
                    else if (webName == StaticData.webNames[(int)WebID.WID_188])
                    {
                        return new ESportParser_188();
                    }
                    else if (webName == StaticData.webNames[(int)WebID.WID_FANYA])
                    {
                        return new ESportParser_Fanya();
                    }
                    else if (webName == StaticData.webNames[(int)WebID.WID_RAY])
                    {
                        return new ESportParser_Ray();
                    }
                    else if (webName == StaticData.webNames[(int)WebID.WID_UWIN])
                    {
                        return new ESportParser_UWin();
                    }
                    else if (webName == StaticData.webNames[(int)WebID.WID_Pingbo])
                    {
                        return new ESportParser_Pingbo();
                    }
                    else if (webName == StaticData.webNames[(int)WebID.WID_CMD])
                    {
                        return new ESportParser_CMD();
                    }
                    break;
            }
            return new BaseParser();
        }
    }
}
