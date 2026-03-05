<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DIRECCION_ADM.aspx.cs" Inherits="GL___C2G.WebForm7" %>


    <asp:Content ID="cphHead" ContentPlaceHolderID="HeadContent" runat="server">
  <link href="<%= ResolveUrl("~/Content/CSS/obras.css") %>" rel="stylesheet" />
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">


 <div class="profile-wrapper">
  <div class="profile-card">
            <h1 class="page-title"></h1>
          
          
            <h3 class="profile-title text-center">Registrar Obra</h3>
       
            <asp:Literal ID="Liobras" runat="server" />


       <div class="row">
           <div class="col-md-6">
             <div class="form-group">
        <label>Usuario</label>
        <asp:TextBox ID="txIdU" runat="server" CssClass="form-control text-bg-dark" Enabled="false"/>
        <asp:Label ID="Label1" runat="server"
            CssClass="text-danger small"
            Visible="false" />
    </div>
               </div>
           <div class="col-md-6">
            <div class="form-group">
                <label>Calle</label>
                <asp:TextBox ID="txCalle" runat="server" CssClass="form-control text-bg-dark"  Placeholder=" ej: Rivadavia" />
                <asp:Label ID="lblCalle" runat="server"
                    CssClass="text-danger small"
                    Visible="false" />
            </div>
               </div>
           <div class="col-md-6">
            <div class="form-group">
                <label>Numero</label>
                <asp:TextBox ID="txNumero" runat="server" CssClass="form-control text-bg-dark" Placeholder=" ej: 401 " />
                <asp:Label ID="lblNumero" runat="server"
                    CssClass="text-danger small"
                    Visible="false" />
            </div>
            </div>
           <div class="col-md-6">
            <div class="form-group">
                <label>Barrio</label>
                <asp:TextBox ID="txBarrio" runat="server" CssClass="form-control text-bg-dark" Placeholder=" ej: Bosque Real" />
                <asp:Label ID="lblBarrio" runat="server"
                    CssClass="text-danger small"
                    Visible="false" />
            </div>
            </div>
           <div class="col-md-6">
            <div class="form-group">
                <label>Localidad</label>
                <asp:TextBox ID="txLocalidad" runat="server" CssClass="form-control text-bg-dark" Placeholder="ej: general rodriguez" />
                <asp:Label ID="lblLocalidad" runat="server"
                    CssClass="text-danger small"
                    Visible="false" />
            </div>
            </div>
           <div class="col-md-6">
            <div class="form-group">
                <label>Provincia</label>
                <asp:TextBox ID="txProvincia" runat="server"
                    CssClass="form-control text-bg-dark" Placeholder="ej: Buenos Aires" />
                <asp:Label ID="lblProvincia" runat="server"
                    CssClass="text-danger small"
                    Visible="false" />
            </div>
               </div>
           <div class="col-md-6">
             <div class="form-group">
     <label>Nombre de la obra</label>
     <asp:TextBox ID="txNombre" runat="server"
         CssClass="form-control text-bg-dark" Placeholder="ej: horizontes al sur"/>
     <asp:Label ID="lblNombre" runat="server"
         CssClass="text-danger small" 
         Visible="false" />
 </div>
               </div>
           <div class="col-md-6">
             <div class="form-group ">
     <label>Metros Totales</label>
     <asp:TextBox ID="txM2T" runat="server"
         CssClass="form-control text-bg-dark" Placeholder="ej: 450"/>
     <asp:Label ID="lblMetros2" runat="server"
         CssClass="text-danger small" 
         Visible="false" />
 </div>
            </div>
               <br />

               
         <div class="button-group">
                <asp:Button ID="btnAceptar"
                    runat="server"
                    class="btn btn-guardar mb-2"
                    Text="Aceptar"
                    OnClick="btnAceptar_Click" />
             </div>
           </div>
        </div>
    </div>
</asp:Content>

