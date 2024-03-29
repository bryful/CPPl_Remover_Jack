﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

using BRY;

using Codeplex.Data;
/// <summary>
/// 基本となるアプリのスケルトン
/// </summary>
namespace CPPl_Remover_Jack
{
	public partial class Form1 : Form
	{
		CPPl_remover cppl = new CPPl_remover();
		//-------------------------------------------------------------
		/// <summary>
		/// コンストラクタ
		/// </summary>
		public Form1()
		{
			InitializeComponent();
		}
		/// <summary>
		/// コントロールの初期化はこっちでやる
		/// </summary>
		protected override void InitLayout()
		{
			base.InitLayout();
		}
		//-------------------------------------------------------------
		/// <summary>
		/// フォーム作成時に呼ばれる
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Form1_Load(object sender, EventArgs e)
		{
			//設定ファイルの読み込み
			JsonPref pref = new JsonPref();
			if (pref.Load())
			{
				bool ok = false;
				Size sz = pref.GetSize("Size", out ok);
				if (ok) this.Size = sz;
				Point p = pref.GetPoint("Point", out ok);
				if (ok) this.Location = p;
			}
			this.Text = Path.GetFileNameWithoutExtension(Application.ExecutablePath);
			cppl.TextBox = edProgress;
			cppl.Form = this;
		}
		//-------------------------------------------------------------
		/// <summary>
		/// フォームが閉じられた時
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Form1_FormClosed(object sender, FormClosedEventArgs e)
		{
			//設定ファイルの保存
			JsonPref pref = new JsonPref();
			pref.SetSize("Size", this.Size);
			pref.SetPoint("Point", this.Location);

			pref.SetIntArray("IntArray", new int[] { 8, 9, 7 });
			pref.Save();

		}
		//-------------------------------------------------------------
		/// <summary>
		/// ドラッグ＆ドロップの準備
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Form1_DragEnter(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				e.Effect = DragDropEffects.All;
			}
			else
			{
				e.Effect = DragDropEffects.None;
			}
		}
		/// <summary>
		/// ドラッグ＆ドロップの本体
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Form1_DragDrop(object sender, DragEventArgs e)
		{
			string[] files = (string[])e.Data.GetData(DataFormats.FileDrop, false);
			//ここでは単純にファイルをリストアップするだけ
			GetCommand(files);
		}
		//-------------------------------------------------------------
		/// <summary>
		/// ダミー関数
		/// </summary>
		/// <param name="cmd"></param>
		public void GetCommand(string[] cmd)
		{
			if (cmd.Length > 0)
			{
				foreach (string s in cmd)
				{
					if(GetCommand(s)==true)
					{
						break;
					}
				}
			}
		}
		public bool　GetCommand(string s)
		{
			bool ret = false;
			if (CPPl_remover.IsAepFile(s) == true)
			{
				if (cppl.CheckAep(s) == true)
				{
					if (cppl.ListSize <= 4)
					{
						edProgress.AppendText("\r\n*** 削除済みです ***\r\n");
						edAep.Text = "";
					}
					else
					{
						edAep.Text = s;
						ret = true;
					}
				}
			}
			return ret;
		}
		/// <summary>
		/// メニューの終了
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		//-------------------------------------------------------------
		private void quitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}

		private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			AppInfoDialog.ShowAppInfoDialog();
		}
		private void button1_Click(object sender, EventArgs e)
		{
			OpenAep();
		}
		private bool OpenAep()
		{
			OpenFileDialog dlg = new OpenFileDialog();
			dlg.Filter = "aep file(*.aep)|*.aep| all files(*.*)|*.*";
			dlg.DefaultExt = ".aep";
			if (dlg.ShowDialog() == DialogResult.OK)
			{
				return GetCommand(dlg.FileName);
			}
			else
			{
				return false;
			}

		}

		private void edAep_TextChanged(object sender, EventArgs e)
		{
			btnExec.Enabled = CPPl_remover.IsAepFile(edAep.Text);
		}

		private void btnExec_Click(object sender, EventArgs e)
		{
			cppl.ConvertAep();
			
		}

		private void openToolStripMenuItem_Click(object sender, EventArgs e)
		{
			OpenAep();
		}

	}
}
