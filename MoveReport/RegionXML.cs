using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoveReport
{
    public class RegionXML
    {
        public static Dictionary<string, string> GetRegionXML(string logistics,string reportName)
        {
            Dictionary<string, string> par = new Dictionary<string, string>();
            if (logistics == "EShop.AliExpress")
            {
                par["DeliveryRegion"] = "国家分拣区号.xml";
                par["SortNo"] = "ChinaPostSortingCode.xml";
                par["MailCountryRegion"] = "ChinaPostNormalPartition.xml";
                par["RegistCountryRegion"] = "ChinaPostRegisteredPartition.xml";
            }
            else if (logistics == "ERP.Reports.BeiJingEMS")
            {
                if (reportName.Contains("北京平邮"))
                {
                    par["DeliveryRegion"] = "中邮北京平邮小包分组规则.xml";
                }
                else if (reportName.Contains("北京挂号"))
                {
                    par["DeliveryRegion"] = "中邮北京挂号小包分组规则.xml";
                }
            }
            else if (logistics == "Logistics.Qy6")
            {
                par["CountryRegion"] = "ChinaPostRegisteredPartition.xml";
            }
            else if (logistics == "ERP.Reports.Postnl")
            {
                par["EU"] = "EU country list(荷兰标签显示EU).xml";
            }
            else if (logistics == "ERP.Reports.Russia")
            {
                par["Region"] = "俄罗斯平邮分区.xml";
            }
            else if (logistics == "ERP.Reports.Singapore")
            {
                par["SGDvisionZone"] = "SG小包分区表15.01.10(国家英文简码).xml";
               
                if (reportName.Contains("新加坡平邮"))
                {
                    par["Region"] = "新加坡平邮收费分区.xml";
                    par["DeliveryRegion"] = "新加坡平邮派送分区.xml";
                }
                else if (reportName.Contains("新加坡挂号"))
                {
                    par["Region"] = "新加坡挂号小包运费模板.xml";
                    par["DeliveryRegion"] = "新加坡挂号派送分区.xml";
                }
            }
            else if (logistics == "Logistics.ePacket")
            {
                par["PartitionCode"] = "E邮宝分区码.xml";
            }
            else if (logistics == "Logistics.FourPX")
            {
                par["Region"] = "华南挂号小包.xml";
            }
            else if (logistics == "Logistics.HHExp")
            {
                par["BeiJingRegions"] = "北京平邮_100_分区地址.xml";
                par["XiaMenRegions"] = "厦门挂号_15_分区地址.xml";
                par["ShenZhenRegions"] = "深圳挂号_59_分区地址.xml";
                par["ShenZhenPYRegions"] = "深圳平邮BA_20_分区地址.xml";
            }
            else if (logistics == "Logistics.SFexpress")
            {
                par["SurfaceRegions"] = "顺丰流向分区.xml";
                par["RegisteredRegions"] = "顺丰流向分区.xml";

            }
            else if (logistics == "Logistics.SFHLExpress")
            {
                par["DistinctArea"] = "顺丰全球经济小包平邮分区.xml";
            }
            else if (logistics == "Logistics.Szice")
            {
                par["Region"] = "华南挂号小包.xml";
                par["HuLianYiRegions"] = "[互联易]新平邮小包分区.xml";
                par["HuLianYiRegionsCN"] = "[互联易]新平邮小包分区.xml";
                par["Regions"] = "瑞典平邮.xml";
                par["RegisterRegionCode"] = "ChinaPostRegisteredPartition.xml";
                par["SortNo"] = "ChinaPostSortingCode.xml";

            }
            else if (logistics == "Logistics.ChinaPost")
            {
                par["MailCountryRegion"] = "ChinaPostNormalPartition.xml";
                par["RegistCountryRegion"] = "ChinaPostRegisteredPartition.xml";
                par["SortNo"] = "ChinaPostSortingCode.xml";

            }
            return par;
        }
    }
}
