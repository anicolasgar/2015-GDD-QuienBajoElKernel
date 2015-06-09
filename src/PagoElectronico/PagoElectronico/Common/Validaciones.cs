﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace PagoElectronico.Common
{
    public class Validaciones
    {
        public static bool validarCampoVacio(TextBox textBox)
        {
            if (string.IsNullOrEmpty(textBox.Text))
	        {
                textBox.BackColor = System.Drawing.Color.LightCoral;
                return false;
            }
            else
            {
                textBox.BackColor = System.Drawing.Color.White;
                return true;   
            }
           
        }

        public static bool validarCampoString(TextBox textBox)
        {
            Regex expRegular = new Regex("^[a-zA-Z]*$");
            if (!expRegular.IsMatch(textBox.Text))
            {
                textBox.BackColor = System.Drawing.Color.LightCoral;
                return false;
            }
            else
            {
                textBox.BackColor = System.Drawing.Color.White;
                return true;
            }           
        }

        public static bool validarCampoNumericoEntero(TextBox textBox)
        {
            Regex expRegular = new Regex("^[0-9]*$");
            if (!expRegular.IsMatch(textBox.Text))
            {
                textBox.BackColor=System.Drawing.Color.LightCoral;
                return false;
            }
            else
            {
                textBox.BackColor = System.Drawing.Color.White;
                return true;     
            }            
        }

        public static bool validarCampoNumericoDouble(TextBox textBox)
        {
            Double num;
            if (!Double.TryParse(textBox.Text, out num))
            {
                textBox.BackColor = System.Drawing.Color.LightCoral;
                return false;
            }
            else
            {
                textBox.BackColor = System.Drawing.Color.White;
                return true;   
            }                     
        }

        public static bool validarCampoMail(TextBox textBox)
        {
            Regex expRegular = new Regex(@"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
            @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$");
            if (!expRegular.IsMatch(textBox.Text))
            {
                textBox.BackColor=System.Drawing.Color.LightCoral;
                return false;
            }
            else
            {
                textBox.BackColor = System.Drawing.Color.White;
                return true;
            }            
        }

    }
}
