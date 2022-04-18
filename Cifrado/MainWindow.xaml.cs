using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
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

using log4net;
using ArtisanCode.Log4NetMessageEncryptor;

namespace Cifrado
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public byte[] prueba;
        string Contraseña;
        public static string error = "";

        enum Tipo
        {
            Todo,
            Desencriptar
        }

        public MainWindow()
        {
            InitializeComponent();
            Contraseña = txtcontrasena.Text;
            log4net.Appender.FileAppender fileAppender = new log4net.Appender.FileAppender();
            
           ILog log =  LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            log.Info("jesus");


        }

        private void probandoAESGCM(string texto)
        {
            byte[] llave = AESThenHMAC.NewKey();
            txtTexto.Text = texto;
            lblCaracteresTexto.Content = texto.Length.ToString();
            string textoencriptado = AESGCM.SimpleEncryptWithPassword(texto, Contraseña, llave);
            lblCaracteresTextoEncriptado.Content = textoencriptado.Length.ToString();
            txtTextoEncriptado.Text = textoencriptado;
            string textoDesencriptado = AESGCM.SimpleDecryptWithPassword(textoencriptado, Contraseña, llave.Count());
            lblCaracteresTextoDesencriptado.Content = textoDesencriptado.Length.ToString();
            txtDesencriptado.Text = textoDesencriptado;
            File.WriteAllText("prueba_desencriptado_AESGCM.bin", textoDesencriptado);
        }

        private void probandoAESThenHMAC(string texto)
        {
            byte[] llave = AESThenHMAC.NewKey();
            txtTexto.Text = texto;
            lblCaracteresTexto.Content = texto.Length.ToString();
            string textoencriptado = AESThenHMAC.SimpleEncryptWithPassword(texto, Contraseña, llave);
            txtTextoEncriptado.Text = textoencriptado;
            lblCaracteresTextoEncriptado.Content = textoencriptado.Length.ToString();
            string textoDesencriptado = AESThenHMAC.SimpleDecryptWithPassword(textoencriptado, Contraseña, llave.Count());
            lblCaracteresTextoDesencriptado.Content = textoDesencriptado.Length.ToString();
            txtDesencriptado.Text = textoDesencriptado;
            File.WriteAllText("prueba_desencriptado_AESThenHMAC.bin", textoDesencriptado);

        }

        private void DesencriptarAESGCM(string texto)
        {
            try
            {


                byte[] llave = AESThenHMAC.NewKey();
                lblCaracteresTextoEncriptado2.Content = texto.Length.ToString();
                txtTextoEncriptado2.Text = texto;
                string textoDesencriptado = AESGCM.SimpleDecryptWithPassword(texto, Contraseña, llave.Count());
                if (!string.IsNullOrEmpty(textoDesencriptado))
                {
                    lblCaracteresTextoDesencriptado2.Content = textoDesencriptado.Length.ToString();
                    txtDesencriptado2.Text = textoDesencriptado;
                    File.WriteAllText("prueba_desencriptado_AESGCM.bin", textoDesencriptado);
                }
                else
                {
                    MessageBox.Show(error);
                }
            }
            catch(Exception msj)
            {
                MessageBox.Show(msj.Message);
            }

        }

        private void DesencriptarAESThenHMAC(string texto)
        {
            try
            {
                byte[] llave = AESThenHMAC.NewKey();
                txtTextoEncriptado2.Text = texto;
                lblCaracteresTextoEncriptado2.Content = texto.Length.ToString();
                string textoDesencriptado = AESThenHMAC.SimpleDecryptWithPassword(texto, Contraseña, llave.Count());
                if (!string.IsNullOrEmpty(textoDesencriptado))
                {
                    lblCaracteresTextoDesencriptado2.Content = textoDesencriptado.Length.ToString();
                    txtDesencriptado2.Text = textoDesencriptado;
                    File.WriteAllText("prueba_desencriptado_AESThenHMAC.bin", textoDesencriptado);
                }
                else
                {
                    MessageBox.Show(error);
                }
            }
            catch (Exception msj)
            {
                MessageBox.Show(msj.Message);
            }
        }

        private void seleccionarMetodo(string texto,Tipo tipo= Tipo.Todo)
        {
            if (!string.IsNullOrEmpty(texto))
            {
                if (tipo == Tipo.Todo)
                {
                    if (rbAESGCM.IsChecked == true)
                    {
                        probandoAESGCM(texto);
                    }
                    else if (rbAEAESThenHMACSGCM.IsChecked == true)
                    {
                        probandoAESThenHMAC(texto);
                    }
                }
                else
                {
                    if (rbAESGCM.IsChecked == true)
                    {
                        DesencriptarAESGCM(texto);
                    }
                    else if (rbAEAESThenHMACSGCM.IsChecked == true)
                    {
                        DesencriptarAESThenHMAC(texto);
                    }
                }
            }
        }

        private void Limpiar()
        {
            txtDesencriptado.Text = string.Empty;
            txtDesencriptado2.Text = string.Empty;
            txtTexto.Text = string.Empty;
            txtTextoEncriptado.Text = string.Empty;
            txtTextoEncriptado2.Text = string.Empty;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            seleccionarMetodo(txtTexto.Text);
        }

        private void btnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                lblArchivo.Content = openFileDialog.FileName;
                seleccionarMetodo(File.ReadAllText(openFileDialog.FileName,Encoding.UTF8));
                //txtTexto.Text = File.ReadAllText(openFileDialog.FileName);
            }
        }

        private void txtcontrasena_TextChanged(object sender, TextChangedEventArgs e)
        {
            Contraseña = txtcontrasena.Text;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            seleccionarMetodo(txtTextoEncriptado2.Text, Tipo.Desencriptar);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Limpiar();
        }

        
    }
}
