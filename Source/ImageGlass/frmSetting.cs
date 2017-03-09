﻿/*
ImageGlass Project - Image viewer for Windows
Copyright (C) 2017 DUONG DIEU PHAP
Project homepage: http://imageglass.org

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using ImageGlass.Services.Configuration;
using ImageGlass.Library;
using System.Linq;

namespace ImageGlass
{
    public partial class frmSetting : Form
    {
        public frmSetting()
        {
            InitializeComponent();
        }

        private Color M_COLOR_MENU_ACTIVE = Color.FromArgb(255, 220, 220, 220);
        private Color M_COLOR_MENU_HOVER = Color.FromArgb(255, 247, 247, 247);
        private Color M_COLOR_MENU_NORMAL = Color.FromArgb(255, 240, 240, 240);
        private List<Library.Language> dsLanguages = new List<Library.Language>();

        #region MOUSE ENTER - HOVER - DOWN MENU
        private void lblMenu_MouseDown(object sender, MouseEventArgs e)
        {
            Label lbl = (Label)sender;
            lbl.BackColor = M_COLOR_MENU_ACTIVE;
        }

        private void lblMenu_MouseUp(object sender, MouseEventArgs e)
        {
            Label lbl = (Label)sender;

            if (int.Parse(lbl.Tag.ToString()) == 1)
            {
                lbl.BackColor = M_COLOR_MENU_ACTIVE;
            }
            else
            {
                lbl.BackColor = M_COLOR_MENU_HOVER;
            }
        }

        private void lblMenu_MouseEnter(object sender, EventArgs e)
        {
            Label lbl = (Label)sender;

            if (int.Parse(lbl.Tag.ToString()) == 1)
            {
                lbl.BackColor = M_COLOR_MENU_ACTIVE;
            }
            else
            {
                lbl.BackColor = M_COLOR_MENU_HOVER;
            }

        }

        private void lblMenu_MouseLeave(object sender, EventArgs e)
        {
            Label lbl = (Label)sender;
            if (int.Parse(lbl.Tag.ToString()) == 1)
            {
                lbl.BackColor = M_COLOR_MENU_ACTIVE;
            }
            else
            {
                lbl.BackColor = M_COLOR_MENU_NORMAL;
            }
        }
        #endregion

        #region MOUSE ENTER - HOVER - DOWN BUTTON
        private void lblButton_MouseDown(object sender, MouseEventArgs e)
        {
            Label lbl = (Label)sender;
            lbl.BackColor = M_COLOR_MENU_ACTIVE;            
        }

        private void lblButton_MouseUp(object sender, MouseEventArgs e)
        {
            Label lbl = (Label)sender;
            lbl.BackColor = M_COLOR_MENU_HOVER;
        }

        private void lblButton_MouseEnter(object sender, EventArgs e)
        {
            Label lbl = (Label)sender;
            lbl.BackColor = M_COLOR_MENU_HOVER;
        }

        private void lblButton_MouseLeave(object sender, EventArgs e)
        {
            Label lbl = (Label)sender; 
            lbl.BackColor = M_COLOR_MENU_NORMAL;            
        }
        #endregion


        private void frmSetting_Load(object sender, EventArgs e)
        {
            //Load config
            //Windows Bound (Position + Size)------------------------------------------------
            Rectangle rc = GlobalSetting.StringToRect(GlobalSetting.GetConfig($"{Name}.WindowsBound", "280,125,610,570"));

            if (!Helper.IsOnScreen(rc.Location))
            {
                rc.Location = new Point(280, 125);
            }
            Bounds = rc;

            //windows state--------------------------------------------------------------
            string s = GlobalSetting.GetConfig(Name + ".WindowsState", "Normal");
            if (s == "Normal")
            {
                WindowState = FormWindowState.Normal;
            }
            else if (s == "Maximized")
            {
                WindowState = FormWindowState.Maximized;
            }

            LoadTabGeneralConfig();
            InitLanguagePack();
        }

        private void frmSetting_SizeChanged(object sender, EventArgs e)
        {
            Refresh();
        }
        
        private void frmSetting_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Save config---------------------------------
            if (WindowState == FormWindowState.Normal)
            {
                //Windows Bound-------------------------------------------------------------------
                GlobalSetting.SetConfig(Name + ".WindowsBound", GlobalSetting.RectToString(Bounds));
            }

            //Windows State-------------------------------------------------------------------
            GlobalSetting.SetConfig(Name + ".WindowsState", WindowState.ToString());

            //Save extra supported extensions
            string extraExts = "";
            foreach (var control in panExtraExts.Controls)
            {
                var chk = (CheckBox)control;
                
                if(chk.Checked)
                {
                    extraExts += chk.Tag.ToString() + ";";
                }
            }
            GlobalSetting.OptionalImageFormats = extraExts;
            GlobalSetting.SetConfig("OptionalImageFormats", GlobalSetting.OptionalImageFormats);

            //Force to apply the configurations
            GlobalSetting.IsForcedActive = true;
        }

        private void frmSetting_KeyDown(object sender, KeyEventArgs e)
        {
            //close dialog
            if (e.KeyCode == Keys.Escape && !e.Control && !e.Shift && !e.Alt)
            {
                Close();
            }
        }

        /// <summary>
        /// Load language pack
        /// </summary>
        private void InitLanguagePack()
        {
            RightToLeft = GlobalSetting.LangPack.IsRightToLeftLayout;

            Text = GlobalSetting.LangPack.Items["frmSetting._Text"];
            lblGeneral.Text = GlobalSetting.LangPack.Items["frmSetting.lblGeneral"];
            lblFileAssociations.Text = GlobalSetting.LangPack.Items["frmSetting.lblFileAssociations"];
            lblLanguage.Text = GlobalSetting.LangPack.Items["frmSetting.lblLanguage"];

            //General tab
            chkPortableMode.Text = GlobalSetting.LangPack.Items["frmSetting.chkPortableMode"];
            chkAutoUpdate.Text = GlobalSetting.LangPack.Items["frmSetting.chkAutoUpdate"];
            chkFindChildFolder.Text = GlobalSetting.LangPack.Items["frmSetting.chkFindChildFolder"];
            chkWelcomePicture.Text = GlobalSetting.LangPack.Items["frmSetting.chkWelcomePicture"];
            chkHideToolBar.Text = GlobalSetting.LangPack.Items["frmSetting.chkHideToolBar"];
            chkLoopViewer.Text = GlobalSetting.LangPack.Items["frmSetting.chkLoopViewer"];
            chkLoopSlideshow.Text = GlobalSetting.LangPack.Items["frmSetting.chkLoopSlideshow"];
            chkImageBoosterBack.Text = GlobalSetting.LangPack.Items["frmSetting.chkImageBoosterBack"];
            chkESCToQuit.Text = GlobalSetting.LangPack.Items["frmSetting.chkESCToQuit"];
            chkAllowMultiInstances.Text = GlobalSetting.LangPack.Items["frmSetting.chkAllowMultiInstances"];
            chkConfirmationDelete.Text = GlobalSetting.LangPack.Items["frmSetting.chkConfirmationDelete"];
            chkThumbnailVertical.Text = GlobalSetting.LangPack.Items["frmSetting.chkThumbnailVertical"];

            lblSlideshowInterval.Text = string.Format(GlobalSetting.LangPack.Items["frmSetting.lblSlideshowInterval"], barInterval.Value);
            lblGeneral_MaxFileSize.Text = GlobalSetting.LangPack.Items["frmSetting.lblGeneral_MaxFileSize"];
            lblGeneral_ThumbnailSize.Text = GlobalSetting.LangPack.Items["frmSetting.lblGeneral_ThumbnailSize"];
            lblGeneral_ZoomOptimization.Text = GlobalSetting.LangPack.Items["frmSetting.lblGeneral_ZoomOptimization"];
            chkMouseNavigation.Text = GlobalSetting.LangPack.Items["frmSetting.chkMouseNavigation"];
            lblImageLoadingOrder.Text = GlobalSetting.LangPack.Items["frmSetting.lblImageLoadingOrder"];
            lblBackGroundColor.Text = GlobalSetting.LangPack.Items["frmSetting.lblBackGroundColor"];
            

            //File Associations tab
            lblSupportedExtension.Text = GlobalSetting.LangPack.Items["frmSetting.lblSupportedExtension"];
            btnOpenFileAssociations.Text = GlobalSetting.LangPack.Items["frmSetting.btnOpenFileAssociations"];

            //Language tab
            lblLanguageText.Text = GlobalSetting.LangPack.Items["frmSetting.lblLanguageText"];
            lnkRefresh.Text = GlobalSetting.LangPack.Items["frmSetting.lnkRefresh"];
            lblLanguageWarning.Text = string.Format(GlobalSetting.LangPack.Items["frmSetting.lblLanguageWarning"], "ImageGlass " + Application.ProductVersion);
            lnkInstallLanguage.Text = GlobalSetting.LangPack.Items["frmSetting.lnkInstallLanguage"];
            lnkCreateNew.Text = GlobalSetting.LangPack.Items["frmSetting.lnkCreateNew"];
            lnkEdit.Text = GlobalSetting.LangPack.Items["frmSetting.lnkEdit"];
            lnkGetMoreLanguage.Text = GlobalSetting.LangPack.Items["frmSetting.lnkGetMoreLanguage"];
        }

        /// <summary>
        /// TAB LABEL CLICK
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblMenu_Click(object sender, EventArgs e)
        {
            Label lbl = (Label)sender;

            if (lbl.Name == "lblGeneral")
            {
                tab1.SelectedTab = tabGeneral;
            }
            else if (lbl.Name == "lblFileAssociations")
            {
                tab1.SelectedTab = tabFileAssociation;
            }
            else if (lbl.Name == "lblLanguage")
            {
                tab1.SelectedTab = tabLanguage;
            }
        }

        private void tab1_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblGeneral.Tag = 0;
            lblFileAssociations.Tag = 0;
            lblLanguage.Tag = 0;

            lblGeneral.BackColor = M_COLOR_MENU_NORMAL;
            lblFileAssociations.BackColor = M_COLOR_MENU_NORMAL;
            lblLanguage.BackColor = M_COLOR_MENU_NORMAL;

            if (tab1.SelectedTab == tabGeneral)
            {
                lblGeneral.Tag = 1;
                lblGeneral.BackColor = M_COLOR_MENU_ACTIVE;

                LoadTabGeneralConfig();
            }
            else if (tab1.SelectedTab == tabFileAssociation)
            {
                lblFileAssociations.Tag = 1;
                lblFileAssociations.BackColor = M_COLOR_MENU_ACTIVE;

                txtSupportedExtensionDefault.Text = GlobalSetting.DefaultImageFormats;
                
                foreach (var control in panExtraExts.Controls)
                {
                    var chk = (CheckBox)control;

                    chk.Checked = GlobalSetting.OptionalImageFormats.Contains(chk.Tag.ToString());
                }
            }
            else if (tab1.SelectedTab == tabLanguage)
            {
                lblLanguage.Tag = 1;
                lblLanguage.BackColor = M_COLOR_MENU_ACTIVE;

                lnkRefresh_LinkClicked(null, null);
            }
        }


        #region TAB GENERAL

        /// <summary>
        /// Get and load value of General tab
        /// </summary>
        private void LoadTabGeneralConfig()
        {
            //Get Portable mode value
            chkPortableMode.Checked = GlobalSetting.IsPortableMode;
            if (!GlobalSetting.IsConfigFileWritable())
            {
                chkPortableMode.Enabled = false;
            }

            //Get value of chkFindChildFolder
            chkFindChildFolder.Checked = bool.Parse(GlobalSetting.GetConfig("IsRecursiveLoading", "false"));

            //Get value of cmbAutoUpdate
            string s = GlobalSetting.GetConfig("AutoUpdate", DateTime.Now.ToString());
            if (s != "0")
            {
                chkAutoUpdate.Checked = true;
            }
            else
            {
                chkAutoUpdate.Checked = false;
            }

            //Get value of chkWelcomePicture
            chkWelcomePicture.Checked = GlobalSetting.IsShowWelcome;

            //Get value of chkHideToolBar
            chkHideToolBar.Checked = bool.Parse(GlobalSetting.GetConfig("IsHideToolbar", "false"));

            //Get value of chkLoopViewer
            chkLoopViewer.Checked = bool.Parse(GlobalSetting.GetConfig("IsLoopBackViewer", "true"));

            //Get value of chkLoopSlideshow
            chkLoopSlideshow.Checked = bool.Parse(GlobalSetting.GetConfig("IsLoopBackSlideShow", "true"));

            //Get value of chkImageBoosterBack
            chkImageBoosterBack.Checked = bool.Parse(GlobalSetting.GetConfig("IsImageBoosterBack", "false"));

            //Get value of IsPressESCToQuit
            chkESCToQuit.Checked = bool.Parse(GlobalSetting.GetConfig("IsPressESCToQuit", "true"));

            //Get value of IsPressESCToQuit
            chkAllowMultiInstances.Checked = bool.Parse(GlobalSetting.GetConfig("IsAllowMultiInstances", "true"));

            //Get value of IsConfirmationDelete
            chkConfirmationDelete.Checked = bool.Parse(GlobalSetting.GetConfig("IsConfirmationDelete", "false"));

            //Load items of cmbZoomOptimization
            cmbZoomOptimization.Items.Clear();
            cmbZoomOptimization.Items.Add(GlobalSetting.LangPack.Items["frmSetting.cmbZoomOptimization._Auto"]);
            cmbZoomOptimization.Items.Add(GlobalSetting.LangPack.Items["frmSetting.cmbZoomOptimization._SmoothPixels"]);
            cmbZoomOptimization.Items.Add(GlobalSetting.LangPack.Items["frmSetting.cmbZoomOptimization._ClearPixels"]);

            //Get value of cmbZoomOptimization
            s = GlobalSetting.GetConfig("ZoomOptimization", "0");
            int i = 0;
            if (int.TryParse(s, out i))
            {
                if (-1 < i && i < cmbZoomOptimization.Items.Count)
                { }
                else
                {
                    i = 0;
                }
            }
            cmbZoomOptimization.SelectedIndex = i;

            //Get value of barInterval
            i = int.Parse(GlobalSetting.GetConfig("SlideShowInterval", "5"));
            if (0 < i && i < 61)
            {
                barInterval.Value = i;
            }
            else
            {
                barInterval.Value = 5;
            }

            lblSlideshowInterval.Text = string.Format(GlobalSetting.LangPack.Items["frmSetting.lblSlideshowInterval"], barInterval.Value);

            //load thumbnail dimension
            i = int.Parse(GlobalSetting.GetConfig("ThumbnailDimension", "48"));
            GlobalSetting.ThumbnailDimension = i;
            cmbThumbnailDimension.SelectedItem = i.ToString();
            
            //Load items of cmbImageOrder
            cmbImageOrder.Items.Clear();
            cmbImageOrder.Items.Add(GlobalSetting.LangPack.Items["frmSetting.cmbImageOrder._Name"]);
            cmbImageOrder.Items.Add(GlobalSetting.LangPack.Items["frmSetting.cmbImageOrder._Length"]);
            cmbImageOrder.Items.Add(GlobalSetting.LangPack.Items["frmSetting.cmbImageOrder._CreationTime"]);
            cmbImageOrder.Items.Add(GlobalSetting.LangPack.Items["frmSetting.cmbImageOrder._LastAccessTime"]);
            cmbImageOrder.Items.Add(GlobalSetting.LangPack.Items["frmSetting.cmbImageOrder._LastWriteTime"]);
            cmbImageOrder.Items.Add(GlobalSetting.LangPack.Items["frmSetting.cmbImageOrder._Extension"]);
            cmbImageOrder.Items.Add(GlobalSetting.LangPack.Items["frmSetting.cmbImageOrder._Random"]);

            //Get value of cmbImageOrder
            s = GlobalSetting.GetConfig("ImageLoadingOrder", "0");
            i = 0;
            if (int.TryParse(s, out i))
            {
                if (-1 < i && i < cmbImageOrder.Items.Count)
                { }
                else
                {
                    i = 0;
                }
            }
            cmbImageOrder.SelectedIndex = i;

            //Get background color
            picBackgroundColor.BackColor = GlobalSetting.BackgroundColor;

            //Thumbnail bar on right side
            chkThumbnailVertical.Checked = !GlobalSetting.IsThumbnailHorizontal;

            //Use mouse wheel to browse images
            chkMouseNavigation.Checked = GlobalSetting.IsMouseNavigation;
        }

        private void chkPortableMode_CheckedChanged(object sender, EventArgs e)
        {
            GlobalSetting.IsPortableMode = chkPortableMode.Checked;
            

            // Check if user ia using temporary Portable mode from param
            if(Environment.GetCommandLineArgs().ToList().IndexOf("--portable") == -1)
            {
                GlobalSetting.SetConfig("IsPortableMode", GlobalSetting.IsPortableMode.ToString(), true);
            }
        }

        private void chkAutoUpdate_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAutoUpdate.Checked)
            {
                GlobalSetting.SetConfig("AutoUpdate", DateTime.Now.ToString());
            }
            else
            {
                GlobalSetting.SetConfig("AutoUpdate", "0");
            }
        }

        private void chkFindChildFolder_CheckedChanged(object sender, EventArgs e)
        {
            GlobalSetting.SetConfig("IsRecursiveLoading", chkFindChildFolder.Checked.ToString());
        }

        private void chkHideToolBar_CheckedChanged(object sender, EventArgs e)
        {
            GlobalSetting.IsShowToolBar = chkHideToolBar.Checked;
            GlobalSetting.SetConfig("IsHideToolbar", GlobalSetting.IsShowToolBar.ToString());
        }

        private void chkWelcomePicture_CheckedChanged(object sender, EventArgs e)
        {
            GlobalSetting.IsShowWelcome = chkWelcomePicture.Checked;
            GlobalSetting.SetConfig("IsShowWelcome", GlobalSetting.IsShowWelcome.ToString());
        }

        private void chkLoopViewer_CheckedChanged(object sender, EventArgs e)
        {
            GlobalSetting.IsLoopBackViewer = chkLoopViewer.Checked;
            GlobalSetting.SetConfig("IsLoopBackViewer", GlobalSetting.IsLoopBackViewer.ToString());
        }

        private void chkLoopSlideshow_CheckedChanged(object sender, EventArgs e)
        {
            GlobalSetting.IsLoopBackSlideShow = chkLoopSlideshow.Checked;
            GlobalSetting.SetConfig("IsLoopBackSlideShow", GlobalSetting.IsLoopBackSlideShow.ToString());
        }

        private void chkImageBoosterBack_CheckedChanged(object sender, EventArgs e)
        {
            GlobalSetting.IsImageBoosterBack = chkImageBoosterBack.Checked;
            GlobalSetting.SetConfig("IsImageBoosterBack", GlobalSetting.IsImageBoosterBack.ToString());
        }

        private void chkAllowMultiInstances_CheckedChanged(object sender, EventArgs e)
        {
            GlobalSetting.IsAllowMultiInstances = chkAllowMultiInstances.Checked;
            GlobalSetting.SetConfig("IsAllowMultiInstances", GlobalSetting.IsAllowMultiInstances.ToString());
        }

        private void chkConfirmationDelete_CheckedChanged(object sender, EventArgs e)
        {
            GlobalSetting.IsConfirmationDelete = chkConfirmationDelete.Checked;
            GlobalSetting.SetConfig("IsConfirmationDelete", GlobalSetting.IsConfirmationDelete.ToString());
        }

        private void chkESCToQuit_CheckedChanged(object sender, EventArgs e)
        {
            GlobalSetting.IsPressESCToQuit = chkESCToQuit.Checked;
            GlobalSetting.SetConfig("IsPressESCToQuit", GlobalSetting.IsPressESCToQuit.ToString());
        }

        private void barInterval_Scroll(object sender, EventArgs e)
        {
            GlobalSetting.SetConfig("SlideShowInterval", barInterval.Value.ToString());
            lblSlideshowInterval.Text = string.Format(GlobalSetting.LangPack.Items["frmSetting.lblSlideshowInterval"],
                                        barInterval.Value);
        }

        private void cmbZoomOptimization_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbZoomOptimization.SelectedIndex == 1)
            {
                GlobalSetting.ZoomOptimizationMethod = ZoomOptimizationValue.SmoothPixels;
            }
            else if (cmbZoomOptimization.SelectedIndex == 2)
            {
                GlobalSetting.ZoomOptimizationMethod = ZoomOptimizationValue.ClearPixels;
            }
            else
            {
                GlobalSetting.ZoomOptimizationMethod = ZoomOptimizationValue.Auto;
            }
        }

        private void numMaxThumbSize_ValueChanged(object sender, EventArgs e)
        {
            GlobalSetting.SetConfig("MaxThumbnailFileSize", numMaxThumbSize.Value.ToString());
        }

        private void cmbThumbnailDimension_SelectedIndexChanged(object sender, EventArgs e)
        {
            GlobalSetting.SetConfig("ThumbnailDimension", cmbThumbnailDimension.SelectedItem.ToString());
        }

        private void cmbImageOrder_SelectedIndexChanged(object sender, EventArgs e)
        {
            GlobalSetting.SetConfig("ImageLoadingOrder", cmbImageOrder.SelectedIndex.ToString());
            GlobalSetting.LoadImageOrderConfig();
        }

        private void picBackgroundColor_Click(object sender, EventArgs e)
        {
            ColorDialog c = new ColorDialog();
            c.AllowFullOpen = true;

            if (c.ShowDialog() == DialogResult.OK)
            {
                picBackgroundColor.BackColor = c.Color;
                GlobalSetting.BackgroundColor = c.Color;

                //Save background color
                GlobalSetting.SetConfig("BackgroundColor", GlobalSetting.BackgroundColor.ToArgb().ToString());
            }
        }

        private void chkThumbnailVertical_CheckedChanged(object sender, EventArgs e)
        {
            GlobalSetting.IsThumbnailHorizontal = !chkThumbnailVertical.Checked;
        }

        private void chkMouseNavigation_CheckedChanged(object sender, EventArgs e)
        {
            GlobalSetting.IsMouseNavigation = chkMouseNavigation.Checked;
            GlobalSetting.SetConfig("IsMouseNavigation", GlobalSetting.IsMouseNavigation.ToString());
        }

        #endregion


        #region TAB LANGUAGE
        private void lnkGetMoreLanguage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                string version = Application.ProductVersion.Replace(".", "_");
                Process.Start("http://www.imageglass.org/download/languagepacks?utm_source=app_" + version + "&utm_medium=app_click&utm_campaign=app_languagepack");
            }
            catch { }
        }

        private void lnkInstallLanguage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process p = new Process();
            p.StartInfo.FileName = GlobalSetting.StartUpDir + "igtasks.exe";
            p.StartInfo.Arguments = "iginstalllang";
            p.Start();
        }

        private void lnkCreateNew_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process p = new Process();
            p.StartInfo.FileName = GlobalSetting.StartUpDir + "igtasks.exe";
            p.StartInfo.Arguments = "ignewlang";
            p.Start();
        }

        private void lnkEdit_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process p = new Process();
            p.StartInfo.FileName = GlobalSetting.StartUpDir + "igtasks.exe";
            p.StartInfo.Arguments = "igeditlang \"" + GlobalSetting.LangPack.FileName + "\"";
            p.Start();
        }

        private void lnkRefresh_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            cmbLanguage.Items.Clear();
            cmbLanguage.Items.Add("English");
            dsLanguages = new List<Library.Language>();
            dsLanguages.Add(new Library.Language());

            if (!Directory.Exists(GlobalSetting.StartUpDir + "Languages\\"))
            {
                Directory.CreateDirectory(GlobalSetting.StartUpDir + "Languages\\");
            }
            else
            {
                foreach (string f in Directory.GetFiles(GlobalSetting.StartUpDir + "Languages\\"))
                {
                    if (Path.GetExtension(f).ToLower() == ".iglang")
                    {
                        Library.Language l = new Library.Language(f);
                        dsLanguages.Add(l);

                        int iLang = cmbLanguage.Items.Add(l.LangName);
                        string curLang = GlobalSetting.LangPack.FileName;

                        //using current language pack
                        if (f.CompareTo(curLang) == 0)
                        {
                            cmbLanguage.SelectedIndex = iLang;
                        }
                    }
                }
            }

            if (cmbLanguage.SelectedIndex == -1)
            {
                cmbLanguage.SelectedIndex = 0;
            }
        }
        
        private void cmbLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblLanguageWarning.Visible = false;
            GlobalSetting.LangPack = dsLanguages[cmbLanguage.SelectedIndex];

            //check compatibility
            var lang = new Language();
            if(lang.MinVersion.CompareTo(GlobalSetting.LangPack.MinVersion) != 0)
            {
                lblLanguageWarning.Visible = true;
            }
        }

















        #endregion


        #region TAB FILE ASSOCIATIONS
        private void btnOpenFileAssociations_Click(object sender, EventArgs e)
        {
            string controlpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "control.exe"); // path to %windir%\system32\control.exe (ensures the correct control.exe)

            Process.Start(controlpath, "/name Microsoft.DefaultPrograms /page pageFileAssoc");
        }





        #endregion

        
    }
}
