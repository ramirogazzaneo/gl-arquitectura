<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FINANZAS_USR.aspx.cs" Inherits="GL___C2G.WebForm11" %>



<asp:Content ID="HeadCss" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="<%= ResolveUrl("~/Content/CSS/P2.css") %>" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="MainContent" runat="server">
      <div class="canvas-light">
    <div class="sheet-card">
      <div class="page-etapas">
    <h1 class="page-title">Mis Pagos</h1>
        <asp:ScriptManager runat="server" ID="sm1" />


        <div class="card">
            <div class="card-header">
                <div class="row g-2 align-items-end w-100">
                    <div class="col-md-6">
                        <label class="form-label" for="<%= ddlObras.ClientID %>">Obra</label>
                        <asp:DropDownList ID="ddlObras" runat="server"
                            CssClass="form-select"
                            AutoPostBack="true"
                            OnSelectedIndexChanged="Filtro_Changed" />
                    </div>

                    <div class="col-md-6">
                        <label class="form-label">&nbsp;</label>
                        <div>
                            <label class="form-check-label form-label">
                                <asp:CheckBox ID="chkSoloPend" runat="server"
                                    CssClass="form-check-input"
                                    AutoPostBack="true"
                                    OnCheckedChanged="Filtro_Changed"
                                    Checked="true" />
                                Solo pendientes
                            </label>
                        </div>
                    </div>
                </div>
            </div>
        </div>


        <asp:UpdatePanel runat="server" ID="upPagos" UpdateMode="Conditional">
            <ContentTemplate>


                <asp:Panel ID="pnlMsg" runat="server" Visible="false"
                    CssClass="alert" Style="margin-bottom: 12px;">
                    <asp:Literal ID="litMsg" runat="server" />
                </asp:Panel>

                <div class="card">
                    <div class="card-header d-flex justify-content-between align-items-center">
                        <span class="pill">Pagos</span>
                        <span class="form-label">Total pendiente: $
                            <asp:Literal ID="litTotal" runat="server" /></span>
                    </div>

                    <div class="card-body">
                        <asp:GridView ID="gvPagos" runat="server"
                            AutoGenerateColumns="False"
                            CssClass="table grid-sm"
                            GridLines="None">
                            <Columns>

                                <asp:BoundField DataField="obra" HeaderText="Obra" />

                                <asp:BoundField DataField="fecha" HeaderText="Fecha" DataFormatString="{0:dd/MM/yyyy}">
                                    <ItemStyle />
                                </asp:BoundField>

                                <asp:BoundField DataField="fecha_ven" HeaderText="Vence" DataFormatString="{0:dd/MM/yyyy}">
                                    <ItemStyle />
                                </asp:BoundField>

                                <asp:BoundField DataField="descripcion" HeaderText="Descripción" />


                                <asp:BoundField DataField="monto" HeaderText="Monto" DataFormatString="{0:N2}">
                                    <ItemStyle CssClass="col-money" />
                                    <HeaderStyle CssClass="col-money" />
                                </asp:BoundField>

                                

                                <asp:TemplateField HeaderText="Estado">
                                    <ItemTemplate>
        <!-- aca lo que hago es dependiendo del valor leido por eval, disparo depende de si es true o false un mensaje de diferente color definido por clases de html y bootstrap -->
                                        <%# Convert.ToBoolean(Eval("estado_pago"))
                          ? "<span class='badge bg-success'>Pagado</span>"
                          : "<span class='badge bg-warning text-dark'>Pendiente</span>" %>
                                    </ItemTemplate>
                                </asp:TemplateField>

                            </Columns>

                            <EmptyDataTemplate>
                                <em class="text-muted">No hay pagos para los filtros seleccionados.</em>
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </div>
                </div>

            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ddlObras" EventName="SelectedIndexChanged" />
                <asp:AsyncPostBackTrigger ControlID="chkSoloPend" EventName="CheckedChanged" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
        </div>
          </div>

</asp:Content>
