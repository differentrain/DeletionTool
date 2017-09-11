using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeletionTool
{
    public class NumberBox : TextBox
    {

        private Int32 _MinNum;
        private Int32 _MaxNum;



        public Int32 MinNum
        {
            get { return _MinNum; }
            set { _MinNum = value; }
        }

        public Int32 MaxNum
        {
            get { return _MaxNum; }
            set { _MaxNum = value; }
        }

        public Int32 Value => Int32.Parse(this.Text);

        private Boolean IsNop(string Str)
        {

            if (Str.Length == 0)
                return false;

            if (Str[0] == '0' && Str.Length > 1)
                return true;

            if (Str.Contains("-"))
            {
                if (_MinNum < 0)
                {
                    if (Str.IndexOf("-") != Str.LastIndexOf("-") || Str.IndexOf("-") != 0)
                        return true;
                }
                else
                {
                    return true;
                }
            }

            try
            {
                if (Convert.ToInt64(Str) > _MaxNum || Convert.ToInt64(Str) < _MinNum)
                    return true;
            }
            catch (Exception)
            {
                return true;
            }

            return false;
        }


        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            switch (Convert.ToInt32(e.KeyChar))
            {

                case 8:
                    //Keys.Back
                    if (this.SelectionLength == 0)
                    {
                        if (this.SelectionStart > 0)
                            e.Handled = IsNop(this.Text.Remove(this.SelectionStart - 1, 1));
                    }
                    else
                    {
                        e.Handled = IsNop(this.Text.Remove(this.SelectionStart, this.SelectionLength));
                    }
                    break;
                case 22:
                    //V
                    try
                    {
                        e.Handled = IsNop(this.Text.Remove(this.SelectionStart, this.SelectionLength).Insert(this.SelectionStart, Convert.ToInt64(Clipboard.GetText()).ToString()));
                    }
                    catch (Exception)
                    {
                        e.Handled = true;
                    }
                    break;
                case 24:
                    //X
                    e.Handled = IsNop(this.Text.Remove(this.SelectionStart, this.SelectionLength));
                    break;
                case 3:
                    e.Handled = false;
                    break;
                default:
                    e.Handled = IsNop(this.Text.Remove(this.SelectionStart, this.SelectionLength).Insert(this.SelectionStart, e.KeyChar.ToString()));
                    break;
            }

            base.OnKeyPress(e);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Back:
                case Keys.OemMinus:
                case Keys.Subtract:
                case Keys.D0:
                case Keys.D1:
                case Keys.D2:
                case Keys.D3:
                case Keys.D4:
                case Keys.D5:
                case Keys.D6:
                case Keys.D7:
                case Keys.D8:
                case Keys.D9:
                case Keys.NumPad0:
                case Keys.NumPad1:
                case Keys.NumPad2:
                case Keys.NumPad3:
                case Keys.NumPad4:
                case Keys.NumPad5:
                case Keys.NumPad6:
                case Keys.NumPad7:
                case Keys.NumPad8:
                case Keys.NumPad9:
                case Keys.Tab:
                case Keys.Left:
                case Keys.Right:
                    if (e.Control == true || e.Shift == true)
                        e.SuppressKeyPress = true;
                    break;
                case Keys.C:
                case Keys.A:
                case Keys.V:
                case Keys.X:
                    if (e.Control == false || e.Shift == true)
                        e.SuppressKeyPress = true;
                    break;
                case Keys.Delete:
                    if (this.SelectionLength == 0)
                    {
                        e.SuppressKeyPress = IsNop(this.Text.Remove(this.SelectionStart, 1));
                    }
                    else
                    {
                        e.SuppressKeyPress = IsNop(this.Text.Remove(this.SelectionStart, this.SelectionLength));
                    }
                    if (e.Control == true || e.Shift == true)
                        e.SuppressKeyPress = true;

                    break;
                default:
                    e.SuppressKeyPress = true;
                    break;
            }
            base.OnKeyDown(e);
        }

        protected override void OnTextChanged(EventArgs e)
        {
            if (String.IsNullOrEmpty(this.Text)) this.Text = this.MinNum.ToString();
            base.OnTextChanged(e);

        }


    }
}
