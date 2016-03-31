﻿using Newtonsoft.Json;
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
        private string projectPath = string.Empty;
        private bool isSystem = true;
        private string dirName = "ERP.Reports.SIReports";
        private List<string> repeats = new List<string>();
        private Dictionary<string, string> reports = new Dictionary<string, string>();
        public Form1()
        {
            InitializeComponent();
            //ProjectPath.Text = @"D:\repo\ERP";
            //ReportType.Text = "系统模版";
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
                fromDir += @"\WillV.Web\UserData\";
                toDir += @"\WillV.Web\UserData\";
                if (!Directory.Exists(fromDir))
                {
                   // MessageBox.Show(fromDir + "文件不存在");
                    toolStripStatusLabel1.Text = fromDir + "文件不存在";
                    return;
                }
                string[] fromDirs = Directory.GetDirectories(fromDir);
                foreach (string fromDirPath in fromDirs)
                {
                    toDir = Path.Combine(fromDirPath, dirName);
                    if (!Directory.Exists(toDir))
                    {
                        Directory.CreateDirectory(toDir);
                    }
                    MoveReportFile(fromDirPath, toDir);
                }
                toolStripStatusLabel1.Text = "任务完成";
               // MessageBox.Show("任务完成");
            }
            catch (Exception ex)
            {
                WriteLog(WritePath.Log,DateTime.Now + ex.Message);
                toolStripStatusLabel1.Text = ex.Message;
            }
        }

        private void MoveSystemReport(string fromDir, string toDir)
        {
            try
            {
                fromDir += @"\WillV.Web\Plugins\";
                toDir += @"\WillV.Web\Plugins\" + dirName + "\\";
                if (!Directory.Exists(fromDir))
                {
                    //MessageBox.Show(fromDir + "文件不存在");
                    toolStripStatusLabel1.Text = fromDir + "文件不存在";
                    return;
                }
                if (!Directory.Exists(toDir))
                {
                    //MessageBox.Show(toDir + "文件不存在");
                    toolStripStatusLabel1.Text = toDir + "文件不存在";
                    return;
                }

                MoveReportFile(fromDir, toDir);
                toolStripStatusLabel1.Text = "任务完成";
                //MessageBox.Show("任务完成");

            }
            catch (Exception ex)
            {
                WriteLog(WritePath.Log, DateTime.Now + ex.Message);
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
                    WriteLog(WritePath.Log, DateTime.Now + formFileName + "移动成功。");

                }
            }

            string[] fromDirs = Directory.GetDirectories(fromDir);
            foreach (string fromDirPath in fromDirs)
            {
                string logisticsDirName = fromDirPath.Substring(fromDirPath.LastIndexOf("\\") + 1);
                if (logisticsDirName != dirName)
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
                            File.Copy(formFileName, toFileName);

                            reports[fileName] = formFileName;
                            InsertDataSource(formFileName, toFileName, logisticsDirName);
                            //File.Delete(fileName);
                            WriteLog(WritePath.Log, DateTime.Now + formFileName + "移动成功。");

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
                    WriteLog(WritePath.Repeat, f1+"-----------------"+f2);
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
            bool exitSubarea = false;
            XmlDocument doc = new XmlDocument();
            doc.Load(filePath);
            XmlNodeList xlist = doc.SelectSingleNode("Report/Dictionary").SelectNodes("Parameter");
            foreach (XmlNode xnode in xlist)
            {
                if (xnode.Attributes["Name"].Value == "DataSource" || xnode.Attributes["Name"].Value == "Subarea")
                {
                    doc.SelectSingleNode("Report/Dictionary").RemoveChild(xnode);
                }
            }

            //添加数据源
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


            //添加分区码
            XmlDocument subareaXml = new XmlDocument();
            //subareaXml.Load(projectPath+ @"\WillV.Web\Plugins\ERP.Reports.SIReports\面单对应分区码.xml");
            subareaXml.Load("面单对应分区码.xml");
            XmlNodeList logisticsList = subareaXml.SelectNodes("subarea/logistics");
            string fileName = Path.GetFileNameWithoutExtension(formFileName);
            foreach (XmlNode logisticsNode in logisticsList)
            {
                if (logisticsNode.Attributes["name"].Value == logistics)
                {
                    foreach (XmlNode report in logisticsNode.SelectNodes("area/report"))
                    {
                        if ((isSystem && report.Attributes["name"].Value == fileName) || (!isSystem && logisticsNode.SelectNodes("area").Count==1))
                        {
                            XmlNode subareaNode = doc.CreateNode(XmlNodeType.Element, "Parameter", null);
                            doc.SelectSingleNode("Report/Dictionary").AppendChild(subareaNode);

                            XmlAttribute subareaName = doc.CreateAttribute("Name");
                            subareaName.Value = "Subarea";
                            subareaNode.Attributes.Append(subareaName);

                            XmlAttribute subareaDataType = doc.CreateAttribute("DataType");
                            subareaDataType.Value = "System.String";
                            subareaNode.Attributes.Append(subareaDataType);

                            XmlAttribute subareaExpression = doc.CreateAttribute("Expression");
                            subareaExpression.Value = report.ParentNode.Attributes["name"].Value;
                            subareaNode.Attributes.Append(subareaExpression);

                            exitSubarea = true;
                            break;
                        }
                    }
                }
            }

            //添加设置参数
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
                    parExpression.Value = JsonConvert.SerializeObject(par);
                    parNode.Attributes.Append(parExpression);

                    XmlAttribute parDescription = doc.CreateAttribute("Description");
                    parDescription.Value = par.Description;
                    parNode.Attributes.Append(parDescription);
                }
            }

            doc.Save(filePath);

            if (!exitSubarea)
            {
                WriteLog(WritePath.NoSubarea, formFileName);
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

    public class WritePath
    {
        public const string Log = @"D:\MoveReportLog.txt";
        public const string NoSubarea = @"D:\NoSubareaLog.txt";
        public const string Repeat = @"D:\RepeatLog.txt";
    }
}