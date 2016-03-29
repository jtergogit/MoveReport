using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoveReport
{
    public class LogisticsSettingModel
    {
        /// <summary>
        /// 物流
        /// </summary>
        public string Logistics { set; get; }


        public List<ParamModel> ParamModels { get; set; }


    }

    public class LogisticsSetting
    {
        public static List<LogisticsSettingModel> GetLogisticsSetting()
        {
            List<LogisticsSettingModel> list = new List<LogisticsSettingModel>();

            //速卖通
            LogisticsSettingModel AliExpress = new LogisticsSettingModel();
            AliExpress.Logistics = "EShop.AliExpress";
            AliExpress.ParamModels = new List<ParamModel>();
            AliExpress.ParamModels.Add(new ParamModel() { Name = "IsShowLogisticsCompany", Type = "checkbox", Description = "是否显示物流公司", Value = "", Text = "" });
            AliExpress.ParamModels.Add(new ParamModel() { Name = "Signature", Type = "text", Description = "寄件人签字", Value = "", Text = "" });

            AliExpress.ParamModels.Add(new ParamModel() { Name = "ReceivingWarehouse", Type = "text", Description = "收货仓库", Value = "", Text = "" });
            list.Add(AliExpress);

            //发货地址
            LogisticsSettingModel SendAddress = new LogisticsSettingModel();
            SendAddress.Logistics = "ERP.Reports.SendAddress";
            SendAddress.ParamModels = new List<ParamModel>();
            SendAddress.ParamModels.Add(new ParamModel() { Name = "IsShowLogisticsCompany", Type = "checkbox", Description = "是否显示物流公司", Value = "", Text = "" });
            SendAddress.ParamModels.Add(new ParamModel() { Name = "Signature", Type = "text", Description = "寄件人签字", Value = "", Text = "" });
            list.Add(SendAddress);

            //发货地址+产品
            LogisticsSettingModel AddressAndPicking = new LogisticsSettingModel();
            AddressAndPicking.Logistics = "ERP.Reports.AddressAndPicking";
            AddressAndPicking.ParamModels = new List<ParamModel>();
            AddressAndPicking.ParamModels.Add(new ParamModel() { Name = "IsShowLogisticsCompany", Type = "checkbox", Description = "是否显示物流公司", Value = "", Text = "" });
            AddressAndPicking.ParamModels.Add(new ParamModel() { Name = "Signature", Type = "text", Description = "寄件人签字", Value = "", Text = "" });
            list.Add(AddressAndPicking);

            //发货汇总
            LogisticsSettingModel SIProductList = new LogisticsSettingModel();
            SIProductList.Logistics = "ERP.Reports.SIProductList";
            SIProductList.ParamModels = new List<ParamModel>();
            SIProductList.ParamModels.Add(new ParamModel() { Name = "IsShowLogisticsCompany", Type = "checkbox", Description = "是否显示物流公司", Value = "", Text = "" });
            SIProductList.ParamModels.Add(new ParamModel() { Name = "Signature", Type = "text", Description = "寄件人签字", Value = "", Text = "" });
            list.Add(SIProductList);

            //新加坡
            LogisticsSettingModel Singapore = new LogisticsSettingModel();
            Singapore.Logistics = "ERP.Reports.Singapore";
            Singapore.ParamModels = new List<ParamModel>();
            Singapore.ParamModels.Add(new ParamModel() { Name = "IsShowLogisticsCompany", Type = "checkbox", Description = "是否显示物流公司", Value = "", Text = "" });
            Singapore.ParamModels.Add(new ParamModel() { Name = "Signature", Type = "text", Description = "寄件人签字", Value = "", Text = "" });

            Singapore.ParamModels.Add(new ParamModel() { Name = "CustomerCode", Type = "text", Description = "客户代码", Value = "", Text = "" });
            list.Add(Singapore);


            //北京平邮
            LogisticsSettingModel BeiJingEMS = new LogisticsSettingModel();
            BeiJingEMS.Logistics = "ERP.Reports.BeiJingEMS";
            BeiJingEMS.ParamModels = new List<ParamModel>();
            BeiJingEMS.ParamModels.Add(new ParamModel() { Name = "IsShowLogisticsCompany", Type = "checkbox", Description = "是否显示物流公司", Value = "", Text = "" });
            BeiJingEMS.ParamModels.Add(new ParamModel() { Name = "Signature", Type = "text", Description = "寄件人签字", Value = "", Text = "" });

            BeiJingEMS.ParamModels.Add(new ParamModel() { Name = "CustomerCode", Type = "text", Description = "客户代码", Value = "", Text = "" });
            BeiJingEMS.ParamModels.Add(new ParamModel() { Name = "PostAccountName", Type = "text", Description = "协议客户名称", Value = "", Text = "" });
            BeiJingEMS.ParamModels.Add(new ParamModel() { Name = "PostAccountId", Type = "text", Description = "协议客户账号", Value = "", Text = "" });
            BeiJingEMS.ParamModels.Add(new ParamModel() { Name = "ReturnPostOffice", Type = "text", Description = "退件单位", Value = "", Text = "" });
            BeiJingEMS.ParamModels.Add(new ParamModel() { Name = "PostAccountAddress", Type = "text", Description = "协议客户地址", Value = "", Text = "" });
            BeiJingEMS.ParamModels.Add(new ParamModel() { Name = "postCode", Type = "text", Description = "寄件人邮编", Value = "", Text = "" });
            BeiJingEMS.ParamModels.Add(new ParamModel() { Name = "Tel", Type = "text", Description = "寄件人电话", Value = "", Text = "" });
            list.Add(BeiJingEMS);

            //荷兰邮政小包挂号含电
            LogisticsSettingModel Postnl = new LogisticsSettingModel();
            Postnl.Logistics = "ERP.Reports.Postnl";
            Postnl.ParamModels = new List<ParamModel>();
            Postnl.ParamModels.Add(new ParamModel() { Name = "IsShowLogisticsCompany", Type = "checkbox", Description = "是否显示物流公司", Value = "", Text = "" });
            Postnl.ParamModels.Add(new ParamModel() { Name = "Signature", Type = "text", Description = "寄件人签字", Value = "", Text = "" });

            Postnl.ParamModels.Add(new ParamModel() { Name = "CustomerCode", Type = "text", Description = "客户代码", Value = "", Text = "" });
            list.Add(Postnl);

            //[燕文]中邮新疆挂号
            LogisticsSettingModel YanWen = new LogisticsSettingModel();
            YanWen.Logistics = "Logistics.YanWen";
            YanWen.ParamModels = new List<ParamModel>();
            YanWen.ParamModels.Add(new ParamModel() { Name = "IsShowLogisticsCompany", Type = "checkbox", Description = "是否显示物流公司", Value = "", Text = "" });
            YanWen.ParamModels.Add(new ParamModel() { Name = "Signature", Type = "text", Description = "寄件人签字", Value = "", Text = "" });

            YanWen.ParamModels.Add(new ParamModel() { Name = "CustomerCode", Type = "text", Description = "客户代码", Value = "", Text = "" });
            list.Add(YanWen);


            //一体化面单
            LogisticsSettingModel ChinaPost = new LogisticsSettingModel();
            ChinaPost.Logistics = "Logistics.ChinaPost";
            ChinaPost.ParamModels = new List<ParamModel>();
            ChinaPost.ParamModels.Add(new ParamModel() { Name = "IsShowLogisticsCompany", Type = "checkbox", Description = "是否显示物流公司", Value = "", Text = "" });
            ChinaPost.ParamModels.Add(new ParamModel() { Name = "Signature", Type = "text", Description = "寄件人签字", Value = "", Text = "" });

            ChinaPost.ParamModels.Add(new ParamModel() { Name = "IsShowPostAccount", Type = "checkbox", Description = "是否显示协议客户", Value = "True", Text = "" });
            ChinaPost.ParamModels.Add(new ParamModel() { Name = "PostAccountName", Type = "text", Description = "协议客户名称", Value = "", Text = "" });
            ChinaPost.ParamModels.Add(new ParamModel() { Name = "PostAccountId", Type = "text", Description = "协议客户账号", Value = "", Text = "" });
            ChinaPost.ParamModels.Add(new ParamModel() { Name = "ReturnPostOffice", Type = "text", Description = "退件单位", Value = "", Text = "" });
            ChinaPost.ParamModels.Add(new ParamModel() { Name = "PostAccountAddress", Type = "text", Description = "协议客户地址", Value = "", Text = "" });
            ChinaPost.ParamModels.Add(new ParamModel() { Name = "ReceivingWarehouse", Type = "text", Description = "收货仓库", Value = "", Text = "" });
            ChinaPost.ParamModels.Add(new ParamModel() { Name = "PostAccountZipCode", Type = "text", Description = "协议客户邮编", Value = "", Text = "" });
            ChinaPost.ParamModels.Add(new ParamModel() { Name = "PostAccountMobile", Type = "text", Description = "协议客户电话", Value = "", Text = "" });
            list.Add(ChinaPost);

            //4PX
            LogisticsSettingModel FourPX = new LogisticsSettingModel();
            FourPX.Logistics = "Logistics.FourPX";
            FourPX.ParamModels = new List<ParamModel>();
            FourPX.ParamModels.Add(new ParamModel() { Name = "IsShowLogisticsCompany", Type = "checkbox", Description = "是否显示物流公司", Value = "", Text = "" });
            FourPX.ParamModels.Add(new ParamModel() { Name = "Signature", Type = "text", Description = "寄件人签字", Value = "", Text = "" });

            FourPX.ParamModels.Add(new ParamModel() { Name = "PostAccountName", Type = "text", Description = "协议客户名称", Value = "", Text = "" });
            FourPX.ParamModels.Add(new ParamModel() { Name = "PostAccountId", Type = "text", Description = "协议客户账号", Value = "", Text = "" });
            FourPX.ParamModels.Add(new ParamModel() { Name = "ReturnPostOffice", Type = "text", Description = "退件单位", Value = "", Text = "" });
            FourPX.ParamModels.Add(new ParamModel() { Name = "PostAccountAddress", Type = "text", Description = "协议客户地址", Value = "", Text = "" });
            list.Add(FourPX);

            //华翰
            LogisticsSettingModel HHExp = new LogisticsSettingModel();
            HHExp.Logistics = "Logistics.HHExp";
            HHExp.ParamModels = new List<ParamModel>();
            HHExp.ParamModels.Add(new ParamModel() { Name = "IsShowLogisticsCompany", Type = "checkbox", Description = "是否显示物流公司", Value = "", Text = "" });
            HHExp.ParamModels.Add(new ParamModel() { Name = "Signature", Type = "text", Description = "寄件人签字", Value = "", Text = "" });
            
            list.Add(HHExp);

            //宝通达
            LogisticsSettingModel Emmis = new LogisticsSettingModel();
            Emmis.Logistics = "Logistics.Emmis";
            Emmis.ParamModels = new List<ParamModel>();
            Emmis.ParamModels.Add(new ParamModel() { Name = "IsShowLogisticsCompany", Type = "checkbox", Description = "是否显示物流公司", Value = "", Text = "" });
            Emmis.ParamModels.Add(new ParamModel() { Name = "Signature", Type = "text", Description = "寄件人签字", Value = "", Text = "" });

            Emmis.ParamModels.Add(new ParamModel() { Name = "CustomerCode", Type = "text", Description = "用户账号", Value = "", Text = "" });

            list.Add(Emmis);

            //俄速通
            LogisticsSettingModel Ruston = new LogisticsSettingModel();
            Ruston.Logistics = "Logistics.Ruston";
            Ruston.ParamModels = new List<ParamModel>();
            Ruston.ParamModels.Add(new ParamModel() { Name = "IsShowLogisticsCompany", Type = "checkbox", Description = "是否显示物流公司", Value = "", Text = "" });
            Ruston.ParamModels.Add(new ParamModel() { Name = "Signature", Type = "text", Description = "寄件人签字", Value = "", Text = "" });
            
            Ruston.ParamModels.Add(new ParamModel() { Name = "PostAccountName", Type = "text", Description = "协议客户名称", Value = "", Text = "" });
            Ruston.ParamModels.Add(new ParamModel() { Name = "PostAccountId", Type = "text", Description = "协议客户账号", Value = "", Text = "" });
            Ruston.ParamModels.Add(new ParamModel() { Name = "ReturnPostOffice", Type = "text", Description = "退件单位", Value = "", Text = "" });
            Ruston.ParamModels.Add(new ParamModel() { Name = "PostAccountAddress", Type = "text", Description = "协议客户地址", Value = "", Text = "" });
            Ruston.ParamModels.Add(new ParamModel() { Name = "postCode", Type = "text", Description = "寄件人邮编", Value = "", Text = "" });
            Ruston.ParamModels.Add(new ParamModel() { Name = "Tel", Type = "text", Description = "寄件人电话", Value = "", Text = "" });
            list.Add(Ruston);

            //E邮宝
            LogisticsSettingModel ePacket = new LogisticsSettingModel();
            ePacket.Logistics = "Logistics.ePacket";
            ePacket.ParamModels = new List<ParamModel>();
            ePacket.ParamModels.Add(new ParamModel() { Name = "IsShowLogisticsCompany", Type = "checkbox", Description = "是否显示物流公司", Value = "", Text = "" });
            ePacket.ParamModels.Add(new ParamModel() { Name = "Signature", Type = "text", Description = "寄件人签字", Value = "", Text = "" });

            list.Add(ePacket);

            //顺丰俄罗斯
            LogisticsSettingModel SFexpress = new LogisticsSettingModel();
            SFexpress.Logistics = "Logistics.SFexpress";
            SFexpress.ParamModels = new List<ParamModel>();
            SFexpress.ParamModels.Add(new ParamModel() { Name = "IsShowLogisticsCompany", Type = "checkbox", Description = "是否显示物流公司", Value = "", Text = "" });
            SFexpress.ParamModels.Add(new ParamModel() { Name = "Signature", Type = "text", Description = "寄件人签字", Value = "", Text = "" });

            SFexpress.ParamModels.Add(new ParamModel() { Name = "CustomerCode", Type = "text", Description = "客户代码", Value = "", Text = "" });

            list.Add(SFexpress);

            //顺丰国际
            LogisticsSettingModel SFHLExpress = new LogisticsSettingModel();
            SFHLExpress.Logistics = "Logistics.SFHLExpress";
            SFHLExpress.ParamModels = new List<ParamModel>();
            SFHLExpress.ParamModels.Add(new ParamModel() { Name = "IsShowLogisticsCompany", Type = "checkbox", Description = "是否显示物流公司", Value = "", Text = "" });
            SFHLExpress.ParamModels.Add(new ParamModel() { Name = "Signature", Type = "text", Description = "寄件人签字", Value = "", Text = "" });

            SFHLExpress.ParamModels.Add(new ParamModel() { Name = "CustomerCode", Type = "text", Description = "客户代码", Value = "", Text = "" });

            list.Add(SFHLExpress);

            //互联易
            LogisticsSettingModel Szice = new LogisticsSettingModel();
            Szice.Logistics = "Logistics.Szice";
            Szice.ParamModels = new List<ParamModel>();
            Szice.ParamModels.Add(new ParamModel() { Name = "IsShowLogisticsCompany", Type = "checkbox", Description = "是否显示物流公司",Value="",Text=""});
            Szice.ParamModels.Add(new ParamModel() { Name = "Signature", Type = "text", Description = "寄件人签字", Value = "", Text = "" });

            Szice.ParamModels.Add(new ParamModel() { Name = "PostAccountName", Type = "text", Description = "协议客户名称", Value = "", Text = "" });
            Szice.ParamModels.Add(new ParamModel() { Name = "PostAccountId", Type = "text", Description = "协议客户账号", Value = "", Text = "" });
            Szice.ParamModels.Add(new ParamModel() { Name = "ReturnPostOffice", Type = "text", Description = "退件单位", Value = "", Text = "" });
            Szice.ParamModels.Add(new ParamModel() { Name = "PostAccountAddress", Type = "text", Description = "协议客户地址", Value = "", Text = "" });
            list.Add(Szice);

            //八达通
            LogisticsSettingModel Kuaidi = new LogisticsSettingModel();
            Kuaidi.Logistics = "Logistics.Kuaidi";
            Kuaidi.ParamModels = new List<ParamModel>();
            Kuaidi.ParamModels.Add(new ParamModel() { Name = "IsShowLogisticsCompany", Type = "checkbox", Description = "是否显示物流公司", Value = "", Text = "" });
            Kuaidi.ParamModels.Add(new ParamModel() { Name = "Signature", Type = "text", Description = "寄件人签字", Value = "", Text = "" });

            List<ParamItem> AbColsetItems = new List<ParamItem>();
            AbColsetItems.Add(new ParamItem() { Value = "N",Text= "不打印" });
            AbColsetItems.Add(new ParamItem() { Value = "Y", Text = "一起打印" });
            Kuaidi.ParamModels.Add(new ParamModel() { Name = "AbColset", Type = "select", Description = "是否打印报关单", ParamItems = AbColsetItems, Value = "", Text = "" });
            List<ParamItem> PageTypeItems = new List<ParamItem>();
            PageTypeItems.Add(new ParamItem() { Value = "Label_100_90", Text = "标签纸(100*90)" });
            PageTypeItems.Add(new ParamItem() { Value = "Label_100_100", Text = "标签纸(100*100)" });
            Kuaidi.ParamModels.Add(new ParamModel() { Name = "PageType", Type = "select", Description = "纸张", ParamItems = PageTypeItems, Value = "", Text = "" });
            List<ParamItem> ItemTitleItems = new List<ParamItem>();
            ItemTitleItems.Add(new ParamItem() { Value = "0", Text = "显示物品详情" });
            ItemTitleItems.Add(new ParamItem() { Value = "1", Text = "不显示物品详情" });
            Kuaidi.ParamModels.Add(new ParamModel() { Name = "ItemTitle", Type = "select", Description = "物品详情", ParamItems = ItemTitleItems, Value = "", Text = "" });
            List<ParamItem> PrintTypeItems = new List<ParamItem>();
            PrintTypeItems.Add(new ParamItem() { Value = "html", Text = "网页" });
            PrintTypeItems.Add(new ParamItem() { Value = "pdf", Text = "PDF" });
            Kuaidi.ParamModels.Add(new ParamModel() { Name = "PrintType", Type = "select", Description = "打印输出方式", ParamItems = PrintTypeItems, Value = "", Text = "" });
            list.Add(Kuaidi);

            //一体化面单
            LogisticsSettingModel Qy6 = new LogisticsSettingModel();
            Qy6.Logistics = "Logistics.Qy6";
            Qy6.ParamModels = new List<ParamModel>();
            Qy6.ParamModels.Add(new ParamModel() { Name = "IsShowLogisticsCompany", Type = "checkbox", Description = "是否显示物流公司", Value = "", Text = "" });
            Qy6.ParamModels.Add(new ParamModel() { Name = "Signature", Type = "text", Description = "寄件人签字", Value = "", Text = "" });

            Qy6.ParamModels.Add(new ParamModel() { Name = "IsShowPostAccount", Type = "checkbox", Description = "是否显示协议客户", Value = "True", Text = "" });
            Qy6.ParamModels.Add(new ParamModel() { Name = "PostAccountName", Type = "text", Description = "协议客户名称", Value = "", Text = "" });
            Qy6.ParamModels.Add(new ParamModel() { Name = "PostAccountId", Type = "text", Description = "协议客户账号", Value = "", Text = "" });
            Qy6.ParamModels.Add(new ParamModel() { Name = "ReturnPostOffice", Type = "text", Description = "退件单位", Value = "", Text = "" });
            Qy6.ParamModels.Add(new ParamModel() { Name = "PostAccountAddress", Type = "text", Description = "协议客户地址", Value = "", Text = "" });
            Qy6.ParamModels.Add(new ParamModel() { Name = "ReceivingWarehouse", Type = "text", Description = "收货仓库", Value = "", Text = "" });
            Qy6.ParamModels.Add(new ParamModel() { Name = "PostAccountZipCode", Type = "text", Description = "协议客户邮编", Value = "", Text = "" });
            Qy6.ParamModels.Add(new ParamModel() { Name = "PostAccountMobile", Type = "text", Description = "协议客户电话", Value = "", Text = "" });
            list.Add(Qy6);

            //SIReports
            LogisticsSettingModel SIReports = new LogisticsSettingModel();
            SIReports.Logistics = "ERP.Reports.SIReports";
            SIReports.ParamModels = new List<ParamModel>();
            SIReports.ParamModels.Add(new ParamModel() { Name = "IsShowLogisticsCompany", Type = "checkbox", Description = "是否显示物流公司", Value = "", Text = "" });
            SIReports.ParamModels.Add(new ParamModel() { Name = "Signature", Type = "text", Description = "寄件人签字", Value = "", Text = "" });
            list.Add(SIReports);

            return list;
        }
    }
}
