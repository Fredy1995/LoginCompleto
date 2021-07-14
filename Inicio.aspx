<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Inicio.aspx.cs" Inherits="AppWeb.Inicio" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="section" runat="server">
      <div class="jumbotron text-center">
        <h2>!BIENVENIDO!  <asp:Label ID="Lbluser" runat="server" Text=""></asp:Label></h2> 
          <asp:Button ID="BtnCerrarSesion" runat="server" Text="Salir" OnClick="BtnCerrarSesion_Click" />

    </div>
</asp:Content>
