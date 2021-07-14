using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using System.Net;


namespace AppWeb
{
    public partial class Default : System.Web.UI.Page
    {
        public string clave = "#3zl40nd4",passw;
        private bool ExisteEmail,ExisteUsuario, BloquearSesion;
        protected void Page_Load(object sender, EventArgs e)
        {
            AlertDanger.Visible = false;
            AlertWarning.Visible = false;
            if (!IsPostBack)
            {
                if (Response.Cookies["user"] != null && Response.Cookies["pass"] != null)
                {
                    TxtUser.Text = Request.Cookies["user"].Value;
                    TxtContrasenia.Attributes["value"] = Request.Cookies["pass"].Value;
                }
            }
            if (Request.Params["id"] != null)
            {
                String identificador = Request.Params["id"];
                if (identificador == "1")
                {
                    AlertDanger.Visible = true;
                    lblDanger.Text = "<strong>* Debe iniciar sesión para ingresar</strong>";
                }

            }

           
            
        }

        
        protected void BtnIngresar_Click(object sender, EventArgs e)
        {
            
            byte[] PassEncryp = Encoding.UTF8.GetBytes(cifrar(TxtContrasenia.Text));
            string PassUser = "0x" + BitConverter.ToString(PassEncryp).Replace("-", string.Empty);
            if (TxtUser.Text != "" || TxtContrasenia.Text != "")
            {
                if (!VerificarIntentos(TxtUser.Text)) //Verificar si esta bloqueada la sesión
                {
                    try
                    {
                        using (geoBoxWebEntity gbwE = new geoBoxWebEntity())
                        {
                            var Query = from order in gbwE.Usuarios
                                        where order.usuario == TxtUser.Text || order.password == PassEncryp
                                        select new
                                        {
                                            nombreusuario = order.usuario,
                                            pass = order.password
                                        };
                            if (Query.Count() > 0)
                            {
                            foreach (var user in Query)
                            {

                                
                                if (TxtUser.Text == user.nombreusuario)
                                {
                                    string sringData = "0x" + BitConverter.ToString(user.pass).Replace("-", string.Empty);
                                    if (PassUser == sringData)
                                    {
                                        if (InputCheck.Checked)
                                        {
                                            Response.Cookies["user"].Value = TxtUser.Text;
                                            Response.Cookies["pass"].Value = TxtContrasenia.Text;
                                            Response.Cookies["user"].Expires = DateTime.Now.AddMinutes(1);
                                            Response.Cookies["pass"].Expires = DateTime.Now.AddMinutes(1);
                                        }
                                        else
                                        {
                                            Response.Cookies["user"].Expires = DateTime.Now.AddMinutes(-1);
                                            Response.Cookies["pass"].Expires = DateTime.Now.AddMinutes(-1);
                                        }
                                        AlertDanger.Visible = false;
                                        ActualizarIngreso(user.nombreusuario);
                                        Session["Usuario"] = user.nombreusuario;
                                        Response.Redirect("Inicio.aspx");
                                        

                                    }
                                    else
                                    {
                                        AlertWarning.Visible = true;
                                        lblWarning.Text = "<strong>¡Cuidado!</strong>La contraseña es incorrecta...";
                                        TxtContrasenia.Text = "";
                                        TxtContrasenia.Focus();
                                        ActualizarIntentos(TxtUser.Text); //Actualizar el número de intentos del usuario en la BD
                                    }
                                }
                                else
                                {
                                    AlertWarning.Visible = true;
                                    lblWarning.Text = "<strong>¡Cuidado!</strong>El usuario es incorrecto..";
                                    TxtUser.Text = "";
                                    TxtUser.Focus();
                                }
                            }

                            }
                            else
                            {
                                AlertWarning.Visible = true;
                                lblWarning.Text = "<strong>¡Cuidado!</strong>El usuario no existe...";
                                TxtUser.Text = "";
                                TxtContrasenia.Text = "";
                                TxtUser.Focus();
                            }
                        }

                       
                      
                    }
                    catch (Exception ex)
                    {
                        AlertDanger.Visible = true;
                        lblDanger.Text = "<strong>¡Error!</strong> Al conectarse al servidor:" + ex.Message;
                    }
                }
                else
                {
                    AlertDanger.Visible = true;
                    lblDanger.Text = "<strong>¡TU CUENTA HA SIDO BLOQUEADA, Se detectaron varios intentos de acceso incorrectos a su cuenta.! Espere 30 segundos y vuelva a intentarlo.</strong>";
                    BloquearSesion = false;
                    //Activar Temporizador para desbloqueo de cuenta
                    LblMsjTemp1.Visible = true;
                    LblTempBloqueo.Visible = true;
                    LblMsjTemp2.Visible = true;
                    Timer1.Enabled = true;
                }
                
            }
        }
        public bool VerificarIntentos(string NameUser) //*******************Metodo que se encarga de buscar si el No. de intentos es = a 3, si es asi, entonces la cuenta se bloquea
        {
            try
            {
                using (geoBoxWebEntity gbwE = new geoBoxWebEntity())
                {
                    var Query = from order in gbwE.Usuarios
                                where order.usuario == NameUser
                                select new
                                {
                                    Intentos = order.intentos
                                };
                    foreach (var user in Query)
                    {
                        if (user.Intentos == 3)
                        {
                            BloquearSesion = true;
                        }

                    }


                }
            }
            catch (Exception ex)
            {
                AlertDanger.Visible = true;
                lblDanger.Text = "<strong>¡Error!</strong> Info:" + ex.Message;
            }
            return BloquearSesion;
        }
        public void ActualizarIntentos(string NameUser) //******************Metodo utilizado para actualizar el numero de intentos en la BD
        {
           
            try
            {
                using (geoBoxWebEntity gbwE = new geoBoxWebEntity())
                {
                    var Query = from order in gbwE.Usuarios
                                where order.usuario == NameUser
                                select new
                                {
                                    ID = order.iduser,
                                    Intentos = order.intentos
                                   
                                };
                    foreach (var user in Query)
                    {
                        if(user.Intentos < 3)
                        {
                            //Actualizo el compo Intentos de la BD
                            using (var dbContext = new geoBoxWebEntity())
                            {
                                var usuario = dbContext.Usuarios.Find(user.ID);
                                usuario.intentos += 1; //Actualizar número de intentos
                                dbContext.SaveChanges();
                            }
                            ActualizarIngreso(NameUser);
                        }
                       
                    }


                }
            }
            catch (Exception ex)
            {
                AlertDanger.Visible = true;
                lblDanger.Text = "<strong>¡Error!</strong> Info:" + ex.Message;
            }

        }

