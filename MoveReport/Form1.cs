using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace MoveReport
{
    public partial class Form1 : Form
    {
        private string projectPath = string.Empty;
        private bool isSystem = true;
        private string dirName = "ERP.Reports.SIReports";
        private List<string> repeats = new List<string>();
        private Dictionary<string, string> reports = new Dictionary<string, string>();
        private bool IsPlugins = true;

        private string Log
        {
            get { return projectPath + @"\MoveReportLog.txt"; }
        }
        private string NoRegion
        {
            get { return projectPath + @"\NoRegionLog.txt"; }
        }
        private string Repeat
        {
            get { return projectPath + @"\RepeatLog.txt"; }
        }

        public Form1()
        {
            InitializeComponent();
            //ProjectPath.Text = @"D:\repo\ERP";
            //ReportType.Text = "系统模版";
            //ReportType.Text = "客户自定义模版";
            this.ReportType.Items.Add("客户自定义模版");
            this.ReportType.Items.Add("系统模版");
            //CheckReport();
        }

        private void SelectFolder_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                if (!string.IsNullOrWhiteSpace(folderBrowserDialog1.SelectedPath))
                {
                    ProjectPath.Text = folderBrowserDialog1.SelectedPath;
                }
            }
        }

        private void MoveFile_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ProjectPath.Text))
            {
                MessageBox.Show("请选择项目路径");
                return;
            }
            if (string.IsNullOrEmpty(ReportType.Text))
            {
                MessageBox.Show("请选择要移动的模版");
                return;
            }
            projectPath = ProjectPath.Text;

            toolStripStatusLabel1.Text = "开始移动";

            string fromDir = projectPath;
            string toDir = projectPath;
            if (ReportType.Text == "客户自定义模版")
            {
                isSystem = false;
                Task.Run(() =>
                {
                    MoveCustormReport(fromDir, toDir);
                    MessageBox.Show(toolStripStatusLabel1.Text);
                });
            }
            else if (ReportType.Text == "系统模版")
            {
                isSystem = true;
                Task.Run(() =>
                {
                    MoveSystemReport(fromDir, toDir);
                    MessageBox.Show(toolStripStatusLabel1.Text);
                });
            }
            else if (ReportType.Text == "")
            {
                MessageBox.Show("请选择要移动的模版");
                return;
            }
            else
            {
                MessageBox.Show("请选择正确的模版");
                return;
            }
        }

        private void MoveCustormReport(string fromDir, string toDir)
        {
            try
            {
                //fromDir += @"\WillV.Web\UserData\";
                //toDir += @"\WillV.Web\UserData\";

                string fpath = fromDir + @"\WillV.Web\UserData\";
                string tpath = toDir + @"\WillV.Web\UserData\";
                if (!Directory.Exists(fpath))
                {
                    fpath = fromDir + @"\UserData\";
                    tpath = toDir + @"\UserData\";
                }
                if (!Directory.Exists(fpath))
                {
                    // MessageBox.Show(fpath + "文件不存在");
                    toolStripStatusLabel1.Text = fpath + "文件不存在";
                    return;
                }

                string[] fromDirs = Directory.GetDirectories(fpath);
                foreach (string fromDirPath in fromDirs)
                {
                    tpath = Path.Combine(fromDirPath, dirName);
                    if (!Directory.Exists(tpath))
                    {
                        Directory.CreateDirectory(tpath);
                    }
                    MoveReportFile(fromDirPath, tpath);
                }
                toolStripStatusLabel1.Text = "任务完成";
               // MessageBox.Show("任务完成");
            }
            catch (Exception ex)
            {
                WriteLog(Log,DateTime.Now + ex.Message);
                toolStripStatusLabel1.Text = ex.Message;
            }
        }

        private void MoveSystemReport(string fromDir, string toDir)
        {
            try
            {
                string fpath = string.Empty;
                string tpath = string.Empty;
                if (IsPlugins)
                {
                    fpath = fromDir + @"\WillV.Web\Plugins\";
                    tpath = toDir + @"\WillV.Web\Plugins\" + dirName + "\\";
                }
                else
                {
                    fpath = fromDir + @"\WillV.Web\Plugins\";
                    tpath = toDir + @"\WillV.Web\ERP\ERP.Reports\" + dirName + "\\";
                }

                if (!Directory.Exists(fpath) || !Directory.Exists(tpath))
                {
                    fpath = fromDir + @"\Plugins\";
                    tpath = toDir + @"\Plugins\" + dirName + "\\";
                }

                if (!Directory.Exists(fpath))
                {
                    //MessageBox.Show(fromDir + "文件不存在");
                    toolStripStatusLabel1.Text = fpath + "文件不存在";
                    return;
                }
                if (!Directory.Exists(tpath))
                {
                    //MessageBox.Show(tpath + "文件不存在");
                    toolStripStatusLabel1.Text = tpath + "文件不存在";
                    return;
                }

                MoveReportFile(fpath, tpath);
                toolStripStatusLabel1.Text = "任务完成";
                //MessageBox.Show("任务完成");

            }
            catch (Exception ex)
            {
                WriteLog(Log, DateTime.Now + ex.Message);
                //string message = ex.Message;
                //if (ex.Message.Length > 60)
                //{
                //    message = ex.Message.Substring(0, 60);
                //}
                toolStripStatusLabel1.Text = ex.Message;
            }
        }

        private void MoveReportFile(string fromDir, string toDir)
        {
            
            string defaultDir = Path.Combine(fromDir, dirName);

            foreach (string formFileName in Directory.GetFiles(defaultDir))
            {
                string suffix = Path.GetExtension(formFileName);
                if (suffix.ToLower() == ".frx")
                {

                    string fileName = Path.GetFileName(formFileName);
                    toolStripStatusLabel1.Text = fileName;
                    reports[fileName] = formFileName;
                    InsertDataSource(formFileName, formFileName, dirName);
                    //File.Delete(fileName);
                    WriteLog(Log, DateTime.Now + formFileName + "移动成功。");

                }
            }

            string[] fromDirs = Directory.GetDirectories(fromDir);
            foreach (string fromDirPath in fromDirs)
            {
                string logisticsDirName = fromDirPath.Substring(fromDirPath.LastIndexOf("\\") + 1);
                string[] ignoreLogistics = new string[] { "ERP.Reports.PlanROrderSummary", "ERP.Reports.PurchaseOrderList" };
                if ((IsPlugins?(logisticsDirName != dirName):true) && !ignoreLogistics.Contains(logisticsDirName))
                {
                    string[] files = Directory.GetFiles(fromDirPath);
                    foreach (string formFileName in files)
                    {
                        string suffix = Path.GetExtension(formFileName);
                        if (suffix.ToLower() == ".frx")
                        {

                            string fileName = Path.GetFileName(formFileName);
                            toolStripStatusLabel1.Text = fileName;
                            
                            string toFileName = GetToFileName(formFileName, toDir, fileName, suffix);
                            File.Copy(formFileName, toFileName, true);

                            reports[fileName] = formFileName;
                            InsertDataSource(formFileName, toFileName, logisticsDirName);
                            //File.Delete(fileName);
                            WriteLog(Log, DateTime.Now + formFileName + "移动成功。");

                        }
                    }
                }
            }
        }

        private string GetToFileName(string formFileName, string path, string fileName, string suffix)
        {
            string filePathName = Path.Combine(path, fileName);
            if (File.Exists(filePathName))
            {
                if (!repeats.Contains(formFileName))
                {
                    string f1 = formFileName.Substring(formFileName.Length > 30 ?30: 0);
                    string f2 = string.Empty;
                    if (reports.Keys.Contains(fileName))
                    {
                        f2 = reports[fileName].Substring(reports[fileName].Length > 30 ? 30 : 0);
                    }
                    WriteLog(Repeat, f1+"-----------------"+f2);
                }
                repeats.Add(formFileName);

                string newfilename = Path.GetFileNameWithoutExtension(fileName);
                int index = newfilename.LastIndexOf("-");
                int i = 0;
                if (index != -1 && int.TryParse(newfilename.Substring(index + 1), out i))
                    newfilename = newfilename.Substring(0, index + 1) + (++i).ToString() + suffix;
                else
                    newfilename += "-" + (++i).ToString() + suffix;
                return GetToFileName(formFileName, path, newfilename, suffix);
            }
            return filePathName;
        }

        private void InsertDataSource(string formFileName,string filePath,string logistics)
        {
            bool exitRegion = false;
            XmlDocument doc = new XmlDocument();
            doc.Load(filePath);
            XmlNodeList xlist = doc.SelectSingleNode("Report/Dictionary").SelectNodes("Parameter");
            foreach (XmlNode xnode in xlist)
            {
                if (xnode.Attributes["Name"].Value == "SortingCode" || xnode.Attributes["Name"].Value == "Region")
                {
                    doc.SelectSingleNode("Report/Dictionary").RemoveChild(xnode);
                }
            }

            //添加数据源
            #region
            //XmlNode dnode = doc.CreateNode(XmlNodeType.Element, "Parameter", null);
            //doc.SelectSingleNode("Report/Dictionary").AppendChild(dnode);

            //XmlAttribute name = doc.CreateAttribute("Name");
            //name.Value = "DataSource";
            //dnode.Attributes.Append(name);

            //XmlAttribute datatype = doc.CreateAttribute("DataType");
            //datatype.Value = "System.String";
            //dnode.Attributes.Append(datatype);

            //XmlAttribute expression = doc.CreateAttribute("Expression");
            //expression.Value = dirName;
            //dnode.Attributes.Append(expression);
            #endregion

            //添加分区码
            #region
            //XmlDocument RegionXml = new XmlDocument();
            ////RegionXml.Load(projectPath+ @"\WillV.Web\Plugins\ERP.Reports.SIReports\面单对应分区码.xml");
            //RegionXml.Load("面单对应分区码.xml");
            //XmlNodeList logisticsList = RegionXml.SelectNodes("region/logistics");
            //string fileName = Path.GetFileNameWithoutExtension(formFileName);
            //foreach (XmlNode logisticsNode in logisticsList)
            //{
            //    if (logisticsNode.Attributes["name"].Value == logistics)
            //    {
            //        if (isSystem && logistics == "Logistics.ChinaPost")
            //        {
            //            foreach (XmlNode report in logisticsNode.SelectNodes("area/report"))
            //            {
            //                if (isSystem && report.Attributes["name"].Value == fileName && report.ParentNode.Attributes["name"].Value == "ChinaPostRegisteredPartition.xml")
            //                {
            //                    XmlNode RegionNode = doc.CreateNode(XmlNodeType.Element, "Parameter", null);
            //                    doc.SelectSingleNode("Report/Dictionary").AppendChild(RegionNode);

            //                    XmlAttribute RegionName = doc.CreateAttribute("Name");
            //                    RegionName.Value = "Region";
            //                    RegionNode.Attributes.Append(RegionName);

            //                    XmlAttribute RegionDataType = doc.CreateAttribute("DataType");
            //                    RegionDataType.Value = "System.String";
            //                    RegionNode.Attributes.Append(RegionDataType);

            //                    XmlAttribute RegionExpression = doc.CreateAttribute("Expression");
            //                    RegionExpression.Value = "\"" + report.ParentNode.Attributes["name"].Value + "\"";
            //                    RegionNode.Attributes.Append(RegionExpression);

            //                    exitRegion = true;
            //                }
            //                else if (isSystem && report.Attributes["name"].Value == fileName && report.ParentNode.Attributes["name"].Value == "ChinaPostSortingCode.xml")
            //                {
            //                    XmlNode RegionNode = doc.CreateNode(XmlNodeType.Element, "Parameter", null);
            //                    doc.SelectSingleNode("Report/Dictionary").AppendChild(RegionNode);

            //                    XmlAttribute RegionName = doc.CreateAttribute("Name");
            //                    RegionName.Value = "SortingCode";
            //                    RegionNode.Attributes.Append(RegionName);

            //                    XmlAttribute RegionDataType = doc.CreateAttribute("DataType");
            //                    RegionDataType.Value = "System.String";
            //                    RegionNode.Attributes.Append(RegionDataType);

            //                    XmlAttribute RegionExpression = doc.CreateAttribute("Expression");
            //                    RegionExpression.Value = "\"" + report.ParentNode.Attributes["name"].Value + "\"";
            //                    RegionNode.Attributes.Append(RegionExpression);

            //                    exitRegion = true;
            //                }

            //            }
            //        }
            //        else if (isSystem && logistics == "ERP.Reports.Singapore")
            //        {
            //            foreach (XmlNode report in logisticsNode.SelectNodes("area/report"))
            //            {
            //                if (isSystem && report.Attributes["name"].Value == fileName && report.ParentNode.Attributes["name"].Value.Contains("派送分区"))
            //                {
            //                    XmlNode RegionNode = doc.CreateNode(XmlNodeType.Element, "Parameter", null);
            //                    doc.SelectSingleNode("Report/Dictionary").AppendChild(RegionNode);

            //                    XmlAttribute RegionName = doc.CreateAttribute("Name");
            //                    RegionName.Value = "SortingCode";
            //                    RegionNode.Attributes.Append(RegionName);

            //                    XmlAttribute RegionDataType = doc.CreateAttribute("DataType");
            //                    RegionDataType.Value = "System.String";
            //                    RegionNode.Attributes.Append(RegionDataType);

            //                    XmlAttribute RegionExpression = doc.CreateAttribute("Expression");
            //                    RegionExpression.Value = "\"" + report.ParentNode.Attributes["name"].Value + "\"";
            //                    RegionNode.Attributes.Append(RegionExpression);

            //                    if (report.ParentNode.Attributes["name"].Value.Contains("挂号"))
            //                    {
            //                        XmlNode sRegionNode = doc.CreateNode(XmlNodeType.Element, "Parameter", null);
            //                        doc.SelectSingleNode("Report/Dictionary").AppendChild(sRegionNode);

            //                        XmlAttribute sRegionName = doc.CreateAttribute("Name");
            //                        sRegionName.Value = "Region";
            //                        sRegionNode.Attributes.Append(sRegionName);

            //                        XmlAttribute sRegionDataType = doc.CreateAttribute("DataType");
            //                        sRegionDataType.Value = "System.String";
            //                        sRegionNode.Attributes.Append(sRegionDataType);

            //                        XmlAttribute sRegionExpression = doc.CreateAttribute("Expression");
            //                        sRegionExpression.Value = "\"新加坡挂号小包运费模板.xml\"";
            //                        sRegionNode.Attributes.Append(sRegionExpression);
            //                    }

            //                    exitRegion = true;
            //                }
            //                else if (isSystem && report.Attributes["name"].Value == fileName && report.ParentNode.Attributes["name"].Value.Contains("平邮收费分区"))
            //                {
            //                    XmlNode RegionNode = doc.CreateNode(XmlNodeType.Element, "Parameter", null);
            //                    doc.SelectSingleNode("Report/Dictionary").AppendChild(RegionNode);

            //                    XmlAttribute RegionName = doc.CreateAttribute("Name");
            //                    RegionName.Value = "Region";
            //                    RegionNode.Attributes.Append(RegionName);

            //                    XmlAttribute RegionDataType = doc.CreateAttribute("DataType");
            //                    RegionDataType.Value = "System.String";
            //                    RegionNode.Attributes.Append(RegionDataType);

            //                    XmlAttribute RegionExpression = doc.CreateAttribute("Expression");
            //                    RegionExpression.Value = "\"" + report.ParentNode.Attributes["name"].Value + "\"";
            //                    RegionNode.Attributes.Append(RegionExpression);


            //                    exitRegion = true;
            //                }

            //            }
            //        }
            //        else
            //        {
            //            foreach (XmlNode report in logisticsNode.SelectNodes("area/report"))
            //            {
            //                if ((isSystem && report.Attributes["name"].Value == fileName) || (!isSystem && logisticsNode.SelectNodes("area").Count == 1))
            //                {
            //                    XmlNode RegionNode = doc.CreateNode(XmlNodeType.Element, "Parameter", null);
            //                    doc.SelectSingleNode("Report/Dictionary").AppendChild(RegionNode);

            //                    XmlAttribute RegionName = doc.CreateAttribute("Name");
            //                    RegionName.Value = "Region";
            //                    RegionNode.Attributes.Append(RegionName);

            //                    XmlAttribute RegionDataType = doc.CreateAttribute("DataType");
            //                    RegionDataType.Value = "System.String";
            //                    RegionNode.Attributes.Append(RegionDataType);

            //                    XmlAttribute RegionExpression = doc.CreateAttribute("Expression");
            //                    RegionExpression.Value = "\"" + report.ParentNode.Attributes["name"].Value + "\"";
            //                    RegionNode.Attributes.Append(RegionExpression);

            //                    exitRegion = true;
            //                    break;
            //                }
            //            }
            //        }

            //    }
            //}
            #endregion
            string reportName = Path.GetFileName(filePath); // filePath
            Dictionary<string,string> regions = RegionXML.GetRegionXML(logistics, reportName);
            foreach (string key in regions.Keys)
            {
                XmlNode RegionNode = doc.CreateNode(XmlNodeType.Element, "Parameter", null);
                doc.SelectSingleNode("Report/Dictionary").AppendChild(RegionNode);

                XmlAttribute RegionName = doc.CreateAttribute("Name");
                RegionName.Value = key;
                RegionNode.Attributes.Append(RegionName);

                XmlAttribute RegionDataType = doc.CreateAttribute("DataType");
                RegionDataType.Value = "System.String";
                RegionNode.Attributes.Append(RegionDataType);

                XmlAttribute RegionExpression = doc.CreateAttribute("Expression");
                RegionExpression.Value = "\"" + regions[key] + "\"";
                RegionNode.Attributes.Append(RegionExpression);

                exitRegion = true;
            }

            //添加设置参数
            #region
            var setting = LogisticsSetting.GetLogisticsSetting().Where(x => x.Logistics == logistics).FirstOrDefault();
            if (setting != null)
            {
                foreach (var par in setting.ParamModels)
                {
                    foreach (XmlNode xnode in xlist)
                    {
                        if (xnode.Attributes["Name"].Value == par.Name)
                        {
                            doc.SelectSingleNode("Report/Dictionary").RemoveChild(xnode);
                        }
                    }

                    XmlNode parNode = doc.CreateNode(XmlNodeType.Element, "Parameter", null);
                    doc.SelectSingleNode("Report/Dictionary").AppendChild(parNode);

                    XmlAttribute parName = doc.CreateAttribute("Name");
                    parName.Value = par.Name;
                    parNode.Attributes.Append(parName);

                    XmlAttribute parDataType = doc.CreateAttribute("DataType");
                    parDataType.Value = "System.String";
                    parNode.Attributes.Append(parDataType);

                    XmlAttribute parExpression = doc.CreateAttribute("Expression");
                    parExpression.Value = "\""+JsonConvert.SerializeObject(par).Replace("\"", "\\\"")+"\"";
                    parNode.Attributes.Append(parExpression);

                    XmlAttribute parDescription = doc.CreateAttribute("Description");
                    parDescription.Value = par.Description;
                    parNode.Attributes.Append(parDescription);
                }
            }
            #endregion

            doc.Save(filePath);
            EditReport(filePath);

            if (!exitRegion)
            {
                WriteLog(NoRegion, formFileName);
            }
        }

        private void EditReport(string fileName)
        {
            StreamReader streamReader = new StreamReader(fileName);
            string contents = streamReader.ReadToEnd().Replace("&#xD", "&#13").Replace("&#xA", "&#10").Replace("(\"", "(&quot;").Replace("\")", "&quot;)").Replace("bool IsShowPostAccount=(bool)Report.GetParameterValue(&quot;IsShowPostAccount&quot;);", "bool IsShowPostAccount= Report.GetColumnValue(&quot;Package.IsShowPostAccount&quot;).ToString()==\"True\";");
            streamReader.Close();
            File.WriteAllText(fileName, contents);  


            //string input = "SHIP TO:&#xD;&#xA;[Package.ContactName]&#xD;&#xA;[Join(&quot;,&quot;,[Package.Street1],[Package.Street2])]&#xD;&#xA;[Package.City],[Package.Province][asdfadf] [Package.Zip][Package.Country]&#xD;&#xA;[StringFormat(&quot;TEL:{0}&quot;,[Package.Tel])][StringFormat(&quot;MOBILE:{[gggg]0}&quot;,[Package.Mobile])]&#xD;&#xA;[Package.CountryCN]";
            //Regex reg = new Regex(@"\[\w+\]");
            //MatchCollection mc = reg.Matches(input);
            //foreach (Match m in mc)
            //{
            //    MessageBox.Show(m.Value);
            //}


        }

        private void CheckReport()
        {
            List<string> fileNames = new List<string>();
            foreach (string formFileName in Directory.GetFiles(@"C:\Users\windows\Desktop\ERP.Reports.SIReports"))
            {
                string suffix = Path.GetExtension(formFileName);
                string fileName = Path.GetFileName(formFileName);
                if (suffix.ToLower() == ".frx")
                {
                    fileNames.Add(fileName);
                }
            }

            string[] checkPath = new string[] { @"C:\Users\windows\Desktop\Plugins-ERP1\Plugins", @"C:\Users\windows\Desktop\Plugins-ERP2\Plugins", @"C:\Users\windows\Desktop\Plugins-ERP3\Plugins" };
            foreach (string path in checkPath)
            {
                string[] fromDirs = Directory.GetDirectories(path);
                foreach (string fromDirPath in fromDirs)
                {
                    string[] files = Directory.GetFiles(fromDirPath);
                    foreach (string formFileName in files)
                    {
                        string suffix = Path.GetExtension(formFileName);
                        string fileName = Path.GetFileName(formFileName);
                        if (suffix.ToLower() == ".frx")
                        {
                            if (!fileNames.Contains(fileName))
                            {
                                WriteLog(@"C:\Users\windows\Desktop\RepeatLog.txt", formFileName);
                            }
                        }
                    }
                }
            }
        }

        public void WriteLog(string path, string message)
        {
            FileStream fs = new FileStream(path, FileMode.Append);
            StreamWriter sw = new StreamWriter(fs);
            sw.Write("\r\n" + message);
            sw.Flush();
            sw.Close();
            fs.Close();
        }

    }

}