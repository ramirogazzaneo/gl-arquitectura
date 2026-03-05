<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="USUARIO.aspx.cs" Inherits="GL___C2G.WebForm1" %>

<asp:Content ContentPlaceHolderID="HeadContent" runat="server">

    <link href="<%= ResolveUrl("~/Content/CSS/profile.css") %>" rel="stylesheet" />

</asp:Content>


<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <div class="profile-wrapper">
        <div class="profile-card">

            <h3 class="profile-title">Mi Perfil</h3>

            <div class="form-group">
                <label>Correo Electrónico</label>
                <asp:TextBox ID="tx_correo" runat="server" CssClass="form-control" Enabled="false" />
            </div>

            <div class="form-group">
                <label>Teléfono</label>
                <asp:TextBox ID="tx_tel" runat="server" CssClass="form-control" />
                <asp:Label ID="lbl_errorTel" runat="server" CssClass="text-danger small" Visible="false" />
            </div>

            <div class="form-group">
                <label>Nombre</label>
                <asp:TextBox ID="tx_nombre" runat="server" CssClass="form-control" />
                <asp:Label ID="lbl_errorNombre" runat="server" CssClass="text-danger small" Visible="false" />
            </div>

            <div class="form-group">
                <label>Apellido</label>
                <asp:TextBox ID="tx_apellido" runat="server" CssClass="form-control" />
                <asp:Label ID="lbl_errorApellido" runat="server" CssClass="text-danger small" Visible="false" />
            </div>

            <div class="form-group">
                <p class="helper-text">
                    ¿olvidaste tu contraseña?
                <a href="/ULTRA/USUARIOS/CLAVE.aspx">Cambiar contraseña</a>
                </p>

                <div class="column w-100 button-group">
                    <asp:Button ID="bt_guardar" runat="server"
                        CssClass="btn btn-guardar mb-2"
                        Text="Guardar" OnClick="bt_guardar_Click" />
                    <asp:Button ID="bt_inicio" runat="server" CssClass="btn btn-inicio" Text="Inicio" />
                </div>
            </div>
        </div>
        </div>
</asp:Content>
