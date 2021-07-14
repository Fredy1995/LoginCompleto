<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="AppWeb.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="nav" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="section" runat="server">
   
<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
 <div class="login">
 <div class="row">
  <div class="col-sm-4"></div>
    <div class="col-sm-4">
        <div class="card login">
            <div class="card-body body">
                <div class="text-center">
                    <img ID="ImgLogin" runat="server" src="img/user.png"  class="rounded-circle img-fluid" width="150" height="150"/>
                </div>
                <div class="text-center text-info" style="  font-weight: bold; font-size:20px;">
                     <asp:Label ID="LblIniciarSesion" runat="server" Text="Iniciar sesión"></asp:Label>
                     <asp:Label ID="LblRegistro" runat="server" Text="Registrarse" Visible="False"></asp:Label>
                </div><br />
                <h3><asp:Label ID="LblPPreguntaNombre" runat="server" Text="Configurar usuario" Visible="false"></asp:Label> </h3>


                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>

                   
              <div class="form-group">
                  <asp:Label ID="LblUsuario" runat="server" Text="Usuario:"></asp:Label>
                   <div class="input-group mb-3">
                     <div class="input-group-prepend" id="iconUser" runat="server"><span class="input-group-text"><i class="mdi mdi-account mdi-dark mdi-12px"></i></span></div>
                     <asp:TextBox ID="TxtUser" runat="server"  Cssclass="form-control"   placeholder="Ingrese usuario" required=""></asp:TextBox> 
                   </div>
                  <asp:Label ID="LblNombre" runat="server" Text="Nombre(s):" Visible="false"></asp:Label>
                  <asp:TextBox ID="TxtNombre" runat="server" Cssclass="form-control" placeholder="Ingrese nombre completo" Visible="false" required=""></asp:TextBox>
              </div>
               <div class="form-group">
                    <asp:Label ID="LblApellido" runat="server" Text="Apellidos" Visible="false"></asp:Label>
                  <asp:TextBox ID="TxtApellido" runat="server" Cssclass="form-control" placeholder="Ingrese apellidos" Visible="false" required=""></asp:TextBox>
                   
                   <asp:Label ID="LblNombreUsuario" runat="server" Text="Nombre de usuario:" Visible="false"></asp:Label>
                     <asp:TextBox ID="TxtNombreUsuario" runat="server" Cssclass="form-control" placeholder="Ingrese nombre de usuario" required="" Visible="false"></asp:TextBox>
               </div>
               <div class="form-group">
                   <asp:Label ID="LblCorreo" runat="server" Text="Correo electrónico:" Visible="False"></asp:Label>
                   <asp:TextBox ID="TxtCorreo" runat="server" Cssclass="form-control"   placeholder="Ingrese correo electrónico" Visible="False" required="" TextMode="Email"></asp:TextBox>
                       <div class="d-flex justify-content-between ">
                       <asp:Label ID="LblContrasenia" runat="server" Text="Contraseña:" CssClass="mr-sm-4"></asp:Label>
                           <asp:LinkButton ID="LbtnEye" runat="server" OnClick="LbtnEye_Click"><i class="mdi mdi-eye-outline mdi-12px text-info"></i></asp:LinkButton>
                           <asp:LinkButton ID="LbtnOffEye" runat="server" OnClick="LbtnOffEye_Click" Visible="false" ><i class="mdi mdi-eye-off-outline mdi-12px text-info"></i></asp:LinkButton>
                       </div>
                   <div class="input-group mb-3">
                         <div class="input-group-prepend" id="iconPass" runat="server"><span class="input-group-text"><i class="mdi mdi-lock-outline mdi-dark mdi-12px"></i></span></div>
                          <asp:TextBox ID="TxtContrasenia" runat="server" Cssclass="form-control" TextMode="Password"  placeholder="Introducir la contraseña" required=""></asp:TextBox> 
                   </div>
                   <asp:TextBox ID="TxtPass" runat="server" Cssclass="form-control" TextMode="Password"  placeholder="Introducir la contraseña" Visible="False" pattern="(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{8,}" title="Debe contener al menos un número y una letra mayúscula y minúscula, y al menos 8 caracteres o más" required=""></asp:TextBox>
               </div>
               </ContentTemplate>
                </asp:UpdatePanel>



               <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <div  class="text-center text-danger"> 
                            <asp:Label ID="LblMsjTemp1" runat="server" Text="Tiempo restante: " Visible="False"></asp:Label>
                        <asp:Label ID="LblTempBloqueo" runat="server" Visible="False">30</asp:Label>
                            <asp:Label ID="LblMsjTemp2" runat="server" Text="s para desbloquear su cuenta." Visible="False"></asp:Label>
                        <asp:Timer ID="Timer1" runat="server" Interval="1000" OnTick="Timer1_Tick" Enabled="False"></asp:Timer>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div class="form-group form-check">
                <label id="LblCheck" runat="server" class="form-check-label">
                    <input class="form-check-input" type="checkbox" id="InputCheck" runat="server" checked="checked"> Recordar contraseña
                </label>
                </div>
                <div id="message">
                    <h6>La contraseña debe contener lo siguiente:</h6>
                    <p id="letter" class="invalid">Una letra <b>minúscula</b></p>
                    <p id="capital" class="invalid">Una letra <b>mayúscula </b></p>
                    <p id="number" class="invalid">Un <b>número</b></p>
                    <p id="length" class="invalid">Mínimo <b>8 caracteres</b></p>
                </div>
                    <asp:Button ID="BtnIngresar" runat="server" Text="Iniciar sesión" CssClass ="btn btn-outline-info btn-block" OnClick="BtnIngresar_Click" />
                    <asp:Button ID="BtnRegistrarse" runat="server" Text="Registrate aquí" CssClass ="btn btn-outline-secondary btn-block" Visible="False"  OnClick="BtnRegistrarse_Click"/>
                    <asp:Button CssClass ="btn btn-outline-secondary btn-block" ID="BtnContinuar" runat="server" Text="Continuar" Visible="false" OnClick="BtnContinuar_Click" />
                <div class="text-center">
                    <asp:Label ID="LblMsjPregunta" runat="server" Text="¿No tienes cuenta?"></asp:Label>
                    <asp:LinkButton class="text-info" ID="LbtnRegistrarse" runat="server" OnClick="LbtnRegistrarse_Click">Registrate</asp:LinkButton>
                    <asp:Label ID="LblTienesCuenta" runat="server" Text="¿Ya tienes una cuenta?" Visible="False"></asp:Label>
                    <asp:LinkButton class="text-info" ID="LbtnIniciarSesion" runat="server" OnClick="LbtnIniciarSesion_Click" Visible="False">Iniciar sesión</asp:LinkButton>
                </div>
                <div class="text-right">
                    <asp:LinkButton class="text-info" ID="LbtnCancelarRegistro" runat="server" OnClick="LbtnCancelarRegistro_Click" Visible="false">Cancelar</asp:LinkButton>
                </div>
            </div>
        </div>
    </div>
  <div class="col-sm-2"></div>
