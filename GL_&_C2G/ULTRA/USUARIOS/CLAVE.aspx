<%@ Page Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="CLAVE.aspx.cs"
    Inherits="GL___C2G.WebForm2" %>

<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
  <link href="<%= ResolveUrl("~/Content/CSS/profile.css") %>"
        rel="stylesheet" />
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
  <div class="profile-wrapper">
    <div class="profile-card">

      <h3 class="profile-title">Cambiar Contraseña</h3>

      
      <div class="form-group">
        <label for="tx_claveActual">Contraseña Actual</label>
        <asp:TextBox ID="tx_claveActual"
                     runat="server"
                     TextMode="Password"
                     CssClass="form-control" />
        <asp:Label ID="lbl_errorClaveActual"
                   runat="server"
                   CssClass="text-danger small"
                   Visible="false" />
      </div>

     
      <div class="form-group">
        <label for="tx_claveNueva">Nueva Clave</label>
        <asp:TextBox ID="tx_claveNueva"
                     runat="server"
                     TextMode="Password"
                     CssClass="form-control" Placeholder="Usa mayúscula, número y símbolo" />
        <asp:Label ID="lbl_erroClaveNueva"
                   runat="server"
                   CssClass="text-danger small"
                   Visible="false" />
      </div>

      
      <div class="form-group">
        <label for="tx_claveConfirmacion">Confirmar Clave</label>
        <asp:TextBox ID="tx_claveConfirmacion"
                     runat="server"
                     TextMode="Password"
                     CssClass="form-control" Placeholder="Repita la Clave"  />
        <asp:Label ID="lbl_erroClaveConfirmacion"
                   runat="server"
                   CssClass="text-danger small"
                   Visible="false" />
      </div>

     <div class="form-group">
      <div class="button-group">
        <asp:Button ID="btnGuardar"
                    runat="server"
                    CssClass="btn btn-guardar mb-2"
                    Text="Guardar"
                    onclick="btnGuardar_Click" />

          <asp:Button ID="btnVolver"
            runat="server"
            CssClass="btn btn-guardar mb-2"
            Text="Volver"
            Onclick="btnVolver_Click" />
      </div>
         </div>
   

        <div class="mt-3 text-center" id="lt_respuesta_wrapper">
  <asp:Literal ID="lt_respuesta" runat="server" />
</div>

    </div>
  </div>
</asp:Content>
