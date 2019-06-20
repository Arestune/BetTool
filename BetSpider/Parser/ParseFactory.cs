using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BetSpider.Parser.Basketball;
using BetSpider.Parser.ESport;
using BetSpider.Item;
namespace BetSpider.Parser
{
    class ParseFactory
    {
        public BaseParser GetParser( SportID sportId,WebID siteID)
        {
            switch(sportId)
            {
                case SportID.SID_BASKETBALL:
                    if(siteID == WebID.WID_YABO)
                    {
                        return new BasketballParser_Yabo();
                    }
                    if (siteID == WebID.WID_YAYOU)
                    {
                        return new BasketballParser_Yayou();
                    }
                    break;
                case SportID.SID_ESPORT:
                    if (siteID == WebID.WID_YABO)
                    {
                        return new ESportParser_Yabo();
                    }
                    if (siteID == WebID.WID_YAYOU)
                    {
                        return new ESportParser_Yayou();
                    }
                    break;
            }
            return new BaseParser();
        }
    }
}
