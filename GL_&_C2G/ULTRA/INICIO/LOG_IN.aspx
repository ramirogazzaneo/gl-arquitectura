<%@ Page Title="About" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="LOG_IN.aspx.cs" Inherits="GL___C2G.About" %>

<asp:Content ID="cphHead" ContentPlaceHolderID="HeadContent" runat="server">
  <link href="<%= ResolveUrl("~/Content/CSS/site-login.css") %>" rel="stylesheet" />
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
  <div class="auth-page">
    <div class="auth-card">

 
      <aside class="auth-left">
        <div class="auth-left-inner">
          <h2 class="auth-welcome">Bienvenido!</h2>

   
          <div class="auth-logo">
            <img src="<%= ResolveUrl("~/Content/imagenes/LOGO.PNG") %>" alt="GL Arquitectura" />
          </div>

          <p class="auth-left-note">
            ¿No tenés cuenta?
            <a class="auth-link" href="/ULTRA/INICIO/REGISTER">Registrate ahora</a>
          </p>
        </div>
      </aside>

   
      <section class="auth-right">
        <div class="auth-right-inner">
          <h3 class="auth-title">Log in</h3>

          <asp:Literal ID="lt_login" runat="server" />
          <asp:Label ID="lb_mensaje" runat="server" CssClass="auth-msg auth-msg-error" Visible="false" />

   
          <div class="auth-field">
            <label for="txtCorreo" class="auth-label">Correo *</label>
            <asp:TextBox ID="txtCorreo" runat="server" CssClass="auth-input" Placeholder="nombre@gmail.com" />
            <asp:Label ID="lblErrorCorreo" runat="server" CssClass="auth-msg auth-msg-error small" Visible="false" />
          </div>

      
          <div class="auth-field">
            <label for="txtPass" class="auth-label">Contraseña *</label>
            <asp:TextBox ID="txtPass" runat="server" TextMode="Password" CssClass="auth-input" Placeholder="Usa mayúscula, número y símbolo" />
            <asp:Label ID="lblErrorPass" runat="server" CssClass="auth-msg auth-msg-error small" Visible="false" />
          </div>

  

   
          <div class="auth-actions">
            <asp:Button ID="btnLogin" runat="server"
              CssClass="auth-btn auth-btn-primary"
              Text="Ingresar"
              OnClick="bt_login_Click" />
          </div>


          <div class="auth-divider" role="separator" aria-label="o inicia con"></div>

        </div>
      </section>

    </div>
  </div>
</asp:Content>