        private void ActualizarIngreso(string NameUser) //Metodo que se encarga de actualizar la hora de ingreso y la ip de dispositivo
        {
            DateTime localDate = DateTime.Now;
            try
            {
                using (geoBoxWebEntity gbwE = new geoBoxWebEntity())
                {
                    var Query = from order in gbwE.Usuarios
                                where order.usuario == NameUser
                                select new
                                {
                                    ID = order.iduser,

                                };
                    foreach (var user in Query)
                    {
                          //Actualizo el compo Intentos de la BD
                            using (var dbContext = new geoBoxWebEntity())
                            {
                                var usuario = dbContext.Usuarios.Find(user.ID);
                                usuario.ingreso = localDate; //Actualizar fecha hora ingreso
                                usuario.dispOrigen = GetIP4Address(); //Actualizar el dispositivo de donde se accedio
                                dbContext.SaveChanges();
                            }
                       
                    }


                }
            }
            catch (Exception ex)
            {
                AlertDanger.Visible = true;
                lblDanger.Text = "<strong>¡Error!</strong> Info:" + ex.Message;
            }
        }
        protected void LbtnRegistrarse_Click(object sender, EventArgs e) //Pasar al formulario de registro, agregar Nombre Completo , Apellido y Correo
        {
            TxtNombre.Text = ""; //Control para agregar nombre completo
            TxtApellido.Text = "";
            TxtCorreo.Text = "";
            TxtNombre.Focus();
            ImgLogin.Visible = false;
            LblIniciarSesion.Visible = false;
            LblUsuario.Visible = false;
            TxtUser.Visible = false;
            LbtnEye.Visible = false;
            LblContrasenia.Visible = false;
            TxtContrasenia.Visible = false;
            LblCheck.Visible = false;
            InputCheck.Visible = false;
            BtnIngresar.Visible = false;
            LblMsjPregunta.Visible = false;
            LbtnRegistrarse.Visible = false;
            iconUser.Visible = false;
            iconPass.Visible = false;
            //Mostrar Controles de Registro de usuario
            LblRegistro.Visible = true;
            LblCorreo.Visible = true;
            TxtCorreo.Visible = true;
            LblNombre.Visible = true;
            TxtNombre.Visible = true;
            LblApellido.Visible = true;
            TxtApellido.Visible = true;
            LblTienesCuenta.Visible = true;
            LbtnIniciarSesion.Visible = true;
            BtnRegistrarse.Visible = true;
        }

        

