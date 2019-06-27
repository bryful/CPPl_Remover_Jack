using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;


namespace CPPl_Remover_Jack
{
	public class CPPl_remover
	{
		private int m_FileSize = 0;
		public int FileSize { get { return m_FileSize; } }
		private int m_ListIndex = 0;
		private int m_ListSize = 0;
		public int ListSize { get { return m_ListSize; } }
		private string m_filepath = "";
		/*
		 * 
		 *  <svap bdata="07182602"/>
			<head bdata="005100080718260280000000310b4bfe067ef08e"/>
			<nhed bdata="0000000000000005010100001e100100000003a4becc3778000000000c381380"/>
			<adfr bdata="40e7700000000000"/>
			<qtlg bdata="00"/>
			<acer bdata="01"/>
			<CPPl>
	*/
		private TextBox m_TextBox = null;
		public TextBox TextBox
		{
			get { return m_TextBox; }
			set
			{
				m_TextBox = value;
				if (m_TextBox!=null)
				{
					m_TextBox.Multiline = true;
					m_TextBox.Text = "";
				}
			}
		}
		public Form Form = null;
		static public bool IsAepFile(string s)
		{
			bool ret = false;
			if (s == "") return ret;
			if (File.Exists(s) == true)
			{
				if (Path.GetExtension(s).ToLower() == ".aep")
				{
					ret = true;
				}
			}
			return ret;
		}
		private void showMessage(string s)
		{
			if (m_TextBox == null) return;
			m_TextBox.AppendText(s+"\r\n");
			m_TextBox.Update();
			m_TextBox.Refresh();

			if (Form!=null)
			{
				Application.DoEvents();
				Form.Update();
				Form.Refresh();

			}
		}
		private int FindTag(byte[] ary,string s,int start =0)
		{
			int ret = -1;
			if (s.Length != 4) return ret;
			int len = ary.Length;
			if (len <= 8) return ret;
			byte[] tagtbl = new byte[4];
			tagtbl[0] = (byte)s[0];
			tagtbl[1] = (byte)s[1];
			tagtbl[2] = (byte)s[2];
			tagtbl[3] = (byte)s[3];

			for ( int i=start; i<len-4;i++)
			{
				if (ary[i]== tagtbl[0])
					if (ary[i+1] == tagtbl[1])
						if (ary[i + 2] == tagtbl[2])
							if (ary[i + 3] == tagtbl[3])
							{
								ret = i;
								break;
							}
			}
			return ret;

		}
		private int GetTaglength(byte[] ary,int tagIndex)
		{
			int ret = 0;
			if(tagIndex+8>=ary.Length)
			{
				return ret;
			}
			int tag = tagIndex + 4;
			ret = ary[tag+3]
				+ (ary[tag + 2]) * 0x100
				+ (ary[tag + 1]) * 0x10000
				+ (ary[tag + 0]) * 0x1000000;

			return ret;
		}
		private bool chkTagSize(string s)
		{
			bool ret = false;
			FileStream fs = new FileStream(s, FileMode.Open, FileAccess.Read);
			try
			{
				long len = fs.Length;
				if (len <= 0)
				{
					showMessage("[" + s + "]は0byteです");
					return ret;
				}
				long rz = 1024 * 1024;
				if (rz > len) rz = len;
				byte[] bs = new byte[rz];

				showMessage("\r\n[" + s + "]を確認\r\n");
				fs.Read(bs, 0, (int)rz);

				int idx = FindTag(bs, "svap", 0);
				if (idx >= 0)
				{
					long sz = GetTaglength(bs, idx);
					showMessage(String.Format("svapタグを見つけました。サイズは{0}byteです", sz));
				}
				idx = FindTag(bs, "head", 0);
				if (idx >= 0)
				{
					long sz = GetTaglength(bs, idx);
					showMessage(String.Format("headタグを見つけました。サイズは{0}byteです", sz));
				}
				idx = FindTag(bs, "nhed", 0);
				if (idx >= 0)
				{
					long sz = GetTaglength(bs, idx);
					showMessage(String.Format("nhedタグを見つけました。サイズは{0}byteです", sz));
				}
				idx = FindTag(bs, "nhed", 0);
				if (idx >= 0)
				{
					long sz = GetTaglength(bs, idx);
					showMessage(String.Format("nhedタグを見つけました。サイズは{0}byteです", sz));
				}
				idx = FindTag(bs, "adfr", 0);
				if (idx >= 0)
				{
					long sz = GetTaglength(bs, idx);
					showMessage(String.Format("adfrタグを見つけました。サイズは{0}byteです", sz));
				}
				idx = FindTag(bs, "qtlg", 0);
				if (idx >= 0)
				{
					long sz = GetTaglength(bs, idx);
					showMessage(String.Format("qtlgタグを見つけました。サイズは{0}byteです", sz));
				}
				idx = FindTag(bs, "acer", 0);
				if (idx >= 0)
				{
					long sz = GetTaglength(bs, idx);
					showMessage(String.Format("acerタグを見つけました。サイズは{0}byteです", sz));
				}
				idx = FindTag(bs, "LIST", 0);
				if (idx >= 0)
				{
					int sz = GetTaglength(bs, idx);
					showMessage(String.Format("LISTタグを見つけました。サイズは{0}byteです", sz));
					m_ListIndex = idx;
					m_ListSize = sz;
					m_FileSize = (int)fs.Length;
					m_filepath = s;
					ret = true;
				}
				else
				{
					m_ListIndex = -1;
					m_ListSize = 0;
					m_FileSize = 0;
					m_filepath = "";
				}
			}
			finally
			{
				fs.Close();
			}
			return ret;
		}
		public bool CheckAep(string s)
		{
			bool ret = false;

			if (IsAepFile(s) == false) {
				showMessage("[" + s + "]は存在しません!");
				return ret;
			}
			if (chkTagSize(s)==true)
			{
				ret = true;
			}
			return ret;
		}
		public bool ConvertAep()
		{
			bool ret = false;
			if (m_filepath == "") return ret;


			string srcPath = m_filepath;
			string dstPath = Path.Combine(Path.GetDirectoryName(m_filepath),"temp.aep");

			if(File.Exists(dstPath)==true)
			{
				File.Delete(dstPath);
			}

			FileStream rs = new System.IO.FileStream(srcPath, FileMode.Open,FileAccess.Read);
			FileStream ws = new System.IO.FileStream(dstPath,FileMode.Append,FileAccess.Write);
			try
			{
				showMessage("\r\n[" + m_filepath + "]変換開始");
				long bf = m_ListIndex + 12;

				byte[] buf = new byte[bf];
				long bufl = rs.Read(buf, 0, (int)bf);
				if (bufl != bf)
				{
					showMessage("読み込みエラー");
					return ret;

				}
				int v = (int)buf[4] * 0x1000000 + (int)buf[5] * 0x10000 + (int)buf[6] * 0x100 + (int)buf[7];
				v -= m_ListSize - 4;
				//全体のサイズ
				buf[7] = (byte)(v & 0xff);
				buf[6] = (byte)((v>>8) & 0xff);
				buf[5] = (byte)((v >> 16) & 0xff);
				buf[4] = (byte)((v >> 24) & 0xff);

				//リストのサイズ
				buf[bf - 8] = 0x00;
				buf[bf - 7] = 0x00;
				buf[bf - 6] = 0x00;
				buf[bf - 5] = 0x04;
				//buf[bf - 4] C
				//buf[bf - 3] P
				//buf[bf - 2] P
				//buf[bf - 1] l

				showMessage(String.Format("Header write ({0}byte)",bf));
				ws.Write(buf, 0, (int)bf);

				long bf2 = m_FileSize - m_ListSize+4-bf;
				byte[] buf2 = new byte[bf2];
				showMessage(String.Format("Data read ({0}byte)", bf2));
				rs.Position += m_ListSize - 4;
				int buf2l = rs.Read(buf2, 0, (int)bf2);
				if (buf2l != bf2)
				{
					
					showMessage(String.Format("読み込みエラー{0}/{1}:{2}",buf2l,bf2, bf2- buf2l));
					//return ret;
				}
				showMessage(String.Format("Data write ({0}byte)", bf2));
				ws.Write(buf2, 0, (int)bf2);
				showMessage("書き込み終了しました");
				
				ret = true;


			}catch(Exception e)
			{
				showMessage(e.ToString());
			}
			finally
			{
				rs.Close();
				ws.Close();

				if (ret == true)
				{
					File.Move(srcPath, srcPath + ".backup");
					File.Move(dstPath, srcPath);

				}
				else
				{
					if (File.Exists(dstPath) == true)
					{
						File.Delete(dstPath);
					}
				}

			}





			return ret;
		}
	}
}