</div> 
</div>

    

    <div class="alert alert-danger alert-dismissible fixed-top" style=" width: 90%;margin-left: 4em;" role="alert" visible="false" runat="server" ID="AlertDanger">
        <button class="close" type="button" data-dismiss="alert"><!--span class="icon icon-cancel-circle"></span-->X</button>
        <!--span class="icon icon-sad" aria-hidden="true"></span-->
        <asp:Label ID="lblDanger" runat="server" ></asp:Label>
    </div>
     <div class=" alert alert-warning  alert-dismissible fixed-top" style=" width: 90%;margin-left: 4em;" role="alert" visible="false" runat="server" id="AlertWarning">
        <button class="close" type="button" data-dismiss="alert"><!--span class="icon icon-cancel-circle"></span-->X</button>
        <!--span class="icon icon-warning" aria-hidden="true"></span-->
        <asp:Label ID="lblWarning" runat="server" ></asp:Label>
    </div>
    
    <div>
    
 </div>
    
    <script>

        var myInput = document.getElementById('<%=TxtPass.ClientID%>');
        var letter = document.getElementById("letter");
        var capital = document.getElementById("capital");
        var number = document.getElementById("number");
        var length = document.getElementById("length");

        // When the user clicks on the password field, show the message box
        myInput.onfocus = function () {
            document.getElementById("message").style.display = "block";
        }

        // When the user clicks outside of the password field, hide the message box
        myInput.onblur = function () {
            document.getElementById("message").style.display = "none";
        }

        // When the user starts to type something inside the password field
        myInput.onkeyup = function () {
            // Validate lowercase letters
            var lowerCaseLetters = /[a-z]/g;
            if (myInput.value.match(lowerCaseLetters)) {
                letter.classList.remove("invalid");
                letter.classList.add("valid");
            } else { 
                letter.classList.remove("valid");
                letter.classList.add("invalid");
            }

            // Validate capital letters
            var upperCaseLetters = /[A-Z]/g;
            if (myInput.value.match(upperCaseLetters)) {
                capital.classList.remove("invalid");
                capital.classList.add("valid");
            } else {
                capital.classList.remove("valid");
                capital.classList.add("invalid");
            }

            // Validate numbers
            var numbers = /[0-9]/g;
            if (myInput.value.match(numbers)) {
                number.classList.remove("invalid");
                number.classList.add("valid");
            } else {
                number.classList.remove("valid");
                number.classList.add("invalid");
            }

            // Validate length
            if (myInput.value.length >= 8) {
                length.classList.remove("invalid");
                length.classList.add("valid");
            } else {
                length.classList.remove("valid");
                length.classList.add("invalid");
            }
        }

        //Ocultar password
        function myFunction() {
            var x = document.getElementById("myInput");
            if (x.type === "password") {
                x.type = "text";
            } else {
                x.type = "password";
            }
        }

        

    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="footer" runat="server"></asp:Content>