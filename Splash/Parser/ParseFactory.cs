﻿using System;
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
        public static BaseParser GetParser( SportID sportId,WebID siteID)
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
                    else if (siteID == WebID.WID_YAYOU)
                    {
                        return new ESportParser_Yayou();
                    }
                    else if(siteID == WebID.WID_188)
                    {
                        return new ESportParser_188();
                    }
                    else if(siteID == WebID.WID_FANYA)
                    {
                        return new ESportParser_Fanya();
                    }
                    else if(siteID == WebID.WID_RAY)
                    {
                        return new ESportParser_Ray();
                    }
                    break;
            }
            return new BaseParser();
        }
    }
}