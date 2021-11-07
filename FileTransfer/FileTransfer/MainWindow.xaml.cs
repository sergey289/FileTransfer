using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FileTransfer.Utility;

namespace FileTransfer
{

public partial class MainWindow : Window
    {

        ComboBoxItem typeItem;
        string typeOfOpiration;
        string sourcePath;
        string targetPath;
        Logs myLog;

        int totalAmountFiles = 0;
        int PassedSuccessfully = 0;

        public MainWindow()
        {
            InitializeComponent();
            string strExeFilePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string logDir = System.IO.Path.GetDirectoryName(strExeFilePath) + "\\Log";
            this.myLog = Logs.createSingletonLog(logDir);
            
        }

        enum operation {successful,failed};

        private void btn_execute_Click(object sender, RoutedEventArgs e)
        {
             sourcePath = txt_source.Text;
             targetPath = txt_dest.Text;

            if (cmb_choice.SelectedIndex == -1)
            {
                MessageBox.Show("Error: No operation selected,\nPlease select an operation and try again");
            }
            else
            {
                typeItem = (ComboBoxItem)cmb_choice.SelectedItem;
                typeOfOpiration = typeItem.Content.ToString();

                if (typeOfOpiration == "Move")
                {
                    Move(sourcePath, targetPath);
                    MessageBox.Show($"Files Passed Successfully:{PassedSuccessfully}\nFiles failed:{totalAmountFiles - PassedSuccessfully}");

                    PassedSuccessfully = 0;
                    totalAmountFiles = 0;



                }
                else if (typeOfOpiration == "Copy")
                {
                    Copy(sourcePath, targetPath);
                    MessageBox.Show($"Files Passed Successfully:{PassedSuccessfully}\nFiles failed:{totalAmountFiles - PassedSuccessfully}");

                     PassedSuccessfully = 0;
                     totalAmountFiles = 0;


                }
            }
           

            


          
           


        }
    
        private void Copy(string sourceDir, string targetDir)
        {
            try
            {

            

            if (!Directory.Exists(targetDir))
                    Directory.CreateDirectory(targetDir);



                string[] files = Directory.GetFiles(sourceDir);
                totalAmountFiles += files.Count();
                foreach (string file in files)
                {
                    string name = System.IO.Path.GetFileName(file);                  
                    string dest = System.IO.Path.Combine(targetDir, name);
                try
                {
                    File.Copy(file, dest, true);
                    PassedSuccessfully++;
                }
                catch(Exception ex)
                 {
                    string dateTime = DateTime.Now.ToString("yyyyMMddhmmss");
                    string errorJson = "{";
                    errorJson += "\"sourceFilePath\":\"" + sourcePath + "\",";
                    errorJson += "\"destinationFilePath\":\"" + targetPath + "\",";
                    errorJson += "\"typeOfOperation\":\"" + typeOfOpiration + "\",";
                    errorJson += "\"dateOfError\":\"" + dateTime + "\",";
                    errorJson += "\"Exception\":\"" + ex.GetType() + "\"";
                    errorJson += "}";
                    this.myLog.write("Error_" + dateTime + ".txt", "TransferLogErrors", errorJson);
                  
                    continue;

                 }
                   this.myLog.write("TransferLog.txt", $"\n\nFileName:{name}\nSourceFilePatch:{sourcePath}\nDestinationFilePatch:{targetPath}\nTypeOfOperation:{typeOfOpiration}\nOperation:{operation.successful.ToString()}");
                  
                }

                string[] folders = Directory.GetDirectories(sourceDir);
                totalAmountFiles += folders.Count();

            foreach (string folder in folders)
            {
                    string name = System.IO.Path.GetFileName(folder);
                    string dest = System.IO.Path.Combine(targetDir, name);
                    PassedSuccessfully++;
                    Copy(folder, dest);
                   
            }



            }
            catch (DirectoryNotFoundException)
            {
                MessageBox.Show("Error:DirectoryNotFoundException:Path is Not Correct!");
            }
            catch (ArgumentException)
            {
                MessageBox.Show("Error:ArgumentException:The Path Cannot be an Empty string or a string of spaces!");
            }




        }

        private void Move(string sourceDir, string targetDir)
        {

            try
            {

            

            if (!Directory.Exists(targetDir))
                Directory.CreateDirectory(targetDir);


            string[] files = Directory.GetFiles(sourceDir);
            totalAmountFiles += files.Count();

            foreach (string file in files)
            {
                string name = System.IO.Path.GetFileName(file);
                string dest = System.IO.Path.Combine(targetDir, name);
                try
                {
                    File.Move(file, dest);
                    PassedSuccessfully++;

                }
                catch(Exception ex)
                {
                    string dateTime = DateTime.Now.ToString("yyyyMMddhmmss");
                    string errorJson = "{";
                    errorJson += "\"sourceFilePath\":\""+ sourcePath +"\",";
                    errorJson += "\"destinationFilePath\":\"" + targetPath + "\",";
                    errorJson += "\"typeOfOperation\":\"" + typeOfOpiration + "\",";
                    errorJson += "\"dateOfError\":\"" + dateTime + "\",";
                    errorJson += "\"Exception\":\"" + ex.GetType() + "\"";
                    errorJson += "}";
                    this.myLog.write("Error_"+ dateTime+".txt", "TransferLogErrors", errorJson); 
                    
                    continue;
                    
                    
                }

                this.myLog.write("TransferLog.txt", $"\n\nFileName:{name}\nSourceFilePatch:{sourcePath}\nDestinationFilePatch:{targetPath}\nTypeOfOperation:{typeOfOpiration}\nOperation:{operation.successful.ToString()}");
                

            }


            string[] folders = Directory.GetDirectories(sourceDir);
            totalAmountFiles += folders.Count();

            foreach (string folder in folders)
            {
                string name = System.IO.Path.GetFileName(folder);
                string dest = System.IO.Path.Combine(targetDir, name);
                PassedSuccessfully++;
                Move(folder, dest);
                Directory.Delete( folder);
            }


            }
            catch (DirectoryNotFoundException)
            {
                MessageBox.Show("Error:DirectoryNotFoundException:Path is Not Correct!");
            }
            catch (ArgumentException)
            {
                MessageBox.Show("Error:ArgumentException:The Path Cannot be an Empty string or a string of spaces!");
            }
        } 

    }
}
