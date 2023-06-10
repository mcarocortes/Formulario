using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
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
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace _2.Formulario
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

        }


        //***************************VARIABLES GENERALES
        bool required1, required2, required3, required4, required5, required6, required7 = false;
        bool required8 = true;
        RadioButton rtb; // VARIABLE TIPO RADIOBUTTON
        TextBox textos; // Texto Obligatorio
        ComboBox casillaCombo;



        //***************************Al HACER CLIC EN RADIOBUTTON: DESAPARECEN LOS TEXTBOX DE "CAMPO OBLIGATORIO" Y HABILITAMOS EL COMBOBOX
        private void radio_Click(object sender, RoutedEventArgs e)
        {
            Obligatory10.Visibility = Visibility.Hidden; Obligatory3.Visibility = Visibility.Hidden; Obligatory4.Visibility = Visibility.Hidden;
            Distr_Combo.IsEnabled = true; Client_Combo.IsEnabled = true; // Y SE HABILITAN LAS COMBOBOX
                                                                              
            rtb = sender as RadioButton; // INFORMACION RECIBIDA DE RADIOBUTTON

            switch (rtb.Name) {
                case "Distribuidor": 
                    Distr_Combo.Visibility = Visibility.Visible; //APARECE EL DE DISTRIBUIDOR
                    Distr_Combo.SelectedIndex = 0;// "en la posicion --"
                    Client_Combo.Visibility = Visibility.Hidden; //DESAPARECE EL DE CLIENTE
                    break;

                case "Cliente":  
                    Distr_Combo.Visibility = Visibility.Hidden; //DESAPARECE EL DE DISTRIBUIDOR
                    Client_Combo.SelectedIndex = 0;// "en la posicion --"
                    Client_Combo.Visibility = Visibility.Visible; //APARECE EL DE CLIENTE
                    break;
            }
        }

        //***************************CREANDO TEXT_CHANGED PARA TODOS +  CONTROL DE EXCEPCIONES
        private void TextChanged(object sender, TextChangedEventArgs e)
        {
            textos = sender as TextBox;// HABLAMOS DE TODOS LO TEXTBOX

            //LOS textos NO TE DEJARÁN ESCRIBIR CUANDO EL PRIMER CARACTER ES ESPACIO
            for (int i = 0; i < textos.Text.Length; i++)
            {
                if (textos.Text[0] == ' ')
                { textos.Clear(); };
            };


            try   //CONTROL DE EXCEPCIONES:
            {
                int number;// CREAMOS UNA VARIABLE QUE SERÁ UN NUMERO

                switch (textos.Name)// CUANDO  EL NOMBRE DE LOS TEXTBLOCKS SEAN 
                {
                    case "Name_Text": if ((textos.Text == "") || (textos.Text == null)) { required1 = false; Obligatory1.Visibility = Visibility.Visible; } else { required1 = true; Obligatory1.Visibility = Visibility.Hidden; }; break;
                    case "FirstSurname_Text": if (textos.Text != "") { required2 = true; Obligatory2.Visibility = Visibility.Hidden; } else { required2 = false; Obligatory2.Visibility = Visibility.Visible; }; break;
                    case "Number_Text": number = Convert.ToInt32(Number_Text.Text); required3 = true; Obligatory5.Visibility = Visibility.Hidden; break;
                    case "NumberSecond_Text": number = Convert.ToInt32(NumberSecond_Text.Text); required8 = true; break;

                    case "Correo_Text": if (textos.Text != "") { required4 = true; Obligatory6.Visibility = Visibility.Hidden; } else { required4 = false; Obligatory6.Visibility = Visibility.Visible; }; break;
                    case "Address_Text": if (textos.Text != "") { required5 = true; Obligatory7.Visibility = Visibility.Hidden; } else { required5 = false; Obligatory7.Visibility = Visibility.Visible; }; break;
                    case "CP_Text": number = Convert.ToInt32(CP_Text.Text); required6 = true; Obligatory8.Visibility = Visibility.Hidden; break;
                    case "Pobla_Text": if (textos.Text != "") { required7 = true; Obligatory9.Visibility = Visibility.Hidden; } else { required7 = false; Obligatory9.Visibility = Visibility.Visible; }; break;
                }
            }
            catch
            {
                if (textos.Name == "Number_Text") { textos.Clear(); required3 = false; Obligatory5.Visibility = Visibility.Visible; };
                if (textos.Name == "CP_Text") { textos.Clear(); required6 = false; Obligatory8.Visibility = Visibility.Visible; };
                if (textos.Name == "NumberSecond_Text") { textos.Clear(); required8 = false; };
                if (textos.Text == "") { if (textos.Name == "NumberSecond_Text") { required8 = true; } }
                else
                { //MOSTRAREMOSLA VENTANA DE NOTIFICACIÓN PERSONALIZADA
                    notificacion.Visibility = Visibility.Visible;
                    Entendido_btn.Visibility = Visibility.Visible;
                    invalid_msg.Visibility = Visibility.Visible;
                };

            };

        }

        //*************************** GUARDANDO LA INFORMACIÓN DE LA SELECCION DEL COMBOBOX Y LO PONEMOS EN NUESTRA VARIABLE 
        private void Seleccion_ComboBox(object sender, SelectionChangedEventArgs e)
        {
            casillaCombo = sender as ComboBox;
        }


        //*************************** EVENTOS EN BOTONES
        private void btn_click(object sender, RoutedEventArgs e)
        {
            Button botones = sender as Button;

            switch (botones.Name)
            {
                case "Entendido_btn": //Desaparece la ventana de notificacion personalizada
                    notificacion.Visibility = Visibility.Hidden;
                    Entendido_btn.Visibility = Visibility.Hidden;
                    insertValues.Visibility = Visibility.Hidden;
                    invalid_msg.Visibility = Visibility.Hidden; break;

                case "Cancel_btn": Close(); break;
                case "Aceptar_btn":
                    insertValues.Text = ("INSERT INTO " + rtb.Name.ToUpper() + " VALUES ( '" + Name_Text.Text.ToUpper() + "', '" + FirstSurname_Text.Text.ToUpper() + "'," + "'" + SecondSurname_Text.Text.ToUpper() + "', " + Number_Text.Text + ", " + NumberSecond_Text.Text + ", '" + Correo_Text.Text.ToUpper() + "', '" + Address_Text.Text.ToUpper() + "', " + CP_Text.Text + ", '" + Pobla_Text.Text.ToUpper() + "', '" + casillaCombo.Text.ToUpper() + "')");
                    //MOSTRAREMOSLA VENTANA DE NOTIFICACIÓN PERSONALIZADA
                    notificacion.Visibility = Visibility.Visible;
                    Entendido_btn.Visibility = Visibility.Visible;
                    insertValues.Visibility = Visibility.Visible; break;
            }
        }


        //*************************** COMPROBACIONES CADA VEZ QUE SE MUEVE EL MOUSE POR LA PANTALLA
        private void movimiento(object sender, MouseEventArgs e)
        {  //SI ESTÁ TODO LO OBLIGATORIO Y ACEPTADO COMPLETO                                                                     si rtb es diferente de valor nulo
            if ((required1 == true)&& (required2 == true) && (required3 == true) && (required4 == true) && (required5 == true) && (required6 == true) && (required7 == true) &&
                 (required8 == true) && (rtb != null) && (casillaCombo.Text != "-- Distribuidor --")&& (casillaCombo.Text != "-- Cliente --"))
            { Aceptar_btn.Visibility = Visibility.Visible; // SE VISIBILIZARÁ EL ACEPTAR
              Aceptar_btn.IsEnabled = true;
            }
            else
            { Aceptar_btn.Visibility = Visibility.Hidden;  //OCULTARE EL BOTON ACEPTAR
            }
        }


        //*************************** ESTE EVENTO PERMITE ARRASTRAR EL FORMULARIO POR TODA LA PÁGINA
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }


      

  


    }
}
