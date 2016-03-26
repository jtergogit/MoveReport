using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace MoveReport
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            ProjectPath.Text = @"D:\repo\ERP";
            this.ReportType.Items.Add("客户自定义模版");
            this.ReportType.Items.Add("系统模版");
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

            toolStripStatusLabel1.Text = "开始移动";

            string fromDir = ProjectPath.Text;
            string toDir = ProjectPath.Text;
            if (ReportType.Text == "客户自定义模版")
            {
                MoveCustormReport(fromDir, toDir);
            }
            else if (ReportType.Text == "系统模版")
            {
                MoveSystemReport(fromDir, toDir);
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
                fromDir += @"\WillV.Web\UserData\";
                toDir += @"\WillV.Web\UserData\";
                if (!Directory.Exists(fromDir))
                {
                    MessageBox.Show(fromDir + "文件不存在");
                    return;
                }
                string[] fromDirs = Directory.GetDirectories(fromDir);
                foreach (string fromDirPath in fromDirs)
                {
                    toDir = Path.Combine(fromDirPath, "CustomReport");
                    if (!Directory.Exists(toDir))
                    {
                        Directory.CreateDirectory(toDir);
                    }
                    MoveReportFile("Custom", fromDirPath, toDir);
                }
                toolStripStatusLabel1.Text = "任务完成";
                MessageBox.Show("任务完成");
            }
            catch (Exception ex)
            {
                WriteLog(DateTime.Now + ex.Message);
                toolStripStatusLabel1.Text = ex.Message;
            }
        }

        private void MoveSystemReport(string fromDir, string toDir)
        {
            try
            {
                fromDir += @"\WillV.Web\Plugins\";
                toDir += @"\WillV.Web\Plugins\ERP.Reports.SIReports\";
                if (!Directory.Exists(fromDir))
                {
                    MessageBox.Show(fromDir + "文件不存在");
                    return;
                }
                if (!Directory.Exists(toDir))
                {
                    MessageBox.Show(toDir + "文件不存在");
                    return;
                }

                string[] files = Directory.GetFiles(toDir);
                foreach (string formFileName in files)
                {
                    string suffix = Path.GetExtension(formFileName);
                    if (suffix.ToLower() == ".frx")
                    {
                        InsertDataSource(formFileName, "ERP.Reports.SIReports");
                    }
                }
                MoveReportFile("System", fromDir, toDir);
                toolStripStatusLabel1.Text = "任务完成";
                MessageBox.Show("任务完成");
            }
            catch (Exception ex)
            {
                WriteLog(DateTime.Now + ex.Message);
                toolStripStatusLabel1.Text = ex.Message;
            }
        }

        private void MoveReportFile(string type, string fromDir, string toDir)
        {
            string[] fromDirs = Directory.GetDirectories(fromDir);
            foreach (string fromDirPath in fromDirs)
            {
                string logisticsDirName = fromDirPath.Substring(fromDirPath.LastIndexOf("\\") + 1);
                if ((type == "System" && logisticsDirName.ToLower() != "erp.reports.sireports") || (type == "Custom" && logisticsDirName.ToLower().StartsWith("logistics.")))
                {
                    string[] files = Directory.GetFiles(fromDirPath);
                    foreach (string formFileName in files)
                    {
                        string suffix = Path.GetExtension(formFileName);
                        if (suffix.ToLower() == ".frx")
                        {
                            //Task.Run(() =>
                            //{
                            string fileName = Path.GetFileName(formFileName);
                            toolStripStatusLabel1.Text = fileName;
                            string toFileName = GetToFileName(toDir, fileName, suffix);
                            File.Copy(formFileName, toFileName);
                            InsertDataSource(toFileName, logisticsDirName);
                            //File.Delete(fileName);
                            WriteLog(DateTime.Now + formFileName + "移动成功。");
                            //});
                        }
                    }
                }
            }
        }

        private string GetToFileName(string path, string fileName, string suffix)
        {
            string filePathName = Path.Combine(path, fileName);
            if (File.Exists(filePathName))
            {
                string newfilename = Path.GetFileNameWithoutExtension(fileName);
                int index = newfilename.LastIndexOf("-");
                int i = 0;
                if (index != -1 && int.TryParse(newfilename.Substring(index + 1), out i))
                    newfilename = newfilename.Substring(0, index + 1) + (++i).ToString() + suffix;
                else
                    newfilename += "-" + (++i).ToString() + suffix;
                return GetToFileName(path, newfilename, suffix);
            }
            return filePathName;
        }

        private void InsertDataSource(string filePath,string logistics)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(filePath);
            XmlNodeList xlist = doc.SelectSingleNode("Report/Dictionary").SelectNodes("Parameter");
            foreach (XmlNode xnode in xlist)
            {
                if (xnode.Attributes["Name"].Value == "DataSource")
                {
                    doc.SelectSingleNode("Report/Dictionary").RemoveChild(xnode);
                }
            }

            //添加数据源
            XmlNode dnode = doc.CreateNode(XmlNodeType.Element, "Parameter", null);
            doc.SelectSingleNode("Report/Dictionary").AppendChild(dnode);

            XmlAttribute name = doc.CreateAttribute("Name");
            name.Value = "DataSource";
            dnode.Attributes.Append(name);

            XmlAttribute datatype = doc.CreateAttribute("DataType");
            datatype.Value = "System.String";
            dnode.Attributes.Append(datatype);

            XmlAttribute expression = doc.CreateAttribute("Expression");
            expression.Value = "ERP.Reports.SIReports";
            dnode.Attributes.Append(expression);

            //添加分区码
            XmlNode subareaNode = doc.CreateNode(XmlNodeType.Element, "Parameter", null);
            doc.SelectSingleNode("Report/Dictionary").AppendChild(subareaNode);

            XmlAttribute subareaName = doc.CreateAttribute("Name");
            subareaName.Value = "Subarea";
            subareaNode.Attributes.Append(subareaName);

            XmlAttribute subareaDataType = doc.CreateAttribute("DataType");
            subareaDataType.Value = "System.String";
            subareaNode.Attributes.Append(subareaDataType);

            XmlAttribute subareaExpression = doc.CreateAttribute("Expression");
            subareaExpression.Value = "分区码.xml";
            subareaNode.Attributes.Append(subareaExpression);

            //添加设置参数
            var setting = LogisticsSetting.GetLogisticsSetting().Where(x => x.Logistics == logistics).FirstOrDefault();
            if (setting != null)
            {
                foreach (var par in setting.ParamModels)
                {
                    XmlNode parNode = doc.CreateNode(XmlNodeType.Element, "Parameter", null);
                    doc.SelectSingleNode("Report/Dictionary").AppendChild(parNode);

                    XmlAttribute parName = doc.CreateAttribute("Name");
                    parName.Value = par.Name;
                    parNode.Attributes.Append(parName);

                    XmlAttribute parDataType = doc.CreateAttribute("DataType");
                    parDataType.Value = "System.String";
                    parNode.Attributes.Append(parDataType);

                    XmlAttribute parExpression = doc.CreateAttribute("Expression");
                    parExpression.Value = JsonConvert.SerializeObject(par);
                    parNode.Attributes.Append(parExpression);

                    XmlAttribute parDescription = doc.CreateAttribute("Description");
                    parDescription.Value = par.Description;
                    parNode.Attributes.Append(parDescription);
                }
            }

            doc.Save(filePath);
        }

        public void WriteLog(string message)
        {
            FileStream fs = new FileStream(@"D:\MoveReportLog.txt", FileMode.Append);
            StreamWriter sw = new StreamWriter(fs);
            sw.Write("\r\n" + message);
            sw.Flush();
            sw.Close();
            fs.Close();
        }

    }
}