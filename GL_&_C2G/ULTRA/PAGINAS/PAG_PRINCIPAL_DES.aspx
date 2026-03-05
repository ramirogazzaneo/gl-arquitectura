<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PAG_PRINCIPAL_DES.aspx.cs" Inherits="GL___C2G.ULTRA.PAGINAS.WebForm1" %>


<asp:Content ContentPlaceHolderID="HeadContent" runat="server">

    <script src="<%= ResolveUrl("~/Scripts/WebForms/chart.umd.js") %>"></script>
    <link href="<%= ResolveUrl("~/Content/CSS/P2.css") %>" rel="stylesheet" />

</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <div class="canvas-light">
        <div class="sheet-card">
            <div class="page-etapas">

                <h1 class="page-title">Bienvenido de nuevo</h1>
                <asp:ScriptManager runat="server" ID="sm" />


            
                <div class="row g-3">
                    <div class="col-md-3">
                        <div class="card">
                            <div class="card-body">
                                <div class="text-muted">Obras activas</div>
                                <h3 id="kpi_activas" runat="server" class="text-muted"></h3>
                                <asp:HyperLink ID="lnkTareas" runat="server"
                                    CssClass="btn btn-sm btn-primary"
                                    Text="Ver obras"
                                    NavigateUrl="~/ULTRA/OBRAS/OBRAS_E.aspx?id_estado=1" />
                            </div>
                        </div>
                    </div>
                    <div class="col-md-3">
                        <div class="card">
                            <div class="card-body">
                                <div class="text-muted">Finalizadas</div>
                                <h3 id="kpi_proceso" runat="server" class="text-muted"></h3>
                                <asp:HyperLink ID="HyperLink1" runat="server"
                                    CssClass="btn btn-sm btn-primary"
                                    Text="Ver obras"
                                    NavigateUrl="~/ULTRA/OBRAS/OBRAS_E.aspx?id_estado=2" />
                            </div>
                        </div>
                    </div>
                    <div class="col-md-3">
                        <div class="card">
                            <div class="card-body">
                                <div class="text-muted">En espera</div>
                                <h3 id="kpi_espera" runat="server" class="text-muted"></h3>
                                <asp:HyperLink ID="HyperLink2" runat="server"
                                    CssClass="btn btn-sm btn-primary"
                                    Text="Ver obras"
                                    NavigateUrl="~/ULTRA/OBRAS/OBRAS_E.aspx?id_estado=3" />
                            </div>
                        </div>
                    </div>

                  
                    <div class="row g-3 mt-1">
                        <div class="col-lg-6">
                            <div class="card">
                                <div class="card-header">Obras por estado</div>
                                <div class="card-body">
                                    <div class="chart-box md">
                                        <canvas id="chartEstados" class="chart-sm"></canvas>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
