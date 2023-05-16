using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Windows;

namespace BarCodeProject
{
    public class Validation
    {
        private string CodeType;
        private string text;

        //public Validation() { }

        public Validation(string CodeType, string text) {
            this.CodeType = CodeType;
            this.text = text;
        }

        ~Validation() { }

        public bool ValidationText()
        {
            switch (CodeType)
            {
                case "EAN-13":
                    if (text.Length < 12)
                    {
                        MessageBox.Show("количество символов в шифруемом тексте должно быть равны 12");
                        return false;
                    }
                    break;
                case "Code-128":
                    {
                        Regex reg = new Regex("[А-Яа-я]");
                        if (reg.IsMatch(text))
                        {
                            return false;
                        }
                        else return true;
                    }
                case "ITF-14":
                    if (text.Length < 13)
                    {
                        MessageBox.Show("количество символов в шифруемом тексте должно быть равны 13");
                        return false;
                    }
                    if (text[0]=='0' || text[0]== '9')
                    {
                        MessageBox.Show("Для написания первого символа 0 и 9 не используется.");
                        return false;
                    }
                    break;

            }
                
            
            return true;
        }

    }
}
