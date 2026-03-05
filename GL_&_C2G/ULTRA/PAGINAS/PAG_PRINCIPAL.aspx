<%@ Page Title="Inicio" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="PAG_PRINCIPAL.aspx.cs"
    Inherits="GL___C2G.Contact" %>

<asp:Content ContentPlaceHolderID="HeadContent" runat="server">

    <link rel="stylesheet" href="<%= ResolveUrl("~/Scripts/WebForms/bootstrap.min.css") %>" />

    <link href="<%= ResolveUrl("~/Content/CSS/P2.css") %>" rel="stylesheet" />
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">

    <div class="canvas-light">
        <div class="sheet-card">
            <div class="page-etapas">


                <h1 class="page-title">Mis obras</h1>
                <asp:ScriptManager runat="server" ID="sm" />

               
                <div class="row g-3">
                    <div class="col-md-3">
                        <div class="card">
                            <div class="card-body">
                                <div class="text-muted">Mis obras activas</div>
                                <h3 id="kpi_activas" runat="server" class="text-muted"></h3>
                                <asp:Button ID="btnObrasActivas" CssClass="btn btn-primary" runat="server" Text="Ver obras" OnClick="btnObrasActivas_Click" />
                            </div>
                        </div>
                    </div>
                    <div class="col-md-3">
                        <div class="card">
                            <div class="card-body">
                                <div class="text-muted">Mis obras finalizadas</div>
                                <h3 id="kpi_proceso" runat="server" class="text-muted"></h3>

                            </div>
                        </div>
                    </div>
                    <div class="col-md-3">
                        <div class="card">
                            <div class="card-body">
                                <div class="text-muted">Mis obras en espera</div>
                                <h3 id="kpi_espera" runat="server" class="text-muted"></h3>

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

                <h2 class="mt-4 mb-3 text-muted">Pagos de mis obras</h2>

                <asp:UpdatePanel runat="server" ID="upPagos">
                    <ContentTemplate>
                        <asp:GridView ID="gvPagos" runat="server"
                            CssClass="table table-hover table-sm align-middle"
                            AutoGenerateColumns="False"
                            OnRowDataBound="gvPagos_RowDataBound"
                            DataKeyNames="id_pago,vencido,estado_pago">
                            <Columns>
                                <asp:BoundField DataField="obra" HeaderText="Obra" />
                                <asp:BoundField DataField="descripcion" HeaderText="Descripción" />

                                <asp:BoundField DataField="monto" HeaderText="Monto" DataFormatString="{0:N2}">
                                </asp:BoundField>

                                <asp:BoundField DataField="fecha_ven" HeaderText="Vence"
                                    DataFormatString="{0:dd/MM/yyyy}" />

                                <asp:TemplateField HeaderText="Estado">
                                    <ItemTemplate>
                                        <asp:Literal ID="litEstado" runat="server"></asp:Literal>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
</asp:Content>