        protected void LbtnIniciarSesion_Click(object sender, EventArgs e)
        {
            ImgLogin.Visible = true;
            LblIniciarSesion.Visible = true;
            LblRegistro.Visible = false;
            LblUsuario.Visible = true;
            LblCorreo.Visible = false;
            TxtUser.Visible = true;
            TxtCorreo.Visible = false;
            LbtnEye.Visible = true;
            LblContrasenia.Visible = true;
            TxtContrasenia.Visible = true;
            TxtContrasenia.Text = "";
            LblCheck.Visible = true;
            InputCheck.Visible = true;
            BtnIngresar.Visible = true;
            LblMsjPregunta.Visible = true;
            LbtnRegistrarse.Visible = true;
            iconUser.Visible = true;
            iconPass.Visible = true;
            LblTienesCuenta.Visible = false;
            LbtnIniciarSesion.Visible = false;
            BtnRegistrarse.Visible = false;
            LblNombre.Visible = false;
            TxtNombre.Visible = false;
            LblApellido.Visible = false;
            TxtApellido.Visible = false;
        }

        protected void BtnRegistrarse_Click(object sender, EventArgs e) //Continuar con el registro, agregar Usuario y Contraseña
        {
            TxtNombreUsuario.Text = "";
            TxtPass.Text = "";
            TxtNombreUsuario.Focus();
            LblIniciarSesion.Visible = false;
            LblNombre.Visible = false;
            TxtNombre.Visible = false;
            LblApellido.Visible = false;
            TxtApellido.Visible = false;
            LblCorreo.Visible = false;
            TxtCorreo.Visible = false;
            LblTienesCuenta.Visible = false;
            LbtnIniciarSesion.Visible = false;
            BtnRegistrarse.Visible = false;
            LblPPreguntaNombre.Visible = true; //Pregunta Nombre de usuario
            LblNombreUsuario.Visible = true;
            TxtNombreUsuario.Visible = true;
            LblContrasenia.Visible = true;
            TxtPass.Visible = true;
            BtnContinuar.Visible = true;
            LbtnCancelarRegistro.Visible = true;
        }

        protected void LbtnCancelarRegistro_Click(object sender, EventArgs e)
        {

            LblRegistro.Visible = false;
            /*Ocultar Registro Nombre y apellido*/
            LblPPreguntaNombre.Visible = false;
            LblNombreUsuario.Visible = false;
            TxtNombreUsuario.Visible = false;
            TxtPass.Visible = false;
            LblNombre.Visible = false;
            TxtNombre.Visible = false;
            LblApellido.Visible = false;
            TxtApellido.Visible = false;
            BtnContinuar.Visible = false;
            LbtnCancelarRegistro.Visible = false;
            /*Mostrar Login */
            ImgLogin.Visible = true;
            LblIniciarSesion.Visible = true;
            LblUsuario.Visible = true;
            TxtUser.Visible = true;
            TxtUser.Text = "";
            LbtnEye.Visible = true;
            LblContrasenia.Visible = true;
            TxtContrasenia.Visible = true;
            iconUser.Visible = true;
            iconPass.Visible = true;
            LblCheck.Visible = true;
            InputCheck.Visible = true;
            BtnIngresar.Visible = true;
            LblMsjPregunta.Visible = true;
            LbtnRegistrarse.Visible = true;

        }

