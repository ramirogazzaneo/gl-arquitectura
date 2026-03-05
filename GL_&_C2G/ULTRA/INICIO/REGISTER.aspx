<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="REGISTER.aspx.cs" Inherits="GL___C2G._Default" %>


<asp:Content ID="cphHead" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="<%= ResolveUrl("~/Content/CSS/site-login.css") %>" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="MainContent" runat="server">
    <div class="auth-page">
        <div class="auth-card">

            <aside class="auth-left">
                <div class="auth-left-inner">
                    <div class="auth-logo">
                        <img src="<%= ResolveUrl("~/Content/imagenes/LOGO.PNG") %>" alt="GL Arquitectura" />
                    </div>
                    <p class="auth-left-note">
                        ¿Ya tenés cuenta?
            <a class="auth-link" href="/ULTRA/INICIO/LOG_IN">Inicia sesión</a>
                    </p>
                </div>
            </aside>

            <section class="auth-right">
                <div class="auth-right-inner">
                    <h3 class="auth-title">Registrarse</h3>

                    <asp:Literal ID="lt_login" runat="server" />

                   
                    <div class="auth-field">
                        <label for="txtCorreo" class="auth-label">Correo electrónico *</label>
                        <asp:TextBox ID="txtCorreo" runat="server" CssClass="auth-input" Placeholder="Nombre@gmail.com" />
                        <asp:Label ID="lblErrorCorreo" runat="server" CssClass="auth-msg auth-msg-error small" Visible="false" />
                    </div>

                   
                    <div class="auth-field">
                        <label for="txtTel" class="auth-label">Teléfono</label>
                        <asp:TextBox ID="txtTel" runat="server" CssClass="auth-input" Placeholder="+11 1234 5678" />
                        <asp:Label ID="lblErrorTel" runat="server" CssClass="auth-msg auth-msg-error small" Visible="false" />
                    </div>

               
                    <div class="auth-field">
                        <label for="txtNombre" class="auth-label">Nombre *</label>
                        <asp:TextBox ID="txtNombre" runat="server" CssClass="auth-input" Placeholder="Ramiro" />
                        <asp:Label ID="lblErrorNombre" runat="server" CssClass="auth-msg auth-msg-error small" Visible="false" />
                    </div>

               
                    <div class="auth-field">
                        <label for="txtApellido" class="auth-label">Apellido *</label>
                        <asp:TextBox ID="txtApellido" runat="server" CssClass="auth-input" Placeholder="Gazzaneo" />
                        <asp:Label ID="lblErrorApellido" runat="server" CssClass="auth-msg auth-msg-error small" Visible="false" />
                    </div>

              
                    <div class="auth-field">
                        <label for="txtPass" class="auth-label">Contraseña *</label>
                        <asp:TextBox ID="txtPass" runat="server" TextMode="Password" CssClass="auth-input"
                            Placeholder="Usa mayúscula, número y símbolo" />
                        <asp:Label ID="lblErrorPass" runat="server" CssClass="auth-msg auth-msg-error small" Visible="false" />
                    </div>

                
                    <div class="auth-actions">
                        <asp:Button ID="btnRegist" runat="server"
                            CssClass="auth-btn auth-btn-primary"
                            Text="Crear cuenta"
                            OnClick="bt_regist_Click" />
                    </div>

              
                    <div class="auth-divider"></div>

                </div>

            </section>

        </div>
    </div>
</asp:Content>
