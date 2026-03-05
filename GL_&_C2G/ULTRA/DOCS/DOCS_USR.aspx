<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DOCS_USR.aspx.cs" Inherits="GL___C2G.ULTRA.DOCS.WebForm1" %>


<asp:Content ID="HeadCss" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="<%= ResolveUrl("~/Content/CSS/P2.css") %>" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="MainContent" runat="server">
    <div class="canvas-light">
        <div class="sheet-card">
            <div class="page-etapas">
                <h1 class="page-title">Documentos técnicos</h1>

                <asp:ScriptManager runat="server" ID="sm" />

                <asp:UpdatePanel runat="server" ID="up" UpdateMode="Conditional">
                    <ContentTemplate>

                        <%-- establezco los filtros --%>
                        <div class="card">
                            <div class="card-body">
                                <div class="row g-3 align-items-end">
                                    <div class="col-md-4">
                                        <asp:Label runat="server" AssociatedControlID="ddlObras" CssClass="form-label" Text="Obra" />
                                        <asp:DropDownList ID="ddlObras" runat="server" CssClass="form-select"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddlObras_SelectedIndexChanged" />
                                    </div>

                                    <div class="col-md-4">
                                        <asp:Label runat="server" AssociatedControlID="ddlCarpetas" CssClass="form-label" Text="Carpeta" />
                                        <asp:DropDownList ID="ddlCarpetas" runat="server" CssClass="form-select"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddlCarpetas_SelectedIndexChanged" />
                                    </div>
                                </div>
                            </div>
                        </div>

                       <%--  Panel de mensajes --%>
                        <asp:Panel ID="pnlMsg" runat="server" Visible="false" CssClass="alert mt-3">
                            <asp:Literal ID="litMsg" runat="server" />
                        </asp:Panel>

                       <%--    documentos --%>
                        <asp:GridView ID="gvDocs" runat="server"
                            CssClass="table table-striped table-hover mt-3"
                            AutoGenerateColumns="False"
                            DataKeyNames="id_documento">

                            <Columns>
                                <asp:BoundField DataField="carpeta" HeaderText="Carpeta" />
                                <asp:BoundField DataField="archivo" HeaderText="Nombre" />
                                <asp:BoundField DataField="tipo" HeaderText="Tipo" />
                                <asp:BoundField DataField="versionn" HeaderText="Versión" />
                                <asp:BoundField DataField="fecha" HeaderText="Fecha" DataFormatString="{0:dd/MM/yyyy}" HtmlEncode="False" />
                                <asp:BoundField DataField="usuario" HeaderText="Usuario" />

                                <asp:TemplateField HeaderText="Acciones">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="lnkVer" runat="server"
                                            NavigateUrl='<%# "~/Handlers/VerDocumento.ashx?id=" + Eval("id_documento") %>'
                                            CssClass="btn btn-sm btn-primary" Text="Ver" Target="_blank" /> <%--el blank abre en una nueva pestana--%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>

                    </ContentTemplate>
                    <Triggers></Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
</asp:Content>
