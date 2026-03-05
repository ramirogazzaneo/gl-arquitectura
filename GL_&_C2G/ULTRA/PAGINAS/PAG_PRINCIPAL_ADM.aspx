<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PAG_PRINCIPAL_ADM.aspx.cs" Inherits="GL___C2G.WebForm3" %>

<asp:Content ContentPlaceHolderID="HeadContent" runat="server">

    <script src="<%= ResolveUrl("~/Scripts/WebForms/chart.umd.js") %>"></script>
    <link href="<%= ResolveUrl("~/Content/CSS/P2.css") %>" rel="stylesheet" />

</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <div class="canvas-light">
        <div class="sheet-card">
            <div class="page-etapas">

                <h1 class="page-title">Reportes</h1>
                <asp:ScriptManager runat="server" ID="sm" />


               
                <div class="card mb-3">
                    <div class="card-body">
                        <div class="row g-2 align-items-end">
                            <div class="col-md-3">
                                <label class="form-label">Desde</label>
                                <asp:TextBox ID="txtDesde" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                            </div>
                            <div class="col-md-3">
                                <label class="form-label">Hasta</label>
                                <asp:TextBox ID="txtHasta" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                            </div>
                            <div class="col-md-4">
                                <label class="form-label">Obra</label>
                                <asp:DropDownList ID="ddlObraFiltro" runat="server" CssClass="form-select"
                                    AutoPostBack="true" OnSelectedIndexChanged="ddlObraFiltro_SelectedIndexChanged" />
                            </div>
                            <div class="col-md-2">
                                <asp:Button ID="btnFiltrar" runat="server" Text="Filtrar" CssClass="btn btn-primary w-100" OnClick="btnFiltrar_Click" />
                            </div>
                        </div>
                    </div>
                </div>

              
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



                    <div class="col-md-3">
                        <div class="card">
                            <div class="card-body">
                                <div class="text-muted">Total deuda</div>
                                <h3 id="kpi_deuda" runat="server" class="text-muted"></h3>
                            </div>
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


                    <div class="col-lg-6">
                        <div class="card">
                            <div class="card-header">Cobrado vs Esperado</div>
                            <div class="card-body">
                                <div class="chart-box md">
                                    <canvas id="chartMes" class="chart-sm"></canvas>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="card mt-3">
                    <div class="card-header">Serie diaria (mes actual)</div>
                    <div class="card-body">
                        <div class="chart-box lg">
                            <canvas id="chartLinea" class="chart-md"></canvas>
                        </div>
                    </div>
                </div>

              
                <div class="card mt-3">
                    <div class="card-header">Top 10 Deudores</div>
                    <div class="card-body">
                        <asp:GridView ID="gvDeudores" runat="server"
                            CssClass="table table-striped table-hover grid-sm"
                            AutoGenerateColumns="False">
                            <Columns>
                                <asp:BoundField DataField="Usuario" HeaderText="Usuario" />
                                <asp:BoundField DataField="Deuda" HeaderText="Deuda ($)" DataFormatString="{0:C0}" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>


            </div>
        </div>
    </div>

</asp:Content>