        protected void BtnContinuar_Click(object sender, EventArgs e) //**************************Insertando los datos a la tabla Usuario
        {
            DateTime localDate = DateTime.Now;
           
            using (geoBoxWebEntity gbwE = new geoBoxWebEntity())
            {
                
                try
                {
                    if (!ExisteCorreo(TxtCorreo.Text))
                    {
                        if (!ExisteUser(TxtNombreUsuario.Text))
                        {
                            var user = new Usuarios()
                            {
                                usuario = TxtNombreUsuario.Text,
                                nombreCompleto = TxtNombre.Text + " " + TxtApellido.Text,
                                password = Encoding.UTF8.GetBytes(cifrar(TxtPass.Text)),
                                email = TxtCorreo.Text,
                                intentos = 0,
                                ingreso = localDate,
                                dispOrigen = GetIP4Address()
                            };
                            gbwE.Usuarios.Add(user);
                            gbwE.SaveChanges();
                            Page.ClientScript.RegisterStartupScript(this.GetType(), "click", " swal('¡Bien hecho!', '¡Se ha registrado correctamente!', 'success');", true);
                            LbtnCancelarRegistro_Click(sender, e);
                        }
                        else
                        {
                            AlertWarning.Visible = true;
                            lblWarning.Text = "<strong>Lo sentimos el nombre de usuario ya se encuentra registrado</strong>, ingrese un nombre de usuario diferente.";
                            ExisteUsuario = false;
                        }
                    }
                    else
                    {
                        AlertWarning.Visible = true;
                        lblWarning.Text = "<strong>Lo sentimos este email ya se encuentra registrado</strong>, ingrese un email diferente.";
                        ExisteEmail = false;
                    }


                }
                catch (Exception ex)
                {
                    AlertDanger.Visible = true;
                    lblDanger.Text = "<strong>¡Error!</strong> Info:" + ex.Message;
                }
               
            }

            
        }
        //****************************** Metodo que devuelve TRUE si el correo ya existe en la BD
        private bool ExisteCorreo(string email)
        {
            try
            {
                using (geoBoxWebEntity gbwE = new geoBoxWebEntity())
                {
                    var Query = from order in gbwE.Usuarios
                                where order.email == email
                                select new
                                {
                                    Email = order.email,

                                };

                    foreach (var user in Query)
                    {
                        if (email == user.Email)
                        {
                            ExisteEmail = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AlertDanger.Visible = true;
                lblDanger.Text = "<strong>¡Error!</strong> Info: " + ex.Message;
            }

            return ExisteEmail;
        }
        //******************************Metodo que devuelve TRUE  si el usuario ya existe en la BD
        private bool ExisteUser(string NameUser) 
        {
            try
            {
                using (geoBoxWebEntity gbwE = new geoBoxWebEntity())
                {
                    var Query = from order in gbwE.Usuarios
                                where order.usuario == NameUser
                                select new
                                {
                                    nombreusuario = order.usuario,
          
                                };

                    foreach (var user in Query)
                    {
                        if(NameUser == user.nombreusuario)
                        {
                            ExisteUsuario = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AlertDanger.Visible = true;
                lblDanger.Text = "<strong>¡Error!</strong> Info: " + ex.Message;
            }
          
                return ExisteUsuario;
        }
        //****************************** Metodo para cifrar una cadena.
        private string cifrar(string cadena)
        {
            byte[] llave; //Arreglo donde guardaremos la llave para el cifrado 3DES.
            byte[] arreglo = UTF8Encoding.UTF8.GetBytes(cadena); //Arreglo donde guardaremos la cadena descifrada.
            // Ciframos utilizando el Algoritmo MD5.
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            llave = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(clave));
            md5.Clear();
            //Ciframos utilizando el Algoritmo 3DES.
            TripleDESCryptoServiceProvider tripledes = new TripleDESCryptoServiceProvider();
            tripledes.Key = llave;
            tripledes.Mode = CipherMode.ECB;
            tripledes.Padding = PaddingMode.PKCS7;
            ICryptoTransform convertir = tripledes.CreateEncryptor(); // Iniciamos la conversión de la cadena
            byte[] resultado = convertir.TransformFinalBlock(arreglo, 0, arreglo.Length); //Arreglo de bytes donde guardaremos la cadena cifrada.
            tripledes.Clear();
            return Convert.ToBase64String(resultado, 0, resultado.Length); // Convertimos la cadena y la regresamos.
        }

        protected void Timer1_Tick(object sender, EventArgs e) //************************Temporizador para desbloqueo de cuenta
        {
           
            int seconds = int.Parse(LblTempBloqueo.Text);
            if (seconds > 0)
            {
                LblTempBloqueo.Text = (seconds - 1).ToString();
            }
            else
            {
                LblTempBloqueo.Text = "30";
                Timer1.Enabled = false;
                LblMsjTemp1.Visible = false;
                LblMsjTemp2.Visible = false;
                LblTempBloqueo.Visible = false;
                RestaurarIntentos(TxtUser.Text);
               
            }
                
        }

        protected void LbtnEye_Click(object sender, EventArgs e) //Ver Contraseña
        {
            TxtContrasenia.TextMode = TextBoxMode.SingleLine;
            LbtnEye.Visible = false;
            LbtnOffEye.Visible = true;
        }

        protected void LbtnOffEye_Click(object sender, EventArgs e) //Ocultar Contraseña
        {
           
            TxtContrasenia.TextMode = TextBoxMode.Password;
            TxtContrasenia.Attributes.Add("value", TxtContrasenia.Text); //Aginar valor como un atributo, asignar el valor cada vez que recarga la página
            LbtnEye.Visible = true;
            LbtnOffEye.Visible = false;
        }





        //*****************************Metodo para restaurar el No. de intentos
        private void RestaurarIntentos(string NameUser)
        {
            try
            {
                using (geoBoxWebEntity gbwE = new geoBoxWebEntity())
                {
                    var Query = from order in gbwE.Usuarios
                                where order.usuario == NameUser
                                select new
                                {
                                    ID = order.iduser,

                                };
                    foreach (var user in Query)
                    {
                        //Actualizo el compo Intentos de la BD
                        using (var dbContext = new geoBoxWebEntity())
                        {
                            var usuario = dbContext.Usuarios.Find(user.ID);
                            usuario.intentos = 0; //Asignar  0 para restaurar el número de intentos
                            dbContext.SaveChanges();
                        }

                    }


                }
            }
            catch (Exception ex)
            {
                AlertDanger.Visible = true;
                lblDanger.Text = "<strong>¡Error!</strong> Info:" + ex.Message;
            }
        }


        //****************************** Metodo para descifrar una cadena.
        private string descifrar(string cadena)
        {
            byte[] llave;
            byte[] arreglo = Convert.FromBase64String(cadena); // Arreglo donde guardaremos la cadena descovertida.
            // Ciframos utilizando el Algoritmo MD5.
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            llave = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(clave));
            md5.Clear();
            //Ciframos utilizando el Algoritmo 3DES.
            TripleDESCryptoServiceProvider tripledes = new TripleDESCryptoServiceProvider();
            tripledes.Key = llave;
            tripledes.Mode = CipherMode.ECB;
            tripledes.Padding = PaddingMode.PKCS7;
            ICryptoTransform convertir = tripledes.CreateDecryptor();
            byte[] resultado = convertir.TransformFinalBlock(arreglo, 0, arreglo.Length);
            tripledes.Clear();
            string cadena_descifrada = UTF8Encoding.UTF8.GetString(resultado); // Obtenemos la cadena
            return cadena_descifrada; // Devolvemos la cadena
        }

        //*********************Metodo para obtener la ip de la maquina
        private static string GetIP4Address()
        {
            string IP4Address = String.Empty;

            foreach (IPAddress IPA in Dns.GetHostAddresses(HttpContext.Current.Request.UserHostAddress))
            {
                if (IPA.AddressFamily.ToString() == "InterNetwork")
                {
                    IP4Address = IPA.ToString();
                    break;
                }
            }

            if (IP4Address != String.Empty)
            {
                return IP4Address;
            }

            foreach (IPAddress IPA in Dns.GetHostAddresses(Dns.GetHostName()))
            {
                if (IPA.AddressFamily.ToString() == "InterNetwork")
                {
                    IP4Address = IPA.ToString();
                    break;
                }
            }

            return IP4Address;
        }
    }
